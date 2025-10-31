using System.Runtime.InteropServices;
using Vyrse.Interop.Windows.Com;

namespace Vyrse.Interop.Windows.Graphics.D3D11;

[Guid("DB6F6DDB-AC77-4E88-8253-819DF9BBF140")]
public unsafe partial class D3D11Device : Unknown
{
	private static partial class Unmanaged
	{
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device_CreateBuffer")]
		// internal static partial HRESULT CreateBuffer(nint device, in D3D11_BUFFER_DESC desc, [Optional] D3D11_SUBRESOURCE_DATA* pInitialData, out nint buffer);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device_CreateTexture1D")]
		// internal static partial HRESULT CreateTexture1D(nint device, in D3D11_TEXTURE1D_DESC desc, [Optional] D3D11_SUBRESOURCE_DATA* pInitialData, out nint texture1D);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device_CreateTexture2D")]
		// internal static partial HRESULT CreateTexture2D(nint device, D3D11_TEXTURE2D_DESC* desc, [Optional] D3D11_SUBRESOURCE_DATA* pInitialData, out nint texture2D);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device_CreateTexture3D")]
		// internal static partial HRESULT CreateTexture3D(nint device, D3D11_TEXTURE3D_DESC* desc, [Optional] D3D11_SUBRESOURCE_DATA* pInitialData, out nint texture3D);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device_CreateShaderResourceView")]
		// internal static partial HRESULT CreateShaderResourceView(nint device, nint resource, [Optional] D3D11_SHADER_RESOURCE_VIEW_DESC* pDesc, out nint shaderResourceView);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device_CreateUnorderedAccessView")]
		// internal static partial HRESULT CreateUnorderedAccessView(nint device, nint resource, [Optional] D3D11_UNORDERED_ACCESS_VIEW_DESC* pDesc, out nint unorderedAccessView);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device_CreateRenderTargetView")]
		// internal static partial HRESULT CreateRenderTargetView(nint device, nint resource, [Optional] D3D11_RENDER_TARGET_VIEW_DESC* pDesc, out nint renderTargetView);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device_CreateDepthStencilView")]
		// internal static partial HRESULT CreateDepthStencilView(nint device, nint resource, [Optional] D3D11_DEPTH_STENCIL_VIEW_DESC* pDesc, out nint depthStencilView);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device_CreateInputLayout")]
		// internal static partial HRESULT CreateInputLayout(nint device, in D3D11_INPUT_ELEMENT_DESC inputElementDescs, uint numElements, void* pShaderBytecodeWithInputSignature, nuint bytecodeLength, out nint inputLayout);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device_CreateVertexShader")]
		// internal static partial HRESULT CreateVertexShader(nint device, nint pShaderBytecode, nuint bytecodeLength, [Optional] void* pClassLinkage, out nint vertexShader);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device_CreateGeometryShader")]
		// internal static partial HRESULT CreateGeometryShader(nint device, nint pShaderBytecode, nuint bytecodeLength, [Optional] void* pClassLinkage, out nint geometryShader);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device_CreateGeometryShaderWithStreamOutput")]
		// internal static partial HRESULT CreateGeometryShaderWithStreamOutput(nint device, nint pShaderBytecode, nuint bytecodeLength, [Optional] D3D11_SO_DECLARATION_ENTRY* pSODeclaration, uint numEntries, [Optional] uint* pBufferStrides, uint numStrides, uint rasterizedStream, [Optional] void* pClassLinkage, out nint geometryShader);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device_CreatePixelShader")]
		// internal static partial HRESULT CreatePixelShader(nint device, nint pShaderBytecode, nuint bytecodeLength, [Optional] void* pClassLinkage, out nint pixelShader);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device_CreateComputeShader")]
		// internal static partial HRESULT CreateComputeShader(nint device, nint pShaderBytecode, nuint bytecodeLength, [Optional] void* pClassLinkage, out nint computeShader);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device_CreateClassLinkage")]
		// internal static partial HRESULT CreateClassLinkage(nint device, out nint classLinkage);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device_CreateBlendState")]
		// internal static partial HRESULT CreateBlendState(nint device, in D3D11_BLEND_DESC desc, out nint blendState);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device_CreateDepthStencilState")]
		// internal static partial HRESULT CreateDepthStencilState(nint device, in D3D11_DEPTH_STENCIL_DESC desc, out nint depthStencilState);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device_CreateRasterizerState")]
		// internal static partial HRESULT CreateRasterizerState(nint device, in D3D11_RASTERIZER_DESC desc, out nint rasterizerState);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device_CreateSamplerState")]
		// internal static partial HRESULT CreateSamplerState(nint device, in D3D11_SAMPLER_DESC desc, out nint samplerState);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device_CreateQuery")]
		// internal static partial HRESULT CreateQuery(nint device, in D3D11_QUERY_DESC desc, out nint query);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device_CreatePredicate")]
		// internal static partial HRESULT CreatePredicate(nint device, in D3D11_QUERY_DESC desc, out nint predicate);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device_CreateCounter")]
		// internal static partial HRESULT CreateCounter(nint device, in D3D11_COUNTER_DESC desc, out nint counter);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device_CreateDeferredContext")]
		// internal static partial HRESULT CreateDeferredContext(nint device, uint contextFlags, out nint deferredContext);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device_OpenSharedResource")]
		// internal static partial HRESULT OpenSharedResource(nint device, HANDLE hResource, Guid* returnedInterface, out nint resource);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device_CheckFormatSupport")]
		// internal static partial HRESULT CheckFormatSupport(nint device, DXGI_FORMAT format, out uint pFormatSupport);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device_CheckMultisampleQualityLevels")]
		// internal static  partial HRESULT CheckMultisampleQualityLevels(nint device, DXGI_FORMAT format, uint sampleCount, out uint pNumQualityLevels);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device_CheckCounterInfo")]
		// internal static partial void CheckCounterInfo(nint device, D3D11_COUNTER_INFO* pCounterInfo);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device_CheckCounter")]
		// internal static partial HRESULT CheckCounter(nint device, in D3D11_COUNTER_DESC desc, out D3D11_COUNTER_TYPE type, out uint pActiveCounters, sbyte* szName, ref uint pNameLength, sbyte* szUnits, ref uint pUnitsLength, sbyte* szDescription, ref uint pDescriptionLength);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device_CheckFeatureSupport")]
		// internal static partial HRESULT CheckFeatureSupport(nint device, D3D11_FEATURE feature, void* pFeatureSupportData, uint featureSupportDataSize);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device_GetPrivateData")]
		// internal static partial HRESULT GetPrivateData(nint device, in Guid guid, ref uint pDataSize, void* pData);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device_SetPrivateData")]
		// internal static partial HRESULT SetPrivateData(nint device, in Guid guid, uint dataSize, void* pData);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device_GetFeatureLevel")]
		// internal static partial D3D_FEATURE_LEVEL GetFeatureLevel(nint device);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device_GetCreationFlags")]
		// internal static partial uint GetCreationFlags(nint device);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device_GetDeviceRemovedReason")]
		// internal static partial HRESULT GetDeviceRemovedReason(nint device);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device_GetImmediateContext")]
		// internal static partial void GetImmediateContext(nint device, out nint context);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device_SetExceptionMode")]
		// internal static partial HRESULT SetExceptionMode(nint device, uint raiseFlags);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device_GetExceptionMode")]
		// internal static partial uint GetExceptionMode(nint device);
	}

	protected D3D11Device(void* raw, bool addRef = false) : base(raw, addRef) { }

	// [MethodImpl(MethodImplOptions.AggressiveInlining)]
	// public D3D11Buffer CreateBuffer(in D3D11_BUFFER_DESC desc, in D3D11_SUBRESOURCE_DATA? initialData = null)
	// {
	// 	ThrowIfNull();
	// 	var i = initialData.GetValueOrDefault();
	// 	D3D11_SUBRESOURCE_DATA* pInit = initialData.HasValue ? &i : null;
	// 	Unmanaged.CreateBuffer(NativePtr, desc, pInit, out var buffer).ThrowOnFailure();
	// 	return Create<D3D11Buffer>(buffer);
	// }
	//
	// public D3D11DepthStencilView CreateDepthStencilView(D3D11Resource resource, in D3D11_DEPTH_STENCIL_VIEW_DESC? desc = null)
	// {
	// 	ThrowIfNull();
	// 	if (resource.IsNull) throw new ArgumentNullException(nameof(resource));
	// 	var d = desc.GetValueOrDefault();
	// 	D3D11_DEPTH_STENCIL_VIEW_DESC* pDesc = desc.HasValue ? &d : null;
	// 	Unmanaged.CreateDepthStencilView(NativePtr, resource.NativePtr, pDesc, out var depthStencilView).ThrowOnFailure();
	// 	return Create<D3D11DepthStencilView>(depthStencilView);
	// }
}

[Guid("A04BFB29-08EF-43D6-A49C-A9BDBDCBE686")]
public unsafe partial class D3D11Device1 : D3D11Device
{
	private static partial class Unmanaged
	{
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device1_GetImmediateContext1")]
		// internal static partial void GetImmediateContext1(nint device, out nint context);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device1_CreateDeferredContext1")]
		// internal static partial HRESULT CreateDeferredContext1(nint device, uint contextFlags, out nint deferredContext);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device1_CreateBlendState1")]
		// internal static partial HRESULT CreateBlendState1(nint device, in D3D11_BLEND_DESC1 desc, out nint blendState);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device1_CreateRasterizerState1")]
		// internal static partial HRESULT CreateRasterizerState1(nint device, in D3D11_RASTERIZER_DESC1 desc, out nint rasterizerState);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device1_CreateDeviceContextState")]
		// internal static partial HRESULT CreateDeviceContextState(nint device, uint flags, [Optional] D3D_FEATURE_LEVEL* pFeatureLevels, uint featureLevels, uint sdkVersion, [Optional] Guid* emulatedInterface, [Optional] D3D_FEATURE_LEVEL* pChosenFeatureLevel, out nint deviceContextState);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device1_OpenSharedResource1")]
		// internal static partial HRESULT OpenSharedResource1(nint device, HANDLE hResource, in Guid returnedInterface, out nint resource);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device1_OpenSharedResourceByName")]
		// internal static partial HRESULT OpenSharedResourceByName(nint device, PCWSTR lpName, uint dwDesiredAccess, in Guid returnedInterface, out nint resource);
	}

	protected D3D11Device1(void* raw, bool addRef = false) : base(raw, addRef) { }
}

[Guid("9D06DFFA-D1E5-4D07-83A8-1BB123F2F841")]
public unsafe partial class D3D11Device2 : D3D11Device1
{
	private static partial class Unmanaged
	{
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device2_GetImmediateContext2")]
		// internal static partial void GetImmediateContext2(nint device, out nint context);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device2_CreateDeferredContext2")]
		// internal static partial HRESULT CreateDeferredContext2(nint device, uint contextFlags, out nint deferredContext);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device2_GetResourceTiling")]
		// internal static partial void GetResourceTiling(nint device, nint pTiledResource, [Optional] uint* pNumTilesForEntireResource, [Optional] D3D11_PACKED_MIP_DESC* pPackedMipDesc, [Optional] D3D11_TILE_SHAPE* pStandardTileShapeForNonPackedMips, [Optional] uint* pNumSubresourceTilings, uint firstSubresourceTilingToGet, [Optional] D3D11_SUBRESOURCE_TILING* pSubresourceTilingsForNonPackedMips);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device2_CheckMultisampleQualityLevels1")]
		// internal static partial HRESULT CheckMultisampleQualityLevels1(nint device, DXGI_FORMAT format, uint sampleCount, uint flags, out uint pNumQualityLevels);
	}

	protected D3D11Device2(void* raw, bool addRef = false) : base(raw, addRef) { }
}

[Guid("A05C8C37-D2C6-4732-B3A0-9CE0B0DC9AE6")]
public unsafe partial class D3D11Device3 : D3D11Device2
{
	private static partial class Unmanaged
	{
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device3_CreateTexture2D1")]
		// internal static partial HRESULT CreateTexture2D1(nint device, in D3D11_TEXTURE2D_DESC1 desc, [Optional] D3D11_SUBRESOURCE_DATA* pInitialData, out nint texture2D);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device3_CreateTexture3D1")]
		// internal static partial HRESULT CreateTexture3D1(nint device, in D3D11_TEXTURE3D_DESC1 desc, [Optional] D3D11_SUBRESOURCE_DATA* pInitialData, out nint texture3D);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device3_CreateRasterizerState2")]
		// internal static partial HRESULT CreateRasterizerState2(nint device, in D3D11_RASTERIZER_DESC2 desc, out nint rasterizerState);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device3_CreateShaderResourceView1")]
		// internal static partial HRESULT CreateShaderResourceView1(nint device, nint resource, [Optional] D3D11_SHADER_RESOURCE_VIEW_DESC1* pDesc, out nint shaderResourceView);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device3_CreateUnorderedAccessView1")]
		// internal static partial HRESULT CreateUnorderedAccessView1(nint device, nint resource, [Optional] D3D11_UNORDERED_ACCESS_VIEW_DESC1* pDesc, out nint unorderedAccessView);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device3_CreateRenderTargetView1")]
		// internal static partial HRESULT CreateRenderTargetView1(nint device, nint resource, [Optional] D3D11_RENDER_TARGET_VIEW_DESC1* pDesc, out nint renderTargetView);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device3_CreateQuery1")]
		// internal static partial HRESULT CreateQuery1(nint device, in D3D11_QUERY_DESC1 desc, out nint query);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device3_GetImmediateContext3")]
		// internal static partial void GetImmediateContext3(nint device, out nint context);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device3_CreateDeferredContext3")]
		// internal static partial HRESULT CreateDeferredContext3(nint device, uint contextFlags, out nint deferredContext);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device3_WriteToSubresource")]
		// internal static partial void WriteToSubresource(nint device, nint pDstResource, uint dstSubresource, [Optional]  D3D11_BOX* pDstBox, void* pSrcData, uint srcRowPitch, uint srcDepthPitch);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device3_ReadFromSubresource")]
		// internal static partial void ReadFromSubresource(nint device, nint pDstData, uint dstRowPitch, nint pSrcResource, uint srcSubresource, uint srcRowPitch, [Optional] D3D11_BOX* pSrcBox);
	}

	protected D3D11Device3(void* raw, bool addRef = false) : base(raw, addRef) { }

	// public D3D11Texture2D1 CreateTexture2D1(in D3D11_TEXTURE2D_DESC1 desc1, in D3D11_SUBRESOURCE_DATA? initialData = null)
	// {
	// 	ThrowIfNull();
	// 	var i = initialData.GetValueOrDefault();
	// 	D3D11_SUBRESOURCE_DATA* pInit = initialData.HasValue ? &i : null;
	// 	Unmanaged.CreateTexture2D1(NativePtr, desc1, pInit, out var raw).ThrowOnFailure();
	// 	return Create<D3D11Texture2D1>(raw);
	// }
	//
	// public D3D11RasterizerState2 CreateRasterizerState2(in D3D11_RASTERIZER_DESC2 desc2)
	// {
	// 	ThrowIfNull();
	// 	Unmanaged.CreateRasterizerState2(NativePtr, desc2, out var raw).ThrowOnFailure();
	// 	return Create<D3D11RasterizerState2>(raw);
	// }
	//
	// public D3D11RenderTargetView1 CreateRenderTargetView1(D3D11Resource resource, in D3D11_RENDER_TARGET_VIEW_DESC1? desc = null)
	// {
	// 	ThrowIfNull();
	// 	if (resource.IsNull) throw new ArgumentNullException(nameof(resource));
	// 	var d = desc.GetValueOrDefault();
	// 	D3D11_RENDER_TARGET_VIEW_DESC1* pDesc = desc.HasValue ? &d : null;
	// 	Unmanaged.CreateRenderTargetView1(NativePtr, resource, pDesc, out var raw).ThrowOnFailure();
	// 	return Create<D3D11RenderTargetView1>(raw);
	// }
}

[Guid("8992AB71-02E6-4B8D-BA48-B056DCDA42C4")]
public unsafe partial class D3D11Device4 : D3D11Device3
{
	private static partial class Unmanaged
	{
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device4_RegisterDeviceRemovedEvent")]
		// internal static partial HRESULT RegisterDeviceRemovedEvent(nint device, HANDLE hEvent, out uint pdwCookie);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device4_UnregisterDeviceRemoved")]
		// internal static partial void UnregisterDeviceRemoved(nint device, uint dwCookie);
	}

	protected D3D11Device4(void* raw, bool addRef = false) : base(raw, addRef) { }
}

[Guid("8FFDE202-A0E7-45DF-9E01-E837801B5EA0")]
public sealed unsafe partial class D3D11Device5 : D3D11Device4
{
	private static partial class Unmanaged
	{
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device5_OpenSharedFence")]
		// internal static partial HRESULT OpenSharedFence(nint device, HANDLE hFence, out Guid returnedInterface, out nint fence);
		//
		// [LibraryImport(Dll, EntryPoint = "ID3D11Device5_CreateFence")]
		// internal static partial HRESULT CreateFence(nint device, ulong initialValue, D3D11_FENCE_FLAG flags, out Guid returnedInterface, out nint fence);
	}

	public D3D11Device5(void* raw, bool addRef = false) : base(raw, addRef) { }
}