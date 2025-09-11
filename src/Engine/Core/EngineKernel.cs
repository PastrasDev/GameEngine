using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Engine.Core;

public sealed class PhaseBarrier
{
	public readonly ManualResetEventSlim MainLoadDone = new(false);
	public readonly ManualResetEventSlim GameLoadDone = new(false);
	public readonly ManualResetEventSlim RenderLoadDone = new(false);

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

	private readonly int _initCount;
	private readonly bool _launch;
	private readonly Threads _affinity;
	private readonly EngineContext _context = null!;

	public required Threads Affinity { get => _affinity; init { _affinity = value; _initCount++; TryLaunch(); } }
	public required EngineContext Context { get => _context; init { _context = value; _initCount++; TryLaunch(); } }
	public required bool Enabled { init { _launch = value; TryLaunch(); } }

	internal struct DispatchTable
	{
		public IEngineModule[] Modules { get; init; }
		public unsafe delegate* managed<IEngineModule, void>[] Methods { get; init; }

		private int _count;
		private int _rev;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe DispatchTable(int capacity)
		{
			Modules = capacity == 0 ? [] : new IEngineModule[capacity];
			Methods = capacity == 0 ? [] : new delegate* managed<IEngineModule, void>[capacity];

			_count = 0;
			_rev = capacity - 1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void Add(IEngineModule m, delegate* managed<IEngineModule, void> fn)
		{
			Modules[_count] = m;
			Methods[_count] = fn;
			_count++;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void AddReverse(IEngineModule m, delegate* managed<IEngineModule, void> fn)
		{
			Modules[_rev] = m;
			Methods[_rev] = fn;
			_rev--;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly unsafe void Execute()
		{
			var modules = Modules;
			var methods = Methods;
			for (int i = 0, n = modules.Length; i < n; i++)
			{
				methods[i](modules[i]);
			}
		}
	}

	private DispatchTable _initTable = new(0);
	private DispatchTable _startTable = new(0);
	private DispatchTable _fixedTable = new(0);
	private DispatchTable _updateTable = new(0);
	private DispatchTable _lateTable = new(0);
	private DispatchTable _shutTable = new(0);

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
			Thread = new Thread(Run)
			{
				Name = Affinity.ToString(),
				IsBackground = true,
			};
			Thread.Start();
			return;
		}

		Run();
	}

	protected void Load()
	{
		var descriptors = Sort(DescriptorRegistry.GetAll<EngineModuleDescriptor>().Where(d => d.Affinity == Affinity).ToList());

		_initTable = new DispatchTable(descriptors.Count(d => d.Mask.HasFlag(MethodMask.Init)));
		_startTable = new DispatchTable(descriptors.Count(d => d.Mask.HasFlag(MethodMask.Start)));
		_fixedTable = new DispatchTable(descriptors.Count(d => d.Mask.HasFlag(MethodMask.Fixed)));
		_updateTable = new DispatchTable(descriptors.Count(d => d.Mask.HasFlag(MethodMask.Update)));
		_lateTable = new DispatchTable(descriptors.Count(d => d.Mask.HasFlag(MethodMask.Late)));
		_shutTable = new DispatchTable(descriptors.Count(d => d.Mask.HasFlag(MethodMask.Shutdown)));

		foreach (var descriptor in descriptors)
		{
			var module = (IEngineModule)Activator.CreateInstance(descriptor.Type)!;
			module.Context = Context;
			var table = descriptor.Table;

			unsafe
			{
				if (table.TryGet(MethodMask.Load, out var method))
					method(module);

				if (table.TryGet(MethodMask.Init, out method))
					_initTable.Add(module, method);

				if (table.TryGet(MethodMask.Start, out method))
					_startTable.Add(module, method);

				if (table.TryGet(MethodMask.Fixed, out method))
					_fixedTable.Add(module, method);

				if (table.TryGet(MethodMask.Update, out method))
					_updateTable.Add(module, method);

				if (table.TryGet(MethodMask.Late, out method))
					_lateTable.Add(module, method);

				if (table.TryGet(MethodMask.Shutdown, out method))
					_shutTable.AddReverse(module, method);
			}
		}

		return;

		static List<EngineModuleDescriptor> Sort(List<EngineModuleDescriptor> list)
		{
			if (list.Count == 0) return [];

			var byType = new Dictionary<Type, EngineModuleDescriptor>(list.Count);
			var inSet = new HashSet<Type>(list.Count);

			foreach (var d in list)
			{
				byType[d.Type] = d;
				inSet.Add(d.Type);
			}

			var indeg = new Dictionary<Type, int>(list.Count);
			var edges = new Dictionary<Type, List<Type>>(list.Count);
			foreach (var d in list)
			{
				indeg[d.Type] = 0;
				edges[d.Type] = [];
			}

			foreach (var d in list)
			{
				var deps = d.Dependencies;
				if (deps is null) continue;
				foreach (var dep in deps)
				{
					if (!inSet.Contains(dep)) continue;
					edges[dep].Add(d.Type);
					indeg[d.Type] += 1;
				}
			}

			var q = new Queue<Type>(indeg.Where(kv => kv.Value == 0).Select(kv => kv.Key).OrderBy(t => t.FullName, StringComparer.Ordinal));

			var ordered = new List<EngineModuleDescriptor>(list.Count);
			while (q.Count > 0)
			{
				var u = q.Dequeue();
				ordered.Add(byType[u]);

				var outs = edges[u];
				foreach (var v in outs)
				{
					if (--indeg[v] == 0)
						q.Enqueue(v);
				}
			}

			return ordered.Count != list.Count ? throw new InvalidOperationException("Cyclical module dependency detected.") : ordered;
		}
	}
}

public sealed class MainKernel : EngineKernel
{
	protected override void Run()
	{
		try
		{
			var barrier = require(Context.Registry.Get<PhaseBarrier>());
			var channel = require(Context.Registry.Get<IMainChannels>());
			var ct = Context.TokenSrc.Token;

			Load();
			barrier.MainLoadDone.Set();

			barrier.RenderLoadDone.Wait(ct);

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

				channel.FrameWriter.Write(new FrameStart(frameId, dt), ct);
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

			barrier.MainLoadDone.Wait(ct);
			Load();
			barrier.GameLoadDone.Set();

			barrier.RenderLoadDone.Wait(ct);
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

				channel.SceneWriter.Publish(new SceneView(simFrame, clamp(accumulator / FixedDt, 0f, 1f)));
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

			barrier.GameLoadDone.Wait(ct);
			Load();
			barrier.RenderLoadDone.Set();

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