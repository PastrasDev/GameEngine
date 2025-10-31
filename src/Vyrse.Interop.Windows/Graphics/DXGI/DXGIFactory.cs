using System.Runtime.InteropServices;

namespace Vyrse.Interop.Windows.Graphics.DXGI;

[Guid("7B7166EC-21C7-44AE-B21A-C9AE321AE369")]
public unsafe class DXGIFactory : DXGIObject
{
	// protected readonly delegate* unmanaged<void*, uint, void**, HRESULT> _EnumAdapters;
	// protected readonly delegate* unmanaged<void*, HWND, DXGI_MWA_FLAGS, HRESULT> _MakeWindowAssociation;
	// protected readonly delegate* unmanaged<void*, HWND*, HRESULT> _GetWindowAssociation;
	// protected readonly delegate* unmanaged<void*, void*, DXGI_SWAP_CHAIN_DESC*, void**, HRESULT> _CreateSwapChain;
	// protected readonly delegate* unmanaged<void*, HMODULE, void**, HRESULT> _CreateSoftwareAdapter;
	
	protected DXGIFactory(void* raw, bool addRef = false) : base(raw, addRef)
	{
		// _EnumAdapters = (delegate* unmanaged<void*, uint, void**, HRESULT>)VTable[7];
		// _MakeWindowAssociation = (delegate* unmanaged<void*, HWND, DXGI_MWA_FLAGS, HRESULT>)VTable[8];
		// _GetWindowAssociation = (delegate* unmanaged<void*, HWND*, HRESULT>)VTable[9];
		// _CreateSwapChain = (delegate* unmanaged<void*, void*, DXGI_SWAP_CHAIN_DESC*, void**, HRESULT>)VTable[10];
		// _CreateSoftwareAdapter = (delegate* unmanaged<void*, HMODULE, void**, HRESULT>)VTable[11];
	}
}

[Guid("770AAE78-F26F-4DBA-A829-253C83D1B387")]
public unsafe class DXGIFactory1 : DXGIFactory
{
	// protected readonly delegate* unmanaged<void*, uint, void**, HRESULT> _EnumAdapters1;
	// protected readonly delegate* unmanaged<void*, BOOL> _IsCurrent;

	protected DXGIFactory1(void* raw, bool addRef = false) : base(raw, addRef)
	{
		// _EnumAdapters1 = (delegate* unmanaged<void*, uint, void**, HRESULT>)VTable[12];
		// _IsCurrent = (delegate* unmanaged<void*, BOOL>)VTable[13];
	}
}

[Guid("50C83A1C-E072-4C48-87B0-3630FA36A6D0")]
public unsafe class DXGIFactory2 : DXGIFactory1
{
	// protected readonly delegate* unmanaged<void*, BOOL> _IsWindowedStereoEnabled;
	// protected readonly delegate* unmanaged<void*, void*, HWND, DXGI_SWAP_CHAIN_DESC1*, DXGI_SWAP_CHAIN_FULLSCREEN_DESC*, void*, void**, HRESULT> _CreateSwapChainForHwnd;
	// protected readonly delegate* unmanaged<void*, void*, void*, DXGI_SWAP_CHAIN_DESC1*, void*, void**, HRESULT> _CreateSwapChainForCoreWindow;
	// protected readonly delegate* unmanaged<void*, HANDLE, LUID*, HRESULT> _GetSharedResourceAdapterLuid;
	// protected readonly delegate* unmanaged<void*, HWND, uint, uint*, HRESULT> _RegisterStereoStatusWindow;
	// protected readonly delegate* unmanaged<void*, HANDLE, uint*, HRESULT> _RegisterStereoStatusEvent;
	// protected readonly delegate* unmanaged<void*, uint, void> _UnregisterStereoStatus;
	// protected readonly delegate* unmanaged<void*, HWND, uint, uint*, HRESULT> _RegisterOcclusionStatusWindow;
	// protected readonly delegate* unmanaged<void*, HANDLE, uint*, HRESULT> _RegisterOcclusionStatusEvent;
	// protected readonly delegate* unmanaged<void*, uint, void> _UnregisterOcclusionStatus;
	// protected readonly delegate* unmanaged<void*, void*, DXGI_SWAP_CHAIN_DESC1*, void*, void**, HRESULT> _CreateSwapChainForComposition;
		
	protected DXGIFactory2(void* raw, bool addRef = false) : base(raw, addRef)
	{
		// _IsWindowedStereoEnabled = (delegate* unmanaged<void*, BOOL>)VTable[14];
		// _CreateSwapChainForHwnd = (delegate* unmanaged<void*, void*, HWND, DXGI_SWAP_CHAIN_DESC1*, DXGI_SWAP_CHAIN_FULLSCREEN_DESC*, void*, void**, HRESULT>)VTable[15];
		// _CreateSwapChainForCoreWindow = (delegate* unmanaged<void*, void*, void*, DXGI_SWAP_CHAIN_DESC1*, void*, void**, HRESULT>)VTable[16];
		// _GetSharedResourceAdapterLuid = (delegate* unmanaged<void*, HANDLE, LUID*, HRESULT>)VTable[17];
		// _RegisterStereoStatusWindow = (delegate* unmanaged<void*, HWND, uint, uint*, HRESULT>)VTable[18];
		// _RegisterStereoStatusEvent = (delegate* unmanaged<void*, HANDLE, uint*, HRESULT>)VTable[19];
		// _UnregisterStereoStatus = (delegate* unmanaged<void*, uint, void>)VTable[20];
		// _RegisterOcclusionStatusWindow = (delegate* unmanaged<void*, HWND, uint, uint*, HRESULT>)VTable[21];
		// _RegisterOcclusionStatusEvent = (delegate* unmanaged<void*, HANDLE, uint*, HRESULT>)VTable[22];
		// _UnregisterOcclusionStatus = (delegate* unmanaged<void*, uint, void>)VTable[23];
		// _CreateSwapChainForComposition = (delegate* unmanaged<void*, void*, DXGI_SWAP_CHAIN_DESC1*, void*, void**, HRESULT>)VTable[24];
	}

	// public DXGISwapChain1 CreateSwapChainForHwnd(Unknown device, HWND hwnd, in DXGI_SWAP_CHAIN_DESC1 desc, DXGI_SWAP_CHAIN_FULLSCREEN_DESC? fullscreenDesc = null, DXGIOutput? restrictToOutput = null)
	// {
	// 	if (device.IsNull) throw new ArgumentNullException(nameof(device));
	// 	if (hwnd.IsNull) throw new ArgumentException("HWND is null.", nameof(hwnd));
	//
	// 	var d = desc;
	// 	var fs = fullscreenDesc.GetValueOrDefault();
	// 	DXGI_SWAP_CHAIN_FULLSCREEN_DESC* pFDesc = fullscreenDesc.HasValue ? &fs : null;
	//
	// 	void* raw = null;
	// 	_CreateSwapChainForHwnd(Raw, device, hwnd, &d, pFDesc, restrictToOutput == null ? null : restrictToOutput.Raw, &raw).ThrowOnFailure();
	// 	return Create<DXGISwapChain1>(raw);
	// }
}

[Guid("25483823-CD46-4C7D-86CA-47AA95B837BD")]
public unsafe class DXGIFactory3 : DXGIFactory2
{
	// protected readonly delegate* unmanaged<void*, DXGI_CREATE_FACTORY_FLAGS> _GetCreationFlags;
	
	protected DXGIFactory3(void* raw, bool addRef = false) : base(raw, addRef)
	{
		// _GetCreationFlags = (delegate* unmanaged<void*, DXGI_CREATE_FACTORY_FLAGS>)VTable[25];
	}
}

[Guid("1BC6EA02-EF36-464F-BF0C-21CA39E5168A")]
public unsafe class DXGIFactory4 : DXGIFactory3
{
	// protected readonly delegate* unmanaged<void*, LUID, Guid*, void**, HRESULT> _EnumAdapterByLuid;
	// protected readonly delegate* unmanaged<void*, Guid*, void**, HRESULT> _EnumWarpAdapter;

	protected DXGIFactory4(void* raw, bool addRef = false) : base(raw, addRef)
	{
		// _EnumAdapterByLuid = (delegate* unmanaged<void*, LUID, Guid*, void**, HRESULT>)VTable[26];
		// _EnumWarpAdapter = (delegate* unmanaged<void*, Guid*, void**, HRESULT>)VTable[27];
	}
}

[Guid("7632E1F5-EE65-4DCA-87FD-84CD75F8838D")]
public unsafe class DXGIFactory5 : DXGIFactory4
{
	// protected readonly delegate* unmanaged<void*, DXGI_FEATURE, void*, uint, HRESULT> _CheckFeatureSupport;

	protected DXGIFactory5(void* raw, bool addRef = false) : base(raw, addRef)
	{
		// _CheckFeatureSupport = (delegate* unmanaged<void*, DXGI_FEATURE, void*, uint, HRESULT>)VTable[28];
	}
}

[Guid("C1B6694F-FF09-44A9-B03C-77900A0A1D17")]
public unsafe class DXGIFactory6 : DXGIFactory5
{
	// protected readonly delegate* unmanaged<void*, uint, DXGI_GPU_PREFERENCE, Guid*, void**, HRESULT> _EnumAdapterByGpuPreference;
	
	protected DXGIFactory6(void* raw, bool addRef = false) : base(raw, addRef)
	{
		// _EnumAdapterByGpuPreference = (delegate* unmanaged<void*, uint, DXGI_GPU_PREFERENCE, Guid*, void**, HRESULT>)VTable[29];
	}
}

[Guid("A4966EED-76DB-44DA-84C1-EE9A7AFB20A8")]
public sealed unsafe class DXGIFactory7 : DXGIFactory6
{
	// private readonly delegate* unmanaged<void*, HANDLE, uint*, HRESULT> _registerAdaptersChangedEvent;
	// private readonly delegate* unmanaged<void*, uint, HRESULT> _unregisterAdaptersChangedEvent;
	
	private DXGIFactory7(void* raw, bool addRef = false) : base(raw, addRef)
	{
		// _registerAdaptersChangedEvent = (delegate* unmanaged<void*, HANDLE, uint*, HRESULT>)VTable[30];
		// _unregisterAdaptersChangedEvent = (delegate* unmanaged<void*, uint, HRESULT>)VTable[31];
	}
}