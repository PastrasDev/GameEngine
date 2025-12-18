using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Windows.Win32.Foundation;

///<include file="HANDLE.xml" path="doc/members/member[@name='T:Windows.Win32.Foundation.HANDLE']/*"/>
[DebuggerDisplay("{Value}")]
[StructLayout(LayoutKind.Sequential)]
public readonly unsafe struct HANDLE : IEquatable<HANDLE>
{
	public readonly void* Value;
	public HANDLE(void* value) => Value = value;
	public HANDLE(nint value) : this((void*)value) { }
	public static HANDLE Null => default;
	public bool IsNull => Value == null;
	public static implicit operator void*(HANDLE value) => value.Value;
	public static explicit operator HANDLE(void* value) => new(value);
	public static bool operator ==(HANDLE left, HANDLE right) => left.Value == right.Value;
	public static bool operator !=(HANDLE left, HANDLE right) => !(left == right);
	public bool Equals(HANDLE other) => Value == other.Value;
	public override bool Equals(object? obj) => obj is HANDLE other && Equals(other);
	public override int GetHashCode() => unchecked((int)Value);
	public override string ToString() => $"0x{(nuint)Value:x}";
	public static implicit operator nint(HANDLE value) => new(value.Value);
	public static explicit operator HANDLE(nint value) => new(value.ToPointer());
	public static explicit operator HANDLE(nuint value) => new(value.ToPointer());
	public static readonly HANDLE INVALID_HANDLE_VALUE = (HANDLE)(nint)(-1);
}