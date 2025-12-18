using System.Runtime.InteropServices;
using Windows.Win32.System.Com;

namespace Windows.Win32.Graphics.Direct3D11;

[Interface][Guid(IID.D3D11.ID3D11Texture2D)]
public partial class D3D11Texture2D : D3D11Resource
{
	public partial void GetDesc(out D3D11_TEXTURE2D_DESC pDesc);
}

[Interface][Guid(IID.D3D11.ID3D11Texture2D1)]
public partial class D3D11Texture2D1 : D3D11Texture2D
{
	public partial void GetDesc1(out D3D11_TEXTURE2D_DESC1 pDesc);
}