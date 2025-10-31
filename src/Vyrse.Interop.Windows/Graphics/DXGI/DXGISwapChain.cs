using System.Runtime.InteropServices;

namespace Vyrse.Interop.Windows.Graphics.DXGI;

[Guid("310D36A0-D2E7-4C0A-AA04-6A9D23B8886A")]
public unsafe class DXGISwapChain : DXGIObject
{
	// protected readonly delegate* unmanaged<void*, Guid*, void**, HRESULT> _GetDevice;
	// protected readonly delegate* unmanaged<void*, uint, DXGI_PRESENT, HRESULT> _Present;
	// protected readonly delegate* unmanaged<void*, uint, Guid*, void**, HRESULT> _GetBuffer;
	// protected readonly delegate* unmanaged<void*, BOOL, void*, HRESULT> _SetFullscreenState;
	// protected readonly delegate* unmanaged<void*, BOOL*, void**, HRESULT> _GetFullscreenState;
	// protected readonly delegate* unmanaged<void*, DXGI_SWAP_CHAIN_DESC*, HRESULT> _GetDesc;
	// protected readonly delegate* unmanaged<void*, uint, uint, uint, DXGI_FORMAT, uint, HRESULT> _ResizeBuffers;
	// protected readonly delegate* unmanaged<void*, DXGI_MODE_DESC*, HRESULT> _ResizeTarget;
	// protected readonly delegate* unmanaged<void*, void**, HRESULT> _GetContainingOutput;
	// protected readonly delegate* unmanaged<void*, DXGI_FRAME_STATISTICS*, HRESULT> _GetFrameStatistics;
	// protected readonly delegate* unmanaged<void*, uint*, HRESULT> _GetLastPresentCount;

	protected DXGISwapChain(void* raw, bool addRef = false) : base(raw, addRef)
	{
		// _GetDevice = (delegate* unmanaged<void*, Guid*, void**, HRESULT>)VTable[7];
		// _Present = (delegate* unmanaged<void*, uint, DXGI_PRESENT, HRESULT>)VTable[8];
		// _GetBuffer = (delegate* unmanaged<void*, uint, Guid*, void**, HRESULT>)VTable[9];
		// _SetFullscreenState = (delegate* unmanaged<void*, BOOL, void*, HRESULT>)VTable[10];
		// _GetFullscreenState = (delegate* unmanaged<void*, BOOL*, void**, HRESULT>)VTable[11];
		// _GetDesc = (delegate* unmanaged<void*, DXGI_SWAP_CHAIN_DESC*, HRESULT>)VTable[12];
		// _ResizeBuffers = (delegate* unmanaged<void*, uint, uint, uint, DXGI_FORMAT, uint, HRESULT>)VTable[13];
		// _ResizeTarget = (delegate* unmanaged<void*, DXGI_MODE_DESC*, HRESULT>)VTable[14];
		// _GetContainingOutput = (delegate* unmanaged<void*, void**, HRESULT>)VTable[15];
		// _GetFrameStatistics = (delegate* unmanaged<void*, DXGI_FRAME_STATISTICS*, HRESULT>)VTable[16];
		// _GetLastPresentCount = (delegate* unmanaged<void*, uint*, HRESULT>)VTable[17];
	}

	// public T GetBuffer<T>(uint buffer = 0u) where T : Unknown
	// {
	// 	void* raw = null;
	// 	var iid = typeof(T).GUID;
	// 	_GetBuffer(Raw, buffer, (Guid*)Unsafe.AsPointer(ref iid), &raw).ThrowOnFailure();
	// 	return Create<T>(raw);
	// }
	//
	// public DXGI_SWAP_CHAIN_DESC GetDesc()
	// {
	// 	DXGI_SWAP_CHAIN_DESC desc;
	// 	_GetDesc(Raw, &desc).ThrowOnFailure();
	// 	return desc;
	// }
}

[Guid("790A45F7-0D42-4876-983A-0A55CFE6F4AA")]
public unsafe class DXGISwapChain1 : DXGISwapChain
{
	// protected readonly delegate* unmanaged<void*, DXGI_SWAP_CHAIN_DESC1*, HRESULT> _GetDesc1;
	// protected readonly delegate* unmanaged<void*, DXGI_SWAP_CHAIN_FULLSCREEN_DESC*, HRESULT> _GetFullscreenDesc;
	// protected readonly delegate* unmanaged<void*, HWND*, HRESULT> _GetHwnd;
	// protected readonly delegate* unmanaged<void*, Guid*, void**, HRESULT> _GetCoreWindow;
	// protected readonly delegate* unmanaged<void*, uint, DXGI_PRESENT, DXGI_PRESENT_PARAMETERS*, HRESULT> _Present1;
	// protected readonly delegate* unmanaged<void*, BOOL> _IsTemporaryMonoSupported;
	// protected readonly delegate* unmanaged<void*, void**, HRESULT> _GetRestrictToOutput;
	// protected readonly delegate* unmanaged<void*, DXGI_RGBA*, HRESULT> _SetBackgroundColor;
	// protected readonly delegate* unmanaged<void*, DXGI_RGBA*, HRESULT> _GetBackgroundColor;
	// protected readonly delegate* unmanaged<void*, DXGI_MODE_ROTATION, HRESULT> _SetRotation;
	// protected readonly delegate* unmanaged<void*, DXGI_MODE_ROTATION*, HRESULT> _GetRotation;

	protected DXGISwapChain1(void* raw, bool addRef = false) : base(raw, addRef)
	{
		// _GetDesc1 = (delegate* unmanaged<void*, DXGI_SWAP_CHAIN_DESC1*, HRESULT>)VTable[18];
		// _GetFullscreenDesc = (delegate* unmanaged<void*, DXGI_SWAP_CHAIN_FULLSCREEN_DESC*, HRESULT>)VTable[19];
		// _GetHwnd = (delegate* unmanaged<void*, HWND*, HRESULT>)VTable[20];
		// _GetCoreWindow = (delegate* unmanaged<void*, Guid*, void**, HRESULT>)VTable[21];
		// _Present1 = (delegate* unmanaged<void*, uint, DXGI_PRESENT, DXGI_PRESENT_PARAMETERS*, HRESULT>)VTable[22];
		// _IsTemporaryMonoSupported = (delegate* unmanaged<void*, BOOL>)VTable[23];
		// _GetRestrictToOutput = (delegate* unmanaged<void*, void**, HRESULT>)VTable[24];
		// _SetBackgroundColor = (delegate* unmanaged<void*, DXGI_RGBA*, HRESULT>)VTable[25];
		// _GetBackgroundColor = (delegate* unmanaged<void*, DXGI_RGBA*, HRESULT>)VTable[26];
		// _SetRotation = (delegate* unmanaged<void*, DXGI_MODE_ROTATION, HRESULT>)VTable[27];
		// _GetRotation = (delegate* unmanaged<void*, DXGI_MODE_ROTATION*, HRESULT>)VTable[28];
	}

	// public DXGI_SWAP_CHAIN_DESC1 GetDesc1()
	// {
	// 	DXGI_SWAP_CHAIN_DESC1 desc;
	// 	_GetDesc1(Raw, &desc).ThrowOnFailure();
	// 	return desc;
	// }
}

[Guid("A8BE2AC4-199F-4946-B331-79599FB98DE7")]
public unsafe class DXGISwapChain2 : DXGISwapChain1
{
	// protected readonly delegate* unmanaged<void*, uint, uint, HRESULT> _SetSourceSize;
	// protected readonly delegate* unmanaged<void*, uint*, uint*, HRESULT> _GetSourceSize;
	// protected readonly delegate* unmanaged<void*, uint, HRESULT> _SetMaximumFrameLatency;
	// protected readonly delegate* unmanaged<void*, uint*, HRESULT> _GetMaximumFrameLatency;
	// protected readonly delegate* unmanaged<void*, HANDLE> _GetFrameLatencyWaitableObject;
	// protected readonly delegate* unmanaged<void*, DXGI_MATRIX_3X2_F*, HRESULT> _SetMatrixTransform;
	// protected readonly delegate* unmanaged<void*, DXGI_MATRIX_3X2_F*, HRESULT> _GetMatrixTransform;

	protected DXGISwapChain2(void* raw, bool addRef = false) : base(raw, addRef)
	{
		// _SetSourceSize = (delegate* unmanaged<void*, uint, uint, HRESULT>)VTable[29];
		// _GetSourceSize = (delegate* unmanaged<void*, uint*, uint*, HRESULT>)VTable[30];
		// _SetMaximumFrameLatency = (delegate* unmanaged<void*, uint, HRESULT>)VTable[31];
		// _GetMaximumFrameLatency = (delegate* unmanaged<void*, uint*, HRESULT>)VTable[32];
		// _GetFrameLatencyWaitableObject = (delegate* unmanaged<void*, HANDLE>)VTable[33];
		// _SetMatrixTransform = (delegate* unmanaged<void*, DXGI_MATRIX_3X2_F*, HRESULT>)VTable[34];
		// _GetMatrixTransform = (delegate* unmanaged<void*, DXGI_MATRIX_3X2_F*, HRESULT>)VTable[35];
	}
}

[Guid("94D99BDB-F1F8-4AB0-B236-7DA0170EDAB1")]
public unsafe class DXGISwapChain3 : DXGISwapChain2
{
	// protected readonly delegate* unmanaged<void*, uint> _GetCurrentBackBufferIndex;
	// protected readonly delegate* unmanaged<void*, DXGI_COLOR_SPACE_TYPE, uint*, HRESULT> _CheckColorSpaceSupport;
	// protected readonly delegate* unmanaged<void*, DXGI_COLOR_SPACE_TYPE, HRESULT> _SetColorSpace1;
	// protected readonly delegate* unmanaged<void*, uint, uint, uint, DXGI_FORMAT, uint, uint*, void**, HRESULT> _ResizeBuffers1;

	protected DXGISwapChain3(void* raw, bool addRef = false) : base(raw, addRef)
	{
		// _GetCurrentBackBufferIndex = (delegate* unmanaged<void*, uint>)VTable[36];
		// _CheckColorSpaceSupport = (delegate* unmanaged<void*, DXGI_COLOR_SPACE_TYPE, uint*, HRESULT>)VTable[37];
		// _SetColorSpace1 = (delegate* unmanaged<void*, DXGI_COLOR_SPACE_TYPE, HRESULT>)VTable[38];
		// _ResizeBuffers1 = (delegate* unmanaged<void*, uint, uint, uint, DXGI_FORMAT, uint, uint*, void**, HRESULT>)VTable[39];
	}
}

[Guid("3D585D5A-BD4A-489E-B1F4-3DBCB6452FFB")]
public sealed unsafe class DXGISwapChain4 : DXGISwapChain3
{
	// private readonly delegate* unmanaged<void*, DXGI_HDR_METADATA_TYPE, uint, void*, HRESULT> _setHdrMetaData;

	private DXGISwapChain4(void* raw, bool addRef = false) : base(raw, addRef)
	{
		// _setHdrMetaData = (delegate* unmanaged<void*, DXGI_HDR_METADATA_TYPE, uint, void*, HRESULT>)VTable[40];
	}
}