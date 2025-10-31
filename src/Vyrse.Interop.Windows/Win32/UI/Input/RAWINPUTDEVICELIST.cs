using Windows.Win32.Foundation;

namespace Windows.Win32.UI.Input;

public struct RAWINPUTDEVICELIST
{
	/// <summary>
	/// <para>Type: <b>HANDLE</b> A handle to the raw input device.</para>
	/// <para><see href="https://learn.microsoft.com/windows/win32/api/winuser/ns-winuser-rawinputdevicelist#members">Read more on learn.microsoft.com</see>.</para>
	/// </summary>
	internal HANDLE hDevice;

	/// <summary>Type: <b>DWORD</b></summary>
	internal RID_DEVICE_INFO_TYPE dwType;
}