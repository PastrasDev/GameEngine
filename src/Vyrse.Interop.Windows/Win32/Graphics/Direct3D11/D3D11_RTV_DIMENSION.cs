namespace Windows.Win32.Graphics.Direct3D11;

public enum D3D11_RTV_DIMENSION
{
	/// <summary>Do not use this value, as it will cause <a href="https://docs.microsoft.com/windows/desktop/api/d3d11/nf-d3d11-id3d11device-createrendertargetview">ID3D11Device::CreateRenderTargetView</a> to fail.</summary>
	D3D11_RTV_DIMENSION_UNKNOWN = 0,
	/// <summary>The resource will be accessed as a buffer.</summary>
	D3D11_RTV_DIMENSION_BUFFER = 1,
	/// <summary>The resource will be accessed as a 1D texture.</summary>
	D3D11_RTV_DIMENSION_TEXTURE1D = 2,
	/// <summary>The resource will be accessed as an array of 1D textures.</summary>
	D3D11_RTV_DIMENSION_TEXTURE1DARRAY = 3,
	/// <summary>The resource will be accessed as a 2D texture.</summary>
	D3D11_RTV_DIMENSION_TEXTURE2D = 4,
	/// <summary>The resource will be accessed as an array of 2D textures.</summary>
	D3D11_RTV_DIMENSION_TEXTURE2DARRAY = 5,
	/// <summary>The resource will be accessed as a 2D texture with multisampling.</summary>
	D3D11_RTV_DIMENSION_TEXTURE2DMS = 6,
	/// <summary>The resource will be accessed as an array of 2D textures with multisampling.</summary>
	D3D11_RTV_DIMENSION_TEXTURE2DMSARRAY = 7,
	/// <summary>The resource will be accessed as a 3D texture.</summary>
	D3D11_RTV_DIMENSION_TEXTURE3D = 8,
}