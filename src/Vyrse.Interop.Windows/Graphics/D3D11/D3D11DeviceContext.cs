using System.Runtime.InteropServices;

namespace Vyrse.Interop.Windows.Graphics.D3D11;

[Guid("C0BFA96C-E089-44FB-8EAF-26F8796190DA")]
public unsafe class D3D11DeviceContext : D3D11DeviceChild
{
	/*protected readonly delegate* unmanaged<void*, uint, uint, void**, void> _VSSetConstantBuffers;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, void> _PSSetShaderResources;
	protected readonly delegate* unmanaged<void*, void*, void**, uint, void> _PSSetShader;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, void> _PSSetSamplers;
	protected readonly delegate* unmanaged<void*, void*, void**, uint, void> _VSSetShader;
	protected readonly delegate* unmanaged<void*, uint, uint, int, void> _DrawIndexed;
	protected readonly delegate* unmanaged<void*, uint, uint, void> _Draw;
	protected readonly delegate* unmanaged<void*, void*, uint, D3D11_MAP, uint, D3D11_MAPPED_SUBRESOURCE*, HRESULT> _Map;
	protected readonly delegate* unmanaged<void*, void*, uint, void> _Unmap;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, void> _PSSetConstantBuffers;
	protected readonly delegate* unmanaged<void*, void*, void> _IASetInputLayout;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, uint*, uint*, void> _IASetVertexBuffers;
	protected readonly delegate* unmanaged<void*, void*, DXGI_FORMAT, uint, void> _IASetIndexBuffer;
	protected readonly delegate* unmanaged<void*, uint, uint, uint, int, uint, void> _DrawIndexedInstanced;
	protected readonly delegate* unmanaged<void*, uint, uint, uint, uint, void> _DrawInstanced;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, void> _GSSetConstantBuffers;
	protected readonly delegate* unmanaged<void*, void*, void**, uint, void> _GSSetShader;
	protected readonly delegate* unmanaged<void*, D3D_PRIMITIVE_TOPOLOGY, void> _IASetPrimitiveTopology;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, void> _VSSetShaderResources;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, void> _VSSetSamplers;
	protected readonly delegate* unmanaged<void*, void*, void> _Begin;
	protected readonly delegate* unmanaged<void*, void*, void> _End;
	protected readonly delegate* unmanaged<void*, void*, void*, uint, uint, HRESULT> _GetData;
	protected readonly delegate* unmanaged<void*, void*, BOOL, void> _SetPredication;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, void> _GSSetShaderResources;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, void> _GSSetSamplers;
	protected readonly delegate* unmanaged<void*, uint, void**, void*, void> _OMSetRenderTargets;
	protected readonly delegate* unmanaged<void*, uint, void**, void*, uint, uint, void**, uint*, void> _OMSetRenderTargetsAndUnorderedAccessViews;
	protected readonly delegate* unmanaged<void*, void*, float*, uint, void> _OMSetBlendState;
	protected readonly delegate* unmanaged<void*, void*, uint, void> _OMSetDepthStencilState;
	protected readonly delegate* unmanaged<void*, uint, void**, uint*, void> _SOSetTargets;
	protected readonly delegate* unmanaged<void*, void> _DrawAuto;
	protected readonly delegate* unmanaged<void*, void*, uint, void> _DrawIndexedInstancedIndirect;
	protected readonly delegate* unmanaged<void*, void*, uint, void> _DrawInstancedIndirect;
	protected readonly delegate* unmanaged<void*, uint, uint, uint, void> _Dispatch;
	protected readonly delegate* unmanaged<void*, void*, uint, void> _DispatchIndirect;
	protected readonly delegate* unmanaged<void*, void*, void> _RSSetState;
	protected readonly delegate* unmanaged<void*, uint, D3D11_VIEWPORT*, void> _RSSetViewports;
	protected readonly delegate* unmanaged<void*, uint, RECT*, void> _RSSetScissorRects;
	protected readonly delegate* unmanaged<void*, void*, uint, uint, uint, uint, void*, uint, D3D11_BOX*, void> _CopySubresourceRegion;
	protected readonly delegate* unmanaged<void*, void*, void*, void> _CopyResource;
	protected readonly delegate* unmanaged<void*, void*, uint, D3D11_BOX*, void*, uint, uint, void> _UpdateSubresource;
	protected readonly delegate* unmanaged<void*, void*, uint, void*, void> _CopyStructureCount;
	protected readonly delegate* unmanaged<void*, void*, float*, void> _ClearRenderTargetView;
	protected readonly delegate* unmanaged<void*, void*, uint*, void> _ClearUnorderedAccessViewUint;
	protected readonly delegate* unmanaged<void*, void*, float*, void> _ClearUnorderedAccessViewFloat;
	protected readonly delegate* unmanaged<void*, void*, uint, float, byte, void> _ClearDepthStencilView;
	protected readonly delegate* unmanaged<void*, void*, void> _GenerateMips;
	protected readonly delegate* unmanaged<void*, void*, float, void> _SetResourceMinLOD;
	protected readonly delegate* unmanaged<void*, void*, float> _GetResourceMinLOD;
	protected readonly delegate* unmanaged<void*, void*, uint, void*, uint, DXGI_FORMAT, void> _ResolveSubresource;
	protected readonly delegate* unmanaged<void*, void*, BOOL, void> _ExecuteCommandList;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, void> _HSSetShaderResources;
	protected readonly delegate* unmanaged<void*, void*, void**, uint, void> _HSSetShader;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, void> _HSSetSamplers;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, void> _HSSetConstantBuffers;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, void> _DSSetShaderResources;
	protected readonly delegate* unmanaged<void*, void*, void**, uint, void> _DSSetShader;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, void> _DSSetSamplers;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, void> _DSSetConstantBuffers;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, void> _CSSetShaderResources;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, uint*, void> _CSSetUnorderedAccessViews;
	protected readonly delegate* unmanaged<void*, void*, void**, uint, void> _CSSetShader;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, void> _CSSetSamplers;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, void> _CSSetConstantBuffers;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, void> _VSGetConstantBuffers;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, void> _PSGetShaderResources;
	protected readonly delegate* unmanaged<void*, void**, void**, uint*, void> _PSGetShader;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, void> _PSGetSamplers;
	protected readonly delegate* unmanaged<void*, void**, void**, uint*, void> _VSGetShader;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, void> _PSGetConstantBuffers;
	protected readonly delegate* unmanaged<void*, void**, void> _IAGetInputLayout;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, uint*, uint*, void> _IAGetVertexBuffers;
	protected readonly delegate* unmanaged<void*, void**, DXGI_FORMAT*, uint*, void> _IAGetIndexBuffer;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, void> _GSGetConstantBuffers;
	protected readonly delegate* unmanaged<void*, void**, void**, uint*, void> _GSGetShader;
	protected readonly delegate* unmanaged<void*, D3D_PRIMITIVE_TOPOLOGY*, void> _IAGetPrimitiveTopology;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, void> _VSGetShaderResources;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, void> _VSGetSamplers;
	protected readonly delegate* unmanaged<void*, void**, BOOL*, void> _GetPredication;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, void> _GSGetShaderResources;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, void> _GSGetSamplers;
	protected readonly delegate* unmanaged<void*, uint, void**, void**, void> _OMGetRenderTargets;
	protected readonly delegate* unmanaged<void*, uint, void**, void**, uint, uint, void**, void> _OMGetRenderTargetsAndUnorderedAccessViews;
	protected readonly delegate* unmanaged<void*, void**, float*, uint*, void> _OMGetBlendState;
	protected readonly delegate* unmanaged<void*, void**, uint*, void> _OMGetDepthStencilState;
	protected readonly delegate* unmanaged<void*, uint, void**, void> _SOGetTargets;
	protected readonly delegate* unmanaged<void*, void**, void> _RSGetState;
	protected readonly delegate* unmanaged<void*, uint*, D3D11_VIEWPORT*, void> _RSGetViewports;
	protected readonly delegate* unmanaged<void*, uint*, RECT*, void> _RSGetScissorRects;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, void> _HSGetShaderResources;
	protected readonly delegate* unmanaged<void*, void**, void**, uint*, void> _HSGetShader;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, void> _HSGetSamplers;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, void> _HSGetConstantBuffers;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, void> _DSGetShaderResources;
	protected readonly delegate* unmanaged<void*, void**, void**, uint*, void> _DSGetShader;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, void> _DSGetSamplers;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, void> _DSGetConstantBuffers;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, void> _CSGetShaderResources;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, void> _CSGetUnorderedAccessViews;
	protected readonly delegate* unmanaged<void*, void**, void**, uint*, void> _CSGetShader;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, void> _CSGetSamplers;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, void> _CSGetConstantBuffers;
	protected readonly delegate* unmanaged<void*, void> _ClearState;
	protected readonly delegate* unmanaged<void*, void> _Flush;
	protected readonly delegate* unmanaged<void*, D3D11_DEVICE_CONTEXT_TYPE> _GetType;
	protected readonly delegate* unmanaged<void*, uint> _GetContextFlags;
	protected readonly delegate* unmanaged<void*, BOOL, void**, HRESULT> _FinishCommandList;*/

	protected D3D11DeviceContext(void* raw, bool addRef = false) : base(raw, addRef)
	{
		/*_VSSetConstantBuffers = (delegate* unmanaged<void*, uint, uint, void**, void>)VTable[7];
		_PSSetShaderResources = (delegate* unmanaged<void*, uint, uint, void**, void>)VTable[8];
		_PSSetShader = (delegate* unmanaged<void*, void*, void**, uint, void>)VTable[9];
		_PSSetSamplers = (delegate* unmanaged<void*, uint, uint, void**, void>)VTable[10];
		_VSSetShader = (delegate* unmanaged<void*, void*, void**, uint, void>)VTable[11];
		_DrawIndexed = (delegate* unmanaged<void*, uint, uint, int, void>)VTable[12];
		_Draw = (delegate* unmanaged<void*, uint, uint, void>)VTable[13];
		_Map = (delegate* unmanaged<void*, void*, uint, D3D11_MAP, uint, D3D11_MAPPED_SUBRESOURCE*, HRESULT>)VTable[14];
		_Unmap = (delegate* unmanaged<void*, void*, uint, void>)VTable[15];
		_PSSetConstantBuffers = (delegate* unmanaged<void*, uint, uint, void**, void>)VTable[16];
		_IASetInputLayout = (delegate* unmanaged<void*, void*, void>)VTable[17];
		_IASetVertexBuffers = (delegate* unmanaged<void*, uint, uint, void**, uint*, uint*, void>)VTable[18];
		_IASetIndexBuffer = (delegate* unmanaged<void*, void*, DXGI_FORMAT, uint, void>)VTable[19];
		_DrawIndexedInstanced = (delegate* unmanaged<void*, uint, uint, uint, int, uint, void>)VTable[20];
		_DrawInstanced = (delegate* unmanaged<void*, uint, uint, uint, uint, void>)VTable[21];
		_GSSetConstantBuffers = (delegate* unmanaged<void*, uint, uint, void**, void>)VTable[22];
		_GSSetShader = (delegate* unmanaged<void*, void*, void**, uint, void>)VTable[23];
		_IASetPrimitiveTopology = (delegate* unmanaged<void*, D3D_PRIMITIVE_TOPOLOGY, void>)VTable[24];
		_VSSetShaderResources = (delegate* unmanaged<void*, uint, uint, void**, void>)VTable[25];
		_VSSetSamplers = (delegate* unmanaged<void*, uint, uint, void**, void>)VTable[26];
		_Begin = (delegate* unmanaged<void*, void*, void>)VTable[27];
		_End = (delegate* unmanaged<void*, void*, void>)VTable[28];
		_GetData = (delegate* unmanaged<void*, void*, void*, uint, uint, HRESULT>)VTable[29];
		_SetPredication = (delegate* unmanaged<void*, void*, BOOL, void>)VTable[30];
		_GSSetShaderResources = (delegate* unmanaged<void*, uint, uint, void**, void>)VTable[31];
		_GSSetSamplers = (delegate* unmanaged<void*, uint, uint, void**, void>)VTable[32];
		_OMSetRenderTargets = (delegate* unmanaged<void*, uint, void**, void*, void>)VTable[33];
		_OMSetRenderTargetsAndUnorderedAccessViews = (delegate* unmanaged<void*, uint, void**, void*, uint, uint, void**, uint*, void>)VTable[34];
		_OMSetBlendState = (delegate* unmanaged<void*, void*, float*, uint, void>)VTable[35];
		_OMSetDepthStencilState = (delegate* unmanaged<void*, void*, uint, void>)VTable[36];
		_SOSetTargets = (delegate* unmanaged<void*, uint, void**, uint*, void>)VTable[37];
		_DrawAuto = (delegate* unmanaged<void*, void>)VTable[38];
		_DrawIndexedInstancedIndirect = (delegate* unmanaged<void*, void*, uint, void>)VTable[39];
		_DrawInstancedIndirect = (delegate* unmanaged<void*, void*, uint, void>)VTable[40];
		_Dispatch = (delegate* unmanaged<void*, uint, uint, uint, void>)VTable[41];
		_DispatchIndirect = (delegate* unmanaged<void*, void*, uint, void>)VTable[42];
		_RSSetState = (delegate* unmanaged<void*, void*, void>)VTable[43];
		_RSSetViewports = (delegate* unmanaged<void*, uint, D3D11_VIEWPORT*, void>)VTable[44];
		_RSSetScissorRects = (delegate* unmanaged<void*, uint, RECT*, void>)VTable[45];
		_CopySubresourceRegion = (delegate* unmanaged<void*, void*, uint, uint, uint, uint, void*, uint, D3D11_BOX*, void>)VTable[46];
		_CopyResource = (delegate* unmanaged<void*, void*, void*, void>)VTable[47];
		_UpdateSubresource = (delegate* unmanaged<void*, void*, uint, D3D11_BOX*, void*, uint, uint, void>)VTable[48];
		_CopyStructureCount = (delegate* unmanaged<void*, void*, uint, void*, void>)VTable[49];
		_ClearRenderTargetView = (delegate* unmanaged<void*, void*, float*, void>)VTable[50];
		_ClearUnorderedAccessViewUint = (delegate* unmanaged<void*, void*, uint*, void>)VTable[51];
		_ClearUnorderedAccessViewFloat = (delegate* unmanaged<void*, void*, float*, void>)VTable[52];
		_ClearDepthStencilView = (delegate* unmanaged<void*, void*, uint, float, byte, void>)VTable[53];
		_GenerateMips = (delegate* unmanaged<void*, void*, void>)VTable[54];
		_SetResourceMinLOD = (delegate* unmanaged<void*, void*, float, void>)VTable[55];
		_GetResourceMinLOD = (delegate* unmanaged<void*, void*, float>)VTable[56];
		_ResolveSubresource = (delegate* unmanaged<void*, void*, uint, void*, uint, DXGI_FORMAT, void>)VTable[57];
		_ExecuteCommandList = (delegate* unmanaged<void*, void*, BOOL, void>)VTable[58];
		_HSSetShaderResources = (delegate* unmanaged<void*, uint, uint, void**, void>)VTable[59];
		_HSSetShader = (delegate* unmanaged<void*, void*, void**, uint, void>)VTable[60];
		_HSSetSamplers = (delegate* unmanaged<void*, uint, uint, void**, void>)VTable[61];
		_HSSetConstantBuffers = (delegate* unmanaged<void*, uint, uint, void**, void>)VTable[62];
		_DSSetShaderResources = (delegate* unmanaged<void*, uint, uint, void**, void>)VTable[63];
		_DSSetShader = (delegate* unmanaged<void*, void*, void**, uint, void>)VTable[64];
		_DSSetSamplers = (delegate* unmanaged<void*, uint, uint, void**, void>)VTable[65];
		_DSSetConstantBuffers = (delegate* unmanaged<void*, uint, uint, void**, void>)VTable[66];
		_CSSetShaderResources = (delegate* unmanaged<void*, uint, uint, void**, void>)VTable[67];
		_CSSetUnorderedAccessViews = (delegate* unmanaged<void*, uint, uint, void**, uint*, void>)VTable[68];
		_CSSetShader = (delegate* unmanaged<void*, void*, void**, uint, void>)VTable[69];
		_CSSetSamplers = (delegate* unmanaged<void*, uint, uint, void**, void>)VTable[70];
		_CSSetConstantBuffers = (delegate* unmanaged<void*, uint, uint, void**, void>)VTable[71];
		_VSGetConstantBuffers = (delegate* unmanaged<void*, uint, uint, void**, void>)VTable[72];
		_PSGetShaderResources = (delegate* unmanaged<void*, uint, uint, void**, void>)VTable[73];
		_PSGetShader = (delegate* unmanaged<void*, void**, void**, uint*, void>)VTable[74];
		_PSGetSamplers = (delegate* unmanaged<void*, uint, uint, void**, void>)VTable[75];
		_VSGetShader = (delegate* unmanaged<void*, void**, void**, uint*, void>)VTable[76];
		_PSGetConstantBuffers = (delegate* unmanaged<void*, uint, uint, void**, void>)VTable[77];
		_IAGetInputLayout = (delegate* unmanaged<void*, void**, void>)VTable[78];
		_IAGetVertexBuffers = (delegate* unmanaged<void*, uint, uint, void**, uint*, uint*, void>)VTable[79];
		_IAGetIndexBuffer = (delegate* unmanaged<void*, void**, DXGI_FORMAT*, uint*, void>)VTable[80];
		_GSGetConstantBuffers = (delegate* unmanaged<void*, uint, uint, void**, void>)VTable[81];
		_GSGetShader = (delegate* unmanaged<void*, void**, void**, uint*, void>)VTable[82];
		_IAGetPrimitiveTopology = (delegate* unmanaged<void*, D3D_PRIMITIVE_TOPOLOGY*, void>)VTable[83];
		_VSGetShaderResources = (delegate* unmanaged<void*, uint, uint, void**, void>)VTable[84];
		_VSGetSamplers = (delegate* unmanaged<void*, uint, uint, void**, void>)VTable[85];
		_GetPredication = (delegate* unmanaged<void*, void**, BOOL*, void>)VTable[86];
		_GSGetShaderResources = (delegate* unmanaged<void*, uint, uint, void**, void>)VTable[87];
		_GSGetSamplers = (delegate* unmanaged<void*, uint, uint, void**, void>)VTable[88];
		_OMGetRenderTargets = (delegate* unmanaged<void*, uint, void**, void**, void>)VTable[89];
		_OMGetRenderTargetsAndUnorderedAccessViews = (delegate* unmanaged<void*, uint, void**, void**, uint, uint, void**, void>)VTable[90];
		_OMGetBlendState = (delegate* unmanaged<void*, void**, float*, uint*, void>)VTable[91];
		_OMGetDepthStencilState = (delegate* unmanaged<void*, void**, uint*, void>)VTable[92];
		_SOGetTargets = (delegate* unmanaged<void*, uint, void**, void>)VTable[93];
		_RSGetState = (delegate* unmanaged<void*, void**, void>)VTable[94];
		_RSGetViewports = (delegate* unmanaged<void*, uint*, D3D11_VIEWPORT*, void>)VTable[95];
		_RSGetScissorRects = (delegate* unmanaged<void*, uint*, RECT*, void>)VTable[96];
		_HSGetShaderResources = (delegate* unmanaged<void*, uint, uint, void**, void>)VTable[97];
		_HSGetShader = (delegate* unmanaged<void*, void**, void**, uint*, void>)VTable[98];
		_HSGetSamplers = (delegate* unmanaged<void*, uint, uint, void**, void>)VTable[99];
		_HSGetConstantBuffers = (delegate* unmanaged<void*, uint, uint, void**, void>)VTable[100];
		_DSGetShaderResources = (delegate* unmanaged<void*, uint, uint, void**, void>)VTable[101];
		_DSGetShader = (delegate* unmanaged<void*, void**, void**, uint*, void>)VTable[102];
		_DSGetSamplers = (delegate* unmanaged<void*, uint, uint, void**, void>)VTable[103];
		_DSGetConstantBuffers = (delegate* unmanaged<void*, uint, uint, void**, void>)VTable[104];
		_CSGetShaderResources = (delegate* unmanaged<void*, uint, uint, void**, void>)VTable[105];
		_CSGetUnorderedAccessViews = (delegate* unmanaged<void*, uint, uint, void**, void>)VTable[106];
		_CSGetShader = (delegate* unmanaged<void*, void**, void**, uint*, void>)VTable[107];
		_CSGetSamplers = (delegate* unmanaged<void*, uint, uint, void**, void>)VTable[108];
		_CSGetConstantBuffers = (delegate* unmanaged<void*, uint, uint, void**, void>)VTable[109];
		_ClearState = (delegate* unmanaged<void*, void>)VTable[110];
		_Flush = (delegate* unmanaged<void*, void>)VTable[111];
		_GetType = (delegate* unmanaged<void*, D3D11_DEVICE_CONTEXT_TYPE>)VTable[112];
		_GetContextFlags = (delegate* unmanaged<void*, uint>)VTable[113];
		_FinishCommandList = (delegate* unmanaged<void*, BOOL, void**, HRESULT>)VTable[114];*/
	}
}

[Guid("BB2C6FAA-B5FB-4082-8E6B-388B8CFA90E1")]
public unsafe class D3D11DeviceContext1 : D3D11DeviceContext
{
	/*protected readonly delegate* unmanaged<void*, void*, uint, uint, uint, uint, void*, uint, D3D11_BOX*, uint, void> _CopySubresourceRegion1;
	protected readonly delegate* unmanaged<void*, void*, uint, D3D11_BOX*, void*, uint, uint, uint, void> _UpdateSubresource1;
	protected readonly delegate* unmanaged<void*, void*, void> _DiscardResource;
	protected readonly delegate* unmanaged<void*, void*, void> _DiscardView;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, uint*, uint*, void> _VSSetConstantBuffers1;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, uint*, uint*, void> _HSSetConstantBuffers1;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, uint*, uint*, void> _DSSetConstantBuffers1;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, uint*, uint*, void> _GSSetConstantBuffers1;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, uint*, uint*, void> _PSSetConstantBuffers1;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, uint*, uint*, void> _CSSetConstantBuffers1;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, uint*, uint*, void> _VSGetConstantBuffers1;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, uint*, uint*, void> _HSGetConstantBuffers1;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, uint*, uint*, void> _DSGetConstantBuffers1;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, uint*, uint*, void> _GSGetConstantBuffers1;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, uint*, uint*, void> _PSGetConstantBuffers1;
	protected readonly delegate* unmanaged<void*, uint, uint, void**, uint*, uint*, void> _CSGetConstantBuffers1;
	protected readonly delegate* unmanaged<void*, void*, void**, void> _SwapDeviceContextState;
	protected readonly delegate* unmanaged<void*, void*, float*, RECT*, uint, void> _ClearView;
	protected readonly delegate* unmanaged<void*, void*, RECT*, uint, void> _DiscardView1;*/

	protected D3D11DeviceContext1(void* raw, bool addRef = false) : base(raw, addRef)
	{
		/*_CopySubresourceRegion1 = (delegate* unmanaged<void*, void*, uint, uint, uint, uint, void*, uint, D3D11_BOX*, uint, void>)VTable[115];
		_UpdateSubresource1 = (delegate* unmanaged<void*, void*, uint, D3D11_BOX*, void*, uint, uint, uint, void>)VTable[116];
		_DiscardResource = (delegate* unmanaged<void*, void*, void>)VTable[117];
		_DiscardView = (delegate* unmanaged<void*, void*, void>)VTable[118];
		_VSSetConstantBuffers1 = (delegate* unmanaged<void*, uint, uint, void**, uint*, uint*, void>)VTable[119];
		_HSSetConstantBuffers1 = (delegate* unmanaged<void*, uint, uint, void**, uint*, uint*, void>)VTable[120];
		_DSSetConstantBuffers1 = (delegate* unmanaged<void*, uint, uint, void**, uint*, uint*, void>)VTable[121];
		_GSSetConstantBuffers1 = (delegate* unmanaged<void*, uint, uint, void**, uint*, uint*, void>)VTable[122];
		_PSSetConstantBuffers1 = (delegate* unmanaged<void*, uint, uint, void**, uint*, uint*, void>)VTable[123];
		_CSSetConstantBuffers1 = (delegate* unmanaged<void*, uint, uint, void**, uint*, uint*, void>)VTable[124];
		_VSGetConstantBuffers1 = (delegate* unmanaged<void*, uint, uint, void**, uint*, uint*, void>)VTable[125];
		_HSGetConstantBuffers1 = (delegate* unmanaged<void*, uint, uint, void**, uint*, uint*, void>)VTable[126];
		_DSGetConstantBuffers1 = (delegate* unmanaged<void*, uint, uint, void**, uint*, uint*, void>)VTable[127];
		_GSGetConstantBuffers1 = (delegate* unmanaged<void*, uint, uint, void**, uint*, uint*, void>)VTable[128];
		_PSGetConstantBuffers1 = (delegate* unmanaged<void*, uint, uint, void**, uint*, uint*, void>)VTable[129];
		_CSGetConstantBuffers1 = (delegate* unmanaged<void*, uint, uint, void**, uint*, uint*, void>)VTable[130];
		_SwapDeviceContextState = (delegate* unmanaged<void*, void*, void**, void>)VTable[131];
		_ClearView = (delegate* unmanaged<void*, void*, float*, RECT*, uint, void>)VTable[132];
		_DiscardView1 = (delegate* unmanaged<void*, void*, RECT*, uint, void>)VTable[133];*/
	}
}

[Guid("420D5B32-B90C-4DA4-BEF0-359F6A24A83A")]
public unsafe class D3D11DeviceContext2 : D3D11DeviceContext1
{
	// protected readonly delegate* unmanaged<void*, void*, uint, D3D11_TILED_RESOURCE_COORDINATE*, D3D11_TILE_REGION_SIZE*, void*, uint, uint*, uint*, uint*, uint, HRESULT> _UpdateTileMappings;
	// protected readonly delegate* unmanaged<void*, void*, D3D11_TILED_RESOURCE_COORDINATE*, void*, D3D11_TILED_RESOURCE_COORDINATE*, D3D11_TILE_REGION_SIZE*, uint, HRESULT> _CopyTileMappings;
	// protected readonly delegate* unmanaged<void*, void*, D3D11_TILED_RESOURCE_COORDINATE*, D3D11_TILE_REGION_SIZE*, void*, ulong, uint, void> _CopyTiles;
	// protected readonly delegate* unmanaged<void*, void*, D3D11_TILED_RESOURCE_COORDINATE*, D3D11_TILE_REGION_SIZE*, void*, uint, void> _UpdateTiles;
	// protected readonly delegate* unmanaged<void*, void*, ulong, HRESULT> _ResizeTilePool;
	// protected readonly delegate* unmanaged<void*, void*, void*, void> _TiledResourceBarrier;
	// protected readonly delegate* unmanaged<void*, BOOL> _IsAnnotationEnabled;
	// protected readonly delegate* unmanaged<void*, PCWSTR, int, void> _SetMarkerInt;
	// protected readonly delegate* unmanaged<void*, PCWSTR, int, void> _BeginEventInt;
	// protected readonly delegate* unmanaged<void*, void> _EndEvent;

	protected D3D11DeviceContext2(void* raw, bool addRef = false) : base(raw, addRef)
	{
		// _UpdateTileMappings = (delegate* unmanaged<void*, void*, uint, D3D11_TILED_RESOURCE_COORDINATE*, D3D11_TILE_REGION_SIZE*, void*, uint, uint*, uint*, uint*, uint, HRESULT>)VTable[134];
		// _CopyTileMappings = (delegate* unmanaged<void*, void*, D3D11_TILED_RESOURCE_COORDINATE*, void*, D3D11_TILED_RESOURCE_COORDINATE*, D3D11_TILE_REGION_SIZE*, uint, HRESULT>)VTable[135];
		// _CopyTiles = (delegate* unmanaged<void*, void*, D3D11_TILED_RESOURCE_COORDINATE*, D3D11_TILE_REGION_SIZE*, void*, ulong, uint, void>)VTable[136];
		// _UpdateTiles = (delegate* unmanaged<void*, void*, D3D11_TILED_RESOURCE_COORDINATE*, D3D11_TILE_REGION_SIZE*, void*, uint, void>)VTable[137];
		// _ResizeTilePool = (delegate* unmanaged<void*, void*, ulong, HRESULT>)VTable[138];
		// _TiledResourceBarrier = (delegate* unmanaged<void*, void*, void*, void>)VTable[139];
		// _IsAnnotationEnabled = (delegate* unmanaged<void*, BOOL>)VTable[140];
		// _SetMarkerInt = (delegate* unmanaged<void*, PCWSTR, int, void>)VTable[141];
		// _BeginEventInt = (delegate* unmanaged<void*, PCWSTR, int, void>)VTable[142];
		// _EndEvent = (delegate* unmanaged<void*, void>)VTable[143];
	}
}

[Guid("B4E3C01D-E79E-4637-91B2-510E9F4C9B8F")]
public unsafe class D3D11DeviceContext3 : D3D11DeviceContext2
{
	// protected readonly delegate* unmanaged<void*, D3D11_CONTEXT_TYPE, HANDLE, void> _Flush1;
	// protected readonly delegate* unmanaged<void*, BOOL, void> _SetHardwareProtectionState;
	// protected readonly delegate* unmanaged<void*, BOOL*, void> _GetHardwareProtectionState;

	protected D3D11DeviceContext3(void* raw, bool addRef = false) : base(raw, addRef)
	{
		// _Flush1 = (delegate* unmanaged<void*, D3D11_CONTEXT_TYPE, HANDLE, void>)VTable[144];
		// _SetHardwareProtectionState = (delegate* unmanaged<void*, BOOL, void>)VTable[145];
		// _GetHardwareProtectionState = (delegate* unmanaged<void*, BOOL*, void>)VTable[146];
	}
}

[Guid("917600DA-F58C-4C33-98D8-3E15B390FA24")]
public sealed unsafe class D3D11DeviceContext4 : D3D11DeviceContext3
{
	// private readonly delegate* unmanaged<void*, void*, ulong, HRESULT> _signal;
	// private readonly delegate* unmanaged<void*, void*, ulong, HRESULT> _wait;

	public D3D11DeviceContext4(void* raw, bool addRef = false) : base(raw, addRef)
	{
		// _signal = (delegate* unmanaged<void*, void*, ulong, HRESULT>)VTable[147];
		// _wait = (delegate* unmanaged<void*, void*, ulong, HRESULT>)VTable[148];
	}
}