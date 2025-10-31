using System.Runtime.InteropServices;

namespace Vyrse.Interop.Windows.Graphics.D3D11;

[Guid("839D1216-BB2E-412B-B7F4-A9DBEBE08ED1")]
public abstract unsafe class D3D11View : D3D11DeviceChild
{
	// protected readonly delegate* unmanaged <void*, void**, void> _GetResource;

	protected D3D11View(void* raw, bool addRef = false) : base(raw, addRef)
	{
		// _GetResource = (delegate* unmanaged<void*, void**, void>)VTable[7];
	}

	// [MethodImpl(MethodImplOptions.AggressiveInlining)]
	// public T GetResource<T>(CallerInfo info = new()) where T : D3D11Resource
	// {
	// 	ThrowIfNull(info);
	// 	void* raw = null;
	// 	_GetResource(Raw, &raw);
	// 	return Create<T>(raw);
	// }
}