using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Windows.Win32.Foundation;

///<include file="ATOM.xml" path="doc/members/member[@name='T:Windows.Win32.Foundation.ATOM']/*"/>
[DebuggerDisplay("{Value}")]
[StructLayout(LayoutKind.Sequential)]
public readonly struct ATOM(ushort value) : IEquatable<ATOM>
{
	public readonly ushort Value = value;
	public static implicit operator ushort(ATOM a) => a.Value;
	public static explicit operator ATOM(ushort v) => new(v);
	public override string ToString() => Value.ToString();
	public bool Equals(ATOM other) => Value == other.Value;
	public override bool Equals(object? obj) => obj is ATOM other && Equals(other);
	public override int GetHashCode() => Value.GetHashCode();
    public static bool operator ==(ATOM left, ATOM right) => left.Equals(right);
    public static bool operator !=(ATOM left, ATOM right) => !(left == right);
	public static implicit operator bool(ATOM a) => a.Value != 0;
}