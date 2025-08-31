namespace Engine.Core;

public interface IEngineContext
{
    float DeltaTime { get; set; }
    IRegistry Registry { get; init; }
    
    static IEngineContext Create(IRegistry registry) => new EngineContext(registry);
}

internal sealed class EngineContext(IRegistry registry) : IEngineContext
{
    public float DeltaTime { get; set; }
    public IRegistry Registry { get; init; } = registry;

}