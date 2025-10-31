using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Vyrse.Interop.Windows.Com;

namespace Vyrse.Interop.Windows.Graphics.DXGI;

[Guid("AEC22FB8-76F3-4639-9BE0-28EB43A67A2E")]
public abstract unsafe class DXGIObject : Unknown
{
	// protected readonly delegate* unmanaged <void*, Guid*, uint, void*, HRESULT> _SetPrivateData;
	// protected readonly delegate* unmanaged <void*, Guid*, void*, HRESULT> _SetPrivateDataInterface;
	// protected readonly delegate* unmanaged <void*, Guid*, uint*, void*, HRESULT> _GetPrivateData;
	// protected readonly delegate* unmanaged <void*, Guid*, void**, HRESULT> _GetParent;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected DXGIObject(void* raw, bool addRef = false) : base(raw, addRef)
	{
		// _SetPrivateData = (delegate* unmanaged <void*, Guid*, uint, void*, HRESULT>)VTable[3];
		// _SetPrivateDataInterface = (delegate* unmanaged <void*, Guid*, void*, HRESULT>)VTable[4];
		// _GetPrivateData = (delegate* unmanaged <void*, Guid*, uint*, void*, HRESULT>)VTable[5];
		// _GetParent = (delegate* unmanaged <void*, Guid*, void**, HRESULT>)VTable[6];
	}

	// [MethodImpl(MethodImplOptions.AggressiveInlining)]
	// public void SetPrivateData(in Guid guid, ReadOnlySpan<byte> data)
	// {
	// 	fixed (byte* pData = data)
	// 	{
	// 		_SetPrivateData(Raw, (Guid*)Unsafe.AsPointer(ref Unsafe.AsRef(in guid)), (uint)data.Length, pData).ThrowOnFailure();
	// 	}
	// }
	//
	// [MethodImpl(MethodImplOptions.AggressiveInlining)]
	// public void SetPrivateDataInterface(in Guid guid, Unknown? obj)
	// {
	// 	_SetPrivateDataInterface(Raw, (Guid*)Unsafe.AsPointer(ref Unsafe.AsRef(in guid)), obj == null ? null : obj.Raw).ThrowOnFailure();
	// }
	//
	// [MethodImpl(MethodImplOptions.AggressiveInlining)]
	// public uint GetPrivateData(in Guid guid, Span<byte> data)
	// {
	// 	uint size = (uint)data.Length;
	// 	fixed (byte* pData = data)
	// 	{
	// 		var hr = _GetPrivateData(Raw, (Guid*)Unsafe.AsPointer(ref Unsafe.AsRef(in guid)), &size, pData);
	// 		if (hr.Value == unchecked((int)0x887A0003)) return size;
	// 		hr.ThrowOnFailure();
	// 		return size;
	// 	}
	// }
	//
	// [MethodImpl(MethodImplOptions.AggressiveInlining)]
	// public T GetParent<T>() where T : Unknown
	// {
	// 	void* raw = null;
	// 	var iid = typeof(T).GUID;
	// 	_GetParent(Raw, (Guid*)Unsafe.AsPointer(ref iid), &raw).ThrowOnFailure();
	// 	return Create<T>(raw);
	// }
}