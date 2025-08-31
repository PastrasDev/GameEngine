using System;

namespace Engine.Abstractions.Graphics;

public abstract class RenderProxy : ILifecycle
{
    /// <summary>
    /// Bound by the Render kernel before any lifecycle method runs.
    /// </summary>
    protected IEngineContext Context { get; private set; } = null!;

    /// <summary>
    /// For sort/bucketing (e.g., opaques before transparents). Lower = earlier.
    /// </summary>
    public virtual int Order => 0;

    /// <summary>
    /// Called by the Render driver. Do not call yourself.
    /// </summary>
    public void BindContext(IEngineContext ctx) => Context = ctx ?? throw new ArgumentNullException(nameof(ctx));
    
    public virtual void Initialize() { }
    public virtual void Start() { }
    public virtual void Update() { }
    public virtual void LateUpdate() { }
    public virtual void Shutdown() { }
}