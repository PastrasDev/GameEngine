using System.Runtime.InteropServices;
using Windows.Win32.Foundation;

namespace Windows.Graphics;

[Guid("00000000-0000-0000-C000-000000000046")]
public class Unknown : IDisposable
{
    public unsafe nint Handle => (nint)Raw;
    protected unsafe void* Raw { get; private set; }
    protected unsafe nint* Vtbl => (nint*)(*(nint*)Raw);
    public unsafe bool IsNull => Raw == null;

    private readonly unsafe delegate* unmanaged[Stdcall]<nint, Guid*, void**, HRESULT> _queryInterface;
    private readonly unsafe delegate* unmanaged[Stdcall]<nint, uint> _addRef;
    private readonly unsafe delegate* unmanaged[Stdcall]<nint, uint> _release;

    protected Unknown(nint ptr, bool addRef = false)
    {
        unsafe
        {
            Raw = (void*)ptr;
            if (!IsNull)
            {
                _queryInterface = (delegate* unmanaged[Stdcall]<nint, Guid*, void**, HRESULT>)Vtbl[0];
                _addRef = (delegate* unmanaged[Stdcall]<nint, uint>)Vtbl[1];
                _release = (delegate* unmanaged[Stdcall]<nint, uint>)Vtbl[2];
                if (addRef) AddRef();
            }
        }
    }

    ~Unknown()
    {
        try
        {
            unsafe
            {
                if (IsNull) return;
                Release();
                Raw = null;
            }
        }
        catch
        {
             /* Swallow all exceptions */
        }
    }

    public static unsafe implicit operator Unknown(void* ptr) => new((nint)ptr);
    public static implicit operator Unknown(nint ptr) => new(ptr);
    public static unsafe implicit operator void* (Unknown obj) => obj.Raw;
    public static unsafe implicit operator nint (Unknown obj) => (nint)obj.Raw;

    public HRESULT QueryInterface(in Guid riid, out Unknown ppvObject)
    {
        ppvObject = null!;
        if (IsNull) return unchecked((int)0x80004003);

        unsafe
        {
            void* result = null;
            fixed (Guid* pRiid = &riid)
            {
                var hr = _queryInterface(Handle, pRiid, &result);
                ppvObject = (Unknown)result;
                return hr;
            }
        }
    }

    public uint AddRef()
    {
        if (IsNull) return 0;
        unsafe { return _addRef(Handle); }
    }

    public uint Release()
    {
        if (IsNull) return 0;
        unsafe { return _release(Handle); }
    }

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

    protected static T Create<T>(nint ptr) where T : Unknown => (T)Activator.CreateInstance(typeof(T), ptr, false)!;

    public HRESULT QueryInterface<T>(out T obj) where T : Unknown
    {
        obj = null!;
        var iid = typeof(T).GUID;

        var hr = QueryInterface(in iid, out var unknown);

        if (hr.Succeeded && !unknown.IsNull)
            obj = Create<T>(unknown.Handle);

        return hr;
    }
}