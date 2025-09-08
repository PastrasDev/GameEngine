using System;
using Engine.Runtime;

namespace Game.Application;

internal static class Program
{
    [STAThread]
    internal static int Main(string[] args) => Runtime.Launch(args);
}