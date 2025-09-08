using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Engine.Core;

public sealed class PhaseBarrier
{
	public readonly ManualResetEventSlim MainInitDone = new(false);
	public readonly ManualResetEventSlim GameInitDone = new(false);
	public readonly ManualResetEventSlim RenderInitDone = new(false);

	public readonly ManualResetEventSlim MainStartDone = new(false);
	public readonly ManualResetEventSlim GameStartDone = new(false);
	public readonly ManualResetEventSlim RenderStartDone = new(false);
}

public abstract class EngineKernel
{
	protected const float FixedDt = 1f / 120f;
	protected const float MaxFrameDt = 0.25f;
	protected const int MaxFixedStepsPerFrame = 8;

	public Thread Thread { get; protected set; } = null!;
	public volatile ExitCode Code;

	private static readonly ModuleLoader Loader = new() { Enabled = true };

	private readonly int _initCount;
	private readonly bool _launch;
	private readonly Threads _affinity;
	private readonly EngineContext _context = null!;

	public required Threads Affinity { get => _affinity; init { _affinity = value; _initCount++; TryLaunch(); } }
	public required EngineContext Context { get => _context; init { _context = value; _initCount++; TryLaunch(); } }
	public required bool Enabled { init { _launch = value; TryLaunch(); } }

	internal struct DispatchTable
	{
		public object[] Modules { get; init; }
		public unsafe delegate* managed<object, bool>[] Enables { get; init; }
		public unsafe delegate* managed<object, void>[] Methods { get; init; }
		public bool IgnoreEnabled { get; init; }

		private int _count;
		private int _rev;

		public unsafe DispatchTable(int capacity, bool ignoreEnabled = false)
		{
			Modules = capacity == 0 ? [] : new object[capacity];
			Enables = capacity == 0 ? [] : new delegate* managed<object, bool>[capacity];
			Methods = capacity == 0 ? [] : new delegate* managed<object, void>[capacity];
			IgnoreEnabled = ignoreEnabled;
			_count = 0;
			_rev = capacity - 1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void Add(object m, delegate* managed<object,bool> en, delegate* managed<object,void> fn)
		{
			Modules[_count] = m;
			Enables[_count] = en;
			Methods[_count] = fn;
			_count++;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void AddReverse(object m, delegate* managed<object,bool> en, delegate* managed<object,void> fn)
		{
			Modules[_rev] = m;
			Enables[_rev] = en;
			Methods[_rev] = fn;
			_rev--;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly unsafe void Execute()
		{
			for (int i = 0, n = Modules.Length; i < n; i++)
			{
				var m = Modules[i];
				if (!IgnoreEnabled && !Enables[i](m)) continue;
				Methods[i](m);
			}
		}
	}

	private DispatchTable _initTable;
	private DispatchTable _startTable;
	private DispatchTable _fixedTable;
	private DispatchTable _updateTable;
	private DispatchTable _lateTable;
	private DispatchTable _shutTable;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected void Load() => Loader.Load(this);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected void Initialize() => _initTable.Execute();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected void Start() => _startTable.Execute();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected void FixedUpdate() => _fixedTable.Execute();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected void Update() => _updateTable.Execute();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected void LateUpdate() => _lateTable.Execute();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected void Shutdown() => _shutTable.Execute();

	protected abstract void Run();

	private void TryLaunch()
	{
		if (!_launch || _initCount < 2)
			return;

		if (Affinity == Threads.None)
			throw new InvalidOperationException("Kernel affinity cannot be None.");

		if (Affinity != Threads.Main)
		{
			Thread = new(Run)
			{
				Name = Affinity.ToString(),
				IsBackground = true,
			};
			Thread.Start();
			return;
		}

		Run();
	}

	internal unsafe void LoadModules(Type[] types)
	{
		_initTable = new(types.Length);
		_startTable = new(types.Length);
		_fixedTable = new(types.Length);
		_updateTable = new(types.Length);
		_lateTable = new(types.Length);
		_shutTable = new(types.Length, true);

		foreach (var type in types)
		{
			var module = (IEngineModule)Activator.CreateInstance(type)!;
			module.Context = Context;

			var desc = module.Descriptor;
			var en = (delegate* managed<object, bool>)desc.EnabledPtr;
			var tbl = desc.Table;

			if (tbl.TryGet(MethodMask.Load, out var method))
				method(module);

			if (tbl.TryGet(MethodMask.Init, out method))
				_initTable.Add(module, en, method);

			if (tbl.TryGet(MethodMask.Start, out method))
				_startTable.Add(module, en, method);

			if (tbl.TryGet(MethodMask.Fixed, out method))
				_fixedTable.Add(module, en, method);

			if (tbl.TryGet(MethodMask.Update, out method))
				_updateTable.Add(module, en, method);

			if (tbl.TryGet(MethodMask.Late, out method))
				_lateTable.Add(module, en, method);

			if (tbl.TryGet(MethodMask.Shutdown, out method))
				_shutTable.AddReverse(module, en, method);
		}
	}
}

public sealed class MainKernel : EngineKernel
{
	protected override void Run()
	{
		try
		{
			Thread = Thread.CurrentThread;

			var barrier = require(Context.Registry.Get<PhaseBarrier>());
			var channel = require(Context.Registry.Get<IMainChannels>());
			var ct = Context.TokenSrc.Token;

			Load();
			Initialize();
			barrier.MainInitDone.Set();

			Start();
			barrier.MainStartDone.Set();

			var sw = Stopwatch.StartNew();
			long frameId = 0;
			double last = 0;

			while (!ct.IsCancellationRequested)
			{
				Update();
				LateUpdate();

				var now = sw.Elapsed.TotalSeconds;
				var raw = (float)(now - last);
				last = now;
				var dt = clamp(raw, 0f, MaxFrameDt);

				channel.FrameWriter.Write(new(frameId, dt), ct);
				frameId++;
			}

			Code = ExitCode.Canceled;
		}
		catch (OperationCanceledException)
		{
			Code = ExitCode.Canceled;
		}
		catch (Exception)
		{
			Code = ExitCode.Fatal;
		}
		finally
		{
			try
			{
				Shutdown();
			}
			catch
			{
				/* ignore */
			}
		}
	}
}

public sealed class GameKernel : EngineKernel
{
	protected override void Run()
	{
		try
		{
			var barrier = require(Context.Registry.Get<PhaseBarrier>());
			var channel = require(Context.Registry.Get<IGameChannels>());
			var ct = Context.TokenSrc.Token;

			barrier.MainInitDone.Wait(ct);
			Load();
			Initialize();
			barrier.GameInitDone.Set();

			barrier.MainStartDone.Wait(ct);
			Start();
			barrier.GameStartDone.Set();

			float accumulator = 0f;
			int simFrame = 0;

			while (!ct.IsCancellationRequested)
			{
				var start = channel.FrameReader.Read(ct);

				var dt = clamp(start.Dt, 0f, MaxFrameDt);
				accumulator += dt;

				int steps = 0;
				while (accumulator >= FixedDt && steps < MaxFixedStepsPerFrame)
				{
					Context.DeltaTime = FixedDt;
					FixedUpdate();
					accumulator -= FixedDt;
					simFrame++;
					steps++;
				}

				Context.DeltaTime = dt;
				Update();
				LateUpdate();

				channel.SceneWriter.Publish(new(simFrame, clamp(accumulator / FixedDt, 0f, 1f)));
			}

			Code = ExitCode.Canceled;
		}
		catch (OperationCanceledException)
		{
			Code = ExitCode.Canceled;
		}
		catch (Exception)
		{
			Code = ExitCode.Fatal;
		}
		finally
		{
			try
			{
				Shutdown();
			}
			catch
			{
				/* ignore */
			}
		}
	}
}

public sealed class RenderKernel : EngineKernel
{
	protected override void Run()
	{
		try
		{
			var barrier = require(Context.Registry.Get<PhaseBarrier>());
			var channel = require(Context.Registry.Get<IRenderChannels>());
			var ct = Context.TokenSrc.Token;

			barrier.GameInitDone.Wait(ct);
			Load();
			Initialize();
			barrier.RenderInitDone.Set();

			barrier.GameStartDone.Wait(ct);
			Start();
			barrier.RenderStartDone.Set();

			while (!ct.IsCancellationRequested)
			{
				channel.SceneReader.Read();

				Update();
				LateUpdate();
			}

			Code = ExitCode.Canceled;
		}
		catch (OperationCanceledException)
		{
			Code = ExitCode.Canceled;
		}
		catch (Exception)
		{
			Code = ExitCode.Fatal;
		}
		finally
		{
			try
			{
				Shutdown();
			}
			catch
			{
				/* ignore */
			}
		}
	}
}