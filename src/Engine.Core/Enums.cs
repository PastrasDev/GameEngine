using System;

namespace Engine.Core;

public enum RuntimeMode
{
    None,
    Editor,
    Game
}

public enum GameRole
{
    None,
    Client,
    Server,
    Both
}

[Flags]
public enum RuntimeThreads
{
    None   = 0,
    Main   = 1 << 0,
    Game   = 1 << 1,
    Render = 1 << 2,
}

public enum RenderControlType 
{ 
    Resize /*, 
    ToggleVsync, 
    ToggleFullscreen, ...*/ 
}

[Flags]
public enum PhaseMask
{
    None=0, 
    Init=1, 
    Start=2, 
    Fixed=4, 
    Update=8, 
    Late=16, 
    Shutdown=32
}