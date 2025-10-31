using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Vyrse.Interop.Windows.Graphics.D3D11;

[Guid("DC8E63F3-D12B-4952-B47B-5E45026A862D")]
public abstract unsafe class D3D11Resource : D3D11DeviceChild
{
	// internal readonly delegate* unmanaged<void*, D3D11_RESOURCE_DIMENSION*, void> _GetResourceDimension;
	// internal readonly delegate* unmanaged<void*, uint, void> _SetEvictionPriority;
	// internal readonly delegate* unmanaged<void*, uint> _GetEvictionPriority;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected D3D11Resource(void* raw, bool addRef = false) : base(raw, addRef)
	{
		// _GetResourceDimension = (delegate* unmanaged<void*, D3D11_RESOURCE_DIMENSION*, void>)VTable[7];
		// _SetEvictionPriority = (delegate* unmanaged<void*, uint, void>)VTable[8];
		// _GetEvictionPriority = (delegate* unmanaged<void*, uint>)VTable[9];
	}

	// [MethodImpl(MethodImplOptions.AggressiveInlining)]
	// public D3D11_RESOURCE_DIMENSION GetResourceDimension()
	// {
	// 	D3D11_RESOURCE_DIMENSION dim;
	// 	_GetResourceDimension(Raw, &dim);
	// 	return dim;
	// }
	//
	// [MethodImpl(MethodImplOptions.AggressiveInlining)]
	// public void SetEvictionPriority(uint priority) => _SetEvictionPriority(Raw, priority);
	//
	// [MethodImpl(MethodImplOptions.AggressiveInlining)]
	// public uint GetEvictionPriority() => _GetEvictionPriority(Raw);
}