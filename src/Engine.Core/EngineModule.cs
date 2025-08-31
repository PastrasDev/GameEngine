using System;

namespace Engine.Core;

/// <summary>
/// Base class for all engine modules. 
/// Modules override lifecycle hooks to participate in initialization, updates,
/// rendering, and shutdown. The <see cref="Context"/> is bound automatically
/// by the kernel when the module is first initialized.
/// </summary>
public abstract class EngineModule : ILifecycle
{
    /// <summary>
    /// The per-thread engine context (Main/Game/Render) this module runs under.
    /// Populated by the kernel before any lifecycle method is called.
    /// Provides access to <see cref="IEngineContext.DeltaTime"/>, 
    /// the <see cref="IEngineContext.Registry"/>, and communication channels.
    /// </summary>
    public IEngineContext Context { get; set; } = null!;
    
    /// <summary>
    /// Called by the kernel to associate this module with a runtime context. 
    /// This is invoked once during initialization. 
    /// You do not call this yourself.
    /// </summary>
    public void BindContext(IEngineContext context) => Context = context ?? throw new ArgumentNullException(nameof(context));
    
    public virtual void Initialize() { }
    public virtual void Start() { }
    public virtual void FixedUpdate() { }
    public virtual void Update() { }
    public virtual void LateUpdate() { }
    public virtual void Shutdown() { }
}