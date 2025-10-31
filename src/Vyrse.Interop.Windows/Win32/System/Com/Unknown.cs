using System.Runtime.InteropServices;
using Windows.Win32.Foundation;

namespace Windows.Win32.System.Com;

[Interface][Guid(IID.IUnknown)]
public partial class Unknown : IDisposable
{
	public unsafe void* Raw { get; private set; }
	protected unsafe nint* Vtbl => (nint*)(*(nint*)Raw);
	public unsafe bool IsNull => Raw == null;

	~Unknown()
	{
		unsafe
		{
			if (IsNull) return;
			Release();
			Raw = null;
		}
	}

	public static unsafe implicit operator Unknown(void* ptr) => new((nint)ptr);
	public static implicit operator Unknown(nint ptr) => new(ptr);
	public static unsafe implicit operator void* (Unknown obj) => obj.Raw;
	public static unsafe implicit operator nint (Unknown obj) => (nint)obj.Raw;

	public partial HRESULT QueryInterface(in Guid riid, out nint ppvObject);
	public partial uint AddRef();
	public partial uint Release();

	protected static T Create<T>(nint ptr) where T : Unknown => (T)Activator.CreateInstance(typeof(T), ptr, false)!;

	public void Dispose()
	{
		unsafe
		{
			if (IsNull) return;
			Release();
			Raw = null;
			GC.SuppressFinalize(this);
		}
	}
}