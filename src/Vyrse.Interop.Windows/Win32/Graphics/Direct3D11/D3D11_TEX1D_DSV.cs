using System.Runtime.InteropServices;

namespace Windows.Win32.Graphics.Direct3D11;

[StructLayout(LayoutKind.Sequential)]
public struct D3D11_TEX1D_DSV
{
	public uint MipSlice;
}