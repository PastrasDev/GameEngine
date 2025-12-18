using System.Runtime.InteropServices;
using Windows.Win32.System.Com;

namespace Windows.Win32.Graphics.Direct3D11;

[Interface][Guid(IID.D3D11.ID3D11DepthStencilView)]
public partial class D3D11DepthStencilView : D3D11View
{
	public partial void GetDesc(out D3D11_DEPTH_STENCIL_VIEW_DESC pDesc);
}