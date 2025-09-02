using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Engine.Core;

public interface IEngineKernel
{
    IEngineContext Context { get; }
    
    ExitCode Run();
    void Add(EngineModule module);
    void Initialize();
    void Start();
    void FixedUpdate();
    void Update();
    void LateUpdate();
    void Shutdown();

    static IEngineKernel Create<T>(IRegistry registry, CancellationToken ct) where T : IEngineKernel
    {
        if (typeof(T) == typeof(IMainKernel))
            return new MainKernel(registry, ct);
        if (typeof(T) == typeof(IGameKernel))
            return new GameKernel(registry, ct);
        if (typeof(T) == typeof(IRenderKernel))
            return new RenderKernel(registry, ct);
        throw new ArgumentException($"Unknown kernel type: {typeof(T)}");
    }
}

public interface IMainKernel : IEngineKernel;
public interface IGameKernel : IEngineKernel;
public interface IRenderKernel : IEngineKernel;

internal abstract class EngineKernel(IRegistry registry) : IEngineKernel
{
    protected const float _fixedDt = 1f / 120f;
    protected const float _maxFrameDt = 0.25f;
    protected const int _maxFixedStepsPerFrame = 8;
    
    private readonly List<Slot> _init = [];
    private readonly List<Slot> _start = [];
    private readonly List<Slot> _fixed = [];
    private readonly List<Slot> _update = [];
    private readonly List<Slot> _late = [];
    private readonly List<Slot> _shutdown = [];
    
    public IEngineContext Context { get; } = IEngineContext.Create(registry);
    
    private readonly HashSet<EngineModule> _allModules = [];
    
    private static readonly ConcurrentDictionary<Type, PhaseMask> s_masks = new();
    
    private readonly unsafe struct Slot(EngineModule module, delegate* managed<EngineModule, void> fn)
    {
        public readonly EngineModule Module = module;
        public readonly delegate* managed<EngineModule, void> Fn = fn;
    }
    
    private static class ModuleDispatch<T> where T : EngineModule
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static void Initialize(EngineModule m) => ((T)m).Initialize();
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static void Start(EngineModule m) => ((T)m).Start();
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static void FixedUpdate(EngineModule m) => ((T)m).FixedUpdate();
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static void Update(EngineModule m) => ((T)m).Update();
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static void LateUpdate(EngineModule m) => ((T)m).LateUpdate();
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static void Shutdown(EngineModule m) => ((T)m).Shutdown();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public abstract ExitCode Run();
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add(EngineModule module) => AddTyped((dynamic)module);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private unsafe void AddTyped<T>(T m) where T : EngineModule
    {
        var mask = GetMask<T>();
        _allModules.Add(m);

        if ((mask & PhaseMask.Init) != 0) { delegate* managed<EngineModule, void> p = &ModuleDispatch<T>.Initialize; _init.Add(new Slot(m, p)); }
        if ((mask & PhaseMask.Start) != 0) { delegate* managed<EngineModule, void> p = &ModuleDispatch<T>.Start; _start.Add(new Slot(m, p)); }
        if ((mask & PhaseMask.Fixed) != 0) { delegate* managed<EngineModule, void> p = &ModuleDispatch<T>.FixedUpdate; _fixed.Add(new Slot(m, p)); }
        if ((mask & PhaseMask.Update) != 0) { delegate* managed<EngineModule, void> p = &ModuleDispatch<T>.Update; _update.Add(new Slot(m, p)); }
        if ((mask & PhaseMask.Late) != 0) { delegate* managed<EngineModule, void> p = &ModuleDispatch<T>.LateUpdate; _late.Add(new Slot(m, p)); }
        if ((mask & PhaseMask.Shutdown) != 0) { delegate* managed<EngineModule, void> p = &ModuleDispatch<T>.Shutdown; _shutdown.Add(new Slot(m, p)); }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PhaseMask GetMask<T>() where T : EngineModule => s_masks.GetOrAdd(typeof(T), static _ => ComputeMask<T>());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static PhaseMask ComputeMask<T>() where T : EngineModule
    {
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;

        PhaseMask mask = PhaseMask.None;
        if (Overrides(nameof(EngineModule.Initialize))) mask |= PhaseMask.Init;
        if (Overrides(nameof(EngineModule.Start))) mask |= PhaseMask.Start;
        if (Overrides(nameof(EngineModule.FixedUpdate))) mask |= PhaseMask.Fixed;
        if (Overrides(nameof(EngineModule.Update))) mask |= PhaseMask.Update;
        if (Overrides(nameof(EngineModule.LateUpdate))) mask |= PhaseMask.Late;
        if (Overrides(nameof(EngineModule.Shutdown))) mask |= PhaseMask.Shutdown;
        return mask;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool Overrides(string name)
        {
            var mi = typeof(T).GetMethod(name, flags, null, Type.EmptyTypes, modifiers: null);
            return mi is not null && mi.DeclaringType != typeof(EngineModule);
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe void Initialize()
    {
        foreach (var m in _allModules) 
            m.Context = Context;
        
        for (int i = 0; i < _init.Count; i++)
            _init[i].Fn(_init[i].Module);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe void Start()
    {
        for (int i = 0; i < _start.Count; i++) 
            _start[i].Fn(_start[i].Module);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe void FixedUpdate()
    {
        for (int i = 0; i < _fixed.Count; i++) 
            _fixed[i].Fn(_fixed[i].Module);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe void Update()
    {
        for (int i = 0; i < _update.Count; i++) 
            _update[i].Fn(_update[i].Module);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe void LateUpdate()
    {
        for (int i = 0; i < _late.Count; i++) 
            _late[i].Fn(_late[i].Module);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe void Shutdown()
    {
        for (int i = _shutdown.Count - 1; i >= 0; i--) 
            _shutdown[i].Fn(_shutdown[i].Module);
    }
}

internal sealed class MainKernel(IRegistry registry, CancellationToken ct) : EngineKernel(registry), IMainKernel
{
    
    public override ExitCode Run()
    {
        try
        {
            Initialize();
            Start();

            var sw = Stopwatch.StartNew();
            long frameId = 0;
            double last = 0;

            var channel = require(Context.Registry.Get<IMainChannels>());

            while (!ct.IsCancellationRequested)
            {
                Update();
                LateUpdate();

                var now = sw.Elapsed.TotalSeconds;
                var raw = (float)(now - last);
                last = now;
                var dt = clamp(raw, 0f, _maxFrameDt);

                channel.FrameWriter.Write(new FrameStart(frameId, dt), ct);
                frameId++;
            }

            return ExitCode.Canceled;
        }
        catch (OperationCanceledException)
        {
            return ExitCode.Canceled;
        }
        catch (Exception)
        {
            // TODO: log
            return ExitCode.Fatal;
        }
        finally
        {
            try { Shutdown(); } catch { /* ignore */ }
        }
    }
}

internal sealed class GameKernel(IRegistry registry, CancellationToken ct) : EngineKernel(registry), IGameKernel
{
    
    public override ExitCode Run()
    {
        try
        {
            Initialize();
            Start();
        
            float accumulator = 0f;
            int simFrame = 0;
        
            var channel = require(Context.Registry.Get<IGameChannels>());
        
            while (!ct.IsCancellationRequested)
            {
                var start = channel.FrameReader.Read(ct);
            
                var dt = clamp(start.Dt, 0f, _maxFrameDt);
                accumulator += dt;
            
                int steps = 0;
                while (accumulator >= _fixedDt && steps < _maxFixedStepsPerFrame)
                {
                    Context.DeltaTime = _fixedDt;
                    FixedUpdate();
                    accumulator -= _fixedDt;
                    simFrame++;
                    steps++;
                }
            
                Context.DeltaTime = dt;
                Update();
                LateUpdate();
            
                channel.SceneWriter.Publish(new SceneView(simFrame, clamp(accumulator / _fixedDt, 0f, 1f)));
            }
            
            return ExitCode.Canceled;
        }
        catch (OperationCanceledException)
        {
            return ExitCode.Canceled;
        }
        catch (Exception)
        {
            // TODO: log
            return ExitCode.Fatal;
        }
        finally
        {
            try { Shutdown(); } catch { /* ignore */ }
        }
    }
}

internal sealed class RenderKernel(IRegistry registry, CancellationToken ct) : EngineKernel(registry), IRenderKernel
{
    
    public override ExitCode Run()
    {
        try
        {
            Initialize();
            Start();
        
            var channel = require(Context.Registry.Get<IRenderChannels>());
            
            while (!ct.IsCancellationRequested)
            {
                channel.SceneReader.Read();

                Update();
                LateUpdate();
            }
            
            return ExitCode.Canceled;
        }
        catch (OperationCanceledException)
        {
            return ExitCode.Canceled;
        }
        catch (Exception)
        {
            // TODO: log
            return ExitCode.Fatal;
        }
        finally
        {
            try { Shutdown(); } catch { /* ignore */ }
        }
    }
}