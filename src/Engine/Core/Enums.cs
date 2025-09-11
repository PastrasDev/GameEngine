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
	None = 0,
	Main = 1 << 0,
	Game = 1 << 1,
	Render = 1 << 2,
}

public enum RenderControlType
{
	Resize /*,
	ToggleVsync,
	ToggleFullscreen, ...*/
}

[Flags]
public enum MethodMask
{
	None = 0,
	Load = 1 << 0,
	Init = 1 << 1,
	Start = 1 << 2,
	Fixed = 1 << 3,
	Update = 1 << 4,
	Late = 1 << 5,
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

public enum KernelStage
{
	None = 0,

	MainLoaded = 10,
	GameLoaded = 20,
	RenderLoaded = 30,
	AllLoaded = 40,

	MainInitialized = 50,
	GameInitialized = 60,
	RenderInitialized = 70,
	AllInitialized = 80,

	MainStarted = 90,
	GameStarted = 100,
	RenderStarted = 110,
	AllStarted = 120,
}