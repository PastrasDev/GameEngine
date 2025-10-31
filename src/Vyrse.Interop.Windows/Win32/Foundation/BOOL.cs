using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Windows.Win32.Foundation;

///<include file="BOOL.xml" path="doc/members/member[@name='T:Windows.Win32.Foundation.BOOL']/*"/>
[DebuggerDisplay("{Value}")]
[StructLayout(LayoutKind.Sequential)]
public readonly struct BOOL : IEquatable<BOOL>
{
	public readonly int Value;
	public BOOL(int value) => Value = value;
	public static implicit operator int(BOOL value) => value.Value;
	public static explicit operator BOOL(int value) => new(value);
	public static bool operator ==(BOOL left, BOOL right) => left.Value == right.Value;
	public static bool operator !=(BOOL left, BOOL right) => !(left == right);
	public bool Equals(BOOL other) => Value == other.Value;
	public override bool Equals(object? obj) => obj is BOOL other && Equals(other);
	public override int GetHashCode() => Value.GetHashCode();
	public override string ToString() => $"0x{Value:x}";
	public BOOL(bool value) => Value = value ? 1 : 0;
	public static implicit operator bool(BOOL value) => value.Value != 0;
	public static implicit operator BOOL(bool value) => new(value);
	public static readonly BOOL TRUE = (BOOL)(1);
	public static readonly BOOL FALSE = (BOOL)(0);
}