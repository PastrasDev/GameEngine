using System.Runtime.InteropServices;
using Vyrse.Interop.Windows.Win32.Graphics.Dxgi.Common;

namespace Windows.Win32.Graphics.Direct3D11;

[StructLayout(LayoutKind.Sequential)]
public struct D3D11_TEXTURE2D_DESC
{
	internal uint Width;
	internal uint Height;
	internal uint MipLevels;
	internal uint ArraySize;
	internal DXGI_FORMAT Format;
	internal DXGI_SAMPLE_DESC SampleDesc;
	internal D3D11_USAGE Usage;
	private uint _BindFlags;
	private uint _CPUAccessFlags;
	private uint _MiscFlags;

	internal D3D11_BIND_FLAG BindFlags
	{
		readonly get => (D3D11_BIND_FLAG)_BindFlags;
		set => _BindFlags = (uint)value;
	}

	internal D3D11_CPU_ACCESS_FLAG CPUAccessFlags
	{
		readonly get => (D3D11_CPU_ACCESS_FLAG)_CPUAccessFlags;
		set => _CPUAccessFlags = (uint)value;
	}

	internal D3D11_RESOURCE_MISC_FLAG MiscFlags
	{
		readonly get => (D3D11_RESOURCE_MISC_FLAG)_MiscFlags;
		set => _MiscFlags = (uint)value;
	}
}