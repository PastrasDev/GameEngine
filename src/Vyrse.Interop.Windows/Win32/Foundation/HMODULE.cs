using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Windows.Win32.Foundation;

///<include file="HMODULE.xml" path="doc/members/member[@name='T:Windows.Win32.Foundation.HMODULE']/*"/>
[DebuggerDisplay("{Value}")]
[StructLayout(LayoutKind.Sequential)]
public readonly unsafe struct HMODULE : IEquatable<HMODULE>
{
	internal readonly void* Value;
	internal HMODULE(void* value) => Value = value;
	internal HMODULE(IntPtr value) : this((void*)value) { }
	internal static HMODULE Null => default;
	internal bool IsNull => Value == null;
	public static implicit operator void*(HMODULE value) => value.Value;
	public static explicit operator HMODULE(void* value) => new(value);
	public static bool operator ==(HMODULE left, HMODULE right) => left.Value == right.Value;
	public static bool operator !=(HMODULE left, HMODULE right) => !(left == right);
	public bool Equals(HMODULE other) => Value == other.Value;
	public override bool Equals(object? obj) => obj is HMODULE other && Equals(other);
	public override int GetHashCode() => (int)Value;
	public override string ToString() => $"0x{(nuint)Value:x}";
	public static implicit operator IntPtr(HMODULE value) => new(value.Value);
	public static explicit operator HMODULE(IntPtr value) => new(value.ToPointer());
	public static explicit operator HMODULE(UIntPtr value) => new(value.ToPointer());
	public static implicit operator HINSTANCE(HMODULE value) => new(value.Value);
}