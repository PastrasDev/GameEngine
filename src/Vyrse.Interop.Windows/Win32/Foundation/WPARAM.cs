using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Win32.UI.WindowsAndMessaging;

namespace Windows.Win32.Foundation;

///<include file="WPARAM.xml" path="doc/members/member[@name='T:Windows.Win32.Foundation.WPARAM']/*"/>
[DebuggerDisplay("{Value}")]
[StructLayout(LayoutKind.Sequential)]
public readonly struct WPARAM : IEquatable<WPARAM>
{
	public readonly nuint Value;
	public static readonly WPARAM Zero = new(0);
	public ushort LOWORD { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => (ushort)(Value & 0xFFFF); }
	public ushort HIWORD { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => (ushort)((Value >> 16) & 0xFFFF); }
	public short SLOWORD { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => (short)(Value & 0xFFFF); }
	public short SHIWORD { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => (short)((Value >> 16) & 0xFFFF); }
	public SC SysCommand { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => (SC)((uint)Value & 0xFFF0u); }
	public WPARAM(nuint value) => Value = value;
	public static implicit operator nuint(WPARAM value) => value.Value;
	public static implicit operator WPARAM(nuint value) => new(value);
	public static implicit operator bool(WPARAM value) => value.Value != 0;
	public static implicit operator WPARAM(bool value) => new(value ? 1u : 0u);
	public static implicit operator uint (WPARAM value) => (uint)value.Value;
	public static implicit operator WPARAM(uint value) => new(value);
	public static bool operator ==(WPARAM left, WPARAM right) => left.Value == right.Value;
	public static bool operator !=(WPARAM left, WPARAM right) => !(left == right);
	public bool Equals(WPARAM other) => Value == other.Value;
	public override bool Equals(object? obj) => obj is WPARAM other && Equals(other);
	public override int GetHashCode() => Value.GetHashCode();
	public override string ToString() => $"0x{Value:x}";
	public static WPARAM FromWords(ushort low, ushort high) => new(low | (uint)(high << 16));
	public void Deconstruct(out ushort low, out ushort high) { low = LOWORD; high = HIWORD; }
}