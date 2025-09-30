using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using Engine.Core;
using Engine.Core.Registries;
using Engine.Core.Threading.Messaging;
using Engine.Debug;
using Math = Engine.Mathematics.Math;

namespace Engine;

public sealed class Application
{
	private static Application _instance = null!;
	internal static readonly CancellationTokenSource TokenSrc = new();
	private readonly Dictionary<Affinity, Kernel> _kernels = new();
	private Affinity _activeAffinity;
	private readonly Registry _registry = new();

	//private static readonly ManualResetEventSlim s_startGate = new(initialState: false);
	//internal static void WaitForGlobalStartGate() => s_startGate.Wait(TokenSrc.Token);
	//private static void ResetGlobalStartGate() => s_startGate.Reset();
	//private static void ReleaseGlobalStartGate() => s_startGate.Set();

	static Application() => DiscoverableAttribute.ForceAll(true);

	public enum Role
	{
		None,
		Client,
		Server,
		Both
	}

	public enum ExitCode
	{
		/// clean shutdown
		Ok = 0,
		/// normal cancellation (e.g., user quit)
		Canceled = 10,
		/// non-fatal error; runtime can shut down gracefully
		Recoverable = 20,
		/// fatal error
		Fatal = 30
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int Run(string[]? args)
	{
		if (_instance != null)
			throw new InvalidOperationException("Runtime is already running.");

		_instance = new Application
		{
			_activeAffinity = GetActiveThreads(args)
		};
		return (int)_instance.Run();
	}

	private ExitCode Run()
	{
		_registry.Global.Add(TokenSrc);
		//_registry.Global.Add(new MessageBus());

		//ResetGlobalStartGate();

		if (_activeAffinity.HasFlag(Affinity.Main))
		{
			_kernels[Affinity.Main] = new MainKernel
			{
				Context = new Context
				{
					Registry = _registry,
				}
			};
			_kernels[Affinity.Main].Launch(Kernel.LaunchWait.Initialized);
		}

		if (_activeAffinity.HasFlag(Affinity.Game))
		{
			_kernels[Affinity.Game] = new GameKernel
			{
				Context = new Context
				{
					Registry = _registry,
				},
			};
			_kernels[Affinity.Game].Launch(Kernel.LaunchWait.Initialized);
		}

		if (_activeAffinity.HasFlag(Affinity.Render))
		{
			_kernels[Affinity.Render] = new RenderKernel
			{
				Context = new Context
				{
					Registry = _registry,
				},
			};
			_kernels[Affinity.Render].Launch(Kernel.LaunchWait.Initialized);
		}

		//foreach (var k in _kernels.Values)
			//k.WaitInitialized(TokenSrc.Token);

		//ReleaseGlobalStartGate();

		//foreach (var k in _kernels.Values)
			//k.WaitStarted(TokenSrc.Token);

		ExitCode final = default;

		if (_kernels.TryGetValue(Affinity.Render, out var render))
		{
			try { render.Thread?.Join(); }
			catch (Exception ex) { Log.Error($"Join failed: {ex}"); }
			final = (ExitCode)max((int)final, (int)render.Code);
			Log.Info($"{render.GetType().Name} exited: {render.Code}");
		}

		if (_kernels.TryGetValue(Affinity.Game, out var game))
		{
			try { game.Thread?.Join(); }
			catch (Exception ex) { Log.Error($"Join failed: {ex}"); }
			final = (ExitCode)max((int)final, (int)game.Code);
			Log.Info($"{game.GetType().Name} exited: {game.Code}");
		}

		if (_kernels.TryGetValue(Affinity.Main, out var main))
		{
			try { main.Thread?.Join(); }
			catch (Exception ex) { Log.Error($"Join failed: {ex}"); }
			final = (ExitCode)max((int)final, (int)main.Code);
			Log.Info($"{main.GetType().Name} exited: {main.Code}");
		}

		TokenSrc.Dispose();
		return final;
	}

	[Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsRunning() => !TokenSrc.IsCancellationRequested;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Shutdown() => TokenSrc.Cancel();

	[Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Affinity GetActiveThreads(string[]? args) => ParseArgs<Role>(args) switch
	{
		Role.Server => Affinity.Game, Role.Client or Role.Both => Affinity.Main | Affinity.Game | Affinity.Render, _ => Affinity.None
	};

	[Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static T ParseArgs<T>(string[]? args) where T : notnull
	{
		if (typeof(T) != typeof(Role))
			throw new InvalidOperationException($"Game.ParseArgs can only return {nameof(Role)}");

		if (args == null)
			return (T)(object)Role.Client;

		foreach (var arg in args)
		{
			switch (arg.TrimStart('-').ToLowerInvariant())
			{
				case "client": return (T)(object)Role.Client;
				case "server" or "dedicated": return (T)(object)Role.Server;
				case "both" or "host": return (T)(object)Role.Both;
			}
		}

		return (T)(object)Role.Client;
	}
}