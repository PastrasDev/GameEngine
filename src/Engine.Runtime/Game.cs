using System;
using Engine.Core;

namespace Engine.Runtime;

internal sealed class Game : Runtime
{
    private readonly GameRole _role;

    internal Game(string[]? args) => _role = ParseArgs<GameRole>(args);
    
    protected override RuntimeThreads ActiveThreads
    {
        get
        {
            return _role switch
            {
                GameRole.Server => RuntimeThreads.Game, 
                GameRole.Client => RuntimeThreads.Main | RuntimeThreads.Game | RuntimeThreads.Render, 
                GameRole.Both => RuntimeThreads.Main | RuntimeThreads.Game | RuntimeThreads.Render, 
                _ => RuntimeThreads.None
            };
        }
    }
    
    // For a dedicated server you likely won't add MAIN modules.
    protected override void SetupMainModules(IEngineKernel kernel, IRegistry registry)
    {
        // kernel.Add(new WindowModule()); // client window on MAIN
    }

    protected override void SetupGameModules(IEngineKernel kernel, IRegistry registry)
    {
        // kernel.Add(new SimulationModule());
        // kernel.Add(new NetworkingModule(_role));
    }

    protected override void SetupRenderModules(IEngineKernel kernel, IRegistry registry)
    {
        // kernel.Add(new RendererModule());
        // kernel.Add(new PostFXModule());
    }
    
    protected override T ParseArgs<T>(string[]? args)
    {
        if (typeof(T) != typeof(GameRole))
            throw new InvalidOperationException($"Game.ParseArgs can only return {nameof(GameRole)}");

        if (args != null)
        {
            foreach (var arg in args)
            {
                switch (arg.TrimStart('-').ToLowerInvariant())
                {
                    case "client": return (T)(object)GameRole.Client;
                    case "server" or "dedicated": return (T)(object)GameRole.Server;
                    case "both" or "host": return (T)(object)GameRole.Both;
                }
            }
        }
        
        return (T)(object)GameRole.Client;
    }
}