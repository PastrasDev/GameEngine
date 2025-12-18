using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using Windows.Win32.System.Com;

namespace Windows.Win32.Graphics.Dxgi;

[Guid(IID.DXGI.IDXGIOutput)]
public partial class DXGIOutput(nint ptr, bool addRef = false) : DXGIObject(ptr, addRef)
{
	
}