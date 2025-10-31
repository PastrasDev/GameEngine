using System.Runtime.InteropServices;
using Windows.Win32.System.Com;

namespace Windows.Win32.Graphics.Direct3D11;

[Interface][Guid(IID.D3D11.ID3D11RenderTargetView)]
public partial class D3D11RenderTargetView : D3D11View
{
	public partial void GetDesc(out D3D11_RENDER_TARGET_VIEW_DESC pDesc);
}

[Interface][Guid(IID.D3D11.ID3D11RenderTargetView1)]
public partial class D3D11RenderTargetView1 : D3D11RenderTargetView
{
	public partial void GetDesc1(out D3D11_RENDER_TARGET_VIEW_DESC1 pDesc);
}