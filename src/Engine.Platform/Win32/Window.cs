using Engine.Core;

namespace Engine.Platform.Win32;

using static Windows.Win32.PInvoke;

public interface IWindow
{
    static IWindow Create() => new Window();
}

internal class Window : EngineModule, IWindow
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