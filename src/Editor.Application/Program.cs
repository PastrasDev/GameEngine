using Engine.Abstractions;
using Engine.Runtime;

namespace Editor.Application;

internal static class Program
{
    internal static void Main(string[] args)
    {
        using var runtime = Runtime.Create(RuntimeMode.Editor, args);
        runtime.Run();
    }
}