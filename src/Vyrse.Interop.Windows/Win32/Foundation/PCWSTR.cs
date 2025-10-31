using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Windows.Win32.Foundation;

///<include file="PCWSTR.xml" path="doc/members/member[@name='T:Windows.Win32.Foundation.PCWSTR']/*"/>
[DebuggerDisplay("{" + nameof(DebuggerDisplay) + "}")]
[StructLayout(LayoutKind.Sequential)]
public readonly unsafe struct PCWSTR : IEquatable<PCWSTR>
{
	public readonly char* Value;
	public PCWSTR(char* value) => Value = value;
	public static explicit operator char*(PCWSTR value) => value.Value;
	public static implicit operator PCWSTR(char* value) => new(value);
	public static bool operator ==(PCWSTR left, PCWSTR right) => left.Equals(right);
	public static bool operator !=(PCWSTR left, PCWSTR right) => !left.Equals(right);
	public bool Equals(PCWSTR other) => Value == other.Value;
	public override bool Equals(object? obj) => obj is PCWSTR other && Equals(other);
	public override int GetHashCode() => (int)Value;

	public int Length
	{
		get
		{
			char* p = Value;
			if (p is null) return 0;
			while (*p != '\0') p++;
			return checked((int)(p - Value));
		}
	}

	public override string ToString() => (Value is null ? null : new string(Value)) ?? string.Empty;
	public ReadOnlySpan<char> AsSpan() => Value is null ? default : new ReadOnlySpan<char>(Value, Length);
	private string DebuggerDisplay => ToString();
}