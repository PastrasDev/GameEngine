using Engine.Core;
using Engine.Core.Lifecycle;
using Engine.Platform.Windows;

namespace Engine.Graphics.D3D11;

[Discoverable]
internal sealed class Renderer : Module<Renderer>, IConfigure
{
	public static void Configure()
	{
		Enabled = true;
		AllowMultiple = false;
		Affinity = Affinity.Render;
		AddDependency<Window>();
	}

	public override void Load()
	{
		base.Load();
	}

	public override void Initialize()
	{
		base.Initialize();

	}

	public override void Start()
	{
		base.Start();

	}

	public override void Update()
	{
		base.Update();
	}

	public override void PostUpdate()
	{
		base.PostUpdate();
	}

	public override void Shutdown()
	{
		base.Shutdown();
	}
}