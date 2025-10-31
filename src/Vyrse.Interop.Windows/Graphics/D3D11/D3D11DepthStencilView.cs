using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Vyrse.Interop.Windows.Graphics.D3D11;

[Guid("9FDAC92A-1876-48C3-AFAD-25B94F84A9B6")]
public sealed unsafe class D3D11DepthStencilView : D3D11View
{
	//private readonly delegate* unmanaged <void*, D3D11_DEPTH_STENCIL_VIEW_DESC*, void> _getDesc;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public D3D11DepthStencilView(void* raw, bool addRef = false) : base(raw, addRef)
	{
		//_getDesc = (delegate* unmanaged<void*, D3D11_DEPTH_STENCIL_VIEW_DESC*, void>)VTable[8];
	}

	// [MethodImpl(MethodImplOptions.AggressiveInlining)]
	// public D3D11_DEPTH_STENCIL_VIEW_DESC GetDesc()
	// {
	// 	D3D11_DEPTH_STENCIL_VIEW_DESC desc;
	// 	_getDesc(Raw, &desc);
	// 	return desc;
	// }
}