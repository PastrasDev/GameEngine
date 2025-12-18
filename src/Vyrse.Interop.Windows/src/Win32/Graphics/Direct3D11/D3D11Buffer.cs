using System.Runtime.InteropServices;
using Windows.Win32.System.Com;

namespace Windows.Win32.Graphics.Direct3D11;

[Interface][Guid(IID.D3D11.ID3D11Buffer)]
public partial class D3D11Buffer : D3D11Resource
{
	public partial void GetDesc(out D3D11_BUFFER_DESC pDesc);
}