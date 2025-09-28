using System.Net.Sockets;
using System.Numerics;
using Engine.Core.Lifecycle;
using Engine.Core.Threading.Communication;
using Engine.Core.Threading.Communication.Messages;
using Engine.Debug;
using Engine.Platform.Windows;

namespace Engine.Core;

[Flags]
public enum Affinity { None = 0, Main = 1 << 0, Game = 1 << 1, Render = 1 << 2, }

public abstract class Kernel
{
	public Thread Thread { get; protected set; } = null!;
	public required Context Context { get; init; }
	public volatile Application.ExitCode Code;

	public enum LaunchWait { None, Initialized, Started }

	public abstract void Launch(LaunchWait waitFor = LaunchWait.None);
	protected abstract void Run();

	public abstract void Load();
	public abstract void Initialize();
	public abstract void Start();
	public abstract void FixedUpdate();
	public abstract void PreUpdate();
	public abstract void Update();
	public abstract void PostUpdate();
	public abstract void Shutdown();
}

public abstract class Kernel<TSelf> : Kernel, IPhases where TSelf : Kernel<TSelf>, IConfigure
{
	protected static Affinity Affinity { get; set; }
	protected static Module.Metadata[] SortedModules { get; }
	protected readonly Thunks[] Phases = new Thunks[IPhases.Length];

	private readonly ManualResetEventSlim _initializedEvt = new(false);
	private readonly ManualResetEventSlim _startedEvt = new(false);

	static Kernel()
	{
		TSelf.Configure();

		if (Affinity == Affinity.None)
			throw new InvalidOperationException($"Kernel<{typeof(TSelf).Name}> must set a valid Affinity in Configure().");

		var registry = Module.Metadata.Registry.Instance;
		var all = registry.Enumerate(global: true).Select(p => p.Value);
		var enabled = all.Where(md => md.Enabled);
		var filtered = enabled.Where(md => (md.Affinity & Affinity) != 0);
		SortedModules = filtered.TopologicalSort(m => m.Type, m => m.Dependencies.Select(d => d.Type)).ToArray();
	}

	protected void FillPhases()
	{
		var metadata = SortedModules;
		var length = metadata.Length;

		for (int i = 0; i < Phases.Length; i++)
			Phases[i].Resize(length);

		foreach (var data in metadata)
		{
			var thunks = data.Thunks.AsSpan();

			unsafe
			{
				var instance = data.Instantiate(Context);
				foreach (ref readonly var thunk in thunks)
				{
					if (thunk.IsNull) continue;
					var newThunk = new Thunk(instance, thunk);
					int idx = BitOperations.TrailingZeroCount((int)newThunk.Phase);
					Phases[idx].Add(newThunk);
				}
			}
		}

		Phases[7].Reverse();
	}

	public override void Launch(LaunchWait waitFor = LaunchWait.None)
	{
		Context.Affinity = Affinity;

		Thread = new Thread (() =>
		{
			Log.Core.Affinity = Affinity;

			try
			{
				Run();
			}
			catch (OperationCanceledException)
			{
				Code = Application.ExitCode.Canceled;
			}
			catch (Exception ex)
			{
				Code = Application.ExitCode.Fatal;
				Log.Error($"[{Affinity}] kernel crashed with fatal exception: {ex}");
			}
			finally
			{
				Code = Application.ExitCode.Ok;
			}
		})
		{
			Name = Affinity.ToString(),
			IsBackground = true,
		};

		if (Affinity == Affinity.Main)
			Thread.SetApartmentState(ApartmentState.STA);

		Thread.Start();

		switch (waitFor)
		{
			case LaunchWait.Initialized: _initializedEvt.Wait(Application.TokenSrc.Token); break;
			case LaunchWait.Started: _startedEvt.Wait(Application.TokenSrc.Token); break;
			case LaunchWait.None: break;
		}
	}

	public sealed override void Load() { FillPhases(); Phases[0].Invoke(); }
	public sealed override void Initialize() { Phases[1].Invoke(); _initializedEvt.Set(); }
	public sealed override void Start() { Phases[2].Invoke(); _startedEvt.Set(); }
	public sealed override void FixedUpdate() { while (Context.Time.FixedUpdate()) Phases[3].Invoke(); }
	public sealed override void PreUpdate() => Phases[4].Invoke();
	public sealed override void Update() => Phases[5].Invoke();
	public sealed override void PostUpdate() { Phases[6].Invoke(); Context.Time.Update(); }
	public sealed override void Shutdown() => Phases[7].Invoke();
}

public sealed class MainKernel : Kernel<MainKernel>, IConfigure
{
	public static void Configure()
	{
		Affinity = Affinity.Main;
	}

	protected override void Run()
	{
		Load();
		Initialize();
		Start();

		while (Application.IsRunning())
		{
			PreUpdate();
			Update();
			PostUpdate();
		}

		Shutdown();
	}
}

public sealed class GameKernel : Kernel<GameKernel>, IConfigure
{
	private Input Input { get; set; } = null!;
	private Time Time { get; set; } = null!;

	private IReadPort<InputSnapshot> InputReader { get; set; } = null!;
	private ISnapshotWrite<SceneView> SceneWriter { get; set; } = null!;

	public static void Configure()
	{
		Affinity = Affinity.Game;
	}

	protected override void Run()
	{
		Input = Context.Input;
		Time = Context.Time;

		InputReader = Context.MessageBus.Claim<InputSnapshot>().Read!;
		SceneWriter = Context.MessageBus.Claim<SceneView>().SnapshotWrite!;

		InputSnapshot snapshot = default;
		long lastProcessedInputFrame = -1;

		Load();
		Initialize();
		Start();

		while (Application.IsRunning())
		{
			while (InputReader!.TryRead(out var s))
				snapshot = s;

			if (snapshot.FrameId != lastProcessedInputFrame)
			{
				Input.Update(in snapshot);
				lastProcessedInputFrame = snapshot.FrameId;
			}

			FixedUpdate();
			PreUpdate();
			Update();
			PostUpdate();

			var sceneView = new SceneView
			{
				FrameIndex = (int)Time.FixedFrameIndex,
				Alpha = (float)Time.FixedAlpha
			};

			SceneWriter!.Publish(sceneView);
		}

		Shutdown();
	}
}

public sealed class RenderKernel : Kernel<RenderKernel>, IConfigure
{
	private ISnapshotRead<SceneView> SceneReader { get; set; } = null!;

	public static void Configure()
	{
		Affinity = Affinity.Render;
	}

	protected override void Run()
	{
		SceneReader = Context.MessageBus.Claim<SceneView>().SnapshotRead!;

		Load();
		Initialize();
		Start();

		while (Application.IsRunning())
		{
			var (view, version) = SceneReader!.Read();

			PreUpdate();
			Update();
			PostUpdate();
		}

		Shutdown();
	}
}