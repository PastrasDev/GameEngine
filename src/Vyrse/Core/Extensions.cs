using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Vyrse.Core
{
	public static partial class Extensions
	{
		/// <summary>
		/// Topologically sorts <paramref name="items"/> using Kahn's algorithm.
		/// Dependencies that are not in the input set are ignored.
		/// Throws with a readable message if a cycle is detected.
		/// </summary>
		public static IEnumerable<T> TopologicalSort<T>(this IEnumerable<T> items, Func<T, Type> keyOf, Func<T, IEnumerable<Type>?> depsOf)
		{
			ArgumentNullException.ThrowIfNull(items);
			ArgumentNullException.ThrowIfNull(keyOf);
			ArgumentNullException.ThrowIfNull(depsOf);

			var nodes = new Dictionary<Type, T>(64);
			foreach (var item in items)
			{
				var k = keyOf(item);
				if (!nodes.TryAdd(k, item))
					throw new InvalidOperationException($"Duplicate node key found: {k.FullName}");
			}

			var incoming = new Dictionary<Type, int>(nodes.Count);
			var outgoing = new Dictionary<Type, List<Type>>(nodes.Count);

			foreach (var (k, item) in nodes)
			{
				var deps = depsOf(item);
				if (deps is null)
				{
					incoming[k] = incoming.GetValueOrDefault(k, 0);
					continue;
				}

				foreach (var d in deps)
				{
					if (!nodes.ContainsKey(d)) continue;

					if (!outgoing.TryGetValue(d, out var list)) outgoing[d] = list = new List<Type>(2);
					list.Add(k);

					incoming[k] = incoming.TryGetValue(k, out var c) ? c + 1 : 1;
					if (!incoming.ContainsKey(d)) incoming[d] = incoming.GetValueOrDefault(d, 0);
				}

				if (!incoming.ContainsKey(k)) incoming[k] = incoming.GetValueOrDefault(k, 0);
			}

			var queue = new Queue<Type>();
			foreach (var (k, deg) in incoming)
			{
				if (deg == 0) queue.Enqueue(k);
			}

			int emitted = 0;
			while (queue.Count > 0)
			{
				var k = queue.Dequeue();
				yield return nodes[k];
				emitted++;

				if (!outgoing.TryGetValue(k, out var next)) continue;
				foreach (var to in next)
				{
					incoming[to] -= 1;
					if (incoming[to] == 0) queue.Enqueue(to);
				}
			}

			if (emitted == nodes.Count) yield break;
			{
				var cyc = new List<string>();
				foreach (var (k, deg) in incoming)
					if (deg > 0) cyc.Add(k.FullName ?? k.Name);
				throw new InvalidOperationException("Dependency cycle detected among components: " + string.Join(" ↔ ", cyc));
			}
		}
	}


	public static partial class Extensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ThrowIfFalse([DoesNotReturnIf(false)] this bool condition, string? message = null, [CallerArgumentExpression("condition")] string? expr = null, [CallerFilePath] string file = "", [CallerLineNumber] int line = 0, [CallerMemberName] string member = "")
		{
			if (!condition) Throw(message ?? $"Condition was false: '{expr}' at {file}:{line} in {member}(): Message: {message}");
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ThrowIfTrue([DoesNotReturnIf(true)] this bool condition, string? message = null, [CallerArgumentExpression("condition")] string? expr = null, [CallerFilePath] string file = "", [CallerLineNumber] int line = 0, [CallerMemberName] string member = "")
		{
			if (condition) Throw(message ?? $"Condition was true: '{expr}' at {file}:{line} in {member}(): Message: {message}");
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ThrowIf(this bool condition, bool value, string? message = null, [CallerArgumentExpression("condition")] string? expr = null, [CallerFilePath] string file = "", [CallerLineNumber] int line = 0, [CallerMemberName] string member = "")
		{
			if (condition == value) Throw(message ?? $"Condition was {value}: '{expr}' at {file}:{line} in {member}(): Message: {message}");
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void LogLastWin32ErrorIf(this bool condition, bool value, string? message = null, [CallerArgumentExpression("condition")] string? expr = null, [CallerFilePath] string file = "", [CallerLineNumber] int line = 0, [CallerMemberName] string member = "")
		{
			if (condition != value) return;
			int errorCode = Marshal.GetLastWin32Error();
			string errorMessage = new Win32Exception(errorCode).Message;
			string fullMessage = message ?? $"Condition was {value}: '{expr}' at {file}:{line} in {member}(): Win32 Error {errorCode}: {errorMessage}";
			Console.Error.WriteLine(fullMessage);
		}










		[DoesNotReturn, MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void Throw(string? message) => throw new InvalidOperationException(message);
	}

	public static partial class Extensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ThrowIfNull<T>([NotNull] this T? obj, string? message = null, [CallerArgumentExpression("obj")] string? expr = null, [CallerFilePath] string file = "", [CallerLineNumber] int line = 0, [CallerMemberName] string member = "") where T : class
		{
			if (obj is null) Throw(message ?? $"Object was null: '{expr}' at {file}:{line} in {member}(): Message: {message}");
		}
	}
}