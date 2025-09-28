using Engine.Core;
using Engine.Core.Lifecycle;
using Engine.Platform.Windows;

namespace Game;

[Discoverable(Enabled = false)]
public class Test : Module<Test>, IConfigure
{
	public static void Configure()
	{
		Enabled = true;
		AllowMultiple = false;
		Affinity = Affinity.Game;

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

	public override void Shutdown()
	{
		base.Shutdown();
	}
}