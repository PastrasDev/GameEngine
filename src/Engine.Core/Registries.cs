using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Engine.Core;

public interface IRegistry
{
    bool Add<T>(T service) where T : class => false;
    void Replace<T>(T service) where T : class { }
    bool Remove<T>() where T : class => false;
    T Get<T>() where T : class => null!;
    bool TryGet<T>(out T? service) where T : class { service = null; return false; }

    static IRegistry Create() => new Registry();
}

internal sealed class Registry : IRegistry
{
    private readonly ConcurrentDictionary<Type, object> _map = new();
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public bool Add<T>(T instance) where T : class => _map.TryAdd(typeof(T), instance ?? throw new ArgumentNullException(nameof(instance)));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public void Replace<T>(T instance) where T : class => _map[typeof(T)] = instance ?? throw new ArgumentNullException(nameof(instance));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public bool Remove<T>() where T : class => _map.TryRemove(typeof(T), out _);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public T Get<T>() where T : class { if (_map.TryGetValue(typeof(T), out var obj) && obj is T t) return t; throw new InvalidOperationException($"Service not found: {typeof(T).FullName}"); }
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public bool TryGet<T>(out T? service) where T : class { if (_map.TryGetValue(typeof(T), out var obj) && obj is T t) { service = t; return true; } service = null; return false; }
}

public interface IRenderProxyRegistry
{
    void Add(RenderProxy proxy);
    bool Remove(RenderProxy proxy);

    /// <summary>Monotonic version; increments on add/remove.</summary>
    int Version { get; }

    /// <summary>Thread-safe snapshot of current proxies.</summary>
    IReadOnlyList<RenderProxy> Snapshot();
    
    static IRenderProxyRegistry Create() => new RenderProxyRegistry();
}

internal sealed class RenderProxyRegistry : IRenderProxyRegistry
{
    private readonly ConcurrentDictionary<RenderProxy, byte> _proxies = new();
    private int _version;

    public int Version => Volatile.Read(ref _version);

    public void Add(RenderProxy proxy)
    {
        ArgumentNullException.ThrowIfNull(proxy);
        if (_proxies.TryAdd(proxy, 0))
            Interlocked.Increment(ref _version);
    }

    public bool Remove(RenderProxy? proxy)
    {
        if (proxy == null || !_proxies.TryRemove(proxy, out _))
        {
            return false;
        }
        
        Interlocked.Increment(ref _version);
        return true;
    }

    public IReadOnlyList<RenderProxy> Snapshot()
    {
        // Cheap, lock-free snapshot (order later in driver).
        return _proxies.Keys.ToArray();
    }
}