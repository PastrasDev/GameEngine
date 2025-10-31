using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Win32.UI.Input.KeyboardAndMouse;

namespace Windows.Win32.UI.Input;

[StructLayout(LayoutKind.Sequential)]
public struct RAWMOUSE
{
	public MOUSE_STATE usFlags;

	public Union Buttons;

	public uint ulRawButtons;

	public int lLastX;

	public int lLastY;

	public uint ulExtraInformation;

	[StructLayout(LayoutKind.Explicit)]
	public struct Union
	{
		[FieldOffset(0)] public uint ulButtons;
		[FieldOffset(0)] public ushort usButtonFlags;
		[FieldOffset(2)] public ushort usButtonData;
	}

	public readonly RI_MOUSE ButtonFlags => (RI_MOUSE)Buttons.usButtonFlags;
	public readonly short WheelDelta => (Buttons.usButtonFlags & (ushort)RI_MOUSE.WHEEL) != 0 ? (short)Buttons.usButtonData : (short)0;
	public readonly short HWheelDelta => (Buttons.usButtonFlags & (ushort)RI_MOUSE.HWHEEL) != 0 ? (short)Buttons.usButtonData : (short)0;
	public readonly bool IsAbsolute => (usFlags & MOUSE_STATE.MOUSE_MOVE_ABSOLUTE) != 0;
	public readonly bool IsVirtualDesktop => (usFlags & MOUSE_STATE.MOUSE_VIRTUAL_DESKTOP) != 0;
	public readonly bool AttributesChanged => (usFlags & MOUSE_STATE.MOUSE_ATTRIBUTES_CHANGED) != 0;
	public readonly bool NoCoalesce => (usFlags & MOUSE_STATE.MOUSE_MOVE_NOCOALESCE) != 0;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static RAWMOUSE Read(ReadOnlySpan<byte> span)
	{
		int size = Unsafe.SizeOf<RAWMOUSE>();
		return MemoryMarshal.Read<RAWMOUSE>(span.Slice(span.Length - size, size));
	}
}