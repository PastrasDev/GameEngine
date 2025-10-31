using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Vyrse.Interop.Windows;

internal static class Extensions
{
	internal static void ThrowIfNull([NotNull] this object? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
	{
		if (argument is null) Throw(paramName);
	}

	[DoesNotReturn]
	internal static void Throw(string? paramName) => throw new ArgumentNullException(paramName);
}