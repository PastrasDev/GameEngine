using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Input.KeyboardAndMouse;
using Vyrse.Interop.Windows.Win32;

namespace Windows.Win32.UI.Input;

using static Winuser;

/// <summary>Defines the raw input data coming from any device.</summary>
/// <remarks>
/// <para><see href="https://learn.microsoft.com/windows/win32/api/winuser/ns-winuser-rid_device_info">Learn more about this API from learn.microsoft.com</see>.</para>
/// </remarks>
[StructLayout(LayoutKind.Explicit)]
public struct RID_DEVICE_INFO
{
	[FieldOffset(0)] public uint cbSize;
	[FieldOffset(4)] public RID_DEVICE_INFO_TYPE dwType;
	[FieldOffset(8)] public RID_DEVICE_INFO_MOUSE mouse;
	[FieldOffset(8)] public RID_DEVICE_INFO_KEYBOARD keyboard;
	[FieldOffset(8)] public RID_DEVICE_INFO_HID hid;

	public unsafe void* Value => Unsafe.AsPointer(ref this);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public readonly RIM GetMessageType() => (RIM)dwType;

	public static unsafe RID_DEVICE_INFO GetDeviceInfo(in HANDLE hDevice)
	{
		var size = (uint)sizeof(RID_DEVICE_INFO);
		RID_DEVICE_INFO info = new() { cbSize = size };
		return GetRawInputDeviceInfo(hDevice, RAW_INPUT_DEVICE_INFO_COMMAND.RIDI_DEVICEINFO, ref size, (nint)info.Value) == 0xFFFFFFFF ? throw new Win32Exception(Marshal.GetLastWin32Error()) : info;
	}
}