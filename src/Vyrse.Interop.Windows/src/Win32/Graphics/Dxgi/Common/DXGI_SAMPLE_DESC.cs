using System.Runtime.InteropServices;

namespace Windows.Win32.Graphics.Dxgi.Common;

[StructLayout(LayoutKind.Sequential)]
public struct DXGI_SAMPLE_DESC
{
	internal uint Count;
	internal uint Quality;
}