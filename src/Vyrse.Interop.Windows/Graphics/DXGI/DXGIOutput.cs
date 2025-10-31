using System.Runtime.InteropServices;

namespace Vyrse.Interop.Windows.Graphics.DXGI;

[Guid("AE02EEDB-C735-4690-8D52-5A8DC20213AA")]
public unsafe class DXGIOutput : DXGIObject
{
	protected DXGIOutput(void* raw, bool addRef = false) : base(raw, addRef)
	{

	}
}

[Guid("00CDDEA8-939B-4B83-A340-A685226666CC")]
public unsafe class DXGIOutput1 : DXGIOutput
{
	protected DXGIOutput1(void* raw, bool addRef = false) : base(raw, addRef)
	{

	}
}

[Guid("595E39D1-2724-4663-99B1-DA969DE28364")]
public unsafe class DXGIOutput2 : DXGIOutput1
{
	protected DXGIOutput2(void* raw, bool addRef = false) : base(raw, addRef)
	{

	}
}

[Guid("8A6BB301-7E7E-41F4-A8E0-5B32F7F99B18")]
public unsafe class DXGIOutput3 : DXGIOutput2
{
	protected DXGIOutput3(void* raw, bool addRef = false) : base(raw, addRef)
	{

	}
}

[Guid("DC7DCA35-2196-414D-9F53-617884032A60")]
public unsafe class DXGIOutput4 : DXGIOutput3
{
	protected DXGIOutput4(void* raw, bool addRef = false) : base(raw, addRef)
	{

	}
}

[Guid("80A07424-AB52-42EB-833C-0C42FD282D98")]
public unsafe class DXGIOutput5 : DXGIOutput4
{
	protected  DXGIOutput5(void* raw, bool addRef = false) : base(raw, addRef)
	{

	}
}

[Guid("068346E8-AAEC-4B84-ADD7-137F513F77A1")]
public sealed unsafe class DXGIOutput6 : DXGIOutput5
{
	private DXGIOutput6(void* raw, bool addRef = false) : base(raw, addRef)
	{

	}
}