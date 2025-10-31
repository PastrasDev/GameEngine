using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace Vyrse.Collections;


public abstract class Queue
{
	public static class Policies
	{
		public enum Capacity
		{
			Grow,
			DropNewest,
			DropOldest
		}
	}
}

public abstract class Queue<T> : Queue
{
	public sealed class DoubleBuffered : IDisposable
	{
		private T[] _a;
		private T[] _b;
		private T[] _write;
		private T[] _read;
		private readonly Policies.Capacity _policy;
		private readonly bool _clearOnReturn;

		public int LastFrameCount { get; private set; }
		public int WriteCount { get; private set; }
		public int Dropped { get; private set; }
		public int Capacity => _write.Length;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public DoubleBuffered(int initialCapacity = 1024, Policies.Capacity policy = Policies.Capacity.Grow, bool clearArraysOnPoolReturn = false)
		{
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(initialCapacity);
            _policy = policy;
			_clearOnReturn = clearArraysOnPoolReturn || RuntimeHelpers.IsReferenceOrContainsReferences<T>();

			_a = ArrayPool<T>.Shared.Rent(initialCapacity);
			_b = ArrayPool<T>.Shared.Rent(initialCapacity);
			_write = _a;
			_read = _b;
			WriteCount = 0;
			LastFrameCount = 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Push(in T item)
		{
			if (WriteCount == _write.Length)
			{
				switch (_policy)
				{
					case Policies.Capacity.Grow:
						Grow(_write.Length * 2);
						break;

					case Policies.Capacity.DropNewest:
						Dropped++;
						return false;

					case Policies.Capacity.DropOldest:
						Array.Copy(_write, 1, _write, 0, WriteCount - 1);
						WriteCount--;
						break;
				}
			}

			_write[WriteCount++] = item;
			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ReadOnlySpan<T> SwapAndRead()
		{
			LastFrameCount = WriteCount;
			(_read, _write) = (_write, _read);
			WriteCount = 0;
			return new ReadOnlySpan<T>(_read, 0, LastFrameCount);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void EnsureCapacity(int minCapacity)
		{
			if (minCapacity <= 0) return;
			if (_write.Length < minCapacity)
				Grow(RoundUpPowerOfTwo(minCapacity));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void TrimExcess(int targetCapacity)
		{
			if (targetCapacity <= 0) return;
			if (_write.Length > targetCapacity) _write = Resize(_write, WriteCount, targetCapacity);
			if (_read.Length > targetCapacity) _read = Resize(_read, LastFrameCount, targetCapacity);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void ClearWrite() => WriteCount = 0;

		public void Dispose()
		{
			ArrayPool<T>.Shared.Return(_a, _clearOnReturn);
			ArrayPool<T>.Shared.Return(_b, _clearOnReturn);
			_a = _b = _write = _read = [];
			WriteCount = LastFrameCount = 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void Grow(int newCapacity)
		{
			var bigger = ArrayPool<T>.Shared.Rent(newCapacity);
			_write.AsSpan(0, WriteCount).CopyTo(bigger);
			ArrayPool<T>.Shared.Return(_write, _clearOnReturn);
			_write = bigger;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static int RoundUpPowerOfTwo(int x)
		{
			if (x < 2) return 2;
			x--;
			x |= x >> 1;
			x |= x >> 2;
			x |= x >> 4;
			x |= x >> 8;
			x |= x >> 16;
			return x + 1;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private T[] Resize(T[] src, int count, int newCapacity)
		{
			var dst = ArrayPool<T>.Shared.Rent(newCapacity);
			var toCopy = Math.Min(count, newCapacity);
			src.AsSpan(0, toCopy).CopyTo(dst);
			ArrayPool<T>.Shared.Return(src, _clearOnReturn);
			if (ReferenceEquals(src, _write)) WriteCount = toCopy;
			if (ReferenceEquals(src, _read)) LastFrameCount = toCopy;
			return dst;
		}
	}
}