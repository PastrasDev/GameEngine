using System.Runtime.InteropServices;
using Windows.Win32.Foundation;
using Windows.Win32.System.Com;

namespace Windows.Win32.Graphics.Direct3D11;

[Interface][Guid(IID.D3D11.ID3D11DeviceChild)]
public partial class D3D11DeviceChild : Unknown
{
	public partial void GetDevice(out D3D11Device ppDevice);
	public partial HRESULT GetPrivateData(in Guid guid, ref uint pDataSize, out nint pData);
	public partial HRESULT SetPrivateData(in Guid guid, uint dataSize, nint pData);
	public partial HRESULT SetPrivateDataInterface(in Guid guid, nint pData);
}