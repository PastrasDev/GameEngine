using System;
using System.Diagnostics;
using System.Threading;
using Engine.Core;

namespace Engine.Runtime;

public interface IRuntime : IDisposable
{
    void Run();
    
    static IRuntime Create(RuntimeMode mode, string[]? args)
    {
        return mode switch
        {
            RuntimeMode.Editor => new Editor(args),
            RuntimeMode.Game => new Game(args),
            RuntimeMode.None => throw new ArgumentException("Runtime mode not specified", nameof(mode)),
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };
    }
}

internal abstract class Runtime : IRuntime
{
    protected const float _fixedDt = 1f / 120f;
    protected const float _maxFrameDt = 0.25f;
    protected const int _maxFixedStepsPerFrame = 8;

    /// IPC capacities (power-of-two recommended for the ring)
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
    
    /// Module setup hooks
    protected virtual void SetupMainModules(IEngineKernel kernel, IRegistry registry) { }
    protected virtual void SetupGameModules(IEngineKernel kernel, IRegistry registry) { }
    protected virtual void SetupRenderModules(IEngineKernel kernel, IRegistry registry) { }
    
    public virtual void Dispose() => GC.SuppressFinalize(this);
    protected virtual T ParseArgs<T>(string[]? args) where T : notnull => default!;

    protected Runtime()
    {
        _registry = IRegistry.Create();
        
        _mainRegistry = _registry.Local;
        _gameRegistry = _registry.Local;
        _renderRegistry = _registry.Local;
        
        _mainKernel = IEngineKernel.Create(_mainRegistry.ReadOnly);
        _gameKernel = IEngineKernel.Create(_gameRegistry.ReadOnly);
        _renderKernel = IEngineKernel.Create(_renderRegistry.ReadOnly);
    }
    
    public virtual void Run()
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
        
        _registry.Add(channels);
        _mainRegistry.Add<IMainChannels>(new MainChannelsView(channels));
        _gameRegistry.Add<IGameChannels>(new GameChannelsView(channels));
        _renderRegistry.Add<IRenderChannels>(new RenderChannelsView(channels));
        
        SetupMainModules(_mainKernel, _mainRegistry);
        SetupGameModules(_gameKernel, _gameRegistry);
        SetupRenderModules(_renderKernel, _renderRegistry);
        
        var ct = _cts.Token;
        
        Thread? renderThread = null;
        Thread? gameThread   = null;
        
        if (ActiveThreads.HasFlag(RuntimeThreads.Render))
        {
            renderThread = new Thread(() => RenderLoop(_renderKernel, ct))
            {
                Name = "Render", 
                IsBackground = true
            };
            renderThread.Start();
        }

        if (ActiveThreads.HasFlag(RuntimeThreads.Game))
        {
            gameThread = new Thread(() => GameLoop(_gameKernel, _fixedDt, _maxFrameDt, _maxFixedStepsPerFrame, ct))
            {
                Name = "Game", 
                IsBackground = true
            };
            gameThread.Start();
        }

        if (ActiveThreads.HasFlag(RuntimeThreads.Main))
        {
            try
            {
                _mainKernel.Initialize();
                _mainKernel.Start();

                var sw = Stopwatch.StartNew();
                long frameId = 0;
                double last  = 0;

                var channel = require(_mainKernel.Context.Registry.Get<IMainChannels>());
                
                while (!ct.IsCancellationRequested)
                {
                    _mainKernel.Update();
                    _mainKernel.LateUpdate();

                    var now = sw.Elapsed.TotalSeconds;
                    var raw = (float)(now - last);
                    last = now;
                    var dt = clamp(raw, 0f, _maxFrameDt);
                    
                    channel.FrameWriter.Write(new FrameStart(frameId, dt), ct);
                    frameId++;
                }
            }
            catch (OperationCanceledException) { /* normal shutdown */ }
            finally
            {
                try { frameRing.Complete(); } catch { /* ignore */ }
                
                try { gameThread?.Join();   } catch { /* ignore */ }
                try { renderThread?.Join(); } catch { /* ignore */ }

                try { _renderKernel.Shutdown(); } catch { /* ignore */ }
                try { _gameKernel.Shutdown();     } catch { /* ignore */ }
                try { _mainKernel.Shutdown();     } catch { /* ignore */ }
            }
        }
    }
    
    private static void GameLoop(IEngineKernel kernel, float fixedDt, float maxFrameDt, int maxStepsPerFrame, CancellationToken ct)
    {
        try
        {
            kernel.Initialize();
            kernel.Start();

            float accumulator = 0f;
            int simFrame = 0;
            
            var context = require(kernel.Context);
            var channel = require(context.Registry.Get<IGameChannels>());
            
            while (!ct.IsCancellationRequested)
            {
                var start = channel.FrameReader.Read(ct);
                
                var dt = clamp(start.Dt, 0f, maxFrameDt);
                accumulator += dt;

                // Fixed-step simulation
                int steps = 0;
                while (accumulator >= fixedDt && steps < maxStepsPerFrame)
                {
                    context.DeltaTime = fixedDt;
                    kernel.FixedUpdate();
                    accumulator -= fixedDt;
                    simFrame++;
                    steps++;
                }

                // Variable updates
                context.DeltaTime = dt;
                kernel.Update();
                kernel.LateUpdate();

                // Interpolation factor for renderer
                channel.SceneWriter.Publish(new SceneView(simFrame, clamp(accumulator / fixedDt, 0f, 1f)));
            }
        }
        catch (OperationCanceledException) { /* normal shutdown */ }
        finally
        {
            try { kernel.Shutdown(); } catch { /* ignore */ }
        }
    }

    private static void RenderLoop(IEngineKernel kernel, CancellationToken ct)
    {
        try
        {
            kernel.Initialize();
            kernel.Start();
            
            var channel = require(kernel.Context.Registry.Get<IRenderChannels>());

            while (!ct.IsCancellationRequested)
            {
                // Read freshest view (no queue: latest-wins snapshot)
                channel.SceneReader.Read();
                
                // context.Channels.RenderCommands.Drain(rc => ApplyRenderControl(ref context, rc));
                
                kernel.Update();
                kernel.LateUpdate();
            }
        }
        catch (OperationCanceledException) { /* normal shutdown */ }
        finally
        {
            try { kernel.Shutdown(); } catch { /* ignore */ }
        }
    }
}

