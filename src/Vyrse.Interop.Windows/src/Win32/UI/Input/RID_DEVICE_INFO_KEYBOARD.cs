using System.Runtime.InteropServices;

namespace Windows.Win32.UI.Input;

/// <summary>Defines the raw input data coming from the specified keyboard.</summary>
/// <remarks>For information about keyboard types, subtypes, scan code modes, and related keyboard layouts, see the documentation in *kbd.h*, *ntdd8042.h* and *ntddkbd.h* headers in Windows SDK, and the [Keyboard Layout Samples](/samples/microsoft/windows-driver-samples/keyboard-layout-samples/).</remarks>
[StructLayout(LayoutKind.Sequential)]
public struct RID_DEVICE_INFO_KEYBOARD
{
	public uint dwType;
	public uint dwSubType;
	public uint dwKeyboardMode;
	public uint dwNumberOfFunctionKeys;
	public uint dwNumberOfIndicators;
	public uint dwNumberOfKeysTotal;
}