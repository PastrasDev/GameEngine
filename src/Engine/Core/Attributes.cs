using System;

namespace Engine.Core;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public sealed class RequiresAttribute(params Type[]? types) : Attribute
{
    public Type[] Types { get; } = types ?? [];
}

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class AffinityAttribute(Threads threads) : Attribute
{
    public Threads Threads { get; } = threads;
}