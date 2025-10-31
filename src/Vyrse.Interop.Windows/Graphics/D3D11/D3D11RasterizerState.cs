using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Vyrse.Interop.Windows.Graphics.D3D11;

[Guid("9BB4AB81-AB1A-4D8F-B506-FC04200B6EE7")]
public unsafe class D3D11RasterizerState : D3D11DeviceChild
{
	// protected readonly delegate* unmanaged <void*, D3D11_RASTERIZER_DESC*, void> _GetDesc;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected D3D11RasterizerState(void* raw, bool addRef = false) : base(raw, addRef)
	{
		// _GetDesc = (delegate* unmanaged<void*, D3D11_RASTERIZER_DESC*, void>)VTable[7];
	}

	// [MethodImpl(MethodImplOptions.AggressiveInlining)]
	// public D3D11_RASTERIZER_DESC GetDesc()
	// {
	// 	D3D11_RASTERIZER_DESC desc;
	// 	_GetDesc(Raw, &desc);
	// 	return desc;
	// }
}

[Guid("1217D7A6-5039-418C-B042-9CBE256AFD6E")]
public unsafe class D3D11RasterizerState1 : D3D11RasterizerState
{
	// protected readonly delegate* unmanaged <void*, D3D11_RASTERIZER_DESC1*, void> _GetDesc1;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected D3D11RasterizerState1(void* raw, bool addRef = false) : base(raw, addRef)
	{
		// _GetDesc1 = (delegate* unmanaged<void*, D3D11_RASTERIZER_DESC1*, void>)VTable[8];
	}

	// [MethodImpl(MethodImplOptions.AggressiveInlining)]
	// public D3D11_RASTERIZER_DESC1 GetDesc1()
	// {
	// 	D3D11_RASTERIZER_DESC1 desc;
	// 	_GetDesc1(Raw, &desc);
	// 	return desc;
	// }
}

[Guid("6FBD02FB-209F-46C4-B059-2ED15586A6AC")]
public sealed unsafe class D3D11RasterizerState2 : D3D11RasterizerState1
{
	// private readonly delegate* unmanaged <void*, D3D11_RASTERIZER_DESC2*, void> _getDesc2;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public D3D11RasterizerState2(void* raw, bool addRef = false) : base(raw, addRef)
	{
		// _getDesc2 = (delegate* unmanaged<void*, D3D11_RASTERIZER_DESC2*, void>)VTable[9];
	}

	// [MethodImpl(MethodImplOptions.AggressiveInlining)]
	// public D3D11_RASTERIZER_DESC2 GetDesc2()
	// {
	// 	D3D11_RASTERIZER_DESC2 desc;
	// 	_getDesc2(Raw, &desc);
	// 	return desc;
	// }
}