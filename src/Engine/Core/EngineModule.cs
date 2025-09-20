using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Engine.Core;

public abstract class EngineModule
{
	public EngineContext Context { get; set; } = null!;
	public bool Enabled { get; set; } = true;

	public readonly struct Metadata
	{
		public required Type Type { get; init; }
		public required Threads Affinity { get; init; }
		public required Type[]? Dependencies { get; init; }
		public required MethodTable Table { get; init; }
		public required MethodMask Mask { get; init; }
		public required Func<EngineContext, EngineModule> Instantiate { get; init; }
	}
}

public abstract class EngineModule<TSelf> : EngineModule, IMetadata<TSelf, EngineModule.Metadata> where TSelf : EngineModule<TSelf>
{
	private static readonly Func<TSelf> Constructor = Expression.Lambda<Func<TSelf>>(Expression.New(typeof(TSelf).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null) ?? throw new MissingMethodException(typeof(TSelf).FullName, ".ctor()"))).Compile();

	static unsafe EngineModule()
	{
		var type = typeof(TSelf);
		var mask = ComputeMethodMask(type);

		MetadataRegistry.Register<TSelf, Metadata>(new Metadata
		{
			Type = type,
			Affinity = type.GetCustomAttribute<AffinityAttribute>()?.Threads ?? Threads.None,
			Dependencies = DependsOnAttribute<object>.GetTypes(type) ?? [],
			Table = new MethodTable(mask, new MethodSet
			{
				Load = &__Load,
				Initialize = &__Init,
				Start = &__Start,
				FixedUpdate = &__Fixed,
				Update = &__Update,
				LateUpdate = &__Late,
				Shutdown = &__Shutdown
			}),
			Mask = mask,
			Instantiate = static context =>
			{
				var instance = Constructor();
				instance.Context = context;
				context.Registry.Add(instance);
				return instance;
			}
		});
	}

	protected virtual void Load()
	{
		Console.WriteLine($"{Context.Affinity} - {typeof(TSelf).Name}::Load");
	}

	protected virtual void Initialize()
	{
		Console.WriteLine($"{Context.Affinity} - {typeof(TSelf).Name}::Initialize");
	}

	protected virtual void Start()
	{
		Console.WriteLine($"{Context.Affinity} - {typeof(TSelf).Name}::Start");
	}

	protected virtual void FixedUpdate() { }
	protected virtual void Update() { }
	protected virtual void LateUpdate() { }

	protected virtual void Shutdown()
	{
		Console.WriteLine($"{Context.Affinity} - {typeof(TSelf).Name}::Shutdown");
	}

	private static void __Load(EngineModule m) => Unsafe.As<TSelf>(m).Load();
	private static void __Init(EngineModule m) => Unsafe.As<TSelf>(m).Initialize();
	private static void __Start(EngineModule m) => Unsafe.As<TSelf>(m).Start();
	private static void __Fixed(EngineModule m) => Unsafe.As<TSelf>(m).FixedUpdate();
	private static void __Update(EngineModule m) => Unsafe.As<TSelf>(m).Update();
	private static void __Late(EngineModule m) => Unsafe.As<TSelf>(m).LateUpdate();
	private static void __Shutdown(EngineModule m) => Unsafe.As<TSelf>(m).Shutdown();

	private static MethodMask ComputeMethodMask(Type type)
	{
		const BindingFlags flags = BindingFlags.Instance |  BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
		MethodMask mask = default;
		SetMaskBit(type, ref mask, nameof(Load), MethodMask.Load);
		SetMaskBit(type, ref mask, nameof(Initialize), MethodMask.Init);
		SetMaskBit(type, ref mask, nameof(Start), MethodMask.Start);
		SetMaskBit(type, ref mask, nameof(FixedUpdate), MethodMask.Fixed);
		SetMaskBit(type, ref mask, nameof(Update), MethodMask.Update);
		SetMaskBit(type, ref mask, nameof(LateUpdate), MethodMask.Late);
		SetMaskBit(type, ref mask, nameof(Shutdown), MethodMask.Shutdown);
		return mask;

		static void SetMaskBit(Type t, ref MethodMask m, string methodName, MethodMask bit)
		{
			if (t.GetMethod(methodName, flags) is { }) m |= bit;
		}
	}
}

public readonly unsafe struct MethodSet
{
	public delegate* managed<EngineModule, void> Load { get; init; }
	public delegate* managed<EngineModule, void> Initialize { get; init; }
	public delegate* managed<EngineModule, void> Start { get; init; }
	public delegate* managed<EngineModule, void> FixedUpdate { get; init; }
	public delegate* managed<EngineModule, void> Update { get; init; }
	public delegate* managed<EngineModule, void> LateUpdate { get; init; }
	public delegate* managed<EngineModule, void> Shutdown { get; init; }
}

public readonly unsafe struct MethodTable
{
	private readonly delegate* managed<EngineModule, void>[] _table;

	public MethodTable(MethodMask mask, MethodSet set)
	{
		_table = new delegate* managed<EngineModule, void>[7];
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
	public bool TryGet(MethodMask methodMask, out delegate* managed<EngineModule, void> fn)
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