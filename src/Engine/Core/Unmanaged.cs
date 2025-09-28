using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace Engine.Core
{
	namespace Unmanaged
	{
		[MustDisposeResource]
		[CollectionBuilder(typeof(ArrayBuilder), nameof(ArrayBuilder.Create))]
		public sealed unsafe class Array<T> : IDisposable, IEnumerable<T> where T : unmanaged
		{
			public static Array<T> Empty { get; } = new(0, owns: false);

			public T* Ptr { get; private set; }
			public int Length { get; private set; }
			private bool _owns;

			public Array(int length, bool zeroInit = false) : this(length, owns: true, zeroInit: zeroInit) { }

			private Array(int length, bool owns, bool zeroInit = false)
			{
				Length = length;
				_owns = owns;
				if (length == 0)
				{
					Ptr = null;
					return;
				}

				nuint elem = (nuint)Unsafe.SizeOf<T>();
				Ptr = zeroInit ? (T*)NativeMemory.AllocZeroed((nuint)length, elem) : (T*)NativeMemory.Alloc((nuint)length, elem);
			}

			#if DEBUG
			~Array()
			{
				if (_owns && Ptr != null) NativeMemory.Free(Ptr);
			}
			#endif

			public ref T this[int index]
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					#if DEBUG
					if ((uint)index >= (uint)Length) throw new IndexOutOfRangeException();
					#endif
					return ref Ptr[index];
				}
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public Span<T> AsSpan() => new(Ptr, Length);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public ReadOnlySpan<T> AsReadOnlySpan() => new(Ptr, Length);

			[MustDisposeResource]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static Array<T> Allocate(int length, bool zeroInit = false) => length <= 0 ? Empty : new Array<T>(length, zeroInit);

			[MustDisposeResource]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static Array<T> From(ReadOnlySpan<T> src)
			{
				if (src.Length == 0) return Empty;
				var arr = new Array<T>(src.Length);
				ref readonly T s0 = ref MemoryMarshal.GetReference(src);
				void* s = Unsafe.AsPointer(ref Unsafe.AsRef(in s0));
				nuint bytes = (nuint)(src.Length * Unsafe.SizeOf<T>());
				Buffer.MemoryCopy(s, arr.Ptr, bytes, bytes);
				return arr;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public T[] ToArray()
			{
				if (Length == 0) return [];
				var a = new T[Length];
				AsReadOnlySpan().CopyTo(a);
				return a;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Clear() => AsSpan().Clear();

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Fill(T value) => AsSpan().Fill(value);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void CopyFrom(ReadOnlySpan<T> src)
			{
				if (src.Length != Length) throw new ArgumentException("Source length must match.", nameof(src));
				src.CopyTo(AsSpan());
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool TryCopyFrom(ReadOnlySpan<T> src)
			{
				if (src.Length != Length) return false;
				src.CopyTo(AsSpan());
				return true;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void CopyTo(Span<T> dst)
			{
				if (dst.Length != Length) throw new ArgumentException("Destination length must match.", nameof(dst));
				AsReadOnlySpan().CopyTo(dst);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool TryCopyTo(Span<T> dst)
			{
				if (dst.Length != Length) return false;
				AsReadOnlySpan().CopyTo(dst);
				return true;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Reallocate(int newLength, bool zeroNew = false, bool preserve = true)
			{
				ArgumentOutOfRangeException.ThrowIfNegative(newLength);
				if (newLength == Length) return;

				if (newLength == 0)
				{
					Dispose();
					Ptr = null;
					Length = 0;
					_owns = false;
					return;
				}

				int size = Unsafe.SizeOf<T>();
				T* newPtr = zeroNew ? (T*)NativeMemory.AllocZeroed((nuint)newLength, (nuint)size) : (T*)NativeMemory.Alloc((nuint)newLength, (nuint)size);

				if (preserve && Ptr != null && Length > 0)
				{
					int copy = Math.Min(Length, newLength);
					nuint bytes = (nuint)(copy * size);
					Buffer.MemoryCopy(Ptr, newPtr, bytes, bytes);
				}

				if (_owns && Ptr != null) NativeMemory.Free(Ptr);
				Ptr = newPtr;
				Length = newLength;
				_owns = true;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Append(ReadOnlySpan<T> tail)
			{
				if (tail.IsEmpty) return;
				int oldLen = Length;
				Reallocate(oldLen + tail.Length, zeroNew: false, preserve: true);
				tail.CopyTo(new Span<T>(Ptr + oldLen, tail.Length));
			}


			[MustDisposeResource]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public Array<T> Clone()
			{
				if (Length == 0) return Empty;
				var copy = new Array<T>(Length);
				nuint bytes = (nuint)(Length * Unsafe.SizeOf<T>());
				Buffer.MemoryCopy(Ptr, copy.Ptr, bytes, bytes);
				return copy;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public Enumerator GetEnumerator() => new(Ptr, Length);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			IEnumerator<T> IEnumerable<T>.GetEnumerator() => new Enumerator(Ptr, Length);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			IEnumerator IEnumerable.GetEnumerator() => new Enumerator(Ptr, Length);

			public struct Enumerator(T* ptr, int len) : IEnumerator<T>
			{
				private int _i = -1;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public bool MoveNext() => ++_i < len;

				public readonly T Current => ptr[_i];
				readonly object IEnumerator.Current => Current;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void Reset() => _i = -1;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void Dispose() { }
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Dispose()
			{
				if (!_owns || Ptr is null) return;
				NativeMemory.Free(Ptr);
				Ptr = null;
				Length = 0;
				_owns = false;
				GC.SuppressFinalize(this);
			}
		}

		public static class ArrayBuilder
		{
			[MustDisposeResource]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static Array<T> Create<T>(ReadOnlySpan<T> items) where T : unmanaged => Array<T>.From(items);
		}
	}
}