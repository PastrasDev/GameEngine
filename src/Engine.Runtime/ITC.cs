using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading;
using Engine.Core;

namespace Engine.Runtime;

internal enum PipeFullMode
{
    /// Producer waits until space exists.
    Wait,
    /// Producer fails fast when full (TryWrite returns false; Write spins).
    DropWrite
}

internal enum WaitStrategy
{
    /// Tight spinWait
    Spin,
    /// Spin then Yield
    Hybrid
}

internal sealed class MainChannelsView(EngineChannels c) : IMainChannels
{
    public IWritePort<FrameStart> FrameWriter { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => c.Frame.Writer; }
    public IWritePort<InputSnapshot> InputWriter { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => c.Input.Writer; }
    public IRenderCommandBus RenderCommands { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => c.RenderCommands; }
}

internal sealed class GameChannelsView(EngineChannels c) : IGameChannels
{
    public IReadPort<FrameStart> FrameReader { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => c.Frame.Reader; }
    public IReadPort<InputSnapshot> InputReader { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => c.Input.Reader; }
    public ISnapshotWrite<SceneView> SceneWriter { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => c.Scene.Writer; }
}

internal sealed class RenderChannelsView(EngineChannels c) : IRenderChannels
{
    public ISnapshotRead<SceneView> SceneReader { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => c.Scene.Reader; }
    public IRenderCommandBus RenderCommands { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => c.RenderCommands; }
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


internal class ControlBusPort<T>(ControlQueue<T> q) : IControlBus<T>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public void Post(in T message) => q.Post(message);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public void Drain(Action<T> apply) => q.Drain(apply);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public void Clear() => q.Clear();
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
    private static bool IsFull(long head, long tail, int mask)
    {
        // occupancy = tail - head; full when occupancy > mask (== capacity)
        return (tail - head) > mask;
    }
    
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


            if (_completed)
                throw new InvalidOperationException("Pipe has been completed");


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
        _buffer[idx] = default!; // allow GC for ref types (no-op for structs)
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
            
            if (_completed)
                throw new OperationCanceledException("Pipe completed", ct);
            
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
        if (_waitStrategy == WaitStrategy.Spin)
        {
            spin.SpinOnce();
            return;
        }
        // Hybrid: brief spin then yield to avoid long stalls.
        if (spin.Count < 20)
        {
            spin.SpinOnce();
        }
        else
        {
            Thread.Yield();
        }
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
    private int _which; // 0 => _a, 1 => _b
    private int _version; // incremented on publish
    
    /// <summary>
    /// Publish a new value (latest wins). Safe with one producer.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Publish(in T value)
    {
        int next = Volatile.Read(ref _which) ^ 1; // flip slot
        if (next == 0) _a = value; else _b = value;
        Volatile.Write(ref _which, next);
        Interlocked.Increment(ref _version);
    }
    
    /// <summary>
    /// Returns a stable read (value + version). If writer races, we retry.
    /// </summary>
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
    
    /// <summary>
    /// Busy-wait until version changes. Useful if the consumer wants to wait for new data.
    /// </summary>
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

        // Try to add (another thread might add concurrently).
        if (!_latest.TryAdd(key, message))
        {
            // If someone just added this key, coalesce.
            _latest[key] = message;
        }
    }
    
    /// <summary>
    /// Applies all pending messages at most once. Call before rendering.
    /// </summary>
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

internal sealed class RenderCommandBusPort(ControlQueue<RenderControl> q) : ControlBusPort<RenderControl>(q), IRenderCommandBus;