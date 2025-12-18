using System.Diagnostics;

namespace Windows.Win32.Graphics.Gdi;

[DebuggerDisplay("{Value}")]
public readonly unsafe struct HBRUSH : IEquatable<HBRUSH>
{
	internal readonly void* Value;

	internal HBRUSH(void* value) => Value = value;

	internal HBRUSH(IntPtr value) : this((void*)value) { }

	internal static HBRUSH Null => default;

	internal bool IsNull => Value == null;

	public static implicit operator void*(HBRUSH value) => value.Value;

	public static explicit operator HBRUSH(void* value) => new(value);

	public static bool operator ==(HBRUSH left, HBRUSH right) => left.Value == right.Value;

	public static bool operator !=(HBRUSH left, HBRUSH right) => !(left == right);

	public bool Equals(HBRUSH other) => Value == other.Value;

	public override bool Equals(object? obj) => obj is HBRUSH other && Equals(other);

	public override int GetHashCode() => unchecked((int)Value);

	public override string ToString() => $"0x{(nuint)Value:x}";

	public static implicit operator IntPtr(HBRUSH value) => new(value.Value);

	public static explicit operator HBRUSH(IntPtr value) => new(value.ToPointer());

	public static explicit operator HBRUSH(UIntPtr value) => new(value.ToPointer());

	public static implicit operator HGDIOBJ(HBRUSH value) => new(value.Value);
}