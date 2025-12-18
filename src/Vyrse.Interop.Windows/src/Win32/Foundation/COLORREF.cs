using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Windows.Win32.Foundation;

///<include file="COLORREF.xml" path="doc/members/member[@name='T:Windows.Win32.Foundation.COLORREF']/*"/>
[DebuggerDisplay("{ToString()}")]
[StructLayout(LayoutKind.Sequential)]
public readonly struct COLORREF : IEquatable<COLORREF>
{
	public readonly uint Value;
	public COLORREF(uint value) => Value = value & 0x00FFFFFFu;
	public static implicit operator uint(COLORREF value) => value.Value;
	public static explicit operator COLORREF(uint value) => new(value);
	public static implicit operator COLORREF(Color value) => new((uint)(value.R | (value.G << 8) | (value.B << 16)));
	public static implicit operator Color(COLORREF value) => Color.FromArgb(255, (int)(value.Value & 0xFF), (int)((value.Value >> 8) & 0xFF), (int)((value.Value >> 16) & 0xFF));
	public static bool operator ==(COLORREF left, COLORREF right) => left.Value == right.Value;
	public static bool operator !=(COLORREF left, COLORREF right) => !(left == right);
	public bool Equals(COLORREF other) => Value == other.Value;
	public override bool Equals(object? obj) => obj is COLORREF other && Equals(other);
	public override int GetHashCode() => (int)Value;
	public override string ToString() => $"COLORREF(0x{Value:x6})";
	public byte R => (byte)(Value & 0xFF);
	public byte G => (byte)((Value >> 8) & 0xFF);
	public byte B => (byte)((Value >> 16) & 0xFF);
	public static COLORREF FromRGB(byte r, byte g, byte b) => new((uint)(r | (g << 8) | (b << 16)));
}