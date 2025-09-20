using System;
using System.Collections.Generic;
using System.Threading;
using Engine.Messaging;
using Engine.Platform.Windows;

namespace Engine.Core;

public sealed class Runtime
{
	private readonly CancellationTokenSource _cts = new();
	private readonly Dictionary<Threads, EngineKernel> _kernels = new();
	private Threads _activeThreads;

	internal ExitCode Run(string[]? args)
	{
		_activeThreads = GetActiveThreads(args);

		var inputRing = new SpscRing<InputSnapshot>(256, PipeFullMode.Wait, WaitStrategy.Hybrid);
		var inputPort = new PipePort<InputSnapshot>(inputRing);

		var viewSnap = new Snapshot<SceneView>();
		var viewPort = new SnapshotPort<SceneView>(viewSnap);

		var ctrlBus = new ControlQueue<RenderControl>(rc => (int)rc.Type, maxItems: 64);
		var ctrlPort = new RenderCommandBusPort(ctrlBus);

		var channels = new EngineChannels
		{
			Input = new InputChannel { Reader = inputPort, Writer = inputPort },
			Scene = new SceneChannel { Reader = viewPort, Writer = viewPort },
			RenderCommands = ctrlPort
		};

		var mainView = new MainChannelsView(channels);
		var gameView = new GameChannelsView(channels);
		var renderView = new RenderChannelsView(channels);

		var root = Registry.Root;
		root.Add(_cts);
		root.Add(channels);
		root.Add(new PhaseBarrier());

		if (_activeThreads.HasFlag(Threads.Game))
		{
			var registry = root.Local;
			registry.Add<IGameChannels>(gameView);
			registry.Add(_cts);

			_kernels[Threads.Game] = new GameKernel
			{
				Context = new EngineContext
				{
					Registry = registry,
					TokenSrc = _cts,
				},
				Enabled = true
			};
		}

		if (_activeThreads.HasFlag(Threads.Render))
		{
			var registry = root.Local;
			registry.Add<IRenderChannels>(renderView);
			registry.Add(_cts);

			_kernels[Threads.Render] = new RenderKernel
			{
				Context = new EngineContext
				{
					Registry = registry,
					TokenSrc = _cts,
				},
				Enabled = true
			};
		}

		if (_activeThreads.HasFlag(Threads.Main))
		{
			var registry = root.Local;
			registry.Add<IMainChannels>(mainView);
			registry.Add(_cts);

			_kernels[Threads.Main] = new MainKernel
			{
				Context = new EngineContext
				{
					Registry = registry,
					TokenSrc = _cts,
				},
				Enabled = true
			};
		}

		ExitCode final = default;
		foreach (var kernel in _kernels.Values)
		{
			try { kernel.Thread?.Join(); }
			catch { /* ignore */ }
			var code = kernel.Code;
			if (code > final) final = code;
		}

		_cts.Dispose();
		Console.WriteLine("Runtime exited");
		return final;
	}

	internal void Shutdown() => _cts.Cancel();

	private static Threads GetActiveThreads(string[]? args) => ParseArgs<GameRole>(args) switch
	{
		GameRole.Server => Threads.Game, GameRole.Client or GameRole.Both => Threads.Main | Threads.Game | Threads.Render, _ => Threads.None
	};

	private static T ParseArgs<T>(string[]? args) where T : notnull
	{
		if (typeof(T) != typeof(GameRole))
			throw new InvalidOperationException($"Game.ParseArgs can only return {nameof(GameRole)}");

		if (args == null)
			return (T)(object)GameRole.Client;

		foreach (var arg in args)
		{
			switch (arg.TrimStart('-').ToLowerInvariant())
			{
				case "client": return (T)(object)GameRole.Client;
				case "server" or "dedicated": return (T)(object)GameRole.Server;
				case "both" or "host": return (T)(object)GameRole.Both;
			}
		}

		return (T)(object)GameRole.Client;
	}
}