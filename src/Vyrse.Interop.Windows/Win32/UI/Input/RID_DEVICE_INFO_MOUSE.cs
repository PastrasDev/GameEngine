using System.Runtime.InteropServices;
using Windows.Win32.Foundation;

namespace Windows.Win32.UI.Input;

/// <summary>Defines the raw input data coming from the specified mouse.</summary>
/// <remarks>For the mouse, the Usage Page is 1 and the Usage is 2.</remarks>
[StructLayout(LayoutKind.Sequential)]
public struct RID_DEVICE_INFO_MOUSE
{
	public uint dwId;
	public uint dwNumberOfButtons;
	public uint dwSampleRate;
	public BOOL fHasHorizontalWheel;
}