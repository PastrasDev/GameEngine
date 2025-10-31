using System.Runtime.InteropServices;

namespace Vyrse.Interop.Windows.Win32.Graphics.Dxgi.Common;

[StructLayout(LayoutKind.Sequential)]
public struct DXGI_SAMPLE_DESC
{
	internal uint Count;
	internal uint Quality;
}