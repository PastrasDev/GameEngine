using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Vyrse.Core;

/// <summary>
/// Marks a type as discoverable by the engine.
/// Discoverable types have their static constructors forced at runtime, which is
/// where most registration (e.g., module/component metadata) is expected to occur.
/// </summary>
/// <remarks>
/// You can apply this attribute to abstract base types (e.g., <c>Module&lt;TSelf&gt;</c>) or
/// to concrete leaves.
/// - If applied to a base type with <see cref="Derived"/> = <c>true</c>, all subclasses
///   that do not declare their own attribute are also considered discoverable.
/// - If applied to a base type with <see cref="Derived"/> = <c>false</c>, only the base
///   itself is considered discoverable (subclasses must declare their own attribute).
/// - If applied directly to a concrete type, that type alone is considered discoverable.
/// </remarks>
[AttributeUsage(AttributeTargets.Class)]
public sealed class DiscoverableAttribute : Attribute
{
	/// <summary>
	/// Enables or disables discovery of the type that declares this attribute.
	/// </summary>
	/// <remarks>
	/// When <c>false</c>, the declaring type is excluded from discovery even if a base
	/// declares <see cref="Derived"/> = <c>true</c>.
	/// </remarks>
	public bool Enabled { get; init; } = true;

	/// <summary>
	/// Controls whether derived classes of this type are also considered discoverable
	/// when they do not declare their own <see cref="DiscoverableAttribute"/>.
	/// </summary>
	/// <remarks>
	/// - If <c>true</c>, derived classes will be included in discovery.
	/// - If <c>false</c>, only the declaring type is discoverable and subclasses must
	///   explicitly declare their own attribute to be included.
	/// </remarks>
	public bool Derived { get; init; } = true;

	private static readonly HashSet<Type> s_forced = [];
	private static readonly object s_lock = new();

	public static void ForceAll(bool futureAssemblies = false)
	{
		var domain = AppDomain.CurrentDomain;

		foreach (var asm in domain.GetAssemblies())
			ForceInAssembly(asm);

		if (futureAssemblies)
			domain.AssemblyLoad += (_, e) => ForceInAssembly(e.LoadedAssembly);
	}

	private static void ForceInAssembly(Assembly assembly)
	{
		Type[] types;
		try { types = assembly.GetTypes(); }
		catch (ReflectionTypeLoadException ex) { types = ex.Types!; }
		foreach (var type in types)
		{
			if (!IsConcreteClosedClass(type)) continue;
			if (!IsEffectivelyDiscoverable(type)) continue;
			lock (s_lock) { if (!s_forced.Add(type)) continue; }
			RuntimeHelpers.RunClassConstructor(type.TypeHandle);

			for (var cur = type.BaseType; cur is { } && cur != typeof(object); cur = cur.BaseType)
			{
				if (cur is { IsGenericType: true, ContainsGenericParameters: false })
				{
					RuntimeHelpers.RunClassConstructor(cur.TypeHandle);
				}
			}
		}
	}

	private static bool IsConcreteClosedClass(Type? t) => t is { IsClass: true, IsAbstract: false, ContainsGenericParameters: false };

	private static bool IsEffectivelyDiscoverable(Type? type)
	{
		if (type is null) return false;

		var own = type.GetCustomAttribute<DiscoverableAttribute>(inherit: false);
		if (own is { }) return own.Enabled;
		for (var b = type.BaseType; b is { }; b = b.BaseType)
		{
			var baseAttr = b.GetCustomAttribute<DiscoverableAttribute>(inherit: false);
			if (baseAttr is null) continue;
			return baseAttr is { Enabled: true, Derived: true };
		}

		return false;
	}
}

[AttributeUsage(AttributeTargets.Class)]
public sealed class ModuleAttribute(bool enabled = true) : Attribute
{
	public bool Enabled { get; init; } = enabled;
}

[AttributeUsage(AttributeTargets.Class)]
public sealed class ComponentAttribute(bool enabled = true) : Attribute
{
	public bool Enabled { get; init; } = enabled;
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class DependencyAttribute<T> : Attribute where T : class
{

}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class DependencyAttribute<T1, T2> : Attribute where T1 : class where T2 : class
{

}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class DependencyAttribute<T1, T2, T3> : Attribute where T1 : class where T2 : class where T3 : class
{

}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class DependencyAttribute<T1, T2, T3, T4> : Attribute where T1 : class where T2 : class where T3 : class where T4 : class
{

}