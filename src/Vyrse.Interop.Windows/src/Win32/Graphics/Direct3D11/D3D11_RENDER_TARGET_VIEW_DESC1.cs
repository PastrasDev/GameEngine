using System.Runtime.InteropServices;
using Windows.Win32.Graphics.Dxgi.Common;

namespace Windows.Win32.Graphics.Direct3D11;

[StructLayout(LayoutKind.Sequential)]
public struct D3D11_RENDER_TARGET_VIEW_DESC1
{
	public DXGI_FORMAT Format;
	public D3D11_RTV_DIMENSION ViewDimension;
	public Union Anonymous;

	[StructLayout(LayoutKind.Explicit)]
	public struct Union
	{
		[FieldOffset(0)] public D3D11_BUFFER_RTV Buffer;
		[FieldOffset(0)] public D3D11_TEX1D_RTV Texture1D;
		[FieldOffset(0)] public D3D11_TEX1D_ARRAY_RTV Texture1DArray;
		[FieldOffset(0)] public D3D11_TEX2D_RTV1 Texture2D;
		[FieldOffset(0)] public D3D11_TEX2D_ARRAY_RTV1 Texture2DArray;
		[FieldOffset(0)] public D3D11_TEX2DMS_RTV Texture2DMS;
		[FieldOffset(0)] public D3D11_TEX2DMS_ARRAY_RTV Texture2DMSArray;
		[FieldOffset(0)] public D3D11_TEX3D_RTV Texture3D;
	}
}