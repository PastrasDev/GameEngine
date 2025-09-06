using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Engine.Core;

/// <summary>
/// Registry with lock-free reads, thread-safe writes (copy-on-write), and parent chaining.
/// Public API matches the previous version: Root, ReadOnly, Local, Add/Replace/Remove/Get/TryGet.
/// </summary>
public sealed class Registry
{
    private readonly Registry? _parent;
    public AccessType Access { get; }
    
    /// <summary>
    /// Immutable, array-backed snapshot of the services map.
    /// We publish a brand-new snapshot on each write; readers use Volatile.Read to obtain it.
    /// </summary>
    private ServiceMap _map = ServiceMap.Empty;

    /// <summary>Short critical section for copy-on-write updates.</summary>
    private readonly object _writeLock = new();

    /// <summary>Cached read-only wrapper (safe to cache; immutable).</summary>
    private Registry? _cachedReadOnly;

    public bool IsRoot => _parent is null;
    public bool IsParent => _parent is not null;

    private Registry(Registry? parent, AccessType access)
    {
        _parent = parent;
        Access = access;
    }

    /// <summary>Create a fresh, writable root registry.</summary>
    public static Registry Root
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(null, AccessType.ReadWrite);
    }

    /// <summary>Read-only view that becomes the parent for children (locals).</summary>
    public Registry ReadOnly
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _cachedReadOnly ??= new Registry(this, AccessType.ReadOnly);
    }

    /// <summary>
    /// Create a new writable child that inherits from this registry’s read-only view.
    /// Always returns a distinct instance (no caching), so separate threads get isolated locals.
    /// </summary>
    public Registry Local
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            require(Access == AccessType.ReadWrite, true);
            return new Registry(ReadOnly, AccessType.ReadWrite);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Add<T>(T instance) where T : class
    {
        require(Access == AccessType.ReadWrite, true);
        require(instance);
        
        lock (_writeLock)
        {
            var current = Volatile.Read(ref _map);
            var updated = current.WithAdded(typeof(T), instance, out bool added);
            if (!added) return false;
            Volatile.Write(ref _map, updated);
            return true;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Replace<T>(T instance) where T : class
    {
        require(Access == AccessType.ReadWrite, true);
        require(instance);

        lock (_writeLock)
        {
            var current = Volatile.Read(ref _map);
            var updated = current.WithReplaced(typeof(T), instance, out bool replaced);
            if (!replaced) return false;
            Volatile.Write(ref _map, updated);
            return true;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Remove<T>() where T : class
    {
        require(Access == AccessType.ReadWrite, true);

        lock (_writeLock)
        {
            var current = Volatile.Read(ref _map);
            var updated = current.WithRemoved(typeof(T), out bool removed);
            if (!removed) return false;
            Volatile.Write(ref _map, updated);
            return true;
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Get<T>() where T : class
    {
        var key = typeof(T);
        for (var r = this; r is not null; r = r._parent)
        {
            var map = Volatile.Read(ref r._map);
            if (map.TryGet(key, out var obj) && obj is T t)
                return t;
        }

        throw new KeyNotFoundException($"Service not found: {typeof(T).FullName}");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGet<T>(out T? service) where T : class
    {
        var key = typeof(T);
        for (var r = this; r is not null; r = r._parent)
        {
            var map = Volatile.Read(ref r._map);
            if (!map.TryGet(key, out var obj) || obj is not T t) continue;
            service = t;
            return true;
        }

        service = null;
        return false;
    }
    
    private sealed class ServiceMap
    {
        private readonly Type?[] _keys;
        private readonly object?[] _vals;
        private readonly int _mask;
        private readonly int _count;

        public static readonly ServiceMap Empty = new(capacity: 8);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ServiceMap(int capacity)
        {
            int cap = NextPow2(max(8, capacity));
            _keys = new Type?[cap];
            _vals = new object?[cap];
            _mask = cap - 1;
            _count = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ServiceMap(Type?[] keys, object?[] vals, int count)
        {
            _keys = keys;
            _vals = vals;
            _mask = keys.Length - 1;
            _count = count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int NextPow2(int v)
        {
            v--;
            v |= v >> 1;
            v |= v >> 2;
            v |= v >> 4;
            v |= v >> 8;
            v |= v >> 16;
            v++;
            return v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Hash(Type t) => t.TypeHandle.Value.GetHashCode();

        /// <summary>Lock-free read: probe snapshot arrays.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGet(Type key, out object? value)
        {
            var keys = _keys;
            var vals = _vals;
            int mask = _mask;
            int i = Hash(key) & mask;

            while (true)
            {
                var kt = Volatile.Read(ref keys[i]);
                if (kt is null)
                {
                    value = null;
                    return false;
                }

                if (ReferenceEquals(kt, key))
                {
                    value = vals[i];
                    return true;
                }

                i = (i + 1) & mask;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ServiceMap WithAdded(Type key, object value, out bool added)
        {
            if (TryGet(key, out _))
            {
                added = false;
                return this;
            }

            int needed = _count + 1;
            int cap = _keys.Length;
            if (needed > (int)(cap * 0.7f)) cap <<= 1;

            var (nk, nv, count) = Rebuild(cap, skipKey: null, replaceKey: null, replaceVal: null);
            Insert(nk, nv, key, value);
            added = true;
            return new ServiceMap(nk, nv, count + 1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ServiceMap WithReplaced(Type key, object value, out bool replaced)
        {
            if (!TryGet(key, out _))
            {
                replaced = false;
                return this;
            }

            var (nk, nv, count) = Rebuild(_keys.Length, skipKey: null, replaceKey: key, replaceVal: value);
            replaced = true;
            return new ServiceMap(nk, nv, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ServiceMap WithRemoved(Type key, out bool removed)
        {
            if (!TryGet(key, out _))
            {
                removed = false;
                return this;
            }

            int cap = _keys.Length;
            if (_count - 1 < (cap >> 2) && cap > 8) cap >>= 1;

            var (nk, nv, count) = Rebuild(cap, skipKey: key, replaceKey: null, replaceVal: null);
            removed = true;
            return new ServiceMap(nk, nv, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private (Type?[] keys, object?[] vals, int count) Rebuild(int capacity, Type? skipKey, Type? replaceKey, object? replaceVal)
        {
            var nk = new Type?[capacity];
            var nv = new object?[capacity];
            int mask = capacity - 1;
            int count = 0;

            for (int i = 0; i < _keys.Length; i++)
            {
                var k = _keys[i];
                if (k is null) continue;
                if (skipKey is not null && ReferenceEquals(k, skipKey)) continue;

                if (replaceKey is not null && ReferenceEquals(k, replaceKey))
                    InsertAll(k, replaceVal!);
                else
                    InsertAll(k, _vals[i]!);
            }

            return (nk, nv, count);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            void InsertAll(Type k, object v)
            {
                int i = Hash(k) & mask;
                while (true)
                {
                    if (nk[i] is null)
                    {
                        nk[i] = k;
                        nv[i] = v;
                        count++;
                        return;
                    }

                    i = (i + 1) & mask;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Insert(Type?[] keys, object?[] vals, Type k, object v)
        {
            int mask = keys.Length - 1;
            int i = Hash(k) & mask;
            while (true)
            {
                if (keys[i] is null)
                {
                    keys[i] = k;
                    vals[i] = v;
                    return;
                }

                i = (i + 1) & mask;
            }
        }
    }
}