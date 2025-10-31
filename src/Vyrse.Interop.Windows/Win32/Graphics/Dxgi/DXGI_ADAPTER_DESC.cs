using System.Runtime.InteropServices;
using Windows.Win32.Foundation;
using Vyrse.Interop.Windows.Primitives;

namespace Windows.Win32.Graphics.Dxgi;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
public struct DXGI_ADAPTER_DESC
{
	public FixedArray128<char> Description;
	public uint VendorId;
	public uint DeviceId;
	public uint SubSysId;
	public uint Revision;
	public nuint DedicatedVideoMemory;
	public nuint DedicatedSystemMemory;
	public nuint SharedSystemMemory;
	public LUID AdapterLuid;
}