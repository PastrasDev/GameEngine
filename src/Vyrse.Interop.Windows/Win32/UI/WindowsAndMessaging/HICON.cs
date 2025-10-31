using System.Diagnostics;

namespace Windows.Win32.UI.WindowsAndMessaging;

[DebuggerDisplay("{Value}")]
public readonly unsafe struct HICON : IEquatable<HICON>
{
	internal readonly void* Value;

	internal HICON(void* value) => Value = value;

	internal HICON(IntPtr value) : this((void*)value) { }

	internal static HICON Null => default;

	internal bool IsNull => Value == null;

	public static implicit operator void*(HICON value) => value.Value;

	public static explicit operator HICON(void* value) => new(value);

	public static bool operator ==(HICON left, HICON right) => left.Value == right.Value;

	public static bool operator !=(HICON left, HICON right) => !(left == right);

	public bool Equals(HICON other) => Value == other.Value;

	public override bool Equals(object? obj) => obj is HICON other && Equals(other);

	public override int GetHashCode() => unchecked((int)Value);

	public override string ToString() => $"0x{(nuint)Value:x}";

	public static implicit operator IntPtr(HICON value) => new(value.Value);

	public static explicit operator HICON(IntPtr value) => new(value.ToPointer());

	public static explicit operator HICON(UIntPtr value) => new(value.ToPointer());
}