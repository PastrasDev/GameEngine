using System.Runtime.InteropServices;
using Windows.Win32.System.Com;

namespace Windows.Win32.Graphics.Direct3D11;


[Interface][Guid(IID.D3D11.ID3D11DeviceContext)]
public partial class D3D11DeviceContext(nint ptr, bool addRef = false) : D3D11DeviceChild(ptr, addRef)
{
	
}

[Interface][Guid(IID.D3D11.ID3D11DeviceContext1)]
public partial class D3D11DeviceContext1(nint ptr, bool addRef = false) : D3D11DeviceContext(ptr, addRef)
{

}


[Interface][Guid(IID.D3D11.ID3D11DeviceContext2)]
public partial class D3D11DeviceContext2(nint ptr, bool addRef = false) : D3D11DeviceContext1(ptr, addRef)
{

}

[Interface][Guid(IID.D3D11.ID3D11DeviceContext3)]
public partial class D3D11DeviceContext3(nint ptr, bool addRef = false) : D3D11DeviceContext2(ptr, addRef)
{

}

[Interface][Guid(IID.D3D11.ID3D11DeviceContext4)]
public partial class D3D11DeviceContext4(nint ptr, bool addRef = false) : D3D11DeviceContext3(ptr, addRef)
{

}