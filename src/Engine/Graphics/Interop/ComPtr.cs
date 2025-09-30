using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Win32.Foundation;
using Windows.Win32.System.Com;
using Engine.Debug;

namespace Engine.Graphics.Interop;

public sealed class ComPtr<T> : IDisposable where T : unmanaged
{
	private const int EPointer = unchecked((int)0x80004003);

	/// <summary>Raw COM pointer (IUnknown*).</summary>
	public nint Ptr
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private set;
	}

	/// <summary>True if Ptr == 0.</summary>
	public bool IsNull => Ptr == 0;

	/// <summary>A null, non-owning instance.</summary>
	public static ComPtr<T> Null => new(0, addRef: false);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ComPtr() { }

	/// <summary>
	/// Construct from an existing pointer. If addRef==true, calls AddRef to take ownership without consuming the caller's reference.
	/// If addRef==false, assumes ownership of the caller's reference (typical for out-params).
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ComPtr(nint ptr, bool addRef = false)
	{
		if (ptr != 0 && addRef) Marshal.AddRef(ptr);
		Ptr = ptr;
	}

	/// <summary>Attach takes ownership of an out-parameter pointer (no AddRef).</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ComPtr<T> Attach(nint ptr) => new(ptr, addRef: false);

	/// <summary>Create a new owner that shares the same pointer (AddRef).</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ComPtr<T> Clone()
	{
		if (Ptr != 0) Marshal.AddRef(Ptr);
		return new ComPtr<T>(Ptr, addRef: false);
	}

	/// <summary>Release ownership and return the raw pointer without calling Release.</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public nint Detach()
	{
		var p = Ptr;
		Ptr = 0;
		return p;
	}

	/// <summary>QueryInterface to another COM interface type and return as ComPtr.</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	#pragma warning disable CS0693
	public unsafe ComPtr<T> QueryInterface<T>([CallerMemberName] string? caller = null, [CallerFilePath] string? file = null, [CallerLineNumber] int line = 0) where T : unmanaged
	#pragma warning restore CS0693
	{
		if (Ptr == 0) throw new NullReferenceException($"ComPtr is null at {caller} ({file}:{line})");
		var iid = typeof(T).GUID;
		void* pObj = null;
		((IUnknown*)Ptr)->QueryInterface(&iid, &pObj).ThrowOnFailure();
		if (pObj is null) throw new InvalidOperationException($"QI<{typeof(T).Name}> returned null at {caller} ({file}:{line})");
		return new ComPtr<T>((nint)pObj);
	}

	#pragma warning disable CS0693
	public unsafe (HRESULT hr, ComPtr<T> result) TryQueryInterface<T>() where T : unmanaged
	#pragma warning restore CS0693
	{
		if (Ptr == 0) return (new HRESULT(EPointer), ComPtr<T>.Null);
		var iid = typeof(T).GUID;
		void* pObj = null;
		var hr = ((IUnknown*)Ptr)->QueryInterface(&iid, &pObj);
		if (hr.Succeeded && pObj is not null) return (hr, new ComPtr<T>((nint)pObj));
		return (hr, ComPtr<T>.Null);
	}

	/// <summary>Manually AddRef (seldom needed).</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int AddRef() => Ptr == 0 ? 0 : Marshal.AddRef(Ptr);

	/// <summary>Manually Release (seldom needed). Does not null Ptr.</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int Release() => Ptr == 0 ? 0 : Marshal.Release(Ptr);

	/// <summary>Dispose releases the owned pointer (if any).</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Dispose()
	{
		ReleaseAndClear();
		GC.SuppressFinalize(this);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	~ComPtr() => ReleaseAndClear();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void ReleaseAndClear()
	{
		var p = Ptr;
		Ptr = 0;
		if (p != 0)
		{
			try
			{
				Marshal.Release(p);
			}
			catch (Exception ex)
			{
				#if DEBUG
				Log.Debug($"ComPtr<{typeof(T).Name}> finalizer Release threw: {ex}");
				#endif
			}
		}
	}


	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static unsafe implicit operator IUnknown*(ComPtr<T> c) => (IUnknown*)c.Ptr;

	/// <summary>Implicit cast to raw nint for interop helpers.</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator nint(ComPtr<T> c) => c.Ptr;

	/// <summary>Truthiness convenience.</summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator bool(ComPtr<T> c) => c is { Ptr: not 0 };

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString() => $"ComPtr<{typeof(T).Name}>({Ptr:X})";
}