using System.Runtime.InteropServices;
using Windows.Win32.Foundation;
using Windows.Win32.System.Com;

namespace Windows.Win32.Graphics.Dxgi;

[Interface][Guid(IID.DXGI.IDXGIAdapter)]
public partial class DXGIAdapter: DXGIObject
{
	public partial HRESULT EnumOutputs(uint output, out DXGIOutput ppOutput);
	public partial HRESULT GetDesc(out DXGI_ADAPTER_DESC pDesc);
	public partial HRESULT CheckInterfaceSupport(in Guid interfaceName, out long pUmdVersion);
}