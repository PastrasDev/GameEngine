using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Engine.Abstractions;

namespace Engine.Runtime;

public sealed class EngineKernel : IEngineKernel
{
    private readonly List<Slot> _init = [];
    private readonly List<Slot> _start = [];
    private readonly List<Slot> _fixed = [];
    private readonly List<Slot> _update = [];
    private readonly List<Slot> _late = [];
    private readonly List<Slot> _shutdown = [];
    
    private readonly HashSet<EngineModule> _allModules = [];
    
    [Flags] 
    private enum PhaseMask { None=0, Init=1, Start=2, Fixed=4, Update=8, Late=16, Shutdown=32 }

    private static readonly ConcurrentDictionary<Type, PhaseMask> s_masks = new();

    [method: MethodImpl(MethodImplOptions.AggressiveInlining)]
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
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public unsafe void Initialize(IEngineContext context) { foreach (var m in _allModules) m.BindContext(context); for (int i = 0; i < _init.Count; i++) _init[i].Fn(_init[i].Module); }
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public unsafe void Start(IEngineContext context) { for (int i = 0; i < _start.Count; i++) _start[i].Fn(_start[i].Module); }
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public unsafe void FixedUpdate(IEngineContext context) { for (int i = 0; i < _fixed.Count; i++) _fixed[i].Fn(_fixed[i].Module); }
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public unsafe void Update(IEngineContext context) { for (int i = 0; i < _update.Count; i++) _update[i].Fn(_update[i].Module); }
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public unsafe void LateUpdate(IEngineContext context) { for (int i = 0; i < _late.Count; i++) _late[i].Fn(_late[i].Module); }
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public unsafe void Shutdown(IEngineContext context) { for (int i = _shutdown.Count - 1; i >= 0; i--) _shutdown[i].Fn(_shutdown[i].Module); }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)] 
    public void Add(EngineModule module) => AddTyped((dynamic)module);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private unsafe void AddTyped<T>(T m) where T : EngineModule
    {
        var mask = GetMask<T>();
        _allModules.Add(m);

        if ((mask & PhaseMask.Init) != 0)   { delegate* managed<EngineModule, void> p = &ModuleDispatch<T>.Initialize; _init.Add(new Slot(m, p)); }
        if ((mask & PhaseMask.Start) != 0)  { delegate* managed<EngineModule, void> p = &ModuleDispatch<T>.Start; _start.Add(new Slot(m, p)); }
        if ((mask & PhaseMask.Fixed) != 0)  { delegate* managed<EngineModule, void> p = &ModuleDispatch<T>.FixedUpdate; _fixed.Add(new Slot(m, p)); }
        if ((mask & PhaseMask.Update) != 0) { delegate* managed<EngineModule, void> p = &ModuleDispatch<T>.Update; _update.Add(new Slot(m, p)); }
        if ((mask & PhaseMask.Late) != 0)   { delegate* managed<EngineModule, void> p = &ModuleDispatch<T>.LateUpdate; _late.Add(new Slot(m, p)); }
        if ((mask & PhaseMask.Shutdown) != 0){delegate* managed<EngineModule, void> p = &ModuleDispatch<T>.Shutdown; _shutdown.Add(new Slot(m, p)); }
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
            var mi = typeof(T).GetMethod(name, flags, binder: null, [typeof(IEngineContext)], modifiers: null);
            return mi is not null && mi.DeclaringType != typeof(EngineModule);
        }
    }
}