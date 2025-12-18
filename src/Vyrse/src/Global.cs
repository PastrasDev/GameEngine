using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Vyrse;

internal static class Global
{
    extension<T>(T obj)
    {
        [Conditional("DEBUG")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Log(string message) => Console.WriteLine($"[{typeof(T).Name}][{DateTime.Now:HH:mm:ss.fff}] {message}");
    }
}