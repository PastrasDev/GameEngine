using System.Runtime.InteropServices;

namespace Vyrse.Interop.Windows.Graphics.D3D11;

[Guid("6F15AAF2-D208-4E89-9AB4-489535D34F9C")]
public unsafe class D3D11Texture2D : D3D11Resource
{
	// protected readonly delegate* unmanaged <void*, D3D11_TEXTURE2D_DESC*, void> _GetDesc;

	protected D3D11Texture2D(void* raw, bool addRef = false) : base(raw, addRef)
	{
		// _GetDesc = (delegate* unmanaged <void*, D3D11_TEXTURE2D_DESC*, void>)VTable[10];
	}

	// public D3D11_TEXTURE2D_DESC GetDesc()
	// {
	// 	D3D11_TEXTURE2D_DESC desc;
	// 	_GetDesc(Raw, &desc);
	// 	return desc;
	// }
}

[Guid("51218251-1E33-4617-9CCB-4D3A4367E7BB")]
public sealed unsafe class D3D11Texture2D1 : D3D11Texture2D
{
	// private readonly delegate* unmanaged <void*, D3D11_TEXTURE2D_DESC1*, void> _getDesc1;

	private D3D11Texture2D1(void* raw, bool addRef = false) : base(raw, addRef)
	{
		// _getDesc1 = (delegate* unmanaged <void*, D3D11_TEXTURE2D_DESC1*, void>)VTable[11];
	}

	// public D3D11_TEXTURE2D_DESC1 GetDesc1()
	// {
	// 	D3D11_TEXTURE2D_DESC1 desc;
	// 	_getDesc1(Raw, &desc);
	// 	return desc;
	// }
}