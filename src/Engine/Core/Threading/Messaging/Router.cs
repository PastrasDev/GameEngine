using System.Runtime.CompilerServices;
using Engine.Core.Threading.Messaging.Policies;

namespace Engine.Core.Threading.Messaging
{
	namespace Messages
	{
		public readonly record struct InputSnapshot
		{
			public required long FrameId { get; init; }
			public required long Timestamp { get; init; }
			public required Engine.Platform.Windows.Keyboard.FrameState KState { get; init; }
			public required Engine.Platform.Windows.Mouse.FrameState MState { get; init; }
		}

		public readonly record struct SceneView
		{
			public required long FrameIndex { get; init; }
			public required float Alpha { get; init; }
		}
	}

	namespace Policies
	{
		public enum WaitMode
		{
			None,
			Spin,
			Hybrid
		}

		public enum FullMode
		{
			None,
			Wait,
			DropWrite
		}

		[Flags]
		public enum DropMode
		{
			None = 0,
			DropOldest = 1 << 0,
			DropNewest = 1 << 1,
			NoConsumer = 1 << 2,
			Deduplicate = 1 << 3,
		}
	}

	public static class Settings
	{
		public readonly struct Snapshot()
		{
			public required DropMode DropMode { get; init; } = DropMode.NoConsumer | DropMode.Deduplicate;
			public required bool BlockUntilFirstPublish { get; init; } = true;
		}

		public readonly struct Stream()
		{
			public required int CapacityPow2 { get; init; } = 1024;
			public required WaitMode WaitMode { get; init; } = WaitMode.Hybrid;
			public required FullMode FullMode { get; init; } = FullMode.Wait;
			public required DropMode DropMode { get; init; } = DropMode.NoConsumer;
		}
	}

	public readonly record struct Snapshot<TData, TTarget>()
	{
		public required TData Data { get; init; }
		public required TTarget Target { get; init; }
		public required Settings.Snapshot Settings { get; init; }
	}

	public readonly record struct Stream<TData, TTarget>
	{
		public required TData Data { get; init; }
		public required TTarget Target { get; init; }
		public required Settings.Stream Settings { get; init; }
	}

	public interface IMessaging
	{
		static abstract void Send<TData, TTarget>(in Snapshot<TData, TTarget> snapshot);
		static abstract void Send<TData, TTarget>(in Stream<TData, TTarget> stream);
		static abstract TData Receive<TData, TSender>() where TData : notnull where TSender : class;
		static abstract bool TryReceive<TData, TSender>(out TData? data) where TData : notnull where TSender : class;
	}

	public sealed class Router<TData, TSender, TTarget>
	{
		private static readonly object s_lock = new();

		public sealed class Snapshot
		{
			private static Snapshot<TData>.Buffer? s_snapshot;
			private static volatile bool s_snapConfigured;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static void EnsureConfigured(in Settings.Snapshot s)
			{
				if (s_snapConfigured) return;
				lock (s_lock)
				{
					if (s_snapConfigured) return;
					var snap = new Snapshot<TData>.Buffer();
					snap.Configure(s);
					s_snapshot = snap;
					s_snapConfigured = true;
				}
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void Publish(in TData data, in Settings.Snapshot settings)
			{
				EnsureConfigured(settings);
				s_snapshot!.Publish(in data);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static (TData value, int version) Read()
			{
				var snap = s_snapshot;
				if (snap is null) return (default!, 0);
				return snap.Read();
			}
		}

		public sealed class Stream
		{
			private static Ring<TData>.SPSC? s_pipe;
			private static volatile bool s_pipeConfigured;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static void EnsureConfigured(in Settings.Stream s)
			{
				if (s_pipeConfigured) return;
				lock (s_lock)
				{
					if (s_pipeConfigured) return;
					int cap = s.CapacityPow2 <= 0 ? 1024 : s.CapacityPow2;
					s_pipe = new Ring<TData>.SPSC(cap, s.FullMode, s.WaitMode, s.DropMode);
					s_pipeConfigured = true;
				}
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void Write(in TData data, in Settings.Stream settings, CancellationToken ct)
			{
				EnsureConfigured(settings);
				s_pipe!.Write(in data, ct);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool TryRead(out TData item)
			{
				var p = s_pipe;
				if (p is null)
				{
					item = default!;
					return false;
				}

				return p.TryRead(out item);
			}
		}
	}

	internal sealed class Snapshot<T>
	{
		internal sealed class Buffer
		{
			private T _a = default!;
			private T _b = default!;
			private int _which;   // 0 or 1
			private int _version; // increments on publish
			private volatile bool _hadReader;
			private volatile bool _hadFirstPublish;

			private DropMode _dropMode;
			private bool _blockUntilFirstPublish;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Configure(Settings.Snapshot s)
			{
				_dropMode = s.DropMode;
				_blockUntilFirstPublish = s.BlockUntilFirstPublish;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void MarkReaderSeen() => _hadReader = true;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Publish(in T value)
			{
				// Drop if NoConsumer requested and no reader yet
				if ((_dropMode & DropMode.NoConsumer) != 0 && !_hadReader)
					return;

				// Dedup: if equal to current, skip
				if ((_dropMode & DropMode.Deduplicate) != 0)
				{
					var (cur, _) = Read();
					if (EqualityComparer<T>.Default.Equals(cur, value))
					{
						_hadFirstPublish = true;
						return;
					}
				}

				int next = Volatile.Read(ref _which) ^ 1;
				if (next == 0) _a = value;
				else _b = value;
				Volatile.Write(ref _which, next);
				Interlocked.Increment(ref _version);
				_hadFirstPublish = true;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public (T value, int version) Read()
			{
				MarkReaderSeen();

				if (_blockUntilFirstPublish)
				{
					var spin = new SpinWait();
					while (!_hadFirstPublish) spin.SpinOnce();
				}

				while (true)
				{
					var v1 = Volatile.Read(ref _version);
					var w = Volatile.Read(ref _which);
					var v = (w == 0) ? _a : _b;
					var v2 = Volatile.Read(ref _version);
					if (v1 == v2) return (v, v2);
				}
			}
		}
	}


	public sealed class Ring<T>
	{
		public sealed class SPSC
		{
			private readonly T[] _buf;
			private readonly int _mask;
			private readonly FullMode _full;
			private readonly WaitMode _wait;
			private readonly DropMode _drop;

			private long _head; // next to read
			private long _tail; // next to write
			private volatile bool _hadReader;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public SPSC(int capacityPow2, FullMode full, WaitMode wait, DropMode drop)
			{
				if (capacityPow2 <= 0 || (capacityPow2 & (capacityPow2 - 1)) != 0)
					throw new ArgumentException("capacity must be a power of two", nameof(capacityPow2));

				_buf = new T[capacityPow2];
				_mask = capacityPow2 - 1;
				_full = full;
				_wait = wait;
				_drop = drop;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static bool IsFull(long head, long tail, int mask) => (tail - head) > mask;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void MarkReaderSeen() => _hadReader = true;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool TryWrite(in T item)
			{
				// Optional: drop when there is not yet any consumer
				if ((_drop & DropMode.NoConsumer) != 0 && !_hadReader)
					return false;

				var head = Volatile.Read(ref _head);
				var tail = Volatile.Read(ref _tail);

				if (IsFull(head, tail, _mask))
				{
					if (_full == FullMode.DropWrite || (_drop & DropMode.DropNewest) != 0)
						return false;
					return false;
				}

				_buf[(int)(tail & _mask)] = item;
				Volatile.Write(ref _tail, tail + 1);
				return true;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Write(in T item, CancellationToken ct)
			{
				if (TryWrite(item)) return;

				if (_full == FullMode.DropWrite || (_drop & DropMode.DropNewest) != 0)
					return;

				var spin = new SpinWait();
				while (true)
				{
					var head = Volatile.Read(ref _head);
					var tail = Volatile.Read(ref _tail);
					if (!IsFull(head, tail, _mask))
					{
						_buf[(int)(tail & _mask)] = item;
						Volatile.Write(ref _tail, tail + 1);
						return;
					}

					ct.ThrowIfCancellationRequested();
					if (_wait == WaitMode.Spin || (_wait == WaitMode.Hybrid && spin.Count < 20))
						spin.SpinOnce();
					else
						Thread.Yield();
				}
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool TryRead(out T item)
			{
				MarkReaderSeen();

				var head = Volatile.Read(ref _head);
				var tail = Volatile.Read(ref _tail);
				if (head >= tail)
				{
					item = default!;
					return false;
				}

				int idx = (int)(head & _mask);
				item = _buf[idx];
				_buf[idx] = default!;
				Volatile.Write(ref _head, head + 1);
				return true;
			}
		}

		public sealed class MPSC;

		public sealed class SPMC;

		public sealed class MPMC;
	}
}