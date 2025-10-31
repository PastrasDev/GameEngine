using System.Diagnostics;
using System.Runtime.InteropServices;
using Vyrse.Interop.Windows.Primitives;

namespace Windows.Win32.Foundation;

[DebuggerDisplay("{ToHexString(),nq}")]
[StructLayout(LayoutKind.Sequential)]
public struct APP_LOCAL_DEVICE_ID : IEquatable<APP_LOCAL_DEVICE_ID>
{
	public FixedArray32<byte> Value;

	public readonly Span<byte> Span => Value.Span;

	public readonly ReadOnlySpan<byte> ReadOnlySpan => Value.ReadOnlySpan;

	public readonly override string ToString() => ToHexString();

	public readonly string ToHexString() => Convert.ToHexString(ReadOnlySpan);

	public readonly bool Equals(APP_LOCAL_DEVICE_ID other) => ReadOnlySpan.SequenceEqual(other.ReadOnlySpan);

	public readonly override bool Equals(object? obj) => obj is APP_LOCAL_DEVICE_ID other && Equals(other);

	public readonly override int GetHashCode()
	{
		HashCode hc = new();
		hc.AddBytes(ReadOnlySpan);
		return hc.ToHashCode();
	}

	public static bool operator ==(APP_LOCAL_DEVICE_ID left, APP_LOCAL_DEVICE_ID right) => left.Equals(right);
	public static bool operator !=(APP_LOCAL_DEVICE_ID left, APP_LOCAL_DEVICE_ID right) => !left.Equals(right);
}