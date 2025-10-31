using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Vyrse.Core
{
	namespace Lifecycle
	{
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

		public interface IPhase
		{
			void Load() { }
			void Initialize() { }
			void Start() { }
			void PreUpdate() { }
			void Update() { }
			void PostUpdate() { }
			void FixedUpdate() { }
			void Shutdown() { }

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
				var iface = typeof(IPhase);
				var ifaceMethods = iface.GetMethods(BindingFlags.Public | BindingFlags.Instance);
				var nameToIndex = new Dictionary<string, int>(ifaceMethods.Length);
				for (int i = 0; i < Names.Length; i++) nameToIndex[Names[i]] = i;
				var map = type.GetInterfaceMap(iface);

				Phase mask = default;
				for (int i = 0; i < map.InterfaceMethods.Length; i++)
				{
					var ifaceMethod = map.InterfaceMethods[i];
					var target = map.TargetMethods[i];
					if (!nameToIndex.TryGetValue(ifaceMethod.Name, out var bit)) continue;
					if (target.DeclaringType == type)
						mask |= (Phase)(1 << bit);
				}

				return mask;
			}
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
				thunk.IsNull.ThrowIfTrue();
				Values[Count++] = thunk;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void SetAt(int slot, Thunk thunk)
			{
				thunk.IsNull.ThrowIfTrue();
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

		public readonly unsafe struct Thunk(delegate* managed<object, void> fn, object instance, Phase phase)
		{
			public readonly Phase Phase = phase;
			public readonly object Instance = instance;
			public readonly delegate* managed<object, void> Fn = fn;

			public Thunk(delegate* managed<object, void> fn, Phase phase) : this(fn, null!, phase) { }
			public Thunk(Thunk other) : this(other.Fn, other.Instance, other.Phase) { }
			public Thunk(object instance, Thunk other) : this(other.Fn, instance, other.Phase) { }

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Invoke() => Fn(Instance);

			public bool IsNull => Fn == null;
		}
	}
}