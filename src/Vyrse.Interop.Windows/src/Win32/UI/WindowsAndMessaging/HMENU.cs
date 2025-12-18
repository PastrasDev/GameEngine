namespace Windows.Win32.UI.WindowsAndMessaging;

public readonly unsafe struct HMENU : IEquatable<HMENU>
{
	internal readonly void* Value;

	internal HMENU(void* value) => Value = value;

	internal HMENU(IntPtr value) : this((void*)value) { }

	internal static HMENU Null => default;

	internal bool IsNull => Value == null;

	public static implicit operator void*(HMENU value) => value.Value;

	public static explicit operator HMENU(void* value) => new(value);

	public static bool operator ==(HMENU left, HMENU right) => left.Value == right.Value;

	public static bool operator !=(HMENU left, HMENU right) => !(left == right);

	public bool Equals(HMENU other) => Value == other.Value;

	public override bool Equals(object? obj) => obj is HMENU other && Equals(other);

	public override int GetHashCode() => (int)Value;

	public override string ToString() => $"0x{(nuint)Value:x}";

	public static implicit operator IntPtr(HMENU value) => new(value.Value);

	public static explicit operator HMENU(IntPtr value) => new(value.ToPointer());

	public static explicit operator HMENU(UIntPtr value) => new(value.ToPointer());
}