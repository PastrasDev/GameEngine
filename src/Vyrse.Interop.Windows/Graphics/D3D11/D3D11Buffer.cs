using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Vyrse.Interop.Windows.Graphics.D3D11;

[Guid("48570B85-D1EE-4FCD-A250-EB350722B037")]
public sealed unsafe class D3D11Buffer : D3D11Resource
{
	//private readonly delegate* unmanaged <void*, D3D11_BUFFER_DESC*, void> _getDesc;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public  D3D11Buffer(void* raw, bool addRef = false) : base(raw, addRef)
	{
		//_getDesc = (delegate* unmanaged <void*, D3D11_BUFFER_DESC*, void>)VTable[10];
	}

	// [MethodImpl(MethodImplOptions.AggressiveInlining)]
	// public D3D11_BUFFER_DESC GetDesc()
	// {
	// 	var desc = new D3D11_BUFFER_DESC();
	// 	_getDesc(Raw, &desc);
	// 	return desc;
	// }
}