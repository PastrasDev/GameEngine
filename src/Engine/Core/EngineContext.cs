using System;
using System.Threading;
using Engine.Platform.Windows;
using Engine.Utilities;

namespace Engine.Core;

public sealed class EngineContext
{
    private readonly Input _input = new();

    public Time Time { get; } = new();
    public Input Input => Affinity != Threads.Game ? throw new InvalidOperationException("Input can only be accessed from the Game thread.") : _input;
    public Threads Affinity { get; internal set; }
    public required Registry Registry { get; init; }
    public required CancellationTokenSource TokenSrc { get; init; }
}