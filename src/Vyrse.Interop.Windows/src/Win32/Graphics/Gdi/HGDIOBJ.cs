using System.Diagnostics;

namespace Windows.Win32.Graphics.Gdi;

[DebuggerDisplay("{Value}")]
public readonly unsafe struct HGDIOBJ : IEquatable<HGDIOBJ>
{
	internal readonly void* Value;

	internal HGDIOBJ(void* value) => Value = value;

	internal HGDIOBJ(IntPtr value) : this((void*)value) { }

	internal static HGDIOBJ Null => default;

	internal bool IsNull => Value == null;

	public static implicit operator void*(HGDIOBJ value) => value.Value;

	public static explicit operator HGDIOBJ(void* value) => new(value);

	public static bool operator ==(HGDIOBJ left, HGDIOBJ right) => left.Value == right.Value;

	public static bool operator !=(HGDIOBJ left, HGDIOBJ right) => !(left == right);

	public bool Equals(HGDIOBJ other) => Value == other.Value;

	public override bool Equals(object? obj) => obj is HGDIOBJ other && Equals(other);

	public override int GetHashCode() => unchecked((int)Value);

	public override string ToString() => $"0x{(nuint)Value:x}";

	public static implicit operator IntPtr(HGDIOBJ value) => new(value.Value);

	public static explicit operator HGDIOBJ(IntPtr value) => new(value.ToPointer());

	public static explicit operator HGDIOBJ(UIntPtr value) => new(value.ToPointer());
}