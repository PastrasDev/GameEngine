using Engine.Core;
using Engine.Core.Lifecycle;
using Engine.Platform.Windows;

namespace Engine.Testing;

[Discoverable]
public sealed class TestModule : Module<TestModule>, IConfigure
{
	private Keyboard _keyboard = null!;
	private Mouse _mouse = null!;

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

		var input = Context.Input;

		_keyboard = input.Keyboard;
		_mouse = input.Mouse;

		// TODO: Make Input events pass an event struct

		_keyboard.OnEvent(Keyboard.Key.W, Keyboard.Event.Down) += KeyboardTest;
		_mouse.OnMove += MouseTest;
	}

	private void MouseTest()
	{
		//Log.Info($"Mouse Delta: {_mouse.Delta}");
	}

	private void KeyboardTest()
	{
		//Log.Info("W Pressed");
	}

	public override void PreUpdate()
	{

	}

	public override void Update()
	{
		//Log.Info($"FPS: {Context.Time.Fps}");
	}

	public override void PostUpdate()
	{

	}

	public override void Shutdown()
	{

	}
}