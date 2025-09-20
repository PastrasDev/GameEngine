using System;
using System.Collections.Generic;

namespace Engine.Core;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public sealed class DependsOnAttribute<T> : Attribute
{
    public Type Type => typeof(T);

    public static Type[]? GetTypes(Type type)
    {
        List<Type>? list = null;
        foreach (var cad in type.GetCustomAttributesData())
        {
            var at = cad.AttributeType;
            if (at.IsGenericType && at.GetGenericTypeDefinition() == typeof(DependsOnAttribute<>))
                (list ??= []).Add(at.GenericTypeArguments[0]);
        }
        return list?.ToArray();
    }
}

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class AffinityAttribute(Threads threads) : Attribute
{
    public Threads Threads { get; } = threads;
}