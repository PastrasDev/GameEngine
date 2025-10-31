using System.Runtime.InteropServices;
using Windows.Win32.System.Com;

namespace Windows.Win32.Graphics.Direct3D11;

[Interface][Guid(IID.D3D11.ID3D11Device)]
public class D3D11Device(nint ptr, bool addRef = false) : Unknown(ptr, addRef)
{

}

[Interface][Guid(IID.D3D11.ID3D11Device1)]
public class D3D11Device1(nint ptr, bool addRef = false) : D3D11Device(ptr, addRef)
{

}

[Interface][Guid(IID.D3D11.ID3D11Device2)]
public class D3D11Device2(nint ptr, bool addRef = false) : D3D11Device1(ptr, addRef)
{

}


[Interface][Guid(IID.D3D11.ID3D11Device3)]
public class D3D11Device3(nint ptr, bool addRef = false) : D3D11Device2(ptr, addRef)
{

}

[Interface][Guid(IID.D3D11.ID3D11Device4)]
public class D3D11Device4(nint ptr, bool addRef = false) : D3D11Device3(ptr, addRef)
{

}

[Interface][Guid(IID.D3D11.ID3D11Device5)]
public sealed class D3D11Device5(nint ptr, bool addRef = false) : D3D11Device4(ptr, addRef)
{

}