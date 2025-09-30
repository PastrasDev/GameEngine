using System.Reflection;
using System.Runtime.CompilerServices;
using Engine.Core.Extensions;
using Engine.Core.Lifecycle;
using Engine.Core.Threading.Messaging;
using Engine.Debug;

namespace Engine.Core;

public abstract class Component
{
	public sealed class Metadata
	{
		public required Type Type { get; init; }
		public required bool Enabled { get; init; }
		public required bool AllowMultiple { get; init; }
		public required Metadata[] Dependencies { get; init; }
		public required Phase Mask { get; init; }
		public unsafe required delegate* managed<Module, Context, Component> Instantiate { get; init; }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Register() => Registry.Instance.Global.Add(Type, this);

		public sealed class Registry : Abstract.Registry<Registry, Metadata>
		{
			public static Registry Instance { get; } = new();
			private Registry() { }
		}
	}

	public sealed class Registry : Abstract.Registry<Registry, Component>
	{
		public static Registry Instance { get; } = new();
		private Registry() { }
	}
}

public abstract class Component<TSelf> : Component, IPhases, IMessaging where TSelf : Component<TSelf>, IConfigure, new()
{
	protected Module Owner { get; private set; } = null!;
	protected Context Context { get; private set; } = null!;
	protected Affinity Affinity => Context.Affinity;

	protected static bool Enabled { get; set; }
	protected static bool AllowMultiple { get; set; }
	private static readonly List<Metadata> s_dependencyData = [];

	private const BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public;
	public static Phase Mask => IPhases.ComputeMask(typeof(TSelf), Flags);

	public static Metadata Data { get; }

	static unsafe Component()
	{
		TSelf.Configure();

		Data = new Metadata
		{
			Type = typeof(TSelf),
			Enabled = Enabled,
			AllowMultiple = AllowMultiple,
			Dependencies = s_dependencyData.ToArray(),
			Mask = Mask,
			Instantiate = &Create
		};

		Data.Register();

		s_dependencyData.Clear();
	}

	public virtual void Load() => Log.Info($"[Component][{typeof(TSelf).Name}::Load]");
	public virtual void Initialize() => Log.Info($"[Component][{typeof(TSelf).Name}::Initialize]");
	public virtual void Start() => Log.Info($"[Component][{typeof(TSelf).Name}::Start]");
	public virtual void FixedUpdate() { }
	public virtual void PostUpdate() { }
	public virtual void PreUpdate() { }
	public virtual void Update() { }
	public virtual void Shutdown() => Log.Info($"[Component][{typeof(TSelf).Name}::Shutdown]");


	private static Component Create(Module owner, Context context)
	{
		var self = new TSelf { Owner = owner, Context = context };
		Registry.Instance.Global.Add(typeof(TSelf), self);
		return self;
	}

	protected static void AddDependency<T>() where T : Component<T>, IConfigure, new() => s_dependencyData.Add(Component<T>.Data);


	public static void Send<TData, TTarget>(in Snapshot<TData, TTarget> snapshot)
	{
		Router<TData, TSelf, TTarget>.Snapshot.Publish(snapshot.Data, snapshot.Settings);
	}

	public static void Send<TData, TTarget>(in Stream<TData, TTarget> stream)
	{
		Router<TData, TSelf, TTarget>.Stream.Write(stream.Data, stream.Settings, Application.TokenSrc.Token);
	}

	public static TData Receive<TData, TSender>() where TData : notnull where TSender : class
	{
		return Router<TData, TSender, TSelf>.Stream.TryRead(out var item) ? item : Router<TData, TSender, TSelf>.Snapshot.Read().value;
	}

	public static bool TryReceive<TData, TSender>(out TData? data) where TData : notnull where TSender : class
	{
		if (Router<TData, TSender, TSelf>.Stream.TryRead(out var item))
		{
			data = item;
			return true;
		}
		data = default;
		return false;
	}
}

public abstract class Module
{
	protected Context Context { get; init; } = null!;
	protected Component[] Components { get; set; } = [];

	public sealed class Metadata
	{
		public required Type Type { get; init; }
		public required bool Enabled { get; init; }
		public required bool AllowMultiple { get; init; }
		public required Affinity Affinity { get; init; }
		public required Metadata[] Dependencies { get; init; }
		public required Component.Metadata[] Components { get; init; }
		public required Thunk[] Thunks { get; init; }
		public required Phase Mask { get; init; }
		public unsafe required delegate* managed<Context, object> Instantiate { get; init; }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Register() => Registry.Instance.Global.Add(Type, this);

		public sealed class Registry : Abstract.Registry<Registry, Metadata>
		{
			public static Registry Instance { get; } = new();
			private Registry() { }
		}
	}

	public sealed class Registry : Abstract.Registry<Registry, Module>
	{
		public static Registry Instance { get; } = new();
		private Registry() { }
	}
}

public abstract class Module<TSelf> : Module, IPhases, IMessaging, IThunks<TSelf> where TSelf : Module<TSelf>, IConfigure, new()
{
	protected static bool Enabled { get; set; }
	protected static bool AllowMultiple { get; set; }
	protected static Affinity Affinity { get; set; }

	private static readonly List<Metadata> s_dependencyData = [];
	private static readonly List<Component.Metadata> s_componentData = [];

	private static readonly Component.Metadata[] s_orderedComponents = [];

	public static Phase Mask { get; }
	public static Thunk[] Thunks { get; } = null!;
	public static Metadata Data { get; } = null!;

	static unsafe Module()
	{
		TSelf.Configure();

		if (!Enabled) return;

		if (Affinity == Affinity.None)
			throw new InvalidOperationException($"Module {typeof(TSelf).Name} must specify a valid Affinity.");

		var typeNodes = s_componentData.GroupBy(m => m.Type).Select(g => new { Type = g.Key, Deps = g.First().Dependencies.Select(d => d.Type) });
		var typeOrder = typeNodes.TopologicalSort(x => x.Type, x => x.Deps).Select(x => x.Type).ToArray();
		var byType = s_componentData.GroupBy(m => m.Type).ToDictionary(g => g.Key, g => g.ToArray());
		s_orderedComponents = typeOrder.SelectMany(t => byType[t]).ToArray();

		const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;
		Mask = IPhases.ComputeMask(typeof(TSelf), flags);

		Phase cMask = default;
		foreach (var data in s_componentData)
			cMask |= data.Mask;

		Mask |= cMask;

		delegate* managed <object, void>[] ptrs =
		[
			&_Load, &_Initialize, &_Start, &_FixedUpdate, &_PreUpdate, &_Update, &_PostUpdate, &_Shutdown,
		];

		var length = IPhases.Length;
		Thunks = new Thunk[length];
		for (int i = 0; i < length; i++)
		{
			var phase = (Phase)(1 << i);
			Thunks[i] = (Mask & phase) != 0 ? new Thunk(ptrs[i], phase) : default;
		}

		Data = new Metadata
		{
			Type = typeof(TSelf),
			Enabled = Enabled,
			AllowMultiple = AllowMultiple,
			Affinity = Affinity,
			Dependencies = s_dependencyData.ToArray(),
			Components = s_componentData.ToArray(),
			Thunks = Thunks,
			Mask = Mask,
			Instantiate = &Create
		};

		Data.Register();

		s_dependencyData.Clear();
		s_componentData.Clear();
	}

	public virtual void Load() => Log.Info($"[Module][{typeof(TSelf).Name}::Load]");
	public virtual void Initialize() => Log.Info($"[Module][{typeof(TSelf).Name}::Initialize]");
	public virtual void Start() => Log.Info($"[Module][{typeof(TSelf).Name}::Start]");
	public virtual void FixedUpdate() { }
	public virtual void PostUpdate() { }
	public virtual void PreUpdate() { }
	public virtual void Update() { }
	public virtual void Shutdown() => Log.Info($"[Module][{typeof(TSelf).Name}::Shutdown]");

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected T GetComponent<T>() where T : Component => TryGetComponent(out T component) ? component : throw new InvalidOperationException($"Component of type {typeof(T).Name} not found in module {typeof(TSelf).Name}.");

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected bool TryGetComponent<T>(out T component) where T : Component
	{
		T? found = null;
		foreach (var c in Components)
		{
			if (c is not T t) continue;
			if (found is { })
			{
				component = null!;
				return false;
			}

			found = t;
		}

		component = found!;
		return found is { };
	}

	private static object Create(Context context)
	{
		var self = new TSelf { Context = context };
		Registry.Instance.Global.Add(typeof(TSelf), self);
		return self;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected int GetComponents<T>(List<T> buffer) where T : Component
	{
		ArgumentNullException.ThrowIfNull(buffer);
		int start = buffer.Count;
		foreach (var c in Components)
			if (c is T t) buffer.Add(t);
		return buffer.Count - start;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static void AddDependency<T>() where T : Module<T>, IConfigure, new() => s_dependencyData.Add(Module<T>.Data);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static void AddComponent<T>() where T : Component<T>, IConfigure, new()
	{
		var metadata = Component<T>.Data;

		if (!metadata.Enabled) return;
		if (!metadata.AllowMultiple && s_componentData.Any(c => c.Type == metadata.Type))
			throw new InvalidOperationException($"Component {metadata.Type.Name} does not allow multiple instances in a single module.");

		s_componentData.Add(metadata);
	}


	public static void Send<TData, TTarget>(in Snapshot<TData, TTarget> snapshot)
	{
		Router<TData, TSelf, TTarget>.Snapshot.Publish(snapshot.Data, snapshot.Settings);
	}

	public static void Send<TData, TTarget>(in Stream<TData, TTarget> stream)
	{
		Router<TData, TSelf, TTarget>.Stream.Write(stream.Data, stream.Settings, Application.TokenSrc.Token);
	}

	public static TData Receive<TData, TSender>() where TData : notnull where TSender : class
	{
		return Router<TData, TSender, TSelf>.Stream.TryRead(out var item) ? item : Router<TData, TSender, TSelf>.Snapshot.Read().value;
	}

	public static bool TryReceive<TData, TSender>(out TData? data) where TData : notnull where TSender : class
	{
		if (Router<TData, TSender, TSelf>.Stream.TryRead(out var item))
		{
			data = item;
			return true;
		}

		data = default;
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void _Load(object o)
	{
		var self = IThunks<TSelf>.Get(o);
		self.Load();

		var ordered = s_orderedComponents;
		var components = new Component[ordered.Length];
		for (int i = 0; i < components.Length; i++)
		{
			unsafe { components[i] = ordered[i].Instantiate(self, self.Context); }
		}

		self.Components = components;
		foreach (var c in components)
			((IPhases)c).Load();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void _Initialize(object o)
	{
		var self = IThunks<TSelf>.Get(o);
		self.Initialize();
		var components = self.Components;
		foreach (var c in components)
			((IPhases)c).Initialize();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void _Start(object o)
	{
		var self = IThunks<TSelf>.Get(o);
		self.Start();
		var components = self.Components;
		foreach (var c in components)
			((IPhases)c).Start();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void _FixedUpdate(object o)
	{
		var self = IThunks<TSelf>.Get(o);
		self.FixedUpdate();
		var components = self.Components;
		foreach (var c in components)
			((IPhases)c).FixedUpdate();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void _PreUpdate(object o)
	{
		var self = IThunks<TSelf>.Get(o);
		self.PreUpdate();
		var components = self.Components;
		foreach (var c in components)
			((IPhases)c).PreUpdate();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void _Update(object o)
	{
		var self = IThunks<TSelf>.Get(o);
		self.Update();
		var components = self.Components;
		foreach (var c in components)
			((IPhases)c).Update();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void _PostUpdate(object o)
	{
		var self = IThunks<TSelf>.Get(o);
		self.PostUpdate();
		var components = self.Components;
		foreach (var c in components)
			((IPhases)c).PostUpdate();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void _Shutdown(object o)
	{
		var self = IThunks<TSelf>.Get(o);

		var components = self.Components;
		for (int i = components.Length - 1; i >= 0; i--)
			((IPhases)components[i]).Shutdown();

		self.Shutdown();
	}
}