using System.Reflection;
using System.Runtime.CompilerServices;
using Vyrse.Core.Lifecycle;

namespace Vyrse.Core;

public interface IComponent : IPhase { }

public abstract class Component
{
	public string Name { get; set; } = string.Empty;
	public bool Enabled { get; set; } = true;
	protected internal Module Owner { get; internal set; } = null!;
	protected internal Context Context => Owner.Context;

	public abstract Phase Mask { get; }
	public abstract Thunk[] Thunks { get; }
}

public abstract class Component<TSelf> : Component where TSelf : Component<TSelf>, IComponent, new()
{
	public static Phase SMask { get; } = IPhase.ComputeMask(typeof(TSelf), BindingFlags.Instance | BindingFlags.Public);
	public override Phase Mask => SMask;

	public static readonly Thunk[] SThunks;
	public override Thunk[] Thunks => SThunks;

	static unsafe Component()
	{
		delegate* managed<object, void>[] ptrs =
		[
			&_Load, &_Initialize, &_Start, &_FixedUpdate, &_PreUpdate, &_Update, &_PostUpdate, &_Shutdown
		];

		SThunks = new Thunk[IPhase.Length];
		for (int i = 0; i < IPhase.Length; i++)
		{
			var phase = (Phase)(1 << i);
			if ((SMask & phase) != 0)
				SThunks[i] = new Thunk(ptrs[i], phase);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static void _Load(object o) => Unsafe.As<TSelf>(o).Load();
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static void _Initialize(object o) => Unsafe.As<TSelf>(o).Initialize();
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static void _Start(object o) => Unsafe.As<TSelf>(o).Start();
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static void _PreUpdate(object o) => Unsafe.As<TSelf>(o).PreUpdate();
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static void _Update(object o) => Unsafe.As<TSelf>(o).Update();
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static void _PostUpdate(object o) => Unsafe.As<TSelf>(o).PostUpdate();
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static void _FixedUpdate(object o) => Unsafe.As<TSelf>(o).FixedUpdate();
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static void _Shutdown(object o) => Unsafe.As<TSelf>(o).Shutdown();
}