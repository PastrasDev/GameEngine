using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Vyrse.Core;

namespace Vyrse;

public sealed class Engine : IDisposable
{
    private Context Context { get; } = new();
    private ModuleGraph Modules { get; set; } = null!;
    private static readonly CancellationTokenSource _cts = new();

    [STAThread]
    public int Run(string[] args)
    {
        try
        {
            Modules = Executable.Execute(new ModuleGraph(Context));

            Load();
            Initialize();
            Start();

            while (Running)
            {
                PreUpdate();
                Update();
                PostUpdate();

                while (Context.Time.FixedUpdate())
                {
                    FixedUpdate();
                }
            }

            Shutdown();
        }
        catch (OperationCanceledException)
        {
            return (int)Code.Canceled;
        }
        catch (Exception ex)
        {
            this.Log($"crashed with fatal exception: {ex}");
            return (int)Code.Fatal;
        }

        return (int)Code.Ok;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Load() { this.Log("Loading..."); Modules.Load(); }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Initialize()
    {
        this.Log("Initializing...");
        Modules.Initialize();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Start()
    {
        this.Log("Starting...");
        Modules.Start();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void FixedUpdate() => Modules.FixedUpdate();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void PreUpdate() => Modules.PreUpdate();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Update() => Modules.Update();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void PostUpdate() => Modules.PostUpdate();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Shutdown()
    {
        this.Log("Shutting down...");
        Modules.Shutdown();
    }

    public static bool Running => !_cts.IsCancellationRequested;
    public static void Stop() => _cts.Cancel();
    public void Dispose() => _cts.Dispose();

    public enum Code
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
}