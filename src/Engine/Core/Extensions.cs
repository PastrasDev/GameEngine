using System.Reflection;
using System.Runtime.CompilerServices;

namespace Engine.Core;

public static partial class Extensions
{
	/// <summary>
	/// Topologically sorts <paramref name="items"/> using Kahn's algorithm.
	/// Dependencies that are not in the input set are ignored.
	/// Throws with a readable message if a cycle is detected.
	/// </summary>
	public static IEnumerable<T> TopologicalSort<T>(this IEnumerable<T> items, Func<T, Type> keyOf, Func<T, IEnumerable<Type>?> depsOf)
	{
        ArgumentNullException.ThrowIfNull(items);
        ArgumentNullException.ThrowIfNull(keyOf);
        ArgumentNullException.ThrowIfNull(depsOf);

        // Build index of nodes present
        var nodes = new Dictionary<Type, T>(64);
		foreach (var item in items)
		{
			var k = keyOf(item);
			if (!nodes.TryAdd(k, item)) throw new InvalidOperationException($"Duplicate node key found: {k.FullName}");
		}

		// Build graph (only edges to nodes that exist in the set)
		var incoming = new Dictionary<Type, int>(nodes.Count);
		var outgoing = new Dictionary<Type, List<Type>>(nodes.Count);

		foreach (var (k, item) in nodes)
		{
			var deps = depsOf(item);
			if (deps is null)
			{
				incoming[k] = incoming.GetValueOrDefault(k, 0);
				continue;
			}

			foreach (var d in deps)
			{
				if (!nodes.ContainsKey(d)) continue; // ignore external deps (modules, etc.)

				if (!outgoing.TryGetValue(d, out var list)) outgoing[d] = list = new List<Type>(2);
				list.Add(k);

				incoming[k] = incoming.TryGetValue(k, out var c) ? c + 1 : 1;
				if (!incoming.ContainsKey(d)) incoming[d] = incoming.GetValueOrDefault(d, 0);
			}

			if (!incoming.ContainsKey(k)) incoming[k] = incoming.GetValueOrDefault(k, 0);
		}

		// Seed with nodes that have no incoming edges
		var queue = new Queue<Type>();
		foreach (var (k, deg) in incoming)
		{
			if (deg == 0) queue.Enqueue(k);
		}

		int emitted = 0;
		while (queue.Count > 0)
		{
			var k = queue.Dequeue();
			yield return nodes[k];
			emitted++;

			if (!outgoing.TryGetValue(k, out var next)) continue;
			foreach (var to in next)
			{
				incoming[to] -= 1;
				if (incoming[to] == 0) queue.Enqueue(to);
			}
		}

		if (emitted == nodes.Count) yield break;
		{
			// Emit the remaining keys with positive in-degree to help debug cycles
			var cyc = new List<string>();
			foreach (var (k, deg) in incoming)
			{
				if (deg > 0) cyc.Add(k.FullName ?? k.Name);
			}

			throw new InvalidOperationException("Dependency cycle detected among components: " + string.Join(" ↔ ", cyc));
		}
	}
}

public static partial class Extensions
{
	public static int GetRequiredPropertyCount(this Type type) => type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Count(p => p.IsDefined(typeof(RequiredMemberAttribute), inherit: false));
	public static int GetRequiredFieldCount(this Type type) => type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Count(f => f.IsDefined(typeof(RequiredMemberAttribute), inherit: false));

	/// <summary>
	/// Finds all non-abstract classes assignable to <paramref name="baseType"/> in its assembly,
	/// and forces static constructors for each.
	/// </summary>
	public static void ForceStaticInitialization(this Type baseType)
	{
        ArgumentNullException.ThrowIfNull(baseType);
        var types = baseType.Assembly.GetTypes().Where(t => t is { IsClass: true, IsAbstract: false } && baseType.IsAssignableFrom(t));
		foreach (var t in types) RuntimeHelpers.RunClassConstructor(t.TypeHandle);
	}

	/// <summary>
	/// Finds all non-abstract classes assignable to <paramref name="baseType"/> in its assembly,
	/// and forces static constructors for each, plus for <paramref name="genericTemplate"/> closed over them.
	/// </summary>
	public static void ForceStaticInitialization(this Type baseType, Type genericTemplate)
	{
        ArgumentNullException.ThrowIfNull(baseType);
        ArgumentNullException.ThrowIfNull(genericTemplate);
        if (!genericTemplate.IsGenericTypeDefinition || genericTemplate.GetGenericArguments().Length != 1)
			throw new ArgumentException($"{genericTemplate.FullName} must be an open generic type with exactly one type parameter.", nameof(genericTemplate));
		var types = baseType.Assembly.GetTypes().Where(t => t is { IsClass: true, IsAbstract: false } && baseType.IsAssignableFrom(t));
		foreach (var t in types)
		{
			RuntimeHelpers.RunClassConstructor(t.TypeHandle);
			RuntimeHelpers.RunClassConstructor(genericTemplate.MakeGenericType(t).TypeHandle);
		}
	}
}