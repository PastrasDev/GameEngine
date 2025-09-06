using System;
using System.Collections.Generic;
using System.Threading;
using Engine.Core;

namespace Engine.Runtime;

public sealed class Runtime(string[]? args)
{
    private readonly CancellationTokenSource _cts = new();
    private readonly Dictionary<Threads, EngineKernel> _kernels = new();
    private readonly Threads _activeThreads = GetActiveThreads(args);
    
    public static int Launch(string[]? args)
    {
        var runtime = new Runtime(args);
        return (int)runtime.Run();
    }
    
    private ExitCode Run()
    {
        var itc = Channels.CreateDefault(256, 256, PipeFullMode.Wait, WaitStrategy.Hybrid, 64);
        
        var root = Registry.Root;
        root.Add(_cts);
        root.Add(itc.channels);
        root.Add(new PhaseBarrier());
        
        if (_activeThreads.HasFlag(Threads.Game))
        {
            var registry = root.Local;
            registry.Add<IGameChannels>(itc.gameView);
            registry.Add(CancellationTokenSource.CreateLinkedTokenSource(_cts.Token));
            
            _kernels[Threads.Game] = new GameKernel
            {
                Affinity = Threads.Game,
                Context = new EngineContext(registry),
                Enabled = true
            };
        }
        
        if (_activeThreads.HasFlag(Threads.Render))
        {
            var registry = root.Local;
            registry.Add<IRenderChannels>(itc.renderView);
            registry.Add(CancellationTokenSource.CreateLinkedTokenSource(_cts.Token));
            
            _kernels[Threads.Render] = new RenderKernel
            {
                Affinity = Threads.Render,
                Context = new EngineContext(registry),
                Enabled = true
            };
        }
        
        if (_activeThreads.HasFlag(Threads.Main))
        {
            var registry = root.Local;
            registry.Add<IMainChannels>(itc.mainView);
            registry.Add(CancellationTokenSource.CreateLinkedTokenSource(_cts.Token));
            
            _kernels[Threads.Main] = new MainKernel
            {
                Affinity = Threads.Main,
                Context = new EngineContext(registry),
                Enabled = true
            };
        }
        
        _cts.Cancel();
        
        ExitCode final = default;
        foreach (var kernel in _kernels.Values)
        {
            try { kernel.Thread?.Join(); } catch { /* ignore */ }
            kernel.Context.TokenSrc.Dispose();
            var code = kernel.Code;
            if (code > final) final = code;
        }
        
        _cts.Dispose();
        return final;
    }
    
    private static Threads GetActiveThreads(string[]? args)
    {
        return ParseArgs<GameRole>(args) switch
        {
            GameRole.Server => Threads.Game, 
            GameRole.Client or GameRole.Both => Threads.Main | Threads.Game | Threads.Render, 
            _ => Threads.None
        };
    }
    
    private static T ParseArgs<T>(string[]? args) where T : notnull
    {
        if (typeof(T) != typeof(GameRole))
            throw new InvalidOperationException($"Game.ParseArgs can only return {nameof(GameRole)}");

        if (args == null) 
            return (T)(object)GameRole.Client;
        
        foreach (var arg in args)
        {
            switch (arg.TrimStart('-').ToLowerInvariant())
            {
                case "client": return (T)(object)GameRole.Client;
                case "server" or "dedicated": return (T)(object)GameRole.Server;
                case "both" or "host": return (T)(object)GameRole.Both;
            }
        }

        return (T)(object)GameRole.Client;
    }
}
