using System;
using Windows.Win32;
using Engine.Core;

namespace Engine.Graphics.D3D11;

using static PInvoke;

[Affinity(Threads.Render)]
public sealed class Renderer : EngineModule<Renderer>
{
	public override void Load()
	{
		Console.WriteLine("Renderer loaded");
		Enabled = true;
	}

	public override void Initialize()
	{



		Console.WriteLine("Renderer initialized");
	}

	public override void Start()
	{
		Console.WriteLine("Renderer started");
	}

	public override void Update() { }

	public override void LateUpdate() { }

	public override void Shutdown()
	{
		Console.WriteLine("Renderer shutdown");
	}
}