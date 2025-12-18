using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Input.KeyboardAndMouse;

namespace Windows.Win32.UI.Input;

/// <summary>
/// <see href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-rawinputheader">RAWINPUTHEADER</see>
/// Contains the header information that is part of the raw input data.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct RAWINPUTHEADER
{
	public uint dwType;
	public uint dwSize;
	public HANDLE hDevice;
	public WPARAM wParam;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public readonly RIM GetMessageType() => (RIM)dwType;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ref readonly RAWINPUTHEADER AsRef(ReadOnlySpan<byte> span) => ref MemoryMarshal.AsRef<RAWINPUTHEADER>(span);
}