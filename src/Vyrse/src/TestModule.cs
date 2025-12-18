
using Vyrse.Core;

namespace Vyrse.Runtime;

public sealed class Test : IModule
{
    public Test(Context context) { }

    public void Load() { }
    public void Initialize() { }
    public void Start() { }
    public void FixedUpdate() { }
    public void PreUpdate() { }
    public void Update() { }
    public void PostUpdate() { }
    public void Shutdown() { }
}