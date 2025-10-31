using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Win32.UI.Input.KeyboardAndMouse;

namespace Windows.Win32.UI.Input;

public struct RAWKEYBOARD
{
	/// <summary>
	/// <para>Type: <b>USHORT</b> Specifies the <see href="https://learn.microsoft.com/en-us/windows/win32/inputdev/about-keyboard-input#scan-codes">scan code</see> associated with a key press. See Remarks.</para>
	/// <para><see href="https://learn.microsoft.com/windows/win32/api/winuser/ns-winuser-rawkeyboard#members">Read more on learn.microsoft.com</see>.</para>
	/// </summary>
	public ushort MakeCode;

	/// Type: <b>USHORT</b> Flags for scan code information.
	public ushort Flags;

	/// <summary>
	/// <para>Type: <b>USHORT</b> Reserved; must be zero.</para>
	/// <para><see href="https://learn.microsoft.com/windows/win32/api/winuser/ns-winuser-rawkeyboard#members">Read more on learn.microsoft.com</see>.</para>
	/// </summary>
	public ushort Reserved;

	/// <summary>
	/// <para>Type: <b>USHORT</b> The corresponding <see href="https://learn.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes">legacy virtual-key code.</see></para>
	/// <para><see href="https://learn.microsoft.com/windows/win32/api/winuser/ns-winuser-rawkeyboard#members">Read more on learn.microsoft.com</see>.</para>
	/// </summary>
	public ushort VKey;

	/// <summary>
	/// <para>Type: <b>UINT</b> The corresponding <see href="https://learn.microsoft.com/en-us/windows/win32/inputdev/keyboard-input-notifications">legacy keyboard window message</see>, for example <see href="https://learn.microsoft.com/en-us/windows/win32/inputdev/wm-keydown">WM_KEYDOWN</see>, <see href="https://learn.microsoft.com/en-us/windows/win32/inputdev/wm-syskeydown">WM_SYSKEYDOWN</see>, and so forth.</para>
	/// <para><see href="https://learn.microsoft.com/windows/win32/api/winuser/ns-winuser-rawkeyboard#members">Read more on learn.microsoft.com</see>.</para>
	/// </summary>
	public uint Message;

	/// <summary>
	/// <para>Type: <b>ULONG</b> The device-specific additional information for the event.</para>
	/// <para><see href="https://learn.microsoft.com/windows/win32/api/winuser/ns-winuser-rawkeyboard#members">Read more on learn.microsoft.com</see>.</para>
	/// </summary>
	public uint ExtraInformation;

	public readonly bool IsE0 => (Flags & (ushort)RI_KEY.E0) != 0;
	public readonly bool IsE1 => (Flags & (ushort)RI_KEY.E1) != 0;

	public readonly bool IsMake => (Flags & (ushort)RI_KEY.BREAK) == 0;
	public readonly bool IsBreak => (Flags & (ushort)RI_KEY.BREAK) != 0;

	public readonly bool IsSynthetic => IsE0 && MakeCode == (ushort)ScanCode.LeftShift;

	public readonly bool IsSyntheticKey(ScanCode code) => IsE0 && MakeCode == (ushort)code;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static RAWKEYBOARD Read(ReadOnlySpan<byte> span)
	{
		int size = Unsafe.SizeOf<RAWKEYBOARD>();
		return MemoryMarshal.Read<RAWKEYBOARD>(span.Slice(span.Length - size, size));
	}
}