using System.Threading;

namespace Engine.Core;

public sealed class EngineContext(Registry registry)
{
    public float DeltaTime { get; set; }
    public Registry Registry { get; init; } = registry;
    public CancellationTokenSource TokenSrc { get; } = registry.Get<CancellationTokenSource>();
}