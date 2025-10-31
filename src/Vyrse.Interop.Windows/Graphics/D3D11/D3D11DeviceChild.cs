using System.Runtime.InteropServices;
using Vyrse.Interop.Windows.Com;

namespace Vyrse.Interop.Windows.Graphics.D3D11;

[Guid("1841E5C8-16B0-489B-BCC8-44CFB0D5DEAE")]
public abstract unsafe class D3D11DeviceChild : Unknown
{
	// protected readonly delegate* unmanaged<void*, void**, void> _GetDevice;
	// protected readonly delegate* unmanaged<void*, Guid*, uint*, void*, HRESULT> _GetPrivateData;
	// protected readonly delegate* unmanaged<void*, Guid*, uint, void*, HRESULT> _SetPrivateData;
	// protected readonly delegate* unmanaged<void*, Guid*, void*, HRESULT> _SetPrivateDataInterface;

	protected D3D11DeviceChild(void* raw, bool addRef = false) : base(raw, addRef)
	{
		// _GetDevice = (delegate* unmanaged<void*, void**, void>)VTable[3];
		// _GetPrivateData = (delegate* unmanaged<void*, Guid*, uint*, void*, HRESULT>)VTable[4];
		// _SetPrivateData = (delegate* unmanaged<void*, Guid*, uint, void*, HRESULT>)VTable[5];
		// _SetPrivateDataInterface = (delegate* unmanaged<void*, Guid*, void*, HRESULT>)VTable[6];
	}

	/*[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public D3D11Device GetDevice(CallerInfo info = new())
	{
		ThrowIfNull(info);
		void* raw = null;
		_GetDevice(Raw, &raw);
		return Create<D3D11Device>(raw, info);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint GetPrivateData(in Guid guid, Span<byte> data, CallerInfo info = new())
	{
		uint size = (uint)data.Length;
		fixed (byte* pData = data)
		{
			var hr = _GetPrivateData(Raw, (Guid*)Unsafe.AsPointer(ref Unsafe.AsRef(in guid)), &size, pData);
			if (hr.Value == unchecked((int)0x887A0003)) return size;
			hr.ThrowOnFailure(info);
			return size;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public  void SetPrivateData(in Guid guid, ReadOnlySpan<byte> data, CallerInfo info = new())
	{
		fixed (byte* pData = data)
		{
			_SetPrivateData(Raw, (Guid*)Unsafe.AsPointer(ref Unsafe.AsRef(in guid)), (uint)data.Length, pData).ThrowOnFailure(info);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SetPrivateDataInterface(in Guid guid, Unknown? obj, CallerInfo info = new())
	{
		_SetPrivateDataInterface(Raw, (Guid*)Unsafe.AsPointer(ref Unsafe.AsRef(in guid)), obj == null ? null : obj.Raw).ThrowOnFailure(info);
	}*/
}