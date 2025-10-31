using System.Runtime.InteropServices;
using Windows.Win32.Foundation;
using Windows.Win32.System.Com;

namespace Windows.Win32.Graphics.Dxgi;

[Interface][Guid(IID.DXGI.IDXGIObject)]
public partial class DXGIObject : Unknown
{
	public partial HRESULT SetPrivateData(in Guid name, uint dataSize, in nint pData);
	public partial HRESULT SetPrivateDataInterface(in Guid name, Unknown? pUnknown);
	public partial HRESULT GetPrivateData(in Guid name, ref uint pDataSize, out nint pData);
	public partial HRESULT GetParent(in Guid riid, out nint ppParent);
}