using System.Runtime.InteropServices;
using Windows.Win32.Graphics.Dxgi.Common;

namespace Windows.Win32.Graphics.Direct3D11;

[StructLayout(LayoutKind.Explicit)]
public struct D3D11_DEPTH_STENCIL_VIEW_DESC
{
	[FieldOffset(0)] public DXGI_FORMAT Format;
	[FieldOffset(4)] public D3D11_DSV_DIMENSION ViewDimension;
	[FieldOffset(8)] public D3D11_DSV_FLAG Flags;

	[FieldOffset(12)] public D3D11_TEX1D_DSV Texture1D;
	[FieldOffset(12)] public D3D11_TEX1D_ARRAY_DSV Texture1DArray;
	[FieldOffset(12)] public D3D11_TEX2D_DSV Texture2D;
	[FieldOffset(12)] public D3D11_TEX2D_ARRAY_DSV Texture2DArray;
	[FieldOffset(12)] public D3D11_TEX2DMS_DSV Texture2DMS;
	[FieldOffset(12)] public D3D11_TEX2DMS_ARRAY_DSV Texture2DMSArray;
}