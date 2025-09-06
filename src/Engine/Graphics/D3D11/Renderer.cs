using System;
using Engine.Core;

namespace Engine.Graphics.D3D11;

[Affinity(Threads.Render)]
public sealed class Renderer : EngineModule<Renderer>
{
    public override void Load()
    {
        Enabled = true;
        Console.WriteLine("D3D11 Renderer loaded");
        
    }
    
    public override void Initialize()
    {
        Console.WriteLine("D3D11 Renderer initialized");
        
    }

    public override void Start()
    {
        Console.WriteLine("D3D11 Renderer started");
        
    }

    public override void Update()
    {
        
    }

    public override void LateUpdate()
    {
        
    }

    public override void Shutdown()
    {
        Console.WriteLine("D3D11 Renderer shutdown");
        
    }
}