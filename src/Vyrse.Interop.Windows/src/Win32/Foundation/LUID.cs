using System.Runtime.InteropServices;

namespace Windows.Win32.Foundation;

[StructLayout(LayoutKind.Sequential)]
public struct LUID
{
	public uint LowPart;
	public int HighPart;
}