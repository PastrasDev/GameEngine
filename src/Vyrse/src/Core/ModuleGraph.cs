using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Vyrse.Core;

public sealed class ModuleGraph(Context context) : Executable
{
    public override bool Enabled => true;

    private readonly List<IModule> _modules = [];

    public override void Execute()
    {
        var types = TopologicallySort(FindAllModules());
        var contextType = context.GetType();

        foreach (var type in types)
        {
            if (!typeof(IModule).IsAssignableFrom(type))
            {
                throw new InvalidOperationException($"Type {type.FullName} does not implement {nameof(IModule)}.");
            }

            IModule module;

            var ctorWithContext = type.GetConstructor([contextType]);
            if (ctorWithContext is not null)
            {
                try
                {
                    module = (IModule)ctorWithContext.Invoke([context])!;
                }
                catch (TargetInvocationException ex)
                {
                    throw new InvalidOperationException($"Constructor {type.FullName}({contextType.Name}) threw an exception.", ex.InnerException ?? ex);
                }
            }
            else
            {
                var defaultCtor = type.GetConstructor(Type.EmptyTypes);
                if (defaultCtor is not null)
                {
                    try
                    {
                        module = (IModule)defaultCtor.Invoke(null)!;
                    }
                    catch (TargetInvocationException ex)
                    {
                        throw new InvalidOperationException($"Parameterless constructor for {type.FullName} threw an exception.", ex.InnerException ?? ex);
                    }
                }
                else
                {
                    throw new InvalidOperationException($"Module type {type.FullName} must define either " + $"public {type.Name}({contextType.Name} context) or public {type.Name}().");
                }
            }

            _modules.Add(module);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Load() { foreach (var module in _modules) module.Load(); }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Initialize() { foreach (var module in _modules) module.Initialize(); }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Start() { foreach (var module in _modules) module.Start(); }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void PreUpdate() { foreach (var module in _modules) module.PreUpdate(); }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Update() { foreach (var module in _modules) module.Update(); }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void PostUpdate() { foreach (var module in _modules) module.PostUpdate(); }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void FixedUpdate() { foreach (var module in _modules) module.FixedUpdate(); }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Shutdown() { for (var i = _modules.Count - 1; i >= 0; i--) _modules[i].Shutdown(); }

    private static Type[] FindAllModules()
    {
        var interfaceType = typeof(IModule);
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var result = new List<Type>();

        foreach (var assembly in assemblies)
        {
            Type?[] types;

            try
            {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                types = ex.Types;
            }

            foreach (var type in types)
            {
                if (type == null) continue;
                if (!type.IsClass) continue;
                if (type.IsAbstract) continue;
                if (!interfaceType.IsAssignableFrom(type)) continue;
                if (type.IsGenericType || type.ContainsGenericParameters) continue;
                result.Add(type);
            }
        }

        return result.ToArray();
    }

    private static Type[] TopologicallySort(Type[] moduleTypes)
    {
        ArgumentNullException.ThrowIfNull(moduleTypes);

        var count = moduleTypes.Length;
        var dependencies = new Type[count][];
        var state = new byte[count]; // 0 = unvisited, 1 = visiting, 2 = visited
        var result = new List<Type>(count);

        for (var i = 0; i < count; i++)
        {
            var type = moduleTypes[i];
            if (type == null)
                throw new ArgumentException("Module type array contains null entries.", nameof(moduleTypes));

            if (!typeof(IModule).IsAssignableFrom(type))
                throw new ArgumentException("All types must implement IModule.", nameof(moduleTypes));

            dependencies[i] = GetDependenciesFromAttributes(type);
        }

        for (var i = 0; i < count; i++)
        {
            if (state[i] == 0)
                Visit(i, moduleTypes, dependencies, state, result);
        }

        return result.ToArray();
    }

    private static void Visit(int index, Type[] moduleTypes, Type[][] dependencies, byte[] state, List<Type> result)
    {
        var s = state[index];

        if (s == 1) throw new InvalidOperationException("Cycle detected in module dependencies involving: " + moduleTypes[index].FullName);
        if (s == 2) return;

        state[index] = 1;

        var deps = dependencies[index];
        foreach (var depType in deps)
        {
            var depIndex = IndexOf(moduleTypes, depType);
            if (depIndex == -1)
            {
                throw new InvalidOperationException("Module " + moduleTypes[index].FullName + " depends on " + depType.FullName + " which is not present in the module set.");
            }

            Visit(depIndex, moduleTypes, dependencies, state, result);
        }

        state[index] = 2;
        result.Add(moduleTypes[index]);
    }

    private static int IndexOf(Type[] types, Type type)
    {
        for (var i = 0; i < types.Length; i++)
        {
            if (types[i] == type)
                return i;
        }

        return -1;
    }

    private static Type[] GetDependenciesFromAttributes(Type type)
    {
        var attrs = type.GetCustomAttributes(inherit: false);
        if (attrs.Length == 0)
            return [];

        var openGeneric = typeof(DependencyAttribute<>);
        var temp = new List<Type>();

        foreach (var attr in attrs)
        {
            var attrType = attr.GetType();
            if (!attrType.IsGenericType)
                continue;

            if (attrType.GetGenericTypeDefinition() != openGeneric)
                continue;

            var prop = attrType.GetProperty("Type");
            if (prop == null)
                continue;

            var value = prop.GetValue(attr) as Type;
            if (value == null)
                continue;

            if (value == type)
            {
                throw new InvalidOperationException("Module " + type.FullName + " cannot declare a dependency on itself.");
            }

            temp.Add(value);
        }

        return temp.Count == 0 ? [] : temp.ToArray();
    }
}

public interface IExecutable : IDisposable
{
    bool Enabled { get; }
    void Execute();
}

public abstract class Executable : IExecutable
{
    public abstract bool Enabled { get; }
    public abstract void Execute();

    public static T Execute<T>(T executable) where T : class, IExecutable
    {
        ArgumentNullException.ThrowIfNull(executable);

        if (!executable.Enabled)
            return executable;

        executable.Execute();
        executable.Dispose();

        return executable;
    }

    public virtual void Dispose() { }
}