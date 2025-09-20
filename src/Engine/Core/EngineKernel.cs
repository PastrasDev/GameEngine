﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Engine.Messaging;
using Engine.Platform.Windows;
using Engine.Utilities;

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
	private readonly int _initCount;
	private readonly bool _launch;
	private readonly EngineContext _context = null!;

	protected virtual Threads Affinity => Threads.None;

	public Thread Thread { get; protected set; } = null!;
	public volatile ExitCode Code;
	public required EngineContext Context { get => _context; init { _context = value; _initCount++; TryLaunch(); } }
	public required bool Enabled { init { _launch = value; TryLaunch(); } }

	internal struct DispatchTable
	{
		public EngineModule[] Modules { get; init; }
		public unsafe delegate* managed<EngineModule, void>[] Methods { get; init; }

		private int _count;
		private int _rev;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe DispatchTable(int capacity)
		{
			Modules = capacity == 0 ? [] : new EngineModule[capacity];
			Methods = capacity == 0 ? [] : new delegate* managed<EngineModule, void>[capacity];

			_count = 0;
			_rev = capacity - 1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void Add(EngineModule m, delegate* managed<EngineModule, void> fn)
		{
			Modules[_count] = m;
			Methods[_count] = fn;
			_count++;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe void AddReverse(EngineModule m, delegate* managed<EngineModule, void> fn)
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

	[MethodImpl(MethodImplOptions.AggressiveInlining)] protected void Initialize() => _initTable.Execute();
	[MethodImpl(MethodImplOptions.AggressiveInlining)] protected void Start() => _startTable.Execute();
	[MethodImpl(MethodImplOptions.AggressiveInlining)] protected void FixedUpdate() { while (Context.Time.FixedUpdate()) _fixedTable.Execute(); }
	[MethodImpl(MethodImplOptions.AggressiveInlining)] protected void Update() { Context.Time.Update(); _updateTable.Execute(); }
	[MethodImpl(MethodImplOptions.AggressiveInlining)] protected void LateUpdate() => _lateTable.Execute();
	[MethodImpl(MethodImplOptions.AggressiveInlining)] protected void Shutdown() => _shutTable.Execute();

	protected abstract void BeforeLoad(PhaseBarrier barrier);
	protected abstract void BeforeInit(PhaseBarrier barrier);
	protected abstract void BeforeStart(PhaseBarrier barrier);
	protected abstract void AfterLoad(PhaseBarrier barrier);
	protected abstract void AfterInit(PhaseBarrier barrier);
	protected abstract void AfterStart(PhaseBarrier barrier);

	private void TryLaunch()
	{
		if (!_launch || _initCount < 1)
			return;

		if (Affinity == Threads.None)
			throw new InvalidOperationException("Kernel affinity cannot be None.");

		_context.Affinity = Affinity;

		if (Affinity != Threads.Main)
		{
			Thread = new Thread(Launch)
			{
				Name = Affinity.ToString(),
				IsBackground = true,
			};
			Thread.Start();
			return;
		}

		Launch();
	}

	private void Launch()
	{
		try
		{
			var barrier = require(Context.Registry.Get<PhaseBarrier>());

			BeforeLoad(barrier);
			Load();
			AfterLoad(barrier);

			BeforeInit(barrier);
			Initialize();
			AfterInit(barrier);

			BeforeStart(barrier);
			Start();
			AfterStart(barrier);

			Code = Run();
		}
		catch (OperationCanceledException) { Code = ExitCode.Canceled; }
		catch (Exception)                  { Code = ExitCode.Fatal; }
		finally
		{
			try { Shutdown(); } catch { /* swallow */ }
		}
	}

	protected abstract ExitCode Run();

	private void Load()
	{
		var metadata = Sort(MetadataRegistry.GetAll<EngineModule.Metadata>().Where(d => (d.Affinity & Affinity) != 0).ToList());

		int initN = 0;
		int startN = 0;
		int fixedN = 0;
		int updateN = 0;
		int lateN = 0;
		int shutN = 0;

		foreach (var descriptor in metadata)
		{
			var m = descriptor.Mask;
			if ((m & MethodMask.Init) != 0) initN++;
			if ((m & MethodMask.Start) != 0) startN++;
			if ((m & MethodMask.Fixed) != 0) fixedN++;
			if ((m & MethodMask.Update) != 0) updateN++;
			if ((m & MethodMask.Late) != 0) lateN++;
			if ((m & MethodMask.Shutdown) != 0) shutN++;
		}

		_initTable = new DispatchTable(initN);
		_startTable = new DispatchTable(startN);
		_fixedTable = new DispatchTable(fixedN);
		_updateTable = new DispatchTable(updateN);
		_lateTable = new DispatchTable(lateN);
		_shutTable = new DispatchTable(shutN);

		foreach (var descriptor in metadata)
		{
			var module = descriptor.Instantiate(Context);
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

		static List<EngineModule.Metadata> Sort(List<EngineModule.Metadata> list)
		{
			if (list.Count == 0) return [];

			var byType = new Dictionary<Type, EngineModule.Metadata>(list.Count);
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

			var ordered = new List<EngineModule.Metadata>(list.Count);
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
	protected override Threads Affinity => Threads.Main;

	protected override void BeforeLoad(PhaseBarrier b) { /* no-op */ }
	protected override void AfterLoad (PhaseBarrier b) { b.MainLoadDone.Set(); b.RenderLoadDone.Wait(); }
	protected override void BeforeInit(PhaseBarrier b) { /* no-op */ }
	protected override void AfterInit (PhaseBarrier b) => b.MainInitDone.Set();
	protected override void BeforeStart(PhaseBarrier b) { /* no-op */ }
	protected override void AfterStart(PhaseBarrier b) => b.MainStartDone.Set();

	protected override ExitCode Run()
	{
		var ct = Context.TokenSrc.Token;
		while (!ct.IsCancellationRequested)
		{
			Update();
			LateUpdate();
		}

		return ExitCode.Canceled;
	}
}

public sealed class GameKernel : EngineKernel
{
	protected override Threads Affinity => Threads.Game;

	protected override void BeforeLoad(PhaseBarrier b) => b.MainLoadDone.Wait();
	protected override void AfterLoad (PhaseBarrier b) { b.GameLoadDone.Set(); b.RenderLoadDone.Wait(); }
	protected override void BeforeInit(PhaseBarrier b) { /* no-op */ }
	protected override void AfterInit (PhaseBarrier b) => b.GameInitDone.Set();
	protected override void BeforeStart(PhaseBarrier b) => b.MainStartDone.Wait();
	protected override void AfterStart(PhaseBarrier b) => b.GameStartDone.Set();

	protected override ExitCode Run()
	{
		var registry = Context.Registry;
		var input = require(Context.Input);
		var channels = require(registry.Get<IGameChannels>());
		var time = require(Context.Time);
		var ct = Context.TokenSrc.Token;

		InputSnapshot snapshot = default;

		while (!ct.IsCancellationRequested)
		{
			while (channels.InputReader.TryRead(out var s))
				snapshot = s;

			input?.Update(snapshot);

			FixedUpdate();
			Update();
			LateUpdate();
			channels.SceneWriter.Publish(new SceneView((int)time.FixedFrameIndex, (float)time.FixedAlpha));
		}

		return ExitCode.Canceled;
	}
}

public sealed class RenderKernel : EngineKernel
{
	protected override Threads Affinity => Threads.Render;

	protected override void BeforeLoad(PhaseBarrier b) => b.GameLoadDone.Wait();
	protected override void AfterLoad (PhaseBarrier b) => b.RenderLoadDone.Set();
	protected override void BeforeInit(PhaseBarrier b) => b.MainInitDone.Wait();
	protected override void AfterInit (PhaseBarrier b) => b.RenderInitDone.Set();
	protected override void BeforeStart(PhaseBarrier b) => b.GameStartDone.Wait();
	protected override void AfterStart(PhaseBarrier b) => b.RenderStartDone.Set();

	protected override ExitCode Run()
	{
		var channel = require(Context.Registry.Get<IRenderChannels>());
		var ct = Context.TokenSrc.Token;

		while (!ct.IsCancellationRequested)
		{
			channel.SceneReader.Read();
			Update();
			LateUpdate();
		}

		return ExitCode.Canceled;
	}
}