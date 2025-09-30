using System.Reflection;
using System.Runtime.CompilerServices;

namespace Engine.Core
{
	namespace Extensions
	{
		public static class IEnumerable
		{
			/// <summary>
			/// Topologically sorts <paramref name="items"/> using Kahn's algorithm.
			/// Dependencies that are not in the input set are ignored.
			/// Throws with a readable message if a cycle is detected.
			/// </summary>
			public static IEnumerable<T> TopologicalSort<T>(this IEnumerable<T> items, Func<T, System.Type> keyOf, Func<T, IEnumerable<System.Type>?> depsOf)
			{
				ArgumentNullException.ThrowIfNull(items);
				ArgumentNullException.ThrowIfNull(keyOf);
				ArgumentNullException.ThrowIfNull(depsOf);

				var nodes = new Dictionary<System.Type, T>(64);
				foreach (var item in items)
				{
					var k = keyOf(item);
					if (!nodes.TryAdd(k, item))
						throw new InvalidOperationException($"Duplicate node key found: {k.FullName}");
				}

				var incoming = new Dictionary<System.Type, int>(nodes.Count);
				var outgoing = new Dictionary<System.Type, List<System.Type>>(nodes.Count);

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
						if (!nodes.ContainsKey(d)) continue;

						if (!outgoing.TryGetValue(d, out var list)) outgoing[d] = list = new List<System.Type>(2);
						list.Add(k);

						incoming[k] = incoming.TryGetValue(k, out var c) ? c + 1 : 1;
						if (!incoming.ContainsKey(d)) incoming[d] = incoming.GetValueOrDefault(d, 0);
					}

					if (!incoming.ContainsKey(k)) incoming[k] = incoming.GetValueOrDefault(k, 0);
				}

				var queue = new Queue<System.Type>();
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
					var cyc = new List<string>();
					foreach (var (k, deg) in incoming) if (deg > 0) cyc.Add(k.FullName ?? k.Name);
					throw new InvalidOperationException("Dependency cycle detected among components: " + string.Join(" ↔ ", cyc));
				}
			}
		}
	}
}