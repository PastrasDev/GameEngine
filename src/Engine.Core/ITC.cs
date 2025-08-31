using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Engine.Core;

/// Frame boundary info from Main → Game
public readonly record struct FrameStart(long FrameId, float Dt);

/// Minimal per-frame view data from Game → Render (expand later)
public readonly record struct SceneView(long FrameId, float Alpha);

/// Input → Game
public readonly record struct InputSnapshot(long FrameId /* add keys/mouse/gamepad fields */);

/// Main → Render control (resize, vsync toggle, etc.)
public readonly record struct RenderControl(RenderControlType Type, int Width = 0, int Height = 0);

public interface IMainChannels
{
    /// main → game
    IWritePort<FrameStart> FrameWriter { get; }

    /// main → game
    IWritePort<InputSnapshot> InputWriter { get; }

    /// main → render
    IRenderCommandBus RenderCommands { get; }
}

public interface IGameChannels
{
    /// main → game
    IReadPort<FrameStart> FrameReader { get; }

    /// main → game
    IReadPort<InputSnapshot> InputReader { get; }

    /// game → render
    ISnapshotWrite<SceneView> SceneWriter { get; }
}

public interface IRenderChannels
{
    /// game → render
    ISnapshotRead<SceneView> SceneReader { get; }

    /// main → render
    IRenderCommandBus RenderCommands { get; }
}

public interface IWritePort<T>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool TryWrite(in T item);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void Write(in T item, CancellationToken ct);
}

public interface IReadPort<T>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool TryRead(out T item);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    T Read(CancellationToken ct);
}

public interface ISnapshotWrite<T>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void Publish(in T value);
}

public interface ISnapshotRead<T>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    (T value, int version) Read();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    int WaitForChange(int lastVersion, CancellationToken ct);
}

public interface IControlBus<T>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void Post(in T message);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void Drain(Action<T> apply);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void Clear();
}

public interface IRenderCommandBus : IControlBus<RenderControl>;

public sealed class EngineChannels
{
    /// Main → Game (frame pacing)
    public required FrameChannel Frame
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        init;
    }

    /// Main → Game (input)
    public required InputChannel Input
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        init;
    }

    /// Game → Render (latest-wins)
    public required SceneChannel Scene
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        init;
    }

    /// Main → Render (side-band commands)
    public required IRenderCommandBus RenderCommands
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        init;
    }
}

public sealed class FrameChannel
{
    /// Game reads
    public required IReadPort<FrameStart> Reader
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        init;
    }

    /// Main writes
    public required IWritePort<FrameStart> Writer
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        init;
    }
}

public sealed class InputChannel
{
    /// Game reads
    public required IReadPort<InputSnapshot> Reader
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        init;
    }

    /// Main writes
    public required IWritePort<InputSnapshot> Writer
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        init;
    }
}

public sealed class SceneChannel
{
    /// Render reads (freshest only)
    public required ISnapshotRead<SceneView> Reader
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        init;
    }

    /// Game writes
    public required ISnapshotWrite<SceneView> Writer
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        init;
    }
}

/// <summary>
/// Minimal SPSC pipe interface. Single-Producer / Single-Consumer only.
/// </summary>
public interface IPipe<T> : IDisposable
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool TryWrite(in T item);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void Write(in T item, CancellationToken ct);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    bool TryRead(out T item);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    T Read(CancellationToken ct);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void Complete();

    bool IsCompleted { get; }
}