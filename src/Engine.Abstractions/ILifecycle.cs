namespace Engine.Abstractions;

public interface ILifecycle
{
    /// <summary>
    /// Runs once at engine startup, before <see cref="Start"/>.
    /// Use this to allocate resources, register services, or perform setup
    /// that does not depend on other modules being ready.
    /// </summary>
    void Initialize() { }
    
    /// <summary>
    /// Runs once after <see cref="Initialize"/>. 
    /// All modules have been initialized at this point, so you can safely 
    /// resolve dependencies from the registry and connect to other modules.
    /// </summary>
    void Start() { }
    
    /// <summary>
    /// Runs on the fixed timestep update loop (e.g., 120Hz).
    /// Use this for deterministic simulation such as physics or networking.
    /// The timestep is exposed as <see cref="IEngineContext.DeltaTime"/>.
    /// </summary>
    void FixedUpdate() { }
    
    /// <summary>
    /// Runs once per frame on the variable timestep update loop.
    /// Use this for general game logic that can tolerate varying frame times.
    /// </summary>
    void Update() { }
    
    /// <summary>
    /// Runs once per frame after <see cref="Update"/>.
    /// Use this for tasks that must occur after all updates, such as
    /// resolving transforms or sending events to the render thread.
    /// </summary>
    void LateUpdate() { }
    
    /// <summary>
    /// Runs once during shutdown in reverse order of initialization.
    /// Use this to dispose resources, unregister services, and perform cleanup.
    /// </summary>
    void Shutdown() { }
}