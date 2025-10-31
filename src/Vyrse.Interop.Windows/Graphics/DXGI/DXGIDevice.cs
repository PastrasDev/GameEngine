using System.Runtime.InteropServices;

namespace Vyrse.Interop.Windows.Graphics.DXGI;

[Guid("54EC77FA-1377-44E6-8C32-88FD5F44C84C")]
public unsafe class DXGIDevice : DXGIObject
{
	// protected readonly delegate* unmanaged <void*, void**, HRESULT> _GetAdapter;
	// protected readonly delegate* unmanaged <void*, DXGI_SURFACE_DESC*, uint, DXGI_USAGE, DXGI_SHARED_RESOURCE*, void**, HRESULT> _CreateSurface;
	// protected readonly delegate* unmanaged <void*, void**, DXGI_RESIDENCY*, uint, HRESULT> _QueryResourceResidency;
	// protected readonly delegate* unmanaged <void*, int, HRESULT> _SetGPUThreadPriority;
	// protected readonly delegate* unmanaged <void*, int*, HRESULT> _GetGPUThreadPriority;

	protected DXGIDevice(void* raw, bool addRef = false) : base(raw, addRef)
	{
		// _GetAdapter = (delegate* unmanaged <void*, void**, HRESULT>)VTable[7];
		// _CreateSurface = (delegate* unmanaged <void*, DXGI_SURFACE_DESC*, uint, DXGI_USAGE, DXGI_SHARED_RESOURCE*, void**, HRESULT>)VTable[8];
		// _QueryResourceResidency = (delegate* unmanaged <void*, void**, DXGI_RESIDENCY*, uint, HRESULT>)VTable[9];
		// _SetGPUThreadPriority = (delegate* unmanaged <void*, int, HRESULT>)VTable[10];
		// _GetGPUThreadPriority = (delegate* unmanaged <void*, int*, HRESULT>)VTable[11];
	}

	// public DXGIAdapter GetAdapter(CallerInfo info = new())
	// {
	// 	ThrowIfNull(info);
	// 	void* raw = null;
	// 	_GetAdapter(Raw, &raw).ThrowOnFailure();
	// 	return Create<DXGIAdapter>(raw);
	// }

	// public T GetAdapter<T>(CallerInfo info = new()) where T : DXGIAdapter
	// {
	// 	ThrowIfNull(info);
	// 	void* raw = null;
	// 	_GetAdapter(Raw, &raw).ThrowOnFailure();
	// 	using var baseAdapter = Create<DXGIAdapter>(raw);
	// 	return baseAdapter.Query<T>();
	// }
}

[Guid("77DB970F-6276-48BA-BA28-070143B4392C")]
public unsafe class DXGIDevice1 : DXGIDevice
{
	// protected readonly delegate* unmanaged <void*, uint, HRESULT> _SetMaximumFrameLatency;
	// protected readonly delegate* unmanaged <void*, uint*, HRESULT> _GetMaximumFrameLatency;

	protected DXGIDevice1(void* raw, bool addRef = false) : base(raw, addRef)
	{
		// _SetMaximumFrameLatency = (delegate* unmanaged <void*, uint, HRESULT>)VTable[12];
		// _GetMaximumFrameLatency = (delegate* unmanaged <void*, uint*, HRESULT>)VTable[13];
	}
}

[Guid("05008617-FBFD-4051-A790-144884B4F6A9")]
public unsafe class DXGIDevice2 : DXGIDevice1
{
	// protected readonly delegate* unmanaged <void*, uint, void**, DXGI_OFFER_RESOURCE_PRIORITY, HRESULT> _OfferResources;
	// protected readonly delegate* unmanaged <void*, uint, void**, BOOL*, HRESULT> _ReclaimResources;
	// protected readonly delegate* unmanaged <void*, HANDLE, HRESULT> _EnqueueSetEvent;

	protected DXGIDevice2(void* raw, bool addRef = false) : base(raw, addRef)
	{
		// _OfferResources = (delegate* unmanaged <void*, uint, void**, DXGI_OFFER_RESOURCE_PRIORITY, HRESULT>)VTable[14];
		// _ReclaimResources = (delegate* unmanaged <void*, uint, void**, BOOL*, HRESULT>)VTable[15];
		// _EnqueueSetEvent = (delegate* unmanaged <void*, HANDLE, HRESULT>)VTable[16];
	}
}

[Guid("6007896C-3244-4AFD-BF18-A6D3BEDA5023")]
public unsafe class DXGIDevice3 : DXGIDevice2
{
	// protected readonly delegate* unmanaged <void*, void> _Trim;

	protected DXGIDevice3(void* raw, bool addRef = false) : base(raw, addRef)
	{
		// _Trim = (delegate* unmanaged <void*, void>)VTable[17];
	}
}

[Guid("95B4F95F-D8DA-4CA4-9EE6-3B76D5968A10")]
public sealed unsafe class DXGIDevice4 : DXGIDevice3
{
	// private readonly delegate *unmanaged <void*, uint, void**, DXGI_OFFER_RESOURCE_PRIORITY, uint, HRESULT> _offerResources1;
	// private readonly delegate *unmanaged <void*, uint, void**, DXGI_RECLAIM_RESOURCE_RESULTS*, HRESULT> _reclaimResources1;

	private DXGIDevice4(void* raw, bool addRef = false) : base(raw, addRef)
	{
		// _offerResources1 = (delegate *unmanaged <void*, uint, void**, DXGI_OFFER_RESOURCE_PRIORITY, uint, HRESULT>)VTable[18];
		// _reclaimResources1 = (delegate *unmanaged <void*, uint, void**, DXGI_RECLAIM_RESOURCE_RESULTS*, HRESULT>)VTable[19];
	}
}