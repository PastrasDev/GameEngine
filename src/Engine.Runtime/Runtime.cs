using System;
using System.Threading;
using Engine.Core;

namespace Engine.Runtime;

public interface IRuntime : IDisposable
{
    static int Launch(RuntimeMode mode, string[]? args)
    {
        using Runtime runtime = mode switch
        {
            RuntimeMode.Editor => new Editor(args),
            RuntimeMode.Game => new Game(args),
            RuntimeMode.None => throw new ArgumentException("Runtime mode not specified", nameof(mode)),
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };

        return (int)runtime.Run();
    }
}

internal abstract class Runtime : IRuntime
{
    protected const float _fixedDt = 1f / 120f;
    protected const float _maxFrameDt = 0.25f;
    protected const int _maxFixedStepsPerFrame = 8;
    
    protected const int _framePipeCapacity = 256;
    protected const int _inputPipeCapacity = 256;

    protected readonly CancellationTokenSource _cts = new();

    protected readonly IRegistry _registry;
    
    protected readonly IRegistry _mainRegistry;
    protected readonly IRegistry _gameRegistry;
    protected readonly IRegistry _renderRegistry;
    
    protected readonly IEngineKernel _mainKernel;
    protected readonly IEngineKernel _gameKernel;
    protected readonly IEngineKernel _renderKernel;
    
    protected abstract RuntimeThreads ActiveThreads { get; }

    protected Thread _gameThread = null!;
    protected Thread _renderThread = null!;
    
    private volatile ExitCode _mainCode = ExitCode.Ok;
    private volatile ExitCode _gameCode = ExitCode.Ok;
    private volatile ExitCode _renderCode = ExitCode.Ok;
    
    protected virtual void SetupMainModules(IEngineKernel kernel, IRegistry registry) { }
    protected virtual void SetupGameModules(IEngineKernel kernel, IRegistry registry) { }
    protected virtual void SetupRenderModules(IEngineKernel kernel, IRegistry registry) { }
    
    public virtual void Dispose() => GC.SuppressFinalize(this);
    protected virtual T ParseArgs<T>(string[]? args) where T : notnull => default!;

    protected Runtime()
    {
        var frameRing = new SPSCRing<FrameStart>(_framePipeCapacity);
        var framePort = new PipePort<FrameStart>(frameRing);

        var inputRing = new SPSCRing<InputSnapshot>(_inputPipeCapacity);
        var inputPort = new PipePort<InputSnapshot>(inputRing);

        var viewSnap  = new Snapshot<SceneView>();
        var viewPort  = new SnapshotPort<SceneView>(viewSnap);

        var ctrlBus   = new ControlQueue<RenderControl>(rc => (int)rc.Type, maxItems: 64);
        var ctrlPort  = new RenderCommandBusPort(ctrlBus);
        
        var channels = new EngineChannels
        {
            Frame = new FrameChannel { Reader = framePort, Writer = framePort },
            Input = new InputChannel { Reader = inputPort, Writer = inputPort },
            Scene = new SceneChannel { Reader = viewPort,  Writer = viewPort  },
            RenderCommands = ctrlPort
        };
        
        _registry = IRegistry.Create();
        
        _mainRegistry = _registry.Local;
        _gameRegistry = _registry.Local;
        _renderRegistry = _registry.Local;
        
        _registry.Add(channels);
        _mainRegistry.Add<IMainChannels>(new MainChannelsView(channels));
        _gameRegistry.Add<IGameChannels>(new GameChannelsView(channels));
        _renderRegistry.Add<IRenderChannels>(new RenderChannelsView(channels));
        
        _mainKernel = IEngineKernel.Create<IMainKernel>(_mainRegistry.ReadOnly, _cts.Token);
        _gameKernel = IEngineKernel.Create<IGameKernel>(_gameRegistry.ReadOnly, _cts.Token);
        _renderKernel = IEngineKernel.Create<IRenderKernel>(_renderRegistry.ReadOnly, _cts.Token);
    }
    
    public virtual ExitCode Run()
    {
        SetupMainModules(_mainKernel, _mainRegistry);
        SetupGameModules(_gameKernel, _gameRegistry);
        SetupRenderModules(_renderKernel, _renderRegistry);
        
        if (ActiveThreads.HasFlag(RuntimeThreads.Render))
        {
            _renderThread = new Thread(() =>
            {
                _renderCode = _renderKernel.Run();
                if (_renderCode != ExitCode.Ok && _renderCode != ExitCode.Canceled)
                    _cts.Cancel();
            })
            {
                Name = "Render",
                IsBackground = true,
                Priority = ThreadPriority.AboveNormal
            };
            
            _renderThread.Start();
        }
        
        if (ActiveThreads.HasFlag(RuntimeThreads.Game))
        {
            _gameThread = new Thread(() =>
            {
                _gameCode = _gameKernel.Run();
                if (_gameCode != ExitCode.Ok && _gameCode != ExitCode.Canceled)
                    _cts.Cancel();
            })
            {
                Name = "Game", 
                IsBackground = true
            };
            
            _gameThread.Start();
        }

        if (ActiveThreads.HasFlag(RuntimeThreads.Main))
        {
            _mainCode = _mainKernel.Run();
            if (_mainCode != ExitCode.Ok && _mainCode != ExitCode.Canceled)
                _cts.Cancel();
        }
        
        _cts.Cancel();
        try {_renderThread?.Join();} catch { /* ignore */ }
        try {_gameThread?.Join();} catch { /* ignore */ }

        ExitCode final = default;
        foreach (var code in new[] { _mainCode, _gameCode, _renderCode })
            if (code > final) 
                final = code;
        
        return final;
    }
}
