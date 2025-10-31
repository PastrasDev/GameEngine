using System.Runtime.InteropServices;

namespace Vyrse.Interop.Windows.Graphics.D3D11;

[Guid("DFDBA067-0B8D-4865-875B-D7B4516CC164")]
public unsafe class D3D11RenderTargetView : D3D11View
{
	// protected readonly delegate* unmanaged <void*, D3D11_RENDER_TARGET_VIEW_DESC*, void> _GetDesc;

	protected D3D11RenderTargetView(void* raw, bool addRef = false) : base(raw, addRef)
	{
		// _GetDesc = (delegate* unmanaged<void*, D3D11_RENDER_TARGET_VIEW_DESC*, void>)VTable[8];
	}

	// [MethodImpl(MethodImplOptions.AggressiveInlining)]
	// public D3D11_RENDER_TARGET_VIEW_DESC GetDesc()
	// {
	// 	D3D11_RENDER_TARGET_VIEW_DESC desc;
	// 	_GetDesc(Raw, &desc);
	// 	return desc;
	// }
}

[Guid("FFBE2E23-F011-418A-AC56-5CEED7C5B94B")]
public sealed unsafe class D3D11RenderTargetView1 : D3D11RenderTargetView
{
	// private readonly delegate* unmanaged <void*, D3D11_RENDER_TARGET_VIEW_DESC1*, void> _getDesc1;

	public D3D11RenderTargetView1(void* raw, bool addRef = false) : base(raw, addRef)
	{
		// _getDesc1 = (delegate* unmanaged<void*, D3D11_RENDER_TARGET_VIEW_DESC1*, void>)VTable[9];
	}

	// [MethodImpl(MethodImplOptions.AggressiveInlining)]
	// public D3D11_RENDER_TARGET_VIEW_DESC1 GetDesc1()
	// {
	// 	D3D11_RENDER_TARGET_VIEW_DESC1 desc;
	// 	_getDesc1(Raw, &desc);
	// 	return desc;
	// }
}