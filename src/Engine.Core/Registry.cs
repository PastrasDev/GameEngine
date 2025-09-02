using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Engine.Core;

public interface IRegistry
{
    IRegistry Local { get; }
    IRegistry ReadOnly { get; }
    
    bool Add<T>(T service) where T : class;
    bool Replace<T>(T service) where T : class;
    bool Remove<T>() where T : class;
    T Get<T>() where T : class;
    bool TryGet<T>(out T? service) where T : class;
    
    static IRegistry Create() => Registry.Root;
}

internal sealed class Registry(Registry? parent, bool @readonly) : IRegistry
{
    private readonly ConcurrentDictionary<Type, object> _local = new();
    
    private Registry? Parent { get; } = parent;
    public bool IsReadOnly { get; } = @readonly;
    
    private Registry? _cachedReadOnly;
    private Registry? _cachedLocal;
    
    public bool IsRoot => Parent is null;
    public bool IsParent => Parent is not null;

    public static IRegistry Root
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Registry(null, false);
    }
    
    public IRegistry ReadOnly
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _cachedReadOnly ??= new Registry(this, true);
    }
    
    public IRegistry Local
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            require(IsReadOnly, false);
            return _cachedLocal ??= new Registry((Registry)ReadOnly, false);
        } 
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IRegistry Create() => Root;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Add<T>(T instance) where T : class
    {
        require(IsReadOnly, false);
        require(instance);
        return _local.TryAdd(typeof(T), instance);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Remove<T>() where T : class
    {
        require(IsReadOnly, false);
        return _local.TryRemove(typeof(T), out _);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Replace<T>(T instance) where T : class
    {
        require(IsReadOnly, false);
        require(instance);
        
        var key = typeof(T);
        while (true)
        {
            if (!_local.TryGetValue(key, out var old))
                return false;
            
            if (_local.TryUpdate(key, instance, old))
                return true;
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Get<T>() where T : class
    {
        var key = typeof(T);
        for (var r = this; r is not null; r = r.Parent)
        {
            if (r._local.TryGetValue(key, out var obj) && obj is T t)
                return t;
        }
        throw new KeyNotFoundException($"Service not found: {typeof(T).FullName}");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGet<T>(out T? service) where T : class
    {
        var key = typeof(T);
        for (var r = this; r is not null; r = r.Parent)
        {
            if (!r._local.TryGetValue(key, out var obj) || obj is not T t) continue;
            service = t; 
            return true;
        }
        service = null; 
        return false;
    }
}