using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Engine.Core;

[DebuggerDisplay("{ToString()}")]
[method: MethodImpl(MethodImplOptions.AggressiveInlining)]
public readonly struct Handle<T>(int index, uint generation) : IEquatable<Handle<T>>
{
	private readonly uint _indexPlus1 = (uint)(index + 1); // 0 => invalid; else index = _indexPlus1 - 1

	public bool IsValid => _indexPlus1 != 0 && Generation != 0;
	public int  Index   => unchecked((int)_indexPlus1) - 1;
	public uint Generation { get; } = generation;

	public static Handle<T> None => default;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString() => IsValid ? $"Handle<{typeof(T).Name}>[{Index}:{Generation}]" : $"Handle<{typeof(T).Name}>[None]";

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(Handle<T> other) => _indexPlus1 == other._indexPlus1 && Generation == other.Generation;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override bool Equals(object? obj) => obj is Handle<T> h && Equals(h);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode() => HashCode.Combine(_indexPlus1, Generation);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator ==(Handle<T> a, Handle<T> b) => a.Equals(b);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator !=(Handle<T> a, Handle<T> b) => !a.Equals(b);
}