using Engine.Abstractions;

namespace Engine.Runtime;

public sealed class EngineContext : IEngineContext
{
    public float DeltaTime { get; set; }
    public IRegistry Registry { get; init; } = null!;
}