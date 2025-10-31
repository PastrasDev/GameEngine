using System.Runtime.InteropServices;

namespace Windows.Win32.UI.Input;

/// <summary>
/// <see href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-rid_device_info_hid">RID_DEVICE_INFO_HID</see>
/// Defines the raw input data coming from the specified Human Interface Device (HID).
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct RID_DEVICE_INFO_HID
{
	public uint dwVendorId;
	public uint dwProductId;
	public uint dwVersionNumber;
	public ushort usUsagePage;
	public ushort usUsage;
}