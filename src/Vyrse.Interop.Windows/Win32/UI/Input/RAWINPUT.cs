using System.Runtime.InteropServices;

namespace Windows.Win32.UI.Input;

/// <summary>Contains the raw input from a device.</summary>
/// <remarks>
/// <para>The handle to this structure is passed in the <i>lParam</i> parameter of <a href="https://docs.microsoft.com/windows/desktop/inputdev/wm-input">WM_INPUT</a>. To get detailed information -- such as the header and the content of the raw input -- call <a href="https://docs.microsoft.com/windows/desktop/api/winuser/nf-winuser-getrawinputdata">GetRawInputData</a>. To read the <b>RAWINPUT</b> in the message loop as a buffered read, call <a href="https://docs.microsoft.com/windows/desktop/api/winuser/nf-winuser-getrawinputbuffer">GetRawInputBuffer</a>. To get device specific information, call <a href="https://docs.microsoft.com/windows/desktop/api/winuser/nf-winuser-getrawinputdeviceinfoa">GetRawInputDeviceInfo</a> with the <i>hDevice</i> from <a href="https://docs.microsoft.com/windows/desktop/api/winuser/ns-winuser-rawinputheader">RAWINPUTHEADER</a>. Raw input is available only when the application calls <a href="https://docs.microsoft.com/windows/desktop/api/winuser/nf-winuser-registerrawinputdevices">RegisterRawInputDevices</a> with valid device specifications.</para>
/// <para><see href="https://learn.microsoft.com/windows/win32/api/winuser/ns-winuser-rawinput#">Read more on learn.microsoft.com</see>.</para>
/// </remarks>
[StructLayout(LayoutKind.Sequential)]
public struct RAWINPUT
{
	/// <summary>
	/// <para>Type: <b><a href="https://docs.microsoft.com/windows/desktop/api/winuser/ns-winuser-rawinputheader">RAWINPUTHEADER</a></b> The raw input data.</para>
	/// <para><see href="https://learn.microsoft.com/windows/win32/api/winuser/ns-winuser-rawinput#members">Read more on learn.microsoft.com</see>.</para>
	/// </summary>
	public RAWINPUTHEADER header;

	public Union data;

	[StructLayout(LayoutKind.Explicit)]
	public struct Union
	{
		[FieldOffset(0)] public RAWMOUSE mouse;
		[FieldOffset(0)] public RAWKEYBOARD keyboard;
		[FieldOffset(0)] public RAWHID hid;
	}
}