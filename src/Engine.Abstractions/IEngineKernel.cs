namespace Engine.Abstractions;

public interface IEngineKernel
{
    void Add(EngineModule module);
    void Initialize(IEngineContext context);
    void Start(IEngineContext context);
    void FixedUpdate(IEngineContext context);
    void Update(IEngineContext context);
    void LateUpdate(IEngineContext context);
    void Shutdown(IEngineContext context);
}