using System.Runtime.InteropServices;

namespace Vyrse.Interop.Windows.Graphics.DXGI;

[Guid("2411E7E1-12AC-4CCF-BD14-9798E8534DC0")]
public unsafe class DXGIAdapter : DXGIObject
{
	protected DXGIAdapter(void* raw, bool addRef = false) : base(raw, addRef)
	{

	}
}

[Guid("29038F61-3839-4626-91FD-086879011A05")]
public unsafe class DXGIAdapter1 : DXGIAdapter
{
	protected DXGIAdapter1(void* raw, bool addRef = false) : base(raw, addRef)
	{

	}
}

[Guid("0AA1AE0A-FA0E-4B84-8644-E05FF8E5ACB5")]
public unsafe class DXGIAdapter2 : DXGIAdapter1
{
	protected DXGIAdapter2(void* raw, bool addRef = false) : base(raw, addRef)
	{

	}
}

[Guid("645967A4-1392-4310-A798-8053CE3E93FD")]
public unsafe class DXGIAdapter3 : DXGIAdapter2
{
	protected DXGIAdapter3(void* raw, bool addRef = false) : base(raw, addRef)
	{

	}
}

[Guid("3C8D99D1-4FBF-4181-A82C-AF66BF7BD24E")]
public sealed unsafe class DXGIAdapter4 : DXGIAdapter3
{
	private DXGIAdapter4(void* raw, bool addRef = false) : base(raw, addRef)
	{

	}
}