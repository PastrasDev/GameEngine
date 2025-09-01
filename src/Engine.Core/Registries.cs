using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

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

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public bool Add<T>(T instance) where T : class => _map.TryAdd(typeof(T), IsNotNull(instance));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public void Replace<T>(T instance) where T : class => _map[typeof(T)] = IsNotNull(instance);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public bool Remove<T>() where T : class => _map.TryRemove(typeof(T), out _);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public T Get<T>() where T : class => IsType<T>(_map.GetValueOrDefault(typeof(T)));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public bool TryGet<T>(out T? service) where T : class => _map.GetValueOrDefault(typeof(T)) is T t ? (service = t) != null : (service = null) == null;
}