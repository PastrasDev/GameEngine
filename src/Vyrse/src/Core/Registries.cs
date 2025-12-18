using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Vyrse.Core.Abstract;
using static Math;

namespace Vyrse.Core
{
	namespace Registries
	{
		public sealed class Registry : Registry<Registry, object>;
	}

	namespace Abstract
	{
		public abstract class Registry;

		public abstract class Registry<TSelf, TItem> : Registry where TSelf : Registry<TSelf, TItem> where TItem : notnull
		{
			[ThreadStatic] private static Segment? _tSection;
			[ThreadStatic] private static bool _tUseGlobalOnce;
			private static readonly Segment SShared = new();

			public Registry<TSelf, TItem> Global
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				get
				{
					_tUseGlobalOnce = true;
					return this;
				}
			}

			// ===================== Scoped API =====================

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool Add<T>(T service) where T : TItem => Add(typeof(T), service);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool Replace<T>(T service) where T : TItem => Replace(typeof(T), service);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool Remove<T>() where T : TItem => Remove(typeof(T));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T Get<T>() where T : TItem => (T)Get(typeof(T));

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool Add(Type key, object service)
            {
	            key.ThrowIfNull();
	            service.ThrowIfNull();
	            if (service is not TItem)
		            throw new ArgumentException($"Service must be {typeof(TItem).FullName}");
	            var (_, seg) = TargetSegment();
	            return seg.Add(key, service);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool Replace(Type key, object service)
            {
	            key.ThrowIfNull();
	            service.ThrowIfNull();
	            if (service is not TItem)
		            throw new ArgumentException($"Service must be {typeof(TItem).FullName}");
	            var (_, seg) = TargetSegment();
	            return seg.Replace(key, service);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool Remove(Type key)
            {
	            key.ThrowIfNull();
	            var (_, seg) = TargetSegment();
	            return seg.Remove(key);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public object Get(Type key, [CallerArgumentExpression("key")] string? expr = null, [CallerFilePath] string file = "", [CallerLineNumber] int line = 0, [CallerMemberName] string member = "")
            {
	            key.ThrowIfNull();
	            var (isGlobal, seg) = TargetSegment();
	            if (seg.TryGet(key, out var obj)) return obj!;
	            throw new KeyNotFoundException("" + (isGlobal ? $"Global service not found: '{expr}' ({key.FullName})" : $"Scoped service not found on thread {Environment.CurrentManagedThreadId}: '{expr}' ({key.FullName})") + $" at {member} in {file}:line {line}");
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool TryGet<T>(out T? service) where T : class, TItem
            {
                var (isGlobal, seg) = TargetSegment();
                var k = typeof(T);
                if (seg.TryGet(k, out var obj) && obj is T t)
                {
                    service = t;
                    return true;
                }

                service = null;
                return false;
            }

            public IEnumerable<(Type Key, TItem Value)> Enumerate(bool global = true)
            {
	            var seg = global ? SShared : Section();
	            foreach (var pair in seg.Snapshot()) yield return (pair.Key, (TItem)pair.Value);
            }

            // ===================== Internals =====================

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static Segment Section() => _tSection ??= new Segment();

            /// <summary>Decides whether current call targets global or scoped, and returns the segment.</summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static (bool isGlobal, Segment seg) TargetSegment()
            {
                if (_tUseGlobalOnce)
                {
                    _tUseGlobalOnce = false; // consume the one-shot
                    return (true, SShared);
                }
                return (false, Section());
            }

            // ===================== Per-segment impl (COW map) =====================


            private sealed class Segment
            {
                private readonly object _writeLock = new();
                private Map _map = Map.Empty;

                public IEnumerable<(Type Key, object Value)> Snapshot() => Volatile.Read(ref _map).Enumerate();

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public bool TryGet(Type key, out object? value) => Volatile.Read(ref _map).TryGet(key, out value);

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public bool Add(Type key, object value)
                {
	                key.ThrowIfNull();
	                value.ThrowIfNull();
                    lock (_writeLock)
                    {
                        var cur = Volatile.Read(ref _map);
                        var next = cur.WithAdded(key, value, out var added);
                        if (!added) return false;
                        Volatile.Write(ref _map, next);
                        return true;
                    }
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public bool Replace(Type key, object value)
                {
	                key.ThrowIfNull();
	                value.ThrowIfNull();
                    lock (_writeLock)
                    {
                        var cur = Volatile.Read(ref _map);
                        var next = cur.WithReplaced(key, value, out var replaced);
                        if (!replaced) return false;
                        Volatile.Write(ref _map, next);
                        return true;
                    }
                }

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public bool Remove(Type key)
                {
	                key.ThrowIfNull();
                    lock (_writeLock)
                    {
                        var cur = Volatile.Read(ref _map);
                        var next = cur.WithRemoved(key, out var removed);
                        if (!removed) return false;
                        Volatile.Write(ref _map, next);
                        return true;
                    }
                }
            }

			private sealed class Map
			{
				private readonly Type?[] _keys;
				private readonly object?[] _vals;
				private readonly int _mask;
				private readonly int _count;

				public static readonly Map Empty = new(8);

				public IEnumerable<(Type Key, object Value)> Enumerate()
				{
					var keys = _keys;
					var vals = _vals;
					for (var i = 0; i < keys.Length; i++)
					{
						var k = keys[i];
						if (k is { }) yield return (k, vals[i]!);
					}
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				private Map(int capacity)
				{
					var cap = NextPow2(Max(8, capacity));
					_keys = new Type?[cap];
					_vals = new object?[cap];
					_mask = cap - 1;
					_count = 0;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				private Map(Type?[] keys, object?[] vals, int count)
				{
					_keys = keys;
					_vals = vals;
					_mask = keys.Length - 1;
					_count = count;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				private static int NextPow2(int v)
				{
					v--;
					v |= v >> 1;
					v |= v >> 2;
					v |= v >> 4;
					v |= v >> 8;
					v |= v >> 16;
					v++;
					return v;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				private static int Hash(Type t) => t.TypeHandle.Value.GetHashCode();

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public bool TryGet(Type key, out object? value)
				{
					var keys = _keys;
					var vals = _vals;
					var i = Hash(key) & _mask;
					while (true)
					{
						var k = Volatile.Read(ref keys[i]);
						if (k is null)
						{
							value = null;
							return false;
						}

						if (ReferenceEquals(k, key))
						{
							value = vals[i];
							return true;
						}

						i = (i + 1) & _mask;
					}
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public Map WithAdded(Type key, object value, out bool added)
				{
					if (TryGet(key, out _))
					{
						added = false;
						return this;
					}

					var need = _count + 1;
					var cap = _keys.Length;
					if (need > (int)(cap * 0.7f)) cap <<= 1;

					var (nk, nv, n) = Rebuild(cap, skipKey: null, replaceKey: null, replaceVal: null);
					Insert(nk, nv, key, value);
					added = true;
					return new Map(nk, nv, n + 1);
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public Map WithReplaced(Type key, object value, out bool replaced)
				{
					if (!TryGet(key, out _))
					{
						replaced = false;
						return this;
					}

					var (nk, nv, n) = Rebuild(_keys.Length, skipKey: null, replaceKey: key, replaceVal: value);
					replaced = true;
					return new Map(nk, nv, n);
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public Map WithRemoved(Type key, out bool removed)
				{
					if (!TryGet(key, out _))
					{
						removed = false;
						return this;
					}

					var cap = _keys.Length;
					if (_count - 1 < (cap >> 2) && cap > 8) cap >>= 1;

					var (nk, nv, n) = Rebuild(cap, skipKey: key, replaceKey: null, replaceVal: null);
					removed = true;
					return new Map(nk, nv, n);
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				private (Type?[] keys, object?[] vals, int count) Rebuild(int capacity, Type? skipKey, Type? replaceKey, object? replaceVal)
				{
					var nk = new Type?[capacity];
					var nv = new object?[capacity];
					var mask = capacity - 1;
					var count = 0;

					for (var i = 0; i < _keys.Length; i++)
					{
						var k = _keys[i];
						if (k is null) continue;
						if (skipKey is { } && ReferenceEquals(k, skipKey)) continue;

						var v = ReferenceEquals(k, replaceKey) ? replaceVal! : _vals[i]!;
						var j = Hash(k) & mask;
						while (true)
						{
							if (nk[j] is null)
							{
								nk[j] = k;
								nv[j] = v;
								count++;
								break;
							}

							j = (j + 1) & mask;
						}
					}

					return (nk, nv, count);
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				private static void Insert(Type?[] keys, object?[] vals, Type k, object v)
				{
					var mask = keys.Length - 1;
					var i = Hash(k) & mask;
					while (true)
					{
						if (keys[i] is null)
						{
							keys[i] = k;
							vals[i] = v;
							return;
						}

						i = (i + 1) & mask;
					}
				}
			}
		}
	}
}