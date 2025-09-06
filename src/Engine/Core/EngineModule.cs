using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Engine.Core;

internal readonly unsafe struct PhaseFn(Phase phase, delegate* managed<object, void> fn)
{
    public readonly Phase Phase = phase;
    public readonly delegate* managed<object, void> Fn = fn;
}

internal readonly record struct ModuleDescriptor
{
    public required Type Type { get; init; }
    public required Threads Affinity { get; init; }
    public required Type[]? Dependencies { get; init; }
    public required PhaseFn[] Overrides { get; init; }
}

public abstract class EngineModule<TSelf> where TSelf : EngineModule<TSelf>
{
    internal static readonly ModuleDescriptor Descriptor = new()
    {
        Type = typeof(TSelf),
        Affinity = typeof(TSelf).GetCustomAttribute<AffinityAttribute>()?.Threads ?? Threads.None,
        Dependencies = typeof(TSelf).GetCustomAttribute<RequiresAttribute>()?.Types ?? [],
        Overrides = BuildPhaseFns()
    };
    
    /// <summary>Per-thread context; set by the kernel before any lifecycle call.</summary>
    public EngineContext Context { get; internal set; } = null!;
    
    /// Determines whether this modules lifecycle methods are called beyond Load.
    public bool Enabled { get; set; } = true;

    public virtual void Load() { }
    public virtual void Initialize() { }
    public virtual void Start() { }
    public virtual void FixedUpdate() { }
    public virtual void Update() { }
    public virtual void LateUpdate() { }
    public virtual void Shutdown() { }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)] private static void __Load(object m) => Unsafe.As<TSelf>(m).Load();
    [MethodImpl(MethodImplOptions.AggressiveInlining)] private static void __Init(object m) => Unsafe.As<TSelf>(m).Initialize();
    [MethodImpl(MethodImplOptions.AggressiveInlining)] private static void __Start(object m) => Unsafe.As<TSelf>(m).Start();
    [MethodImpl(MethodImplOptions.AggressiveInlining)] private static void __Fixed(object m) => Unsafe.As<TSelf>(m).FixedUpdate();
    [MethodImpl(MethodImplOptions.AggressiveInlining)] private static void __Update(object m) => Unsafe.As<TSelf>(m).Update();
    [MethodImpl(MethodImplOptions.AggressiveInlining)] private static void __Late(object m) => Unsafe.As<TSelf>(m).LateUpdate();
    [MethodImpl(MethodImplOptions.AggressiveInlining)] private static void __Shutdown(object m) => Unsafe.As<TSelf>(m).Shutdown();
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe PhaseFn[] BuildPhaseFns()
    {
        var m = ComputePhases();
        int n = 0;
        Span<PhaseFn> tmp = stackalloc PhaseFn[7];
        if ((m & Phase.Load) != 0) tmp[n++] = new PhaseFn(Phase.Load, &__Load);
        if ((m & Phase.Init) != 0) tmp[n++] = new PhaseFn(Phase.Init, &__Init);
        if ((m & Phase.Start) != 0) tmp[n++] = new PhaseFn(Phase.Start, &__Start);
        if ((m & Phase.Fixed) != 0) tmp[n++] = new PhaseFn(Phase.Fixed, &__Fixed);
        if ((m & Phase.Update) != 0) tmp[n++] = new PhaseFn(Phase.Update, &__Update);
        if ((m & Phase.Late) != 0) tmp[n++] = new PhaseFn(Phase.Late, &__Late);
        if ((m & Phase.Shutdown) != 0) tmp[n++] = new PhaseFn(Phase.Shutdown, &__Shutdown);
        return tmp[..n].ToArray();
    }
    
    /// <summary>
    /// Determine which lifecycle methods are overridden by TSelf.
    /// We no longer compare against a non-generic base; instead we ask:
    /// “Is the most-derived method declared on TSelf?”
    /// If yes → it’s overridden.
    /// </summary>
    private static Phase ComputePhases()
    {
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;
        var t = typeof(TSelf);
        Phase mask = Phase.None;
        if (IsOverridden(t.GetMethod(nameof(Load), flags), t)) mask |= Phase.Load;
        if (IsOverridden(t.GetMethod(nameof(Initialize), flags), t)) mask |= Phase.Init;
        if (IsOverridden(t.GetMethod(nameof(Start), flags), t)) mask |= Phase.Start;
        if (IsOverridden(t.GetMethod(nameof(FixedUpdate), flags), t)) mask |= Phase.Fixed;
        if (IsOverridden(t.GetMethod(nameof(Update), flags), t)) mask |= Phase.Update;
        if (IsOverridden(t.GetMethod(nameof(LateUpdate), flags), t)) mask |= Phase.Late;
        if (IsOverridden(t.GetMethod(nameof(Shutdown), flags), t)) mask |= Phase.Shutdown;
        return mask;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool IsOverridden(MethodInfo? mi, Type self) => mi is not null && mi.DeclaringType == self;
    }
}