using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Win32.Foundation;

namespace Vyrse.Interop.Windows.Com
{
	[Guid("00000000-0000-0000-C000-000000000046")]
	public abstract unsafe partial class Unknown : IDisposable, IEquatable<Unknown>
	{
		protected const string Dll = "Vyrse.Interop.Unmanaged.dll";

		public bool IsNull => Raw is null;
		internal nint NativePtr => (nint)Raw;

		public void* Raw { get; private set; }

		private void* _identity;
		private void* Identity
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				if (_identity != null) return _identity;
				if (Raw is null) return null;
				Unmanaged.QueryInterface(Raw, typeof(Unknown).GUID, out var pUnk).ThrowOnFailure();
				_identity = pUnk != 0 ? (void*)pUnk : Raw;
				return _identity;
			}
		}

		private static partial class Unmanaged
		{
			[LibraryImport(Dll, EntryPoint = "IUnknown_QueryInterface")]
			internal static unsafe partial HRESULT QueryInterface(void* instance, in Guid riid, out nint result);

			[LibraryImport(Dll, EntryPoint = "IUnknown_AddRef")]
			internal static unsafe partial uint AddRef(void* instance);

			[LibraryImport(Dll, EntryPoint = "IUnknown_Release")]
			internal static unsafe partial uint Release(void* instance);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected Unknown(void* raw, bool addRef = false)
		{
			if (raw is null) throw new ArgumentNullException(nameof(raw));
			Raw = raw;
			if (addRef) AddRef();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static implicit operator void* (Unknown obj) => obj.Raw;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static implicit operator nint (Unknown obj) => (nint)obj.Raw;

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator true(Unknown? obj) => obj is { } && obj.Raw is not null;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator false(Unknown? obj) => obj is null || obj.Raw is null;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator !(Unknown? obj) => obj is null || obj.Raw is null;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator ==(Unknown? left, Unknown? right) => left?.Equals(right) ?? right is null;
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator !=(Unknown? left, Unknown? right) => !(left == right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(Unknown? other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (other is null) return false;
			return Identity == other.Identity;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(object? obj) => obj is Unknown u && Equals(u);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode() => ((nint)Identity).GetHashCode();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Create<T>(void* raw, bool addRef = false) where T : Unknown
		{
			ThrowIfNull(raw);
			return ComFactory<T>.Create(raw, addRef);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Create<T>(nint raw, bool addRef = false) where T : Unknown
		{
			ThrowIfNull((void*)raw);
			return ComFactory<T>.Create((void*)raw, addRef);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint AddRef() => Unmanaged.AddRef(Raw);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint Release() => Unmanaged.Release(Raw);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T Query<T>() where T : Unknown
		{
			ThrowIfNull();
			Unmanaged.QueryInterface(Raw, typeof(T).GUID, out var raw).ThrowOnFailure();
			return Create<T>(raw);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool TryQuery<T>(out T? result) where T : Unknown
		{
			result = null;
			if (Unmanaged.QueryInterface(Raw, typeof(T).GUID, out var raw).Failed || raw == 0)
				return false;
			result = Create<T>(raw);
			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected void ThrowIfNull()
		{
			if (IsNull) throw new ObjectDisposedException(GetType().Name, "COM object is null");
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected static void ThrowIfNull(void* ptr)
		{
			if (ptr is null) throw new ArgumentNullException(nameof(ptr), "COM object is null");
		}

		private static class ComFactory<T> where T : Unknown
		{
			internal delegate T Ctor(void* raw, bool addRef);

			internal static readonly Ctor Create = Build();

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static Ctor Build()
			{
				var ci = typeof(T).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null, [typeof(void*), typeof(bool)], null) ?? throw new MissingMethodException(typeof(T).FullName, ".ctor(void*, bool)");
				var dm = new DynamicMethod(name: $"{typeof(T).Name}__ctor_ptr_bool_factory", typeof(T), [typeof(void*), typeof(bool)], typeof(T), true);
				var il = dm.GetILGenerator();
				il.Emit(OpCodes.Ldarg_0);
				il.Emit(OpCodes.Ldarg_1);
				il.Emit(OpCodes.Newobj, ci);
				il.Emit(OpCodes.Ret);
				return (Ctor)dm.CreateDelegate(typeof(Ctor));
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected virtual void Dispose(bool disposing)
		{
			// Release cached identity (we AddRef’d it implicitly via QI)
			if (_identity != null && _identity != Raw)
			{
				_ = Unmanaged.Release(_identity);
				_identity = null;
			}

			if (Raw != null)
			{
				Release();
				Raw = null;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		~Unknown() => Dispose(false);
	}
}