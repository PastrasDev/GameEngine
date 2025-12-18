using Windows.Win32.Foundation;

namespace Windows.Win32.UI.Input;

/// <summary>Defines information for the raw input devices.</summary>
/// <remarks>
/// <para>If <b>RIDEV_NOLEGACY</b> is set for a mouse or a keyboard, the system does not generate any legacy message for that device for the application. For example, if the mouse TLC is set with <b>RIDEV_NOLEGACY</b>, **WM_LBUTTONDOWN** and <a href="https://docs.microsoft.com/windows/win32/inputdev/mouse-input-notifications">related legacy mouse messages</a> are not generated. Likewise, if the keyboard TLC is set with <b>RIDEV_NOLEGACY</b>, **WM_KEYDOWN** and <a href="https://docs.microsoft.com/windows/win32/inputdev/keyboard-input-notifications">related legacy keyboard messages</a> are not generated. If <b>RIDEV_REMOVE</b> is set and the <b>hwndTarget</b> member is not set to <b>NULL</b>, then [RegisterRawInputDevices](nf-winuser-registerrawinputdevices.md) function will fail.</para>
/// <para><see href="https://learn.microsoft.com/windows/win32/api/winuser/ns-winuser-rawinputdevice#">Read more on learn.microsoft.com</see>.</para>
/// </remarks>
public struct RAWINPUTDEVICE
{
	/// <summary>
	/// <para>Type: <b>USHORT</b> [Top level collection](/windows-hardware/drivers/hid/top-level-collections) [Usage page](/windows-hardware/drivers/hid/hid-usages#usage-page) for the raw input device. See [HID Clients Supported in Windows](/windows-hardware/drivers/hid/hid-architecture#hid-clients-supported-in-windows) for details on possible values.</para>
	/// <para><see href="https://learn.microsoft.com/windows/win32/api/winuser/ns-winuser-rawinputdevice#members">Read more on learn.microsoft.com</see>.</para>
	/// </summary>
	public ushort usUsagePage;

	/// <summary>
	/// <para>Type: <b>USHORT</b> [Top level collection](/windows-hardware/drivers/hid/top-level-collections) [Usage ID](/windows-hardware/drivers/hid/hid-usages#usage-id) for the raw input device. See [HID Clients Supported in Windows](/windows-hardware/drivers/hid/hid-architecture#hid-clients-supported-in-windows) for details on possible values.</para>
	/// <para><see href="https://learn.microsoft.com/windows/win32/api/winuser/ns-winuser-rawinputdevice#members">Read more on learn.microsoft.com</see>.</para>
	/// </summary>
	public ushort usUsage;

	/// <summary>Type: <b>DWORD</b></summary>
	public RAWINPUTDEVICE_FLAGS dwFlags;

	/// <summary>
	/// <para>Type: <b>HWND</b> A handle to the target window. If <b>NULL</b> it follows the keyboard focus.</para>
	/// <para><see href="https://learn.microsoft.com/windows/win32/api/winuser/ns-winuser-rawinputdevice#members">Read more on learn.microsoft.com</see>.</para>
	/// </summary>
	public HWND hwndTarget;
}