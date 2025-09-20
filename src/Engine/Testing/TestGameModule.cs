using System;
using Engine.Core;
using Engine.Platform.Windows;

namespace Engine.Testing;

[Affinity(Threads.Game)]
public class TestGameModule : EngineModule<TestGameModule>
{
	private Keyboard _keyboard = null!;
	private Mouse _mouse = null!;

	protected override void Load()
	{
		base.Load();
		Enabled = true;
	}

	protected override void Initialize()
	{
		base.Initialize();
	}

	protected override void Start()
	{
		base.Start();

		_keyboard = Context.Input.Keyboard;
		_mouse = Context.Input.Mouse;

		_keyboard.OnEvent(Keyboard.Key.Space, Keyboard.Event.Down) += KeyboardTest;
		_mouse.OnMove += MouseTest;
	}

	private void MouseTest()
	{
		Console.WriteLine($"Mouse moved to {_mouse.Position}");
	}

	private void KeyboardTest()
	{
		Console.WriteLine("Key Space Pressed");
	}

	protected override void Update()
	{
		base.Update();

	}

	protected override void LateUpdate()
	{
		base.LateUpdate();
	}

	protected override void Shutdown()
	{
		base.Shutdown();
	}
	
}