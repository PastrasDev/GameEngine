using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Windows.Win32.Foundation;

///<include file="HINSTANCE.xml" path="doc/members/member[@name='T:Windows.Win32.Foundation.HINSTANCE']/*"/>
[DebuggerDisplay("{Value}")]
[StructLayout(LayoutKind.Sequential)]
public readonly unsafe struct HINSTANCE : IEquatable<HINSTANCE>
{
	internal readonly void* Value;
	internal HINSTANCE(void* value) => Value = value;
	internal HINSTANCE(IntPtr value) : this((void*)value) { }
	internal static HINSTANCE Null => default;
	internal bool IsNull => Value == null;
	public static implicit operator void*(HINSTANCE value) => value.Value;
	public static explicit operator HINSTANCE(void* value) => new(value);
	public static bool operator ==(HINSTANCE left, HINSTANCE right) => left.Value == right.Value;
	public static bool operator !=(HINSTANCE left, HINSTANCE right) => !(left == right);
	public bool Equals(HINSTANCE other) => Value == other.Value;
	public override bool Equals(object? obj) => obj is HINSTANCE other && Equals(other);
	public override int GetHashCode() => unchecked((int)Value);
	public override string ToString() => $"0x{(nuint)Value:x}";
	public static implicit operator IntPtr(HINSTANCE value) => new(value.Value);
	public static explicit operator HINSTANCE(IntPtr value) => new(value.ToPointer());
	public static explicit operator HINSTANCE(UIntPtr value) => new(value.ToPointer());
	public static implicit operator HMODULE(HINSTANCE value) => new(value.Value);
}