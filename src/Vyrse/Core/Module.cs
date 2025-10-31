using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Vyrse.Core.Abstract;
using Vyrse.Core.Lifecycle;

namespace Vyrse.Core;

public interface IModule : IPhase
{
	static abstract Module.Metadata[] Dependencies { get; }
}

public abstract class Module
{
	protected bool Enabled { get; set; }
	protected Component[] Components { get; set; } = [];
	public Thunk[] Thunks { get; protected set; } = [];
	protected Thunks[] ComponentThunks { get; set; } = [];
	protected Phase Mask { get; set; }
	protected internal Context Context { get; protected init; } = null!;

	public sealed class Metadata
	{
		public required Type Type { get; init; }
		public required Metadata[] Dependencies { get; init; }
		public unsafe required delegate* managed<Context, Module> Instantiate { get; init; }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Register() => Registry.Instance.Global.Add(Type, this);

		public sealed class Registry : Registry<Registry, Metadata>
		{
			public static Registry Instance { get; } = new();
			private Registry() { }
		}
	}
}

public abstract class Module<TSelf> : Module where TSelf : Module<TSelf>, IModule, new()
{
	public static Metadata Data { get; }
	private readonly List<Component> _components = [];

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	static unsafe Module()
	{
		Data = new Metadata
		{
			Type = typeof(TSelf),
			Dependencies = TSelf.Dependencies.ToArray(),
			Instantiate = &Create
		};

		Data.Register();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected T AddComponent<T>(T component, string? name = null) where T : Component
	{
		component.Owner = this;
		if (name is { }) component.Name = name;
		_components.Add(component);
		return component;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected T AddComponent<T>(string? name = null) where T : Component<T>, IComponent, new()
	{
		var component = new T { Owner = this };
		if (name is { }) component.Name = name;
		_components.Add(component);
		return component;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected T GetComponent<T>(string? name = null) where T : Component
	{
		foreach (var c in Components)
		{
			if (c is not T t) continue;
			if (name is null || t.Name == name) return t;
		}

		throw new InvalidOperationException($"Component of type {typeof(T).Name} {(name is null ? "" : $"(name={name}) ")}not found in module {typeof(TSelf).Name}.");
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected bool TryGetComponent<T>(out T result, string? name = null) where T : Component
	{
		foreach (var c in Components)
		{
			if (c is not T t) continue;
			if (name is { } && t.Name != name) continue;
			result = t;
			return true;
		}

		result = null!;
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected T[] GetComponents<T>() where T : Component
	{
		var list = new List<T>();
		foreach (var c in Components)
			if (c is T t)
				list.Add(t);
		return list.ToArray();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static Metadata AddDependency<T>() where T : Module<T>, IModule, new() => Module<T>.Data;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Module Create(Context context)
	{
		var self = new TSelf { Context = context };
		_Load(self);
		return self;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static unsafe void _Load(object obj)
	{
		var self = Unsafe.As<TSelf>(obj);
		Console.WriteLine($"[Module][{typeof(TSelf).Name}::Load]");
		self.Load();

		self.Components = self._components.ToArray();

		const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public;
		self.Mask = IPhase.ComputeMask(typeof(TSelf), flags);

		foreach (var data in self.Components) self.Mask |= data.Mask;

		delegate* managed <object, void>[] ptrs =
		[
			&_Load, &_Initialize, &_Start, &_FixedUpdate, &_PreUpdate, &_Update, &_PostUpdate, &_Shutdown,
		];

		var length = IPhase.Length;
		self.Thunks = new Thunk[length];
		for (int i = 0; i < length; i++)
		{
			var phase = (Phase)(1 << i);
			self.Thunks[i] = (self.Mask & phase) != 0 ? new Thunk(ptrs[i], phase) : default;
		}

		self.ComponentThunks = new Thunks[length];
		for (int i = 0; i < length; i++)
			self.ComponentThunks[i] = new Thunks(self.Components.Length);

		foreach (var comp in self.Components)
		{
			var templates = comp.Thunks;

			if (!templates[0].IsNull)
				new Thunk(comp, templates[0]).Invoke();

			for (int i = 1; i < templates.Length; i++)
			{
				ref readonly var t = ref templates[i];
				if (t.IsNull) continue;
				self.ComponentThunks[i].Add(new Thunk(comp, t));
			}
		}

		self.ComponentThunks[7].Reverse();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void _Initialize(object obj)
	{
		var self = Unsafe.As<TSelf>(obj);
		Console.WriteLine($"[Module][{typeof(TSelf).Name}::Initialize]");
		self.Initialize();
		self.ComponentThunks[1].Invoke();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void _Start(object obj)
	{
		var self = Unsafe.As<TSelf>(obj);
		Console.WriteLine($"[Module][{typeof(TSelf).Name}::Start]");
		self.Start();
		self.ComponentThunks[2].Invoke();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void _FixedUpdate(object obj)
	{
		var self = Unsafe.As<TSelf>(obj);
		self.FixedUpdate();
		self.ComponentThunks[3].Invoke();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void _PreUpdate(object obj)
	{
		var self = Unsafe.As<TSelf>(obj);
		self.PreUpdate();
		self.ComponentThunks[4].Invoke();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void _Update(object obj)
	{
		var self = Unsafe.As<TSelf>(obj);
		self.Update();
		self.ComponentThunks[5].Invoke();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void _PostUpdate(object obj)
	{
		var self = Unsafe.As<TSelf>(obj);
		self.PostUpdate();
		self.ComponentThunks[6].Invoke();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void _Shutdown(object obj)
	{
		var self = Unsafe.As<TSelf>(obj);
		self.ComponentThunks[7].Invoke();
		Console.WriteLine($"[Module][{typeof(TSelf).Name}::Shutdown]");
		self.Shutdown();
	}
}