using Unmanaged.Generator.Cef;
using Unmanaged.Generator.Windows;

namespace Unmanaged.Generator;

public static class Program
{
    public static int Main(string[] args)
    {
        try
        {
            Executable.Execute(new CefGenerator(args));
            Executable.Execute(new Win64Generator(args));
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[FATAL] {ex}");
            return -1;
        }
    }
}