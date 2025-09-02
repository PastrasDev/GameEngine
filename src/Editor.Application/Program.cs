using Engine.Core;
using Engine.Runtime;

namespace Editor.Application;

internal static class Program
{
    internal static void Main(string[] args)
    {
        using var runtime = IRuntime.Create(RuntimeMode.Editor, args);
        runtime.Run();
    }
}