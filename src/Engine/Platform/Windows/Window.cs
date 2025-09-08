using System;
using Windows.Win32;
using Engine.Core;

namespace Engine.Platform.Windows;

using static PInvoke;

[Affinity(Threads.Main)]
public sealed class Window : EngineModule<Window>
{
	public override void Load()
	{
		Console.WriteLine("Window loaded");
		Enabled = true;
	}

	public override void Initialize()
	{
		Console.WriteLine("Window initialized");
	}

	public override void Start()
	{
		Console.WriteLine("Window started");
	}

	public override void Update() { }

	public override void LateUpdate() { }

	public override void Shutdown()
	{
		Console.WriteLine("Window shutdown");
	}
}