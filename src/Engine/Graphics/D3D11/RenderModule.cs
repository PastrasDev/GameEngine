using System;
using Windows.Win32;
using Engine.Core;

namespace Engine.Graphics.D3D11;

using static PInvoke;

[Affinity(Threads.Render)]
internal sealed class RenderModule : EngineModule<RenderModule>
{
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

	}

	protected override void Update()
	{

	}

	protected override void LateUpdate()
	{

	}

	protected override void Shutdown()
	{
		base.Shutdown();

	}
}