using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Windows.Win32.Foundation;

///<include file="LRESULT.xml" path="doc/members/member[@name='T:Windows.Win32.Foundation.LRESULT']/*"/>
[DebuggerDisplay("{Value}")]
[StructLayout(LayoutKind.Sequential)]
public readonly struct LRESULT : IEquatable<LRESULT>
{
	public readonly nint Value;
	public static readonly LRESULT Zero = new(0);
	public bool IsZero => Value == 0;
	public bool IsNonZero => Value != 0;
	public LRESULT(nint value) => Value = value;
	public static implicit operator nint(LRESULT value) => value.Value;
	public static explicit operator LRESULT(nint value) => new(value);
	public static bool operator ==(LRESULT left, LRESULT right) => left.Value == right.Value;
	public static bool operator !=(LRESULT left, LRESULT right) => !(left == right);
	public bool Equals(LRESULT other) => Value == other.Value;
	public override bool Equals(object? obj) => obj is LRESULT other && Equals(other);
	public override int GetHashCode() => Value.GetHashCode();
	public override string ToString() => $"0x{(nuint)Value:x}";
	public static implicit operator long(LRESULT value) => value.Value;
	public static implicit operator LRESULT(long value) => new((nint)value);
	public static explicit operator int(LRESULT value) => (int)value.Value;
	public static explicit operator LRESULT(int value) => new(value);
	public static implicit operator bool(LRESULT value) => value.Value != 0;
	public static implicit operator LRESULT(bool value) => new(value ? 1 : 0);
}