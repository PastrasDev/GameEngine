using System.Runtime.InteropServices;
using Windows.Win32.System.Com;

namespace Windows.Win32.Graphics.Direct3D11;

[Interface][Guid(IID.D3D11.ID3D11Resource)]
public partial class D3D11Resource : D3D11DeviceChild
{
	public partial void GetType(out D3D11_RESOURCE_DIMENSION pResourceDimension);
	public partial void SetEvictionPriority(uint evictionPriority);
	public partial uint GetEvictionPriority();
}