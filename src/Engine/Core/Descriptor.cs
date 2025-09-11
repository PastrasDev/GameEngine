using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Engine.Core;

public interface IDescriptor<TSelf, TData> where TSelf : IDescriptor<TSelf, TData>
{
    static abstract TData Describe();
}

public static class DescriptorRegistry
{
    private static Dictionary<Type, IReadOnlyDictionary<Type, object>>? _byDataType;

    private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<Type, object>> Mut = new();

    public static IReadOnlyDictionary<Type, TData> GetByOwner<TData>()
    {
        var by = Volatile.Read(ref _byDataType) ?? throw new InvalidOperationException("DescriptorRegistry not initialized.");
        return !by.TryGetValue(typeof(TData), out var map) ? throw new KeyNotFoundException($"Descriptor type not registered: {typeof(TData)}") : new ReadOnlyCastMap<TData>(map);
    }

    public static IEnumerable<TData> GetAll<TData>() => GetByOwner<TData>().Values;

    public static void Register<TSelf, TData>(TData data) where TSelf : IDescriptor<TSelf, TData>
    {
        var dict = Mut.GetOrAdd(typeof(TData), _ => new ConcurrentDictionary<Type, object>());
        dict[typeof(TSelf)] = data!;
    }

    #pragma warning disable CA2255
    [ModuleInitializer]
    internal static void Initialize()
    {
        BuildFrom(AppDomain.CurrentDomain.GetAssemblies());
        AppDomain.CurrentDomain.AssemblyLoad += (_, e) => BuildFrom([e.LoadedAssembly]);
    }
    #pragma warning restore CA2255

    private static void BuildFrom(IEnumerable<Assembly> assemblies)
    {
        foreach (var asm in assemblies)
        {
            foreach (var t in SafeGetTypes(asm))
            {
                if (!t.IsClass || t.IsAbstract || t.IsGenericTypeDefinition) continue;
                if (!ImplementsAnyDescriptor(t)) continue;

                RuntimeHelpers.RunClassConstructor(t.TypeHandle);

                for (var cur = t.BaseType; cur is { } && cur != typeof(object); cur = cur.BaseType)
                {
                    if (cur is { IsGenericType: true, IsGenericTypeDefinition: false })
                        RuntimeHelpers.RunClassConstructor(cur.TypeHandle);
                }
            }
        }

        var snapshot = new Dictionary<Type, IReadOnlyDictionary<Type, object>>(Mut.Count);

        foreach (var (dataType, dict) in Mut)
            snapshot[dataType] = new ReadOnlyDictionary<Type, object>(new Dictionary<Type, object>(dict));

        Volatile.Write(ref _byDataType, snapshot);
    }

    private static bool ImplementsAnyDescriptor(Type t)
    {
        foreach (var i in t.GetInterfaces())
        {
            if (!i.IsGenericType) continue;
            if (i.GetGenericTypeDefinition() == typeof(IDescriptor<,>))
                return true;
        }

        return false;
    }

    private static IEnumerable<Type> SafeGetTypes(Assembly asm)
    {
        try
        {
            return asm.GetTypes();
        }
        catch (ReflectionTypeLoadException e)
        {
            return e.Types.Where(x => x is { })!;
        }
        catch
        {
            return [];
        }
    }

    private sealed class ReadOnlyCastMap<TData>(IReadOnlyDictionary<Type, object> inner) : IReadOnlyDictionary<Type, TData>
    {
        public int Count => inner.Count;
        public IEnumerable<Type> Keys => inner.Keys;
        public IEnumerable<TData> Values => inner.Values.Cast<TData>();
        public bool ContainsKey(Type key) => inner.ContainsKey(key);

        public bool TryGetValue(Type key, out TData value)
        {
            if (inner.TryGetValue(key, out var obj) && obj is TData t)
            {
                value = t;
                return true;
            }

            value = default!;
            return false;
        }

        public TData this[Type key] => (TData)inner[key];

        public IEnumerator<KeyValuePair<Type, TData>> GetEnumerator()
        {
            foreach (var kv in inner)
                yield return new KeyValuePair<Type, TData>(kv.Key, (TData)kv.Value);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}