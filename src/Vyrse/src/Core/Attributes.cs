using System;

namespace Vyrse.Core;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ModuleAttribute : Attribute;

[AttributeUsage(AttributeTargets.Class)]
public sealed class ComponentAttribute : Attribute;

[AttributeUsage(AttributeTargets.Class)]
public sealed class KernelAttribute : Attribute;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class DependencyAttribute<T> : Attribute where T : IModule
{
    public Type Type { get; init; } = typeof(T);
}

[AttributeUsage(AttributeTargets.All)]
public sealed class EnabledAttribute : Attribute;

[AttributeUsage(AttributeTargets.All)]
public sealed class DisabledAttribute : Attribute;

public interface IModule : IPhase
{

}