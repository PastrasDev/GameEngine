using System;

namespace Engine.Abstractions;

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