using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
//using Engine.Core.Threading.Messaging.Messages;

namespace Engine.Core.Threading.Messaging
{
	/*public enum ChannelType
	{
		None,
		Pipe,
		Snapshot,
		Control
	}

	public enum PipeFullMode
	{
		/// Producer waits until space exists (Write spins/yields).
		Wait,

		/// Producer fails fast when full (TryWrite false; Write spins/yields until space).
		DropWrite
	}

	namespace Messages
	{
		public readonly record struct InputSnapshot : IThreadMessage<InputSnapshot>
		{
			public required long FrameId { get; init; }
			public required long Timestamp { get; init; }
			public required Engine.Platform.Windows.Keyboard.FrameState KState { get; init; }
			public required Engine.Platform.Windows.Mouse.FrameState MState { get; init; }

			public static string Channel => nameof(InputSnapshot);
			public static ChannelType ChannelType => ChannelType.Pipe;
			public static int CapacityPow2 => 1024;
			public static PipeFullMode FullMode => PipeFullMode.DropWrite;
		}

		public readonly record struct SceneView : IThreadMessage<SceneView>
		{
			public required long FrameIndex { get; init; }
			public required float Alpha { get; init; }

			public static string Channel => nameof(SceneView);
			public static ChannelType ChannelType => ChannelType.Snapshot;
		}

		public readonly record struct RenderControl : IThreadMessage<RenderControl>
		{
			public required ControlType Type { get; init; }
			public required int Width { get; init; }
			public required int Height { get; init; }

			public static string Channel => nameof(RenderControl);
			public static ChannelType ChannelType => ChannelType.Control;
			public static int ControlKey(in RenderControl m) => (int)m.Type;

			public enum ControlType
			{
				Resize,
				ToggleVsync,
				ToggleFullscreen
			}
		}
	}

	public enum WaitStrategy
	{
		/// Tight spinWait
		Spin,

		/// Spin then Yield
		Hybrid
	}

	public interface IThreadMessage<T> where T : struct
	{
		static abstract string Channel { get; }

		static abstract ChannelType ChannelType { get; }

		static virtual int CapacityPow2 => 256;
		static virtual PipeFullMode FullMode => PipeFullMode.Wait;
		static virtual WaitStrategy WaitStrategy => WaitStrategy.Hybrid;

		static virtual int ControlKey(in T msg) => msg.GetHashCode();
	}

	public sealed class MessageBus
	{
		private readonly ConcurrentDictionary<string, object> _pipes = new();
		private readonly ConcurrentDictionary<string, object> _snapshots = new();
		private readonly ConcurrentDictionary<string, object> _controls = new();

		public Endpoint<T> Claim<T>() where T : struct, IThreadMessage<T>
		{
			var name = T.Channel;
			switch (T.ChannelType)
			{
				case ChannelType.Pipe:
				{
					var pipe = (IPipe<T>)_pipes.GetOrAdd(name, _ => new SpscRing<T>(T.CapacityPow2, T.FullMode, T.WaitStrategy));
					var port = new PipePort<T>(pipe);
					return Endpoint<T>.Pipe(port, port);
				}
				case ChannelType.Snapshot:
				{
					var snap = (Snapshot<T>)_snapshots.GetOrAdd(name, _ => new Snapshot<T>());
					var port = new SnapshotPort<T>(snap);
					return Endpoint<T>.Snapshot(port, port);
				}
				case ChannelType.Control:
				{
					var q = (ControlQueue<T>)_controls.GetOrAdd(name, _ => new ControlQueue<T>(static m => T.ControlKey(in m), 64));
					return Endpoint<T>.Ctrl(new ControlBusPort<T>(q));
				}

				default: throw new NotSupportedException();
			}
		}
	}

	public readonly struct Endpoint<T> where T : struct
	{
		public IReadPort<T>? Read { get; }
		public IWritePort<T>? Write { get; }
		public ISnapshotRead<T>? SnapshotRead { get; }
		public ISnapshotWrite<T>? SnapshotWrite { get; }
		public IControlBus<T>? Control { get; }

		private Endpoint(IReadPort<T>? r, IWritePort<T>? w, ISnapshotRead<T>? sr, ISnapshotWrite<T>? sw, IControlBus<T>? c) => (Read, Write, SnapshotRead, SnapshotWrite, Control) = (r, w, sr, sw, c);

		public static Endpoint<T> Pipe(IReadPort<T> r, IWritePort<T> w) => new(r, w, null, null, null);
		public static Endpoint<T> Snapshot(ISnapshotRead<T> r, ISnapshotWrite<T> w) => new(null, null, r, w, null);
		public static Endpoint<T> Ctrl(IControlBus<T> c) => new(null, null, null, null, c);
	}

	public interface IWritePort<T>
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		bool TryWrite(in T item);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		void Write(in T item, CancellationToken ct);
	}

	public interface IReadPort<T>
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		bool TryRead(out T item);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		T Read(CancellationToken ct);
	}

	public interface ISnapshotWrite<T>
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		void Publish(in T value);
	}

	public interface ISnapshotRead<T>
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		(T value, int version) Read();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		int WaitForChange(int lastVersion, CancellationToken ct);
	}

	public interface IControlBus<T>
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		void Post(in T message);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		void Drain(Action<T> apply);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		void Clear();
	}

	public interface IRenderCommandBus : IControlBus<RenderControl>;

	/// <summary>
	/// Minimal SPSC pipe interface. Single-Producer / Single-Consumer only.
	/// </summary>
	public interface IPipe<T> : IDisposable
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		bool TryWrite(in T item);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		void Write(in T item, CancellationToken ct);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		bool TryRead(out T item);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		T Read(CancellationToken ct);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		void Complete();

		bool IsCompleted { get; }
	}

	public sealed class PipePort<T>(IPipe<T> pipe) : IReadPort<T>, IWritePort<T>
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool TryWrite(in T item) => pipe.TryWrite(item);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Write(in T item, CancellationToken ct) => pipe.Write(item, ct);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool TryRead(out T item) => pipe.TryRead(out item);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T Read(CancellationToken ct) => pipe.Read(ct);
	}

	public sealed class SnapshotPort<T>(Snapshot<T> snap) : ISnapshotRead<T>, ISnapshotWrite<T>
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Publish(in T value) => snap.Publish(value);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public (T value, int version) Read() => snap.Read();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int WaitForChange(int lastVersion, CancellationToken ct) => snap.WaitForChange(lastVersion, ct);
	}

	public class ControlBusPort<T>(ControlQueue<T> q) : IControlBus<T>
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Post(in T message) => q.Post(message);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Drain(Action<T> apply) => q.Drain(apply);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Clear() => q.Clear();
	}

	public sealed class RenderCommandBusPort(ControlQueue<RenderControl> q) : ControlBusPort<RenderControl>(q), IRenderCommandBus;

	/// <summary>
	/// High-performance SPSC ring buffer with power-of-two capacity.
	/// Safe for exactly one producer thread and one consumer thread.
	/// </summary>
	public sealed class SpscRing<T> : IPipe<T>
	{
		private readonly T[] _buffer;
		private readonly int _mask; // capacity - 1
		private readonly PipeFullMode _fullMode;
		private readonly WaitStrategy _waitStrategy;

		/// next index to read
		private long _head;

		/// next index to write
		private long _tail;

		private volatile bool _completed;

		public bool IsCompleted => _completed;
		public int Capacity { get; }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public SpscRing(int capacityPow2 = 256, PipeFullMode fullMode = PipeFullMode.Wait, WaitStrategy waitStrategy = WaitStrategy.Hybrid)
		{
			if (capacityPow2 <= 0 || (capacityPow2 & (capacityPow2 - 1)) != 0)
				throw new ArgumentException("capacity must be a power of two and > 0", nameof(capacityPow2));

			Capacity = capacityPow2;
			_buffer = new T[capacityPow2];
			_mask = capacityPow2 - 1;
			_fullMode = fullMode;
			_waitStrategy = waitStrategy;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool IsFull(long head, long tail, int mask) => (tail - head) > mask;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool TryWrite(in T item)
		{
			if (_completed) return false;
			var head = Volatile.Read(ref _head);
			var tail = Volatile.Read(ref _tail);
			if (IsFull(head, tail, _mask))
			{
				return _fullMode == PipeFullMode.DropWrite && false;
			}

			_buffer[(int)(tail & _mask)] = item;
			Volatile.Write(ref _tail, tail + 1);
			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Write(in T item, CancellationToken ct)
		{
			var spin = new SpinWait();
			while (true)
			{
				if (TryWrite(item)) return;
				if (_completed) throw new InvalidOperationException("Pipe has been completed");
				ct.ThrowIfCancellationRequested();
				SpinOnce(ref spin);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool TryRead(out T item)
		{
			var head = Volatile.Read(ref _head);
			var tail = Volatile.Read(ref _tail);
			if (head >= tail)
			{
				item = default!;
				return false;
			}

			int idx = (int)(head & _mask);
			item = _buffer[idx];
			_buffer[idx] = default!;
			Volatile.Write(ref _head, head + 1);
			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T Read(CancellationToken ct)
		{
			var spin = new SpinWait();
			while (true)
			{
				if (TryRead(out var item)) return item;
				if (_completed) throw new OperationCanceledException("Pipe completed", ct);
				ct.ThrowIfCancellationRequested();
				SpinOnce(ref spin);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Complete() => _completed = true;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Dispose()
		{
			Complete();
			GC.SuppressFinalize(this);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void SpinOnce(ref SpinWait spin)
		{
			if (_waitStrategy == WaitStrategy.Spin)
			{
				spin.SpinOnce();
				return;
			}

			if (spin.Count < 20) spin.SpinOnce();
			else Thread.Yield();
		}
	}

	/// <summary>
	/// Latest-wins snapshot. No queue. Producer publishes, consumer reads the freshest value.
	/// Use this for Game → Render views, HUD data, etc.
	/// </summary>
	public sealed class Snapshot<T>
	{
		private T _a = default!;
		private T _b = default!;
		private int _which;   // 0 => _a, 1 => _b
		private int _version; // incremented on publish

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Publish(in T value)
		{
			int next = Volatile.Read(ref _which) ^ 1; // flip slot
			if (next == 0) _a = value;
			else _b = value;
			Volatile.Write(ref _which, next);
			Interlocked.Increment(ref _version);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public (T value, int version) Read()
		{
			while (true)
			{
				var v1 = Volatile.Read(ref _version);
				var w = Volatile.Read(ref _which);
				var v = (w == 0) ? _a : _b;
				var v2 = Volatile.Read(ref _version);
				if (v1 == v2) return (v, v2);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int WaitForChange(int lastVersion, CancellationToken ct)
		{
			var spin = new SpinWait();
			while (Volatile.Read(ref _version) == lastVersion)
			{
				ct.ThrowIfCancellationRequested();
				spin.SpinOnce();
			}

			return Volatile.Read(ref _version);
		}
	}

	/// <summary>
	/// Coalescing control queue for side-band commands (e.g., resize, vsync toggle).
	/// Multiple posts of the same key overwrite the previous one. Drain once per frame.
	/// Thread-safe for many producers, one consumer.
	/// </summary>
	public sealed class ControlQueue<T>(Func<T, int> keySelector, int? maxItems = null)
	{
		private readonly ConcurrentDictionary<int, T> _latest = new();
		private readonly Func<T, int> _keyOf = keySelector ?? throw new ArgumentNullException(nameof(keySelector));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Post(in T message)
		{
			var key = _keyOf(message);

			// Unbounded: just coalesce
			if (maxItems is not { } cap)
			{
				_latest[key] = message;
				return;
			}

			// If the key already exists, always coalesce (do NOT drop).
			if (_latest.ContainsKey(key))
			{
				_latest[key] = message;
				return;
			}

			// New key: enforce capacity.
			if (_latest.Count >= cap)
				return; // drop newest when saturated

			if (!_latest.TryAdd(key, message))
			{
				_latest[key] = message;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Drain(Action<T> apply)
		{
			foreach (var kvp in _latest)
			{
				if (_latest.TryRemove(kvp.Key, out var msg))
					apply(msg);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Clear() => _latest.Clear();
	}*/
}