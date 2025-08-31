namespace Engine.Abstractions;

public interface IEngineContext
{
    float DeltaTime { get; set; }
    IRegistry Registry { get; init; }
}