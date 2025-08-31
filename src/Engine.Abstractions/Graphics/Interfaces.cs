using System.Collections.Generic;

namespace Engine.Abstractions.Graphics;

public interface IRenderer
{
    
}

public interface IRenderProxyRegistry
{
    void Add(RenderProxy proxy);
    bool Remove(RenderProxy proxy);

    /// <summary>Monotonic version; increments on add/remove.</summary>
    int Version { get; }

    /// <summary>Thread-safe snapshot of current proxies.</summary>
    IReadOnlyList<RenderProxy> Snapshot();
}