using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Engine.Core;

public static class ModuleLoader
{
    public static void Load(EngineKernel kernel)
    {
        require(kernel);
        
        // 1) Discover all EngineModule<> types and read their static descriptors.
        var allDescriptors = FindDescriptors();

        // 2) Keep only modules that match this kernel’s thread affinity.
        var local = allDescriptors.Where(d => d.Affinity == kernel.Affinity).ToList();

        // 3) Topologically sort by dependencies (considering only in-affinity dependencies).
        var sorted = Sort(local);

        // 4) Hand the ordered list to the kernel; do NOT instantiate here.
        kernel.LoadModules(sorted);
        kernel.PostLoadModules();
    }
    
    private static List<ModuleDescriptor> FindDescriptors()
    {
        var results = new List<ModuleDescriptor>(64);

        foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
        {
            Type[] types;
            try { types = asm.GetTypes(); }
            catch (ReflectionTypeLoadException rtle) { types = rtle.Types.Where(t => t is not null).ToArray()!; }
            catch { continue; }

            foreach (var t in types)
            {
                if (!t.IsClass || t.IsAbstract) continue;

                // Must directly derive from EngineModule<TSelf> (closed)
                var bt = t.BaseType;
                if (bt is null || !bt.IsGenericType) continue;
                if (bt.GetGenericTypeDefinition() != typeof(EngineModule<>)) continue;

                // Static field "Descriptor" lives on the *base* closed generic type (EngineModule<TSelf>)
                var f = bt.GetField("Descriptor", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

                if (f?.GetValue(null) is ModuleDescriptor md)
                    results.Add(md);
            }
        }

        return results;
    }
    
    private static List<ModuleDescriptor> Sort(List<ModuleDescriptor> list)
    {
        if (list.Count == 0) return [];

        // Type → descriptor
        var map = list.ToDictionary(d => d.Type);

        // Build adjacency + indegrees, but only for dependencies that are present in this set
        var edges = new Dictionary<Type, List<Type>>(map.Count);
        var indeg = new Dictionary<Type, int>(map.Count);
        foreach (var d in list)
        {
            indeg[d.Type] = 0;
            edges[d.Type] = [];
        }

        foreach (var d in list)
        {
            var deps = d.Dependencies ?? [];
            foreach (var dep in deps)
            {
                if (!map.ContainsKey(dep)) continue; // ignore out-of-affinity or external deps
                edges[dep].Add(d.Type);
                indeg[d.Type] = indeg[d.Type] + 1;
            }
        }

        // Kahn’s algorithm
        var q = new Queue<Type>(indeg.Where(kv => kv.Value == 0).Select(kv => kv.Key));
        var order = new List<ModuleDescriptor>(list.Count);

        while (q.Count > 0)
        {
            var u = q.Dequeue();
            order.Add(map[u]);

            foreach (var v in edges[u])
            {
                indeg[v] -= 1;
                if (indeg[v] == 0) q.Enqueue(v);
            }
        }

        return order.Count == list.Count ? order : throw new InvalidOperationException($"Cyclical module dependency detected: {DescribeCycle(edges, indeg)}");
    }
    
    private static string DescribeCycle(Dictionary<Type, List<Type>> edges, Dictionary<Type, int> indeg)
    {
        // Nodes with indegree > 0 are part of at least one cycle or blocked by a cycle.
        var nodes = indeg.Where(kv => kv.Value > 0).Select(kv => kv.Key).ToHashSet();

        // DFS to find a single cycle and stringify it.
        var color = new Dictionary<Type, int>(); // 0=unvisited,1=visiting,2=done
        var stack = new Stack<Type>();

        foreach (var n in nodes)
        {
            if (Visit(n)) break;
        }

        return "<unresolved cycle>";

        bool Visit(Type u)
        {
            if (!nodes.Contains(u)) return false;
            if (color.TryGetValue(u, out var c))
            {
                if (c == 1)
                {
                    // Found a back-edge; unwind stack to build the cycle
                    var cycle = new List<string> { TypeName(u) };
                    foreach (var t in stack)
                    {
                        cycle.Add(TypeName(t));
                        if (t == u) break;
                    }
                    cycle.Reverse();
                    throw new InvalidOperationException("  " + string.Join(" -> ", cycle));
                }
                if (c == 2) return false;
            }

            color[u] = 1;
            stack.Push(u);
            if (edges.TryGetValue(u, out var outs))
            {
                foreach (var v in outs) 
                    if (Visit(v)) return true;
            }
            stack.Pop();
            color[u] = 2;
            return false;
        }

        static string TypeName(Type t) => t.FullName ?? t.Name;
    }
}