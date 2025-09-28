using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Engine.Core
{
	namespace Lifecycle
	{
		internal interface IPhases
		{
			internal void Load();
			internal void Initialize();
			internal void Start();
			internal void FixedUpdate();
			internal void PreUpdate();
			internal void Update();
			internal void PostUpdate();
			internal void Shutdown();

			public static int Length => Names.Length;

			public static readonly string[] Names =
			[
				nameof(Load),
				nameof(Initialize),
				nameof(Start),
				nameof(FixedUpdate),
				nameof(PreUpdate),
				nameof(Update),
				nameof(PostUpdate),
				nameof(Shutdown)
			];

			public static Phase ComputeMask(Type type, BindingFlags flags)
			{
				Phase mask = default;
				var names = Names;

				for (int i = 0; i < Names.Length; i++)
				{
					var name = names[i];
					var m = type.GetMethod(name, flags, null, Type.EmptyTypes, null);
					if (m is null) continue;
					if (!m.IsVirtual) continue;
					if (m.DeclaringType != type) continue;
					mask |= (Phase)(1 << i);
				}

				return mask;
			}
		}

		public interface IThunks<TSelf>
		{
			static abstract void _Load(object o);
			static abstract void _Initialize(object o);
			static abstract void _Start(object o);
			static abstract void _FixedUpdate(object o);
			static abstract void _PreUpdate(object o);
			static abstract void _Update(object o);
			static abstract void _PostUpdate(object o);
			static abstract void _Shutdown(object o);

			static abstract Thunk[] Thunks { get; }

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			static TSelf Get(object self) => (TSelf)self;
		}

		public interface IConfigure
		{
			static abstract void Configure();
		}

		[Flags]
		public enum Phase
		{
			None = 0,
			Load = 1 << 0,
			Initialize = 1 << 1,
			Start = 1 << 2,
			FixedUpdate = 1 << 3,
			PreUpdate = 1 << 4,
			Update = 1 << 5,
			PostUpdate = 1 << 6,
			Shutdown = 1 << 7,
		}

		[CollectionBuilder(typeof(Builder), nameof(Builder.Create))]
		public struct Thunks(int capacity) : IEnumerable<Thunk>
		{
			public Thunk[] Values { get; private set; } = new Thunk[capacity];
			public int Count { get; private set; } = 0;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Clear() => Count = 0;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Resize(int capacity)
			{
				Values = new Thunk[capacity];
				Count = 0;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Add(Thunk thunk)
			{
				require(thunk.IsNull, false);
				Values[Count++] = thunk;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void SetAt(int slot, Thunk thunk)
			{
				require(thunk.IsNull, false);
				Values[slot] = thunk;
				if (slot >= Count) Count = slot + 1;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public readonly void Reverse() => Array.Reverse(Values);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static Thunks From(ReadOnlySpan<Thunk> src)
			{
				var set = new Thunks(src.Length);
				for (int i = 0; i < src.Length; i++) set.Values[i] = src[i];
				set.Count = src.Length;
				return set;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public readonly void Invoke()
			{
				var thunks = Values;
				for (int i = 0, n = Count; i < n; i++)
					thunks[i].Invoke();
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public readonly Enumerator GetEnumerator() => new(Values, Count);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly IEnumerator<Thunk> IEnumerable<Thunk>.GetEnumerator() => new Enumerator(Values, Count);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			readonly IEnumerator IEnumerable.GetEnumerator() => new Enumerator(Values, Count);

			[method: MethodImpl(MethodImplOptions.AggressiveInlining)]
			public struct Enumerator(IReadOnlyList<Thunk> entries, int count) : IEnumerator<Thunk>
			{
				private int _i = -1;

				public readonly Thunk Current => entries[_i];
				readonly object IEnumerator.Current => Current;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public bool MoveNext() => ++_i < count;

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public void Reset() => _i = -1;

				public void Dispose() { }
			}

			public static class Builder
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public static Thunks Create(ReadOnlySpan<Thunk> items) => From(items);
			}
		}

		public unsafe struct Thunk
		{
			public readonly Phase Phase;
			public object Instance;
			public readonly delegate* managed<object, void> Fn;

			public Thunk(delegate* managed<object, void> fn, object instance, Phase phase)
			{
				Fn = fn;
				Instance = instance;
				Phase = phase;
			}

			public Thunk(delegate* managed<object, void> fn, Phase phase)
			{
				Fn = fn;
				Instance = null!;
				Phase = phase;
			}

			public Thunk(Thunk other)
			{
				Fn = other.Fn;
				Instance = other.Instance;
				Phase = other.Phase;
			}

			public Thunk(object instance, Thunk other)
			{
				Fn = other.Fn;
				Instance = instance;
				Phase = other.Phase;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public readonly void Invoke() => Fn(Instance);

			public readonly bool IsNull => Fn == null;
		}
	}
}