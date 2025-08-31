using Engine.Abstractions;

namespace Engine.Runtime;

internal sealed class Editor : Runtime
{
    internal Editor(string[]? args) { }
    
    protected override RuntimeThreads ActiveThreads => RuntimeThreads.Main | RuntimeThreads.Game | RuntimeThreads.Render;
    
    // Wire up your editor-side modules here when you add them.
    protected override void SetupMainModules(IEngineKernel kernel, IRegistry registry)
    {
        // kernel.Add(new WindowModule());     // polls OS/input on MAIN
        // kernel.Add(new EditorUIModule());   // menus, inspector, etc. (MAIN)
    }

    protected override void SetupGameModules(IEngineKernel kernel, IRegistry registry)
    {
        // kernel.Add(new SceneModule());      // authoring scene / runtime world (GAME)
        // kernel.Add(new PhysicsModule());    // fixed-step physics (GAME)
    }

    protected override void SetupRenderModules(IEngineKernel kernel, IRegistry registry)
    {
        // kernel.Add(new RendererModule());   // GPU renderer (RENDER)
        // kernel.Add(new GizmosModule());     // editor gizmos (RENDER)
    }
}