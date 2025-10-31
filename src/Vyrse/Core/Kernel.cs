using System;
using System.Linq;
using Vyrse.Core.Lifecycle;
using Vyrse.Debug;

namespace Vyrse.Core;

public sealed class Kernel
{
	private static Module.Metadata[] SortedModules { get; }
	private readonly Thunks[] _phases = new Thunks[IPhase.Length];

	public required Context Context { get; init; }

	static Kernel()
	{
		DiscoverableAttribute.ForceAll(true);
		var registry = Module.Metadata.Registry.Instance;
		var all = registry.Enumerate(global: true).Select(p => p.Value);
		SortedModules = all.TopologicalSort(m => m.Type, m => m.Dependencies.Select(d => d.Type)).ToArray();
	}

	public Application.ExitCode Run()
	{
		try
		{
			Load();
			Initialize();
			Start();

			while (Application.IsRunning())
			{
				PreUpdate();
				Update();
				PostUpdate();
				FixedUpdate();
			}

			Shutdown();
		}
		catch (OperationCanceledException)
		{
			return Application.ExitCode.Canceled;
		}
		catch (Exception ex)
		{
			Log.Error($"[Kernel] crashed with fatal exception: {ex}");
			return Application.ExitCode.Fatal;
		}

		return Application.ExitCode.Ok;
	}

	public void Load()
	{
		var metadata = SortedModules;
		var count = metadata.Length;

		for (int i = 0; i < _phases.Length; i++)
			_phases[i].Resize(count);

		var instances = new Module[count];
		for (int i = 0; i < count; i++)
		{
			unsafe
			{
				var data = metadata[i];
				var instance = data.Instantiate(Context);
				instances[i] = instance;
			}
		}

		foreach (var module in instances)
		{
			var thunks = module.Thunks;
			for (int i = 1; i < thunks.Length; i++)
			{
				ref readonly var t = ref thunks[i];
				if (t.IsNull) continue;
				var bound = new Thunk(module, t);
				_phases[i].Add(bound);
			}
		}

		_phases[7].Reverse();
	}

	public void Initialize() => _phases[1].Invoke();
	public void Start() => _phases[2].Invoke();

	public void FixedUpdate()
	{
		while (Context.Time.FixedUpdate()) _phases[3].Invoke();
	}

	public void PreUpdate() => _phases[4].Invoke();
	public void Update() => _phases[5].Invoke();

	public void PostUpdate()
	{
		_phases[6].Invoke();
		Context.Time.Update();
	}

	public void Shutdown() => _phases[7].Invoke();
}