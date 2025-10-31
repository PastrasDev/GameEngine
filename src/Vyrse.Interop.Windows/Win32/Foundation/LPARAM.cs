using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Windows.Win32.Foundation;

///<include file="LPARAM.xml" path="doc/members/member[@name='T:Windows.Win32.Foundation.LPARAM']/*"/>
[DebuggerDisplay("{ToString()}")]
[StructLayout(LayoutKind.Sequential)]
public readonly struct LPARAM : IEquatable<LPARAM>
{
	public readonly nint Value;
	public static readonly LPARAM Zero = new(0);
	public LPARAM(nint value) => Value = value;
	public static implicit operator nint(LPARAM value) => value.Value;
	public static implicit operator LPARAM(nint value) => new(value);
	public static implicit operator bool(LPARAM value) => value.Value != 0;
	public static implicit operator LPARAM(bool value) => new(value ? 1 : 0);
	public static explicit operator int(LPARAM value) => (int)value.Value;
	public static explicit operator LPARAM(int value) => new(value);
	public static explicit operator long(LPARAM value) => value.Value;
	public static explicit operator LPARAM(long value) => new((nint)value);
	public static bool operator ==(LPARAM left, LPARAM right) => left.Value == right.Value;
	public static bool operator !=(LPARAM left, LPARAM right) => !(left == right);
	public bool Equals(LPARAM other) => Value == other.Value;
	public override bool Equals(object? obj) => obj is LPARAM other && Equals(other);
	public override int GetHashCode() => Value.GetHashCode();
	public override string ToString() => $"LPARAM(0x{(nuint)Value:x})";
	public ushort LOWORD { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => (ushort)((nuint)Value & 0xFFFFu); }
	public ushort HIWORD { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => (ushort)(((nuint)Value >> 16) & 0xFFFFu); }
	public short SLOWORD { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => (short)((nuint)Value & 0xFFFFu); }
	public short SHIWORD { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => (short)(((nuint)Value >> 16) & 0xFFFFu); }
	public int X { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => (short)((nuint)Value & 0xFFFFu); }
	public int Y { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => (short)(((nuint)Value >> 16) & 0xFFFFu); }
	public static LPARAM FromWords(ushort low, ushort high) => new((nint)(low | (uint)(high << 16)));
	public static LPARAM FromPoint(int x, int y) => new((nint)((ushort)x | ((uint)(ushort)y << 16)));
	public void Deconstruct(out ushort low, out ushort high) { low = LOWORD; high = HIWORD; }
}