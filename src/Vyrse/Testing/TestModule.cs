using System.Runtime.CompilerServices;
using Vyrse.Core;
using Vyrse.Core.Lifecycle;

namespace Vyrse.Testing;

public interface IModule : IPhase;

[Module]
public sealed partial class MyModule
{
	private MyComponent _myComponent = null!;
	private MyComponent _myComponent1 = null!;

	public void Load() { }
	public void Initialize() { }
	public void Start() { }
	public void FixedUpdate() { }
	public void PreUpdate() { }
	public void Update() { }
	public void PostUpdate() { }
	public void Shutdown() { }
}

// Auto-generated code
public sealed partial class MyModule(Context context) : IModule
{
	public readonly Context Context = context;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void __Load() // called by the kernel
	{
		_myComponent = new MyComponent(Context, this);
		_myComponent1 = new MyComponent(Context, this);
		// And so on for other components if any
		Load();
		_myComponent.Load();
		_myComponent1.Load();
		// And so on for other components if any
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void __Initialize() // called by the kernel
	{
		Initialize();
		_myComponent.Initialize();
		_myComponent1.Initialize();

		// And so on for other components if any
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void __Start() // called by the kernel
	{
		Start();
		_myComponent.Start();
		_myComponent1.Start();

		// And so on for other components if any
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void __FixedUpdate() // called by the kernel
	{
		FixedUpdate();
		_myComponent.FixedUpdate();
		_myComponent1.FixedUpdate();

		// And so on for other components if any
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void __PreUpdate() // called by the kernel
	{
		PreUpdate();
		_myComponent.PreUpdate();
		_myComponent1.PreUpdate();

		// And so on for other components if any
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void __Update() // called by the kernel
	{
		Update();
		_myComponent.Update();
		_myComponent1.Update();

		// And so on for other components if any
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void __PostUpdate() // called by the kernel
	{
		PostUpdate();
		_myComponent.PostUpdate();
		_myComponent1.PostUpdate();

		// And so on for other components if any
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void __Shutdown() // called by the kernel
	{
		Shutdown();
		_myComponent.Shutdown();
		_myComponent1.Shutdown();

		// And so on for other components if any
	}
}

[Component]
public sealed partial class MyComponent
{
	public void Load() { }
	public void Initialize() { }
	public void Start() { }
	public void FixedUpdate() { }
	public void PreUpdate() { }
	public void Update() { }
	public void PostUpdate() { }
	public void Shutdown() { }
}

// Auto-generated code
public sealed partial class MyComponent(Context context, IModule owner) : IPhase
{
	public readonly Context Context = context;
	public readonly IModule Owner = owner;
}