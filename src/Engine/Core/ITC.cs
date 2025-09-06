using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Engine.Core;

// ===============================
// Messages & Commands
// ===============================

/// Frame boundary info from Main → Game
public readonly record struct FrameStart(long FrameId, float Dt);

/// Minimal per-frame view data from Game → Render (expand later)
public readonly record struct SceneView(long FrameId, float Alpha);

/// Input → Game
public readonly record struct InputSnapshot(long FrameId /* add keys/mouse/gamepad fields */);

/// Main → Render control (resize, vsync toggle, etc.)
public readonly record struct RenderControl(RenderControlType Type, int Width = 0, int Height = 0);

// ===============================
// Interfaces
// ===============================

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

// ===============================
// Channels (concrete)
// ===============================

public sealed class EngineChannels
{
    /// Main → Game (frame pacing)
    public required FrameChannel Frame { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; [MethodImpl(MethodImplOptions.AggressiveInlining)] init; }

    /// Main → Game (input)
    public required InputChannel Input { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; [MethodImpl(MethodImplOptions.AggressiveInlining)] init; }

    /// Game → Render (latest-wins)
    public required SceneChannel Scene { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; [MethodImpl(MethodImplOptions.AggressiveInlining)] init; }

    /// Main → Render (side-band commands)
    public required IRenderCommandBus RenderCommands { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; [MethodImpl(MethodImplOptions.AggressiveInlining)] init; }
}

public sealed class FrameChannel
{
    /// Game reads
    public required IReadPort<FrameStart> Reader { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; [MethodImpl(MethodImplOptions.AggressiveInlining)] init; }
    /// Main writes
    public required IWritePort<FrameStart> Writer { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; [MethodImpl(MethodImplOptions.AggressiveInlining)] init; }
}

public sealed class InputChannel
{
    /// Game reads
    public required IReadPort<InputSnapshot> Reader { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; [MethodImpl(MethodImplOptions.AggressiveInlining)] init; }
    /// Main writes
    public required IWritePort<InputSnapshot> Writer { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; [MethodImpl(MethodImplOptions.AggressiveInlining)] init; }
}

public sealed class SceneChannel
{
    /// Render reads (freshest only)
    public required ISnapshotRead<SceneView> Reader { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; [MethodImpl(MethodImplOptions.AggressiveInlining)] init; }
    /// Game writes
    public required ISnapshotWrite<SceneView> Writer { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; [MethodImpl(MethodImplOptions.AggressiveInlining)] init; }
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

// ===============================
// Public views (so registries can add them)
// ===============================

public sealed class MainChannelsView(EngineChannels c) : IMainChannels
{
    public IWritePort<FrameStart> FrameWriter     { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => c.Frame.Writer; }
    public IWritePort<InputSnapshot> InputWriter  { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => c.Input.Writer; }
    public IRenderCommandBus RenderCommands       { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => c.RenderCommands; }
}

public sealed class GameChannelsView(EngineChannels c) : IGameChannels
{
    public IReadPort<FrameStart> FrameReader          { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => c.Frame.Reader; }
    public IReadPort<InputSnapshot> InputReader       { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => c.Input.Reader; }
    public ISnapshotWrite<SceneView> SceneWriter      { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => c.Scene.Writer; }
}

public sealed class RenderChannelsView(EngineChannels c) : IRenderChannels
{
    public ISnapshotRead<SceneView> SceneReader   { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => c.Scene.Reader; }
    public IRenderCommandBus RenderCommands       { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => c.RenderCommands; }
}

// ===============================
// Defaults builder (public API)
// ===============================

public static class Channels
{
    public static (EngineChannels channels, MainChannelsView mainView, GameChannelsView gameView, RenderChannelsView renderView) CreateDefault(int frameCapacityPow2 = 256, int inputCapacityPow2 = 256, PipeFullMode pipeFullMode = PipeFullMode.Wait, WaitStrategy waitStrategy = WaitStrategy.Hybrid, int renderCtrlCapacity = 64)
    {
        // Main ↔ Game pipes
        var frameRing = new SPSCRing<FrameStart>(frameCapacityPow2, pipeFullMode, waitStrategy);
        var framePort = new PipePort<FrameStart>(frameRing);

        var inputRing = new SPSCRing<InputSnapshot>(inputCapacityPow2, pipeFullMode, waitStrategy);
        var inputPort = new PipePort<InputSnapshot>(inputRing);

        // Game → Render snapshot
        var viewSnap  = new Snapshot<SceneView>();
        var viewPort  = new SnapshotPort<SceneView>(viewSnap);

        // Main → Render control bus
        var ctrlBus  = new ControlQueue<RenderControl>(rc => (int)rc.Type, maxItems: renderCtrlCapacity);
        var ctrlPort = new RenderCommandBusPort(ctrlBus);

        var bundle = new EngineChannels
        {
            Frame = new FrameChannel { Reader = framePort, Writer = framePort },
            Input = new InputChannel { Reader = inputPort, Writer = inputPort },
            Scene = new SceneChannel { Reader = viewPort,  Writer = viewPort  },
            RenderCommands = ctrlPort
        };

        return (bundle, new MainChannelsView(bundle), new GameChannelsView(bundle), new RenderChannelsView(bundle));
    }
}

// ===============================
// Low-level primitives (kept internal)
// ===============================

public enum PipeFullMode
{
    /// Producer waits until space exists (Write spins/yields).
    Wait,
    /// Producer fails fast when full (TryWrite false; Write spins/yields until space).
    DropWrite
}

public enum WaitStrategy
{
    /// Tight spinWait
    Spin,
    /// Spin then Yield
    Hybrid
}

internal sealed class PipePort<T>(SPSCRing<T> ring) : IReadPort<T>, IWritePort<T>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public bool TryWrite(in T item) => ring.TryWrite(item);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public void Write(in T item, CancellationToken ct) => ring.Write(item, ct);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public bool TryRead(out T item) => ring.TryRead(out item);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public T Read(CancellationToken ct) => ring.Read(ct);
}

internal sealed class SnapshotPort<T>(Snapshot<T> snap) : ISnapshotRead<T>, ISnapshotWrite<T>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public void Publish(in T value) => snap.Publish(value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public (T value, int version) Read() => snap.Read();
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public int WaitForChange(int lastVersion, CancellationToken ct) => snap.WaitForChange(lastVersion, ct);
}

/// <summary>
/// High-performance SPSC ring buffer with power-of-two capacity.
/// Safe for exactly one producer thread and one consumer thread.
/// </summary>
internal sealed class SPSCRing<T> : IPipe<T>
{
    private readonly T[] _buffer;
    private readonly int _mask; // capacity - 1
    private readonly PipeFullMode _fullMode;
    private readonly WaitStrategy _waitStrategy;

    /// next index to read
    private long _head;
    /// next index to write
    private long _tail;
    private volatile bool _completed;

    public bool IsCompleted => _completed;
    public int Capacity { get; }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SPSCRing(int capacityPow2 = 256, PipeFullMode fullMode = PipeFullMode.Wait, WaitStrategy waitStrategy = WaitStrategy.Hybrid)
    {
        if (capacityPow2 <= 0 || (capacityPow2 & (capacityPow2 - 1)) != 0)
            throw new ArgumentException("capacity must be a power of two and > 0", nameof(capacityPow2));

        Capacity = capacityPow2;
        _buffer = new T[capacityPow2];
        _mask = capacityPow2 - 1;
        _fullMode = fullMode;
        _waitStrategy = waitStrategy;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsFull(long head, long tail, int mask) => (tail - head) > mask;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryWrite(in T item)
    {
        if (_completed) return false;
        var head = Volatile.Read(ref _head);
        var tail = Volatile.Read(ref _tail);
        if (IsFull(head, tail, _mask))
        {
            if (_fullMode == PipeFullMode.DropWrite) return false;
            return false; // Wait mode handled by Write()
        }

        _buffer[(int)(tail & _mask)] = item;
        Volatile.Write(ref _tail, tail + 1);
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Write(in T item, CancellationToken ct)
    {
        var spin = new SpinWait();
        while (true)
        {
            if (TryWrite(item)) return;
            if (_completed) throw new InvalidOperationException("Pipe has been completed");
            ct.ThrowIfCancellationRequested();
            SpinOnce(ref spin);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryRead(out T item)
    {
        var head = Volatile.Read(ref _head);
        var tail = Volatile.Read(ref _tail);
        if (head >= tail)
        {
            item = default!;
            return false;
        }

        int idx = (int)(head & _mask);
        item = _buffer[idx];
        _buffer[idx] = default!;
        Volatile.Write(ref _head, head + 1);
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Read(CancellationToken ct)
    {
        var spin = new SpinWait();
        while (true)
        {
            if (TryRead(out var item)) return item;
            if (_completed) throw new OperationCanceledException("Pipe completed", ct);
            ct.ThrowIfCancellationRequested();
            SpinOnce(ref spin);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Complete() => _completed = true;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
    {
        Complete();
        GC.SuppressFinalize(this);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void SpinOnce(ref SpinWait spin)
    {
        if (_waitStrategy == WaitStrategy.Spin) { spin.SpinOnce(); return; }
        if (spin.Count < 20) spin.SpinOnce(); else Thread.Yield();
    }
}

/// <summary>
/// Latest-wins snapshot. No queue. Producer publishes, consumer reads the freshest value.
/// Use this for Game → Render views, HUD data, etc.
/// </summary>
internal sealed class Snapshot<T>
{
    private T _a = default!;
    private T _b = default!;
    private int _which;   // 0 => _a, 1 => _b
    private int _version; // incremented on publish

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Publish(in T value)
    {
        int next = Volatile.Read(ref _which) ^ 1; // flip slot
        if (next == 0) _a = value; else _b = value;
        Volatile.Write(ref _which, next);
        Interlocked.Increment(ref _version);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (T value, int version) Read()
    {
        while (true)
        {
            var v1 = Volatile.Read(ref _version);
            var w = Volatile.Read(ref _which);
            var v = (w == 0) ? _a : _b;
            var v2 = Volatile.Read(ref _version);
            if (v1 == v2) return (v, v2);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int WaitForChange(int lastVersion, CancellationToken ct)
    {
        var spin = new SpinWait();
        while (Volatile.Read(ref _version) == lastVersion)
        {
            ct.ThrowIfCancellationRequested();
            spin.SpinOnce();
        }
        return Volatile.Read(ref _version);
    }
}

/// <summary>
/// Coalescing control queue for side-band commands (e.g., resize, vsync toggle).
/// Multiple posts of the same key overwrite the previous one. Drain once per frame.
/// Thread-safe for many producers, one consumer.
/// </summary>
internal sealed class ControlQueue<T>(Func<T, int> keySelector, int? maxItems = null)
{
    private readonly ConcurrentDictionary<int, T> _latest = new();
    private readonly Func<T, int> _keyOf = keySelector ?? throw new ArgumentNullException(nameof(keySelector));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Post(in T message)
    {
        var key = _keyOf(message);

        // Unbounded: just coalesce
        if (maxItems is not { } cap)
        {
            _latest[key] = message;
            return;
        }

        // If the key already exists, always coalesce (do NOT drop).
        if (_latest.ContainsKey(key))
        {
            _latest[key] = message;
            return;
        }

        // New key: enforce capacity.
        if (_latest.Count >= cap)
            return; // drop newest when saturated

        if (!_latest.TryAdd(key, message))
        {
            _latest[key] = message;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Drain(Action<T> apply)
    {
        foreach (var kvp in _latest)
        {
            if (_latest.TryRemove(kvp.Key, out var msg))
                apply(msg);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear() => _latest.Clear();
}

internal class ControlBusPort<T>(ControlQueue<T> q) : IControlBus<T>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public void Post(in T message) => q.Post(message);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public void Drain(Action<T> apply) => q.Drain(apply);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public void Clear() => q.Clear();
}

internal sealed class RenderCommandBusPort(ControlQueue<RenderControl> q) : ControlBusPort<RenderControl>(q), IRenderCommandBus;