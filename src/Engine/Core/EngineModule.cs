using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Engine.Core;

public interface IEngineModule
{
	ModuleDescriptor Descriptor { get; }
	EngineContext Context { get; set; }
	bool Enabled { get; set; }

	void Load();
	void Initialize();
	void Start();
	void FixedUpdate();
	void Update();
	void LateUpdate();
	void Shutdown();
}

public abstract class EngineModule<TSelf> : IEngineModule where TSelf : EngineModule<TSelf>
{
	public virtual void Load() { }
	public virtual void Initialize() { }
	public virtual void Start() { }
	public virtual void FixedUpdate() { }
	public virtual void Update() { }
	public virtual void LateUpdate() { }
	public virtual void Shutdown() { }

	public EngineContext Context { get; set; } = null!;
	public bool Enabled { get; set; } = true;

	private static readonly ModuleDescriptor Descriptor;
	ModuleDescriptor IEngineModule.Descriptor => Descriptor;

	static unsafe EngineModule()
	{
		var type = typeof(TSelf);
		var mask = ComputeMethodMask(type);

		var methodSet = new MethodSet
		{
			Load = &__Load,
			Initialize = &__Init,
			Start = &__Start,
			FixedUpdate = &__Fixed,
			Update = &__Update,
			LateUpdate = &__Late,
			Shutdown = &__Shutdown
		};

		Descriptor = new()
		{
			Type = type,
			Affinity = type.GetCustomAttribute<AffinityAttribute>()?.Threads ?? Threads.None,
			Dependencies = type.GetCustomAttribute<RequiresAttribute>()?.Types ?? [],
			Table = new(mask, methodSet),
			Mask = mask,
			EnabledPtr = (nint)(delegate* managed<object, bool>)&__Enabled
		};
	}

	private static bool __Enabled(object o) => Unsafe.As<TSelf>(o).Enabled;
	private static void __Load(object m) => Unsafe.As<TSelf>(m).Load();
	private static void __Init(object m) => Unsafe.As<TSelf>(m).Initialize();
	private static void __Start(object m) => Unsafe.As<TSelf>(m).Start();
	private static void __Fixed(object m) => Unsafe.As<TSelf>(m).FixedUpdate();
	private static void __Update(object m) => Unsafe.As<TSelf>(m).Update();
	private static void __Late(object m) => Unsafe.As<TSelf>(m).LateUpdate();
	private static void __Shutdown(object m) => Unsafe.As<TSelf>(m).Shutdown();

	private static MethodMask ComputeMethodMask(Type type)
	{
		const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly;
		MethodMask mask = default;
		SetMaskBit(type, ref mask, nameof(IEngineModule.Load), MethodMask.Load);
		SetMaskBit(type, ref mask, nameof(IEngineModule.Initialize), MethodMask.Init);
		SetMaskBit(type, ref mask, nameof(IEngineModule.Start), MethodMask.Start);
		SetMaskBit(type, ref mask, nameof(IEngineModule.FixedUpdate), MethodMask.Fixed);
		SetMaskBit(type, ref mask, nameof(IEngineModule.Update), MethodMask.Update);
		SetMaskBit(type, ref mask, nameof(IEngineModule.LateUpdate), MethodMask.Late);
		SetMaskBit(type, ref mask, nameof(IEngineModule.Shutdown), MethodMask.Shutdown);

		return mask;

		static void SetMaskBit(Type t, ref MethodMask m, string methodName, MethodMask bit)
		{
			if (t.GetMethod(methodName, flags) is { }) m |= bit;
		}
	}
}

public readonly record struct ModuleDescriptor
{
	public required Type Type { get; init; }
	public required Threads Affinity { get; init; }
	public required Type[]? Dependencies { get; init; }
	public required MethodTable Table { get; init; }
	public required MethodMask Mask { get; init; }
	public required nint EnabledPtr { get; init; }
}

public readonly unsafe struct MethodSet
{
	public delegate* managed<object, void> Load { get; init; }
	public delegate* managed<object, void> Initialize { get; init; }
	public delegate* managed<object, void> Start { get; init; }
	public delegate* managed<object, void> FixedUpdate { get; init; }
	public delegate* managed<object, void> Update { get; init; }
	public delegate* managed<object, void> LateUpdate { get; init; }
	public delegate* managed<object, void> Shutdown { get; init; }
}

public readonly unsafe struct MethodTable
{
	private readonly delegate* managed<object, void>[] _table;

	public MethodTable(MethodMask mask, MethodSet set)
	{
		_table = new delegate* managed<object, void>[7];
		if ((mask & MethodMask.Load) != 0) _table[0] = set.Load;
		if ((mask & MethodMask.Init) != 0) _table[1] = set.Initialize;
		if ((mask & MethodMask.Start) != 0) _table[2] = set.Start;
		if ((mask & MethodMask.Fixed) != 0) _table[3] = set.FixedUpdate;
		if ((mask & MethodMask.Update) != 0) _table[4] = set.Update;
		if ((mask & MethodMask.Late) != 0) _table[5] = set.LateUpdate;
		if ((mask & MethodMask.Shutdown) != 0) _table[6] = set.Shutdown;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static int Idx(MethodMask s) => s switch
	{
		MethodMask.Load => 0,
		MethodMask.Init => 1,
		MethodMask.Start => 2,
		MethodMask.Fixed => 3,
		MethodMask.Update => 4,
		MethodMask.Late => 5,
		MethodMask.Shutdown => 6,
		_ => -1
	};

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool TryGet(MethodMask methodMask, out delegate* managed<object, void> fn)
	{
		int i = Idx(methodMask);
		if ((uint)i >= 7u)
		{
			fn = default;
			return false;
		}

		fn = _table[i];
		return fn != null;
	}
}