using Engine.Core;

namespace Engine.Graphics.D3D11;

public interface IRenderer
{
    static IRenderer Create() => new Renderer();
    
}

internal class Renderer : EngineModule, IRenderer
{
    public override void Initialize()
    {
        
    }

    public override void Start()
    {
        
    }

    public override void Update()
    {
        
    }

    public override void LateUpdate()
    {
        
    }

    public override void Shutdown()
    {
        
    }
}