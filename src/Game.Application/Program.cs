using System;
using Engine;

namespace Game.Application;

internal static class Program
{
    [STAThread]
    internal static int Main(string[] args) => IEngine.Start(args);
}