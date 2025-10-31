export module Vyrse.Unmanaged.D3D11Device;

import <d3d11.h>;
import <d3d11_1.h>;
import <d3d11_2.h>;
import <d3d11_3.h>;
import <d3d11_4.h>;

import <d3d11shader.h>;
import <d3d11sdklayers.h>;
import <d3d11on12.h>;

import <dxgi.h>;
import <dxgi1_6.h>;

import <Unknwn.h>;
import <unknwnbase.h>;

import <GameInput.h>;

#define VYRSEAPI extern "C" __declspec(dllexport)

// ID3D11Device
VYRSEAPI HRESULT WINAPI ID3D11Device_CreateBuffer(ID3D11Device* device, const D3D11_BUFFER_DESC* pDesc, const D3D11_SUBRESOURCE_DATA* pInitialData, ID3D11Buffer** ppBuffer) noexcept { return device->CreateBuffer(pDesc, pInitialData, ppBuffer); }
VYRSEAPI HRESULT WINAPI ID3D11Device_CreateTexture1D(ID3D11Device* device, const D3D11_TEXTURE1D_DESC* pDesc, const D3D11_SUBRESOURCE_DATA* pInitialData, ID3D11Texture1D** ppTexture1D) noexcept { return device->CreateTexture1D(pDesc, pInitialData, ppTexture1D); }
VYRSEAPI HRESULT WINAPI ID3D11Device_CreateTexture2D(ID3D11Device* device, const D3D11_TEXTURE2D_DESC* pDesc, const D3D11_SUBRESOURCE_DATA* pInitialData, ID3D11Texture2D** ppTexture2D) noexcept { return device->CreateTexture2D(pDesc, pInitialData, ppTexture2D); }
VYRSEAPI HRESULT WINAPI ID3D11Device_CreateTexture3D(ID3D11Device* device, const D3D11_TEXTURE3D_DESC* pDesc, const D3D11_SUBRESOURCE_DATA* pInitialData, ID3D11Texture3D** ppTexture3D) noexcept { return device->CreateTexture3D(pDesc, pInitialData, ppTexture3D); }
VYRSEAPI HRESULT WINAPI ID3D11Device_CreateShaderResourceView(ID3D11Device* device, ID3D11Resource* pResource, const D3D11_SHADER_RESOURCE_VIEW_DESC* pDesc, ID3D11ShaderResourceView** ppSRView) noexcept { return device->CreateShaderResourceView(pResource, pDesc, ppSRView); }
VYRSEAPI HRESULT WINAPI ID3D11Device_CreateUnorderedAccessView(ID3D11Device* device, ID3D11Resource* pResource, const D3D11_UNORDERED_ACCESS_VIEW_DESC* pDesc, ID3D11UnorderedAccessView** ppUAView) noexcept { return device->CreateUnorderedAccessView(pResource, pDesc, ppUAView); }
VYRSEAPI HRESULT WINAPI ID3D11Device_CreateRenderTargetView(ID3D11Device* device, ID3D11Resource* pResource, const D3D11_RENDER_TARGET_VIEW_DESC* pDesc, ID3D11RenderTargetView** ppRTView) noexcept { return device->CreateRenderTargetView(pResource, pDesc, ppRTView); }
VYRSEAPI HRESULT WINAPI ID3D11Device_CreateDepthStencilView(ID3D11Device* device, ID3D11Resource* pResource, const D3D11_DEPTH_STENCIL_VIEW_DESC* pDesc, ID3D11DepthStencilView** ppDepthStencilView) noexcept { return device->CreateDepthStencilView(pResource, pDesc, ppDepthStencilView); }
VYRSEAPI HRESULT WINAPI ID3D11Device_CreateInputLayout(ID3D11Device* device, const D3D11_INPUT_ELEMENT_DESC* pInputElementDescs, UINT NumElements, const void* pShaderBytecodeWithInputSignature, SIZE_T BytecodeLength, ID3D11InputLayout** ppInputLayout) noexcept { return device->CreateInputLayout(pInputElementDescs, NumElements, pShaderBytecodeWithInputSignature, BytecodeLength, ppInputLayout); }
VYRSEAPI HRESULT WINAPI ID3D11Device_CreateVertexShader(ID3D11Device* device, const void* pShaderBytecode, SIZE_T BytecodeLength, ID3D11ClassLinkage* pClassLinkage, ID3D11VertexShader** ppVertexShader) noexcept { return device->CreateVertexShader(pShaderBytecode, BytecodeLength, pClassLinkage, ppVertexShader); }
VYRSEAPI HRESULT WINAPI ID3D11Device_CreateGeometryShader(ID3D11Device* device, const void* pShaderBytecode, SIZE_T BytecodeLength, ID3D11ClassLinkage* pClassLinkage, ID3D11GeometryShader** ppGeometryShader) noexcept { return device->CreateGeometryShader(pShaderBytecode, BytecodeLength, pClassLinkage, ppGeometryShader); }
VYRSEAPI HRESULT WINAPI ID3D11Device_CreateGeometryShaderWithStreamOutput(ID3D11Device* device, const void* pShaderBytecode, SIZE_T BytecodeLength, const D3D11_SO_DECLARATION_ENTRY* pSODeclaration, UINT NumEntries, const UINT* pBufferStrides, UINT NumStrides, UINT RasterizedStream, ID3D11ClassLinkage* pClassLinkage, ID3D11GeometryShader** ppGeometryShader) noexcept { return device->CreateGeometryShaderWithStreamOutput(pShaderBytecode, BytecodeLength, pSODeclaration, NumEntries, pBufferStrides, NumStrides, RasterizedStream, pClassLinkage, ppGeometryShader); }
VYRSEAPI HRESULT WINAPI ID3D11Device_CreatePixelShader(ID3D11Device* device, const void* pShaderBytecode, SIZE_T BytecodeLength, ID3D11ClassLinkage* pClassLinkage, ID3D11PixelShader** ppPixelShader) noexcept { return device->CreatePixelShader(pShaderBytecode, BytecodeLength, pClassLinkage, ppPixelShader); }
VYRSEAPI HRESULT WINAPI ID3D11Device_CreateComputeShader(ID3D11Device* device, const void* pShaderBytecode, SIZE_T BytecodeLength, ID3D11ClassLinkage* pClassLinkage, ID3D11ComputeShader** ppComputeShader) noexcept { return device->CreateComputeShader(pShaderBytecode, BytecodeLength, pClassLinkage, ppComputeShader); }
VYRSEAPI HRESULT WINAPI ID3D11Device_CreateClassLinkage(ID3D11Device* device, ID3D11ClassLinkage** ppLinkage) noexcept { return device->CreateClassLinkage(ppLinkage); }
VYRSEAPI HRESULT WINAPI ID3D11Device_CreateBlendState(ID3D11Device* device, const D3D11_BLEND_DESC* pBlendStateDesc, ID3D11BlendState** ppBlendState) noexcept { return device->CreateBlendState(pBlendStateDesc, ppBlendState); }
VYRSEAPI HRESULT WINAPI ID3D11Device_CreateDepthStencilState(ID3D11Device* device, const D3D11_DEPTH_STENCIL_DESC* pDepthStencilDesc, ID3D11DepthStencilState** ppDepthStencilState) noexcept { return device->CreateDepthStencilState(pDepthStencilDesc, ppDepthStencilState); }
VYRSEAPI HRESULT WINAPI ID3D11Device_CreateRasterizerState(ID3D11Device* device, const D3D11_RASTERIZER_DESC* pRasterizerDesc, ID3D11RasterizerState** ppRasterizerState) noexcept { return device->CreateRasterizerState(pRasterizerDesc, ppRasterizerState); }
VYRSEAPI HRESULT WINAPI ID3D11Device_CreateSamplerState(ID3D11Device* device, const D3D11_SAMPLER_DESC* pSamplerDesc, ID3D11SamplerState** ppSamplerState) noexcept { return device->CreateSamplerState(pSamplerDesc, ppSamplerState); }
VYRSEAPI HRESULT WINAPI ID3D11Device_CreateQuery(ID3D11Device* device, const D3D11_QUERY_DESC* pQueryDesc, ID3D11Query** ppQuery) noexcept { return device->CreateQuery(pQueryDesc, ppQuery); }
VYRSEAPI HRESULT WINAPI ID3D11Device_CreatePredicate(ID3D11Device* device, const D3D11_QUERY_DESC* pPredicateDesc, ID3D11Predicate** ppPredicate) noexcept { return device->CreatePredicate(pPredicateDesc, ppPredicate); }
VYRSEAPI HRESULT WINAPI ID3D11Device_CreateCounter(ID3D11Device* device, const D3D11_COUNTER_DESC* pCounterDesc, ID3D11Counter** ppCounter) noexcept { return device->CreateCounter(pCounterDesc, ppCounter); }
VYRSEAPI HRESULT WINAPI ID3D11Device_CreateDeferredContext(ID3D11Device* device, UINT ContextFlags, ID3D11DeviceContext** ppDeferredContext) noexcept { return device->CreateDeferredContext(ContextFlags, ppDeferredContext); }
VYRSEAPI HRESULT WINAPI ID3D11Device_OpenSharedResource(ID3D11Device* device, HANDLE hResource, REFIID ReturnedInterface, void** ppResource) noexcept { return device->OpenSharedResource(hResource, ReturnedInterface, ppResource); }
VYRSEAPI HRESULT WINAPI ID3D11Device_CheckFormatSupport(ID3D11Device* device, DXGI_FORMAT Format, UINT* pFormatSupport) noexcept { return device->CheckFormatSupport(Format, pFormatSupport); }
VYRSEAPI HRESULT WINAPI ID3D11Device_CheckMultisampleQualityLevels(ID3D11Device* device, DXGI_FORMAT Format, UINT SampleCount, UINT* pNumQualityLevels) noexcept { return device->CheckMultisampleQualityLevels(Format, SampleCount, pNumQualityLevels); }
VYRSEAPI void WINAPI ID3D11Device_CheckCounterInfo(ID3D11Device* device, D3D11_COUNTER_INFO* pCounterInfo) noexcept { device->CheckCounterInfo(pCounterInfo); }
VYRSEAPI HRESULT WINAPI ID3D11Device_CheckCounter(ID3D11Device* device, const D3D11_COUNTER_DESC* pDesc, D3D11_COUNTER_TYPE* pType, UINT* pActiveCounters, LPSTR szName, UINT* pNameLength, LPSTR szUnits, UINT* pUnitsLength, LPSTR szDescription, UINT* pDescriptionLength) noexcept { return device->CheckCounter(pDesc, pType, pActiveCounters, szName, pNameLength, szUnits, pUnitsLength, szDescription, pDescriptionLength); }
VYRSEAPI HRESULT WINAPI ID3D11Device_CheckFeatureSupport(ID3D11Device* device, D3D11_FEATURE Feature, void* pFeatureSupportData, UINT FeatureSupportDataSize) noexcept { return device->CheckFeatureSupport(Feature, pFeatureSupportData, FeatureSupportDataSize); }
VYRSEAPI HRESULT WINAPI ID3D11Device_GetPrivateData(ID3D11Device* device, const GUID& guid, UINT* pDataSize, void* pData) noexcept { return device->GetPrivateData(guid, pDataSize, pData); }
VYRSEAPI HRESULT WINAPI ID3D11Device_SetPrivateData(ID3D11Device* device, const GUID& guid, UINT DataSize, const void* pData) noexcept { return device->SetPrivateData(guid, DataSize, pData); }
VYRSEAPI HRESULT WINAPI ID3D11Device_SetPrivateDataInterface(ID3D11Device* device, const GUID& guid, const IUnknown* pData) noexcept { return device->SetPrivateDataInterface(guid, pData); }
VYRSEAPI D3D_FEATURE_LEVEL WINAPI ID3D11Device_GetFeatureLevel(ID3D11Device* device) noexcept { return device->GetFeatureLevel(); }
VYRSEAPI UINT WINAPI ID3D11Device_GetCreationFlags(ID3D11Device* device) noexcept { return device->GetCreationFlags(); }
VYRSEAPI HRESULT WINAPI ID3D11Device_GetDeviceRemovedReason(ID3D11Device* device) noexcept { return device->GetDeviceRemovedReason(); }
VYRSEAPI void WINAPI ID3D11Device_GetImmediateContext(ID3D11Device* device, ID3D11DeviceContext** ppImmediateContext) noexcept { device->GetImmediateContext(ppImmediateContext); }
VYRSEAPI HRESULT WINAPI ID3D11Device_SetExceptionMode(ID3D11Device* device, UINT RaiseFlags) noexcept { return device->SetExceptionMode(RaiseFlags); }
VYRSEAPI UINT WINAPI ID3D11Device_GetExceptionMode(ID3D11Device* device) noexcept { return device->GetExceptionMode(); }

// ID3D11Device1
VYRSEAPI void WINAPI ID3D11Device1_GetImmediateContext1(ID3D11Device1* device, ID3D11DeviceContext1** ppImmediateContext) noexcept { device->GetImmediateContext1(ppImmediateContext); }
VYRSEAPI HRESULT WINAPI ID3D11Device1_CreateDeferredContext1(ID3D11Device1* device, UINT ContextFlags, ID3D11DeviceContext1** ppDeferredContext) noexcept { return device->CreateDeferredContext1(ContextFlags, ppDeferredContext); }
VYRSEAPI HRESULT WINAPI ID3D11Device1_CreateBlendState1(ID3D11Device1* device, const D3D11_BLEND_DESC1* pBlendStateDesc, ID3D11BlendState1** ppBlendState) noexcept { return device->CreateBlendState1(pBlendStateDesc, ppBlendState); }
VYRSEAPI HRESULT WINAPI ID3D11Device1_CreateRasterizerState1(ID3D11Device1* device, const D3D11_RASTERIZER_DESC1* pRasterizerDesc, ID3D11RasterizerState1** ppRasterizerState) noexcept { return device->CreateRasterizerState1(pRasterizerDesc, ppRasterizerState); }
VYRSEAPI HRESULT WINAPI ID3D11Device1_CreateDeviceContextState(ID3D11Device1* device, UINT Flags, D3D_FEATURE_LEVEL FeatureLevels[], UINT FeatureLevelsCount, UINT SDKVersion, const IID& EmulatedInterface, D3D_FEATURE_LEVEL* pChosenFeatureLevel, ID3DDeviceContextState** ppContextState) noexcept { return device->CreateDeviceContextState(Flags, FeatureLevels, FeatureLevelsCount, SDKVersion, EmulatedInterface, pChosenFeatureLevel, ppContextState); }
VYRSEAPI HRESULT WINAPI ID3D11Device1_OpenSharedResource1(ID3D11Device1* device, HANDLE hResource, const IID& returnedInterface, void** ppResource) noexcept { return device->OpenSharedResource1(hResource, returnedInterface, ppResource); }
VYRSEAPI HRESULT WINAPI ID3D11Device1_OpenSharedResourceByName(ID3D11Device1* device, LPCWSTR lpName, DWORD dwDesiredAccess, const IID& returnedInterface, void** ppResource) noexcept { return device->OpenSharedResourceByName(lpName, dwDesiredAccess, returnedInterface, ppResource); }

// ID3D11Device2
VYRSEAPI void WINAPI ID3D11Device2_GetImmediateContext2(ID3D11Device2* device, ID3D11DeviceContext2** ppImmediateContext) noexcept { device->GetImmediateContext2(ppImmediateContext); }
VYRSEAPI HRESULT WINAPI ID3D11Device2_CreateDeferredContext2(ID3D11Device2* device, UINT ContextFlags, ID3D11DeviceContext2** ppDeferredContext) noexcept { return device->CreateDeferredContext2(ContextFlags, ppDeferredContext); }
VYRSEAPI void WINAPI ID3D11Device2_GetResourceTiling(ID3D11Device2* device, ID3D11Resource* pTiledResource, UINT* pNumTilesForEntireResource, D3D11_PACKED_MIP_DESC* pPackedMipDesc, D3D11_TILE_SHAPE* pStandardTileShapeForNonPackedMips, UINT* pNumSubresourceTilings, UINT FirstSubresourceTilingToGet, D3D11_SUBRESOURCE_TILING* pSubresourceTilingsForNonPackedMips) noexcept { device->GetResourceTiling(pTiledResource, pNumTilesForEntireResource, pPackedMipDesc, pStandardTileShapeForNonPackedMips, pNumSubresourceTilings, FirstSubresourceTilingToGet, pSubresourceTilingsForNonPackedMips); }
VYRSEAPI HRESULT WINAPI ID3D11Device2_CheckMultisampleQualityLevels1(ID3D11Device2* device, DXGI_FORMAT Format, UINT SampleCount, UINT Flags, UINT* pNumQualityLevels) noexcept { return device->CheckMultisampleQualityLevels1(Format, SampleCount, Flags, pNumQualityLevels); }

// ID3D11Device3
VYRSEAPI HRESULT WINAPI ID3D11Device3_CreateTexture2D1(ID3D11Device3* device, const D3D11_TEXTURE2D_DESC1* pDesc, const D3D11_SUBRESOURCE_DATA* pInitialData, ID3D11Texture2D1** ppTexture2D) noexcept { return device->CreateTexture2D1(pDesc, pInitialData, ppTexture2D); }
VYRSEAPI HRESULT WINAPI ID3D11Device3_CreateTexture3D1(ID3D11Device3* device, const D3D11_TEXTURE3D_DESC1* pDesc, const D3D11_SUBRESOURCE_DATA* pInitialData, ID3D11Texture3D1** ppTexture3D) noexcept { return device->CreateTexture3D1(pDesc, pInitialData, ppTexture3D); }
VYRSEAPI HRESULT WINAPI ID3D11Device3_CreateRasterizerState2(ID3D11Device3* device, const D3D11_RASTERIZER_DESC2* pRasterizerDesc, ID3D11RasterizerState2** ppRasterizerState) noexcept { return device->CreateRasterizerState2(pRasterizerDesc, ppRasterizerState); }
VYRSEAPI void WINAPI ID3D11Device3_CreateShaderResourceView1(ID3D11Device3* device, ID3D11Resource* pResource, const D3D11_SHADER_RESOURCE_VIEW_DESC1* pDesc, ID3D11ShaderResourceView1** ppSRView) noexcept { device->CreateShaderResourceView1(pResource, pDesc, ppSRView); }
VYRSEAPI void WINAPI ID3D11Device3_CreateUnorderedAccessView1(ID3D11Device3* device, ID3D11Resource* pResource, const D3D11_UNORDERED_ACCESS_VIEW_DESC1* pDesc, ID3D11UnorderedAccessView1** ppUAView) noexcept { device->CreateUnorderedAccessView1(pResource, pDesc, ppUAView); }
VYRSEAPI HRESULT WINAPI ID3D11Device3_CreateRenderTargetView1(ID3D11Device3* device, ID3D11Resource* pResource, const D3D11_RENDER_TARGET_VIEW_DESC1* pDesc, ID3D11RenderTargetView1** ppRTView) noexcept { return device->CreateRenderTargetView1(pResource, pDesc, ppRTView); }
VYRSEAPI HRESULT WINAPI ID3D11Device3_CreateQuery1(ID3D11Device3* device, const D3D11_QUERY_DESC1* pQueryDesc, ID3D11Query1** ppQuery) noexcept { return device->CreateQuery1(pQueryDesc, ppQuery); }
VYRSEAPI void WINAPI ID3D11Device3_GetImmediateContext3(ID3D11Device3* device, ID3D11DeviceContext3** ppImmediateContext) noexcept { device->GetImmediateContext3(ppImmediateContext); }
VYRSEAPI HRESULT WINAPI ID3D11Device3_CreateDeferredContext3(ID3D11Device3* device, UINT ContextFlags, ID3D11DeviceContext3** ppDeferredContext) noexcept { return device->CreateDeferredContext3(ContextFlags, ppDeferredContext); }
VYRSEAPI void WINAPI ID3D11Device3_WriteToSubresource(ID3D11Device3* device, ID3D11Resource* pDstResource, UINT DstSubresource, const D3D11_BOX* pDstBox, const void* pSrcData, UINT SrcRowPitch, UINT SrcDepthPitch) noexcept { device->WriteToSubresource(pDstResource, DstSubresource, pDstBox, pSrcData, SrcRowPitch, SrcDepthPitch); }
VYRSEAPI void WINAPI ID3D11Device3_ReadFromSubresource(ID3D11Device3* device, void* pDstData, UINT DstRowPitch, UINT DstDepthPitch, ID3D11Resource* pSrcResource, UINT SrcSubresource, const D3D11_BOX* pSrcBox) noexcept { device->ReadFromSubresource(pDstData, DstRowPitch, DstDepthPitch, pSrcResource, SrcSubresource, pSrcBox); }

// ID3D11Device4
VYRSEAPI HRESULT WINAPI ID3D11Device4_RegisterDeviceRemovedEvent(ID3D11Device4* device, HANDLE hEvent, DWORD* pdwCookie) noexcept { return device->RegisterDeviceRemovedEvent(hEvent, pdwCookie); }
VYRSEAPI void WINAPI ID3D11Device4_UnregisterDeviceRemoved(ID3D11Device4* device, DWORD dwCookie) noexcept { device->UnregisterDeviceRemoved(dwCookie); }

// ID3D11Device5
VYRSEAPI HRESULT WINAPI ID3D11Device5_OpenSharedFence(ID3D11Device5* device, HANDLE hFence, const IID& returnedInterface, void** ppFence) noexcept { return device->OpenSharedFence(hFence, returnedInterface, ppFence); }
VYRSEAPI HRESULT WINAPI ID3D11Device5_CreateFence(ID3D11Device5* device, UINT64 InitialValue, D3D11_FENCE_FLAG Flags, const IID& riid, void** ppFence) noexcept { return device->CreateFence(InitialValue, Flags, riid, ppFence); }