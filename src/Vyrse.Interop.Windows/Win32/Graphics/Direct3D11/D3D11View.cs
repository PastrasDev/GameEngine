using System.Runtime.InteropServices;
using Windows.Win32.System.Com;

namespace Windows.Win32.Graphics.Direct3D11;

[Interface][Guid(IID.D3D11.ID3D11View)]
public partial class D3D11View : D3D11DeviceChild
{
	public partial void GetResource(out D3D11Resource ppResource);
}