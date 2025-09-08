using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Engine.Core;

public sealed class ModuleLoader
{
	private static Type[] _mainModules = [];
	private static Type[] _gameModules = [];
	private static Type[] _renderModules = [];

	public required bool Enabled
	{
		init
		{
			if (!value) return;
			var all = FindModuleTypes();
			_mainModules = SortTypes(all.Where(t => GetAffinity(t) == Threads.Main).ToList()).ToArray();
			_gameModules = SortTypes(all.Where(t => GetAffinity(t) == Threads.Game).ToList()).ToArray();
			_renderModules = SortTypes(all.Where(t => GetAffinity(t) == Threads.Render).ToList()).ToArray();
		}
	}

	public void Load(EngineKernel kernel)
	{
		require(kernel);

		Type[] selected = kernel.Affinity switch
		{
			Threads.Main => _mainModules,
			Threads.Game => _gameModules,
			Threads.Render => _renderModules,
			_ => throw new InvalidOperationException($"Unsupported kernel affinity: {kernel.Affinity}")
		};

		kernel.LoadModules(selected);
	}

	private static List<Type> FindModuleTypes()
	{
		var results = new List<Type>(64);

		foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
		{
			Type[] types;
			try
			{
				types = asm.GetTypes();
			}
			catch (ReflectionTypeLoadException rtle)
			{
				types = rtle.Types.Where(t => t is { }).ToArray()!;
			}
			catch
			{
				continue;
			}

			foreach (var t in types)
			{
				if (!t.IsClass || t.IsAbstract) continue;
				var bt = t.BaseType;
				if (bt is null || !bt.IsGenericType) continue;
				if (bt.GetGenericTypeDefinition() != typeof(EngineModule<>)) continue;

				results.Add(t);
			}
		}

		return results;
	}

	private static Threads GetAffinity(Type t) => t.GetCustomAttribute<AffinityAttribute>(inherit: false)?.Threads ?? Threads.None;

	private static Type[] GetDeps(Type t) => t.GetCustomAttributes<RequiresAttribute>(inherit: false).SelectMany(a => a.Types ?? []).ToArray();

	private static List<Type> SortTypes(List<Type> list)
	{
		if (list.Count == 0) return [];

		var set = new HashSet<Type>(list);
		var indeg = new Dictionary<Type, int>(list.Count);
		var edges = new Dictionary<Type, List<Type>>(list.Count);

		foreach (var t in list)
		{
			indeg[t] = 0;
			edges[t] = [];
		}

		foreach (var t in list)
		{
			foreach (var dep in GetDeps(t))
			{
				if (!set.Contains(dep)) continue;
				edges[dep].Add(t);
				indeg[t] += 1;
			}
		}

		var q = new Queue<Type>(indeg.Where(kv => kv.Value == 0).Select(kv => kv.Key).OrderBy(t => t.FullName));
		var order = new List<Type>(list.Count);

		while (q.Count > 0)
		{
			var u = q.Dequeue();
			order.Add(u);
			foreach (var v in edges[u])
			{
				if (--indeg[v] == 0) q.Enqueue(v);
			}
		}

		return order.Count != list.Count ? throw new InvalidOperationException("Cyclical module dependency detected.") : order;
	}
}