using System.Runtime.InteropServices;

namespace Windows.Win32.Graphics.Direct3D11;

[StructLayout(LayoutKind.Sequential)]
public struct D3D11_BUFFER_RTV
{
	public Union Anonymous;
	public Union1 Anonymous1;

	[StructLayout(LayoutKind.Explicit)]
	public struct Union
	{
		[FieldOffset(0)] public uint FirstElement;
		[FieldOffset(0)] public uint ElementOffset;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct Union1
	{
		[FieldOffset(0)] public uint NumElements;
		[FieldOffset(0)] public uint ElementWidth;
	}
}