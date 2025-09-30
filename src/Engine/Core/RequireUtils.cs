using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Engine.Core;

/// <summary>
/// Exception thrown when a contract requirement is violated via the <see cref="RequireUtils"/> utility.
/// </summary>
public class RequireException(string msg, string file, int line, string member) : InvalidOperationException($"{msg} (at {Path.GetFileName(file)}:{line} in {member})");

/// <summary>
/// Provides runtime validation and contract checks that throw exceptions in all builds if violated.
/// Useful for enforcing invariants and preconditions in critical code paths.
/// Each method automatically includes the caller's file, line number, and member name in the exception message for easier debugging.
/// Throws <see cref="RequireException"/> if a validation fails (unless logging is specified).
/// </summary>
public static class RequireUtils
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNull<T>(T? obj) => obj == null;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNotNull<T>(T? obj) => obj != null;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsTrue(bool condition) => condition;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFalse(bool condition) => !condition;
    
    [StackTraceHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Require<T>(T value, Func<T, bool> predicate, string? message = null, [CallerArgumentExpression("value")] string? expr = null, [CallerFilePath] string file = "", [CallerLineNumber] int line = 0, [CallerMemberName] string member = "")
    {
        return predicate(value) ? value : throw new RequireException(message ?? $"'{expr}' does not satisfy required condition.", file, line, member);
    }
    
    [StackTraceHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Require(bool value, bool condition, string? message = null, [CallerArgumentExpression("value")] string? expr = null, [CallerFilePath] string file = "", [CallerLineNumber] int line = 0, [CallerMemberName] string member = "")
    {
        return (value == condition) ? value : throw new RequireException(message ?? $"'{expr}' is not {condition}.", file, line, member);
    }

    [StackTraceHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Require(bool condition, string? message = null, [CallerArgumentExpression("condition")] string? expr = null, [CallerFilePath] string file = "", [CallerLineNumber] int line = 0, [CallerMemberName] string member = "")
    {
        return condition ? condition : throw new RequireException(message ?? $"'{expr}' is false.", file, line, member);
    }

    [return: NotNull][StackTraceHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Require<T>(T? obj, string? message = null, [CallerArgumentExpression("obj")] string? expr = null, [CallerFilePath] string file = "", [CallerLineNumber] int line = 0, [CallerMemberName] string member = "") where T : class
    {
        return obj ?? throw new RequireException(message ?? $"'{expr}' is null.", file, line, member);
    }
    
    [return: NotNull][StackTraceHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T IsType<T>(object? obj, string? message = null, [CallerArgumentExpression("obj")] string? expr = null, [CallerFilePath] string file = "", [CallerLineNumber] int line = 0, [CallerMemberName] string member = "")
    {
        return obj is T t ? t : throw new RequireException(message ?? $"'{expr}' is not of type {typeof(T).Name}.", file, line, member);
    }
    
    [StackTraceHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string IsNotNullOrEmpty(string? str, string? message = null, [CallerArgumentExpression("str")] string? expr = null, [CallerFilePath] string file = "", [CallerLineNumber] int line = 0, [CallerMemberName] string member = "")
    {
        return string.IsNullOrEmpty(str) ? throw new RequireException(message ?? $"'{expr}' is null or empty.", file, line, member) : str;
    }
    
    [StackTraceHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string IsNotNullOrWhiteSpace(string? str, string? message = null, [CallerArgumentExpression("str")] string? expr = null, [CallerFilePath] string file = "", [CallerLineNumber] int line = 0, [CallerMemberName] string member = "")
    {
        return string.IsNullOrWhiteSpace(str) ? throw new RequireException(message ?? $"'{expr}' is null or whitespace.", file, line, member) : str;
    }
    
    [StackTraceHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ICollection<T> IsNotEmpty<T>(ICollection<T>? collection, string? message = null, [CallerArgumentExpression("collection")] string? expr = null, [CallerFilePath] string file = "", [CallerLineNumber] int line = 0, [CallerMemberName] string member = "")
    {
        return collection != null && collection.Count != 0 ? collection : throw new RequireException(message ?? $"'{expr}' is null or empty.", file, line, member);
    }
    
    [StackTraceHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IReadOnlyCollection<T> IsNotEmpty<T>(IReadOnlyCollection<T>? collection, string? message = null, [CallerArgumentExpression("collection")] string? expr = null, [CallerFilePath] string file = "", [CallerLineNumber] int line = 0, [CallerMemberName] string member = "")
    {
        return collection is { Count: > 0 } ? collection : throw new RequireException(message ?? $"'{expr}' is null or empty.", file, line, member);
    }
    
    [StackTraceHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T[] IsNotEmpty<T>(T[]? array, string? message = null, [CallerArgumentExpression("array")] string? expr = null, [CallerFilePath] string file = "", [CallerLineNumber] int line = 0, [CallerMemberName] string member = "")
    {
        return array != null && array.Length != 0 ? array : throw new RequireException(message ?? $"'{expr}' is null or empty.", file, line, member);
    }
    
    [StackTraceHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static List<T> IsNotEmpty<T>(List<T>? list, string? message = null, [CallerArgumentExpression("list")] string? expr = null, [CallerFilePath] string file = "", [CallerLineNumber] int line = 0, [CallerMemberName] string member = "")
    {
        return list != null && list.Count != 0 ? list : throw new RequireException(message ?? $"'{expr}' is null or empty.", file, line, member);
    }
    
    [StackTraceHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<T> IsNotEmpty<T>(IEnumerable<T>? sequence, string? message = null, [CallerArgumentExpression("sequence")] string? expr = null, [CallerFilePath] string file = "", [CallerLineNumber] int line = 0, [CallerMemberName] string member = "")
    {
        if (sequence is null)
            throw new RequireException(message ?? $"'{expr}' is null or empty.", file, line, member);

        if (sequence.TryGetNonEnumeratedCount(out var count))
            return count != 0 ? sequence : throw new RequireException(message ?? $"'{expr}' is empty.", file, line, member);

        return IterateOnce(sequence, message, expr, file, line, member);

        static IEnumerable<T> IterateOnce(IEnumerable<T> src, string? message, string? expr, string file, int line, string member)
        {
            using var e = src.GetEnumerator();
            if (!e.MoveNext())
                throw new RequireException(message ?? $"'{expr}' is empty.", file, line, member);

            do { yield return e.Current; } while (e.MoveNext());
        }
    }
    
    
}