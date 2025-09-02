using System;
using Engine.Core;
using Engine.Runtime;

namespace Editor.Application;

internal static class Program
{
    [STAThread]
    internal static int Main(string[] args)
    {
        return IRuntime.Launch(RuntimeMode.Editor, args);
    }
}