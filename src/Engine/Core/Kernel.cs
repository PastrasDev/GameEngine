using System.Numerics;
using System.Runtime.CompilerServices;
using Engine.Core.Lifecycle;
using Engine.Core.Threading.Communication.Messages;
using Engine.Debug;

namespace Engine.Core;

[Flags]
public enum Affinity
{
	None = 0,
	Main = 1 << 0,
	Game = 1 << 1,
	Render = 1 << 2,
}

public sealed class PhaseBarrier
{
	public readonly ManualResetEventSlim MainStartDone = new(false);
	public readonly ManualResetEventSlim GameStartDone = new(false);
	public readonly ManualResetEventSlim RenderStartDone = new(false);
}

public abstract class Kernel
{
	public Thread Thread { get; protected set; } = null!;
	public required Context Context { get; init; }
	public volatile Application.ExitCode Code;

	public abstract void Launch();
}

public abstract class Kernel<TSelf> : Kernel, IPhases where TSelf : Kernel<TSelf>, IConfigure
{
	protected static Affinity Affinity { get; set; }

	static Kernel()
	{
		TSelf.Configure();

		if (Affinity == Affinity.None)
			throw new InvalidOperationException($"Kernel<{typeof(TSelf).Name}> must set a valid Affinity in Configure().");
	}

	private readonly Thunks[] _phases = new Thunks[IPhases.Length];


	public virtual void Load()
	{
		var registry = Module.Metadata.Registry.Instance;
		var all = registry.Enumerate(global: true).Select(p => p.Value);
		var enabled = all.Where(md => md.Enabled);
		var filtered = enabled.Where(md => (md.Affinity & Affinity) != 0);
		var metadata = filtered.TopologicalSort(m => m.Type, m => m.Dependencies.Select(d => d.Type)).ToArray();
		var length = metadata.Length;

		for (int i = 0; i < _phases.Length; i++)
			_phases[i].Resize(length);

		foreach (var data in metadata)
		{
			var thunks = data.Thunks.AsSpan();

			unsafe
			{
				var instance = data.Instantiate(Context);
				foreach (ref var thunk in thunks)
				{
					if (thunk.IsNull) continue;
					thunk.Instance = instance;
					int idx = BitOperations.TrailingZeroCount((int)thunk.Phase);
					_phases[idx].Add(thunk);
				}
			}
		}

		_phases[7].Reverse();
		_phases[0].Invoke();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)] public virtual void Initialize()
	{
		_phases[1].Invoke();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)] public virtual void Start()
	{
		_phases[2].Invoke();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public virtual void FixedUpdate()
	{
		while (Context.Time.FixedUpdate()) _phases[3].Invoke();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)] public virtual void PreUpdate()
	{
		_phases[4].Invoke();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)] public virtual void Update()
	{
		_phases[5].Invoke();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public virtual void PostUpdate()
	{
		_phases[6].Invoke(); Context.Time.Update();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)] public virtual void Shutdown()
	{
		_phases[7].Invoke();
	}

	protected abstract void BeforeStart(PhaseBarrier barrier);
	protected abstract void AfterStart(PhaseBarrier barrier);

	public override void Launch()
	{
		Context.Affinity = Affinity;

		if (Affinity != Affinity.Main)
		{
			Thread = new Thread(_Launch) { Name = Affinity.ToString(), IsBackground = true, };
			Thread.Start();
			return;
		}

		_Launch();
	}

	private void _Launch()
	{
		try
		{
			Log.Core.Affinity = Affinity;

			var barrier = require(Context.Registry.Global.Get<PhaseBarrier>());

			Load();
			Initialize();

			BeforeStart(barrier);
			Start();
			AfterStart(barrier);

			Code = Run();
		}
		catch (OperationCanceledException ex)
		{
			Code = Application.ExitCode.Canceled;
			Log.Info($"[{Affinity}] kernel canceled: {ex.Message}");
		}
		catch (Exception ex)
		{
			Code = Application.ExitCode.Fatal;

			Log.Error($"[{Affinity}] kernel crashed with fatal exception:");
			Log.Error(ex);
		}
		finally
		{
			try
			{
				Shutdown();
			}
			catch (Exception ex)
			{
				Log.Error($"[{Affinity}] Shutdown threw: {ex}");
			}
		}
	}

	protected abstract Application.ExitCode Run();
}

public sealed class MainKernel : Kernel<MainKernel>, IConfigure
{
	public static void Configure()
	{
		Affinity = Affinity.Main;
	}

	protected override void BeforeStart(PhaseBarrier b) { }
	protected override void AfterStart(PhaseBarrier b) => b.MainStartDone.Set();

	public override void Load()
	{
		base.Load();

	}

	public override void Initialize()
	{
		base.Initialize();

	}

	public override void Start()
	{
		base.Start();

	}

	protected override Application.ExitCode Run()
	{
		while (Application.IsRunning())
		{
			PreUpdate();
			Update();
			PostUpdate();
		}

		return Application.ExitCode.Canceled;
	}
}

public sealed class GameKernel : Kernel<GameKernel>, IConfigure
{
	public static void Configure()
	{
		Affinity = Affinity.Game;
	}

	protected override void BeforeStart(PhaseBarrier b) => b.MainStartDone.Wait();
	protected override void AfterStart(PhaseBarrier b) => b.GameStartDone.Set();

	public override void Load()
	{
		base.Load();

	}

	public override void Initialize()
	{
		base.Initialize();

	}

	public override void Start()
	{
		base.Start();

	}

	protected override Application.ExitCode Run()
	{
		var input = Context.Input;
		var time = Context.Time;

		var inputEndPoint = Context.MessageBus.Claim<InputSnapshot>();
		var inputReader = inputEndPoint.Read;

		var sceneEndPoint = Context.MessageBus.Claim<SceneView>();
		var sceneWriter = sceneEndPoint.SnapshotWrite;

		InputSnapshot snapshot = default;
		long lastProcessedInputFrame = -1;

		while (Application.IsRunning())
		{
			while (inputReader!.TryRead(out var s))
				snapshot = s;

			if (snapshot.FrameId != lastProcessedInputFrame)
			{
				input.Update(in snapshot);
				lastProcessedInputFrame = snapshot.FrameId;
			}

			FixedUpdate();
			PreUpdate();
			Update();
			PostUpdate();

			var sceneView = new SceneView
			{
				FrameIndex = (int)time.FixedFrameIndex,
				Alpha = (float)time.FixedAlpha
			};

			sceneWriter!.Publish(sceneView);
		}

		return Application.ExitCode.Canceled;
	}
}

public sealed class RenderKernel : Kernel<RenderKernel>, IConfigure
{
	public static void Configure()
	{
		Affinity = Affinity.Render;
	}

	protected override void BeforeStart(PhaseBarrier b) => b.GameStartDone.Wait();
	protected override void AfterStart(PhaseBarrier b) => b.RenderStartDone.Set();

	public override void Load()
	{
		base.Load();

	}

	public override void Initialize()
	{
		base.Initialize();

	}

	public override void Start()
	{
		base.Start();

	}

	protected override Application.ExitCode Run()
	{
		var sceneEndPoint = Context.MessageBus.Claim<SceneView>();
		var sceneReader = sceneEndPoint.SnapshotRead;

		while (Application.IsRunning())
		{
			var (view, version) = sceneReader!.Read();

			PreUpdate();
			Update();
			PostUpdate();
		}

		return Application.ExitCode.Canceled;
	}
}