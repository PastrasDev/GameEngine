using System;

namespace Engine.Core;

public enum GameRole
{
    None,
    Client,
    Server,
    Both
}

[Flags]
public enum Threads
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
public enum Phase
{
    None     = 0,
    Load     = 1 << 0,
    Init     = 1 << 1,
    Start    = 1 << 2,
    Fixed    = 1 << 3,
    Update   = 1 << 4,
    Late     = 1 << 5,
    Shutdown = 1 << 6
}

public enum ExitCode
{
    /// clean shutdown
    Ok = 0,
    /// normal cancellation (e.g., user quit)
    Canceled = 10,
    /// non-fatal error; runtime can shut down gracefully
    Recoverable = 20,
    /// fatal error
    Fatal = 30       
}

public enum AccessType
{
    None,
    ReadWrite,
    ReadOnly
}