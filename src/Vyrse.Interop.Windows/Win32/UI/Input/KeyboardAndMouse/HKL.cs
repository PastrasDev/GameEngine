using System.Diagnostics;

namespace Windows.Win32.UI.Input.KeyboardAndMouse;

[DebuggerDisplay("{Value}")]
public readonly unsafe struct HKL : IEquatable<HKL>
{
	internal readonly void* Value;
	internal HKL(void* value) => Value = value;
	internal HKL(IntPtr value) : this((void*)value) { }
	internal static HKL Null => default;
	internal bool IsNull => Value == null;
	public static implicit operator void*(HKL value) => value.Value;
	public static explicit operator HKL(void* value) => new(value);
	public static bool operator ==(HKL left, HKL right) => left.Value == right.Value;
	public static bool operator !=(HKL left, HKL right) => !(left == right);
	public bool Equals(HKL other) => Value == other.Value;
	public override bool Equals(object? obj) => obj is HKL other && Equals(other);
	public override int GetHashCode() => (int)Value;
	public override string ToString() => $"0x{(nuint)Value:x}";
	public static implicit operator IntPtr(HKL value) => new(value.Value);
	public static explicit operator HKL(IntPtr value) => new(value.ToPointer());
	public static explicit operator HKL(UIntPtr value) => new(value.ToPointer());
}