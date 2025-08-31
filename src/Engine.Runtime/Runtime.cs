using System;
using System.Diagnostics;
using System.Threading;
using Engine.Core;

namespace Engine.Runtime;

public abstract class Runtime : IDisposable
{
    protected virtual float FixedDt => 1f / 120f;
    protected virtual float MaxFrameDt => 0.25f;
    protected virtual int   MaxFixedStepsPerFrame => 8;

    /// IPC capacities (power-of-two recommended for the ring)
    protected virtual int FramePipeCapacity => 256;
    protected virtual int InputPipeCapacity => 256;
    
    protected CancellationTokenSource Cts { get; } = new();
    
    protected static IRegistry Registry { get; } = IRegistry.Create();
    
    protected IEngineKernel MainKernel { get; } = IEngineKernel.Create(Registry);
    protected IEngineKernel GameKernel { get; } = IEngineKernel.Create(Registry);
    protected IEngineKernel RenderKernel { get; } = IEngineKernel.Create(Registry);
    
    protected abstract RuntimeThreads ActiveThreads { get; }
    
    /// Module setup hooks
    protected virtual void SetupMainModules(IEngineKernel kernel, IRegistry registry) { }
    protected virtual void SetupGameModules(IEngineKernel kernel, IRegistry registry) { }
    protected virtual void SetupRenderModules(IEngineKernel kernel, IRegistry registry) { }
    
    public virtual void Dispose() => GC.SuppressFinalize(this);
    protected virtual T ParseArgs<T>(string[]? args) where T : notnull => default!;

    public virtual void Run()
    {
        var frameRing = new SPSCRing<FrameStart>(FramePipeCapacity);
        var framePort = new PipePort<FrameStart>(frameRing);

        var inputRing = new SPSCRing<InputSnapshot>(InputPipeCapacity);
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
        
        Registry.Add(channels);
        Registry.Add<IMainChannels>(new MainChannelsView(channels));
        Registry.Add<IGameChannels>(new GameChannelsView(channels));
        Registry.Add<IRenderChannels>(new RenderChannelsView(channels));
        Registry.Add(IRenderProxyRegistry.Create());
        
        SetupMainModules(MainKernel, Registry);
        SetupGameModules(GameKernel, Registry);
        SetupRenderModules(RenderKernel, Registry);
        
        var ct = Cts.Token;
        
        Thread? renderThread = null;
        Thread? gameThread   = null;
        
        if (ActiveThreads.HasFlag(RuntimeThreads.Render))
        {
            renderThread = new Thread(() => RenderLoop(RenderKernel, ct))
            {
                Name = "Render", 
                IsBackground = true
            };
            renderThread.Start();
        }

        if (ActiveThreads.HasFlag(RuntimeThreads.Game))
        {
            gameThread = new Thread(() => GameLoop(GameKernel, FixedDt, MaxFrameDt, MaxFixedStepsPerFrame, ct))
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
                MainKernel.Initialize();
                MainKernel.Start();

                var sw = Stopwatch.StartNew();
                long frameId = 0;
                double last  = 0;

                var channel = Registry.Get<IMainChannels>();
                
                while (!ct.IsCancellationRequested)
                {
                    MainKernel.Update();
                    MainKernel.LateUpdate();

                    var now = sw.Elapsed.TotalSeconds;
                    var raw = (float)(now - last);
                    last = now;
                    var dt = clamp(raw, 0f, MaxFrameDt);
                    
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

                try { RenderKernel.Shutdown(); } catch { /* ignore */ }
                try { GameKernel.Shutdown();     } catch { /* ignore */ }
                try { MainKernel.Shutdown();     } catch { /* ignore */ }
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
            
            var context = kernel.Context;
            var channel = context.Registry.Get<IGameChannels>();
            
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
            
            var channel = kernel.Context.Registry.Get<IRenderChannels>();

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
    
    public static Runtime Create(RuntimeMode mode, string[]? args)
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