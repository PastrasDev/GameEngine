using System.Diagnostics;
using System.Runtime.CompilerServices;
using Vyrse.Interop.Windows.Win32;

namespace Windows.Win32.UI.Input;

[DebuggerDisplay("{Value}")]
public readonly unsafe struct HRAWINPUT : IEquatable<HRAWINPUT>
{
	public readonly void* Value;

	public HRAWINPUT(void* value) => Value = value;

	public HRAWINPUT(nint value) : this((void*)value) { }

	public static HRAWINPUT Null => default;

	public bool IsNull => Value == null;

	public static implicit operator void*(HRAWINPUT value) => value.Value;

	public static explicit operator HRAWINPUT(void* value) => new(value);

	public static bool operator ==(HRAWINPUT left, HRAWINPUT right) => left.Value == right.Value;

	public static bool operator !=(HRAWINPUT left, HRAWINPUT right) => !(left == right);

	public bool Equals(HRAWINPUT other) => Value == other.Value;

	public override bool Equals(object? obj) => obj is HRAWINPUT other && Equals(other);

	public override int GetHashCode() => unchecked((int)Value);

	public override string ToString() => $"0x{(nuint)Value:x}";

	public static implicit operator nint(HRAWINPUT value) => new(value.Value);

	public static explicit operator HRAWINPUT(nint value) => new(value.ToPointer());

	public static explicit operator HRAWINPUT(nuint value) => new(value.ToPointer());

	public uint Size
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (IsNull) throw new InvalidOperationException("HRAWINPUT is null.");
			uint size = 0;
			if (Winuser.GetRawInputData(this, RAW_INPUT_DATA_COMMAND_FLAGS.RID_INPUT, 0, ref size) == uint.MaxValue || size == 0)
				return 0;
			return size;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool GetDataBuffer(ref Span<byte> buffer)
	{
		uint size = Size;
		if (size == 0) return false;

		var result = Winuser.GetRawInputData(this, RAW_INPUT_DATA_COMMAND_FLAGS.RID_INPUT, buffer, ref size) != uint.MaxValue && size != 0;

		if (result && size < buffer.Length)
			buffer = buffer[..(int)size];

		return result;
	}
}