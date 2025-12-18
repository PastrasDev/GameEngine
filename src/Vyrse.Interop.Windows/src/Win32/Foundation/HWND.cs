using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Windows.Win32.Foundation;

///<include file="HWND.xml" path="doc/members/member[@name='T:Windows.Win32.Foundation.HWND']/*"/>
[DebuggerDisplay("{Value}")]
[StructLayout(LayoutKind.Sequential)]
public readonly unsafe struct HWND : IEquatable<HWND>
{
	public readonly void* Value;
	public HWND(void* value) => Value = value;
	public HWND(IntPtr value) : this((void*)value) { }
	public static HWND Null => default;
	public bool IsNull => Value == null;
	public static implicit operator void*(HWND value) => value.Value;
	public static explicit operator HWND(void* value) => new(value);
	public static bool operator ==(HWND left, HWND right) => left.Value == right.Value;
	public static bool operator !=(HWND left, HWND right) => !(left == right);
	public static implicit operator bool (HWND value) => !value.IsNull;
	public bool Equals(HWND other) => Value == other.Value;
	public override bool Equals(object? obj) => obj is HWND other && Equals(other);
	public override int GetHashCode() => unchecked((int)Value);
	public override string ToString() => $"0x{(nuint)Value:x}";
	public static implicit operator IntPtr(HWND value) => new(value.Value);
	public static explicit operator HWND(IntPtr value) => new(value.ToPointer());
	public static explicit operator HWND(UIntPtr value) => new(value.ToPointer());
	public static implicit operator HANDLE(HWND value) => new(value.Value);
}