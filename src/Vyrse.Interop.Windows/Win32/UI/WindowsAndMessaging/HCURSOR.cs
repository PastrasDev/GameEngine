using System.Diagnostics;

namespace Windows.Win32.UI.WindowsAndMessaging;

[DebuggerDisplay("{Value}")]
public readonly unsafe struct HCURSOR : IEquatable<HCURSOR>
{
	internal readonly void* Value;

	internal HCURSOR(void* value) => Value = value;

	internal HCURSOR(IntPtr value) : this((void*)value) { }

	internal static HCURSOR Null => default;

	internal bool IsNull => Value == null;

	public static implicit operator void*(HCURSOR value) => value.Value;

	public static explicit operator HCURSOR(void* value) => new(value);

	public static bool operator ==(HCURSOR left, HCURSOR right) => left.Value == right.Value;

	public static bool operator !=(HCURSOR left, HCURSOR right) => !(left == right);

	public bool Equals(HCURSOR other) => Value == other.Value;

	public override bool Equals(object? obj) => obj is HCURSOR other && Equals(other);

	public override int GetHashCode() => unchecked((int)Value);

	public override string ToString() => $"0x{(nuint)Value:x}";

	public static implicit operator IntPtr(HCURSOR value) => new(value.Value);

	public static explicit operator HCURSOR(IntPtr value) => new(value.ToPointer());

	public static explicit operator HCURSOR(UIntPtr value) => new(value.ToPointer());

	public static implicit operator HICON(HCURSOR value) => new(value.Value);
}