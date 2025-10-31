using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Vyrse.Interop.Windows.Primitives;

[InlineArray(Length)]
public struct FixedArray8<T> where T : unmanaged
{
	private T _element0;
	public const int Length = 8;
	public readonly Span<T> Span => MemoryMarshal.CreateSpan(ref Unsafe.AsRef(in _element0), Length);
	public readonly ReadOnlySpan<T> ReadOnlySpan => MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(in _element0), Length);
}

[InlineArray(Length)]
public struct FixedArray16<T> where T : unmanaged
{
	private T _element0;
	public const int Length = 16;
	public readonly Span<T> Span => MemoryMarshal.CreateSpan(ref Unsafe.AsRef(in _element0), Length);
	public readonly ReadOnlySpan<T> ReadOnlySpan => MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(in _element0), Length);
}

[InlineArray(Length)]
public struct FixedArray32<T> where T : unmanaged
{
	private T _element0;
	public const int Length = 32;
	public readonly Span<T> Span => MemoryMarshal.CreateSpan(ref Unsafe.AsRef(in _element0), Length);
	public readonly ReadOnlySpan<T> ReadOnlySpan => MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(in _element0), Length);
}

[InlineArray(Length)]
public struct FixedArray64<T> where T : unmanaged
{
	private T _element0;
	public const int Length = 64;
	public readonly Span<T> Span => MemoryMarshal.CreateSpan(ref Unsafe.AsRef(in _element0), Length);
	public readonly ReadOnlySpan<T> ReadOnlySpan => MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(in _element0), Length);
}

[InlineArray(Length)]
public struct FixedArray128<T> where T : unmanaged
{
	private T _element0;
	public const int Length = 128;
	public readonly Span<T> Span => MemoryMarshal.CreateSpan(ref Unsafe.AsRef(in _element0), Length);
	public readonly ReadOnlySpan<T> ReadOnlySpan => MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(in _element0), Length);
}

[InlineArray(Length)]
public struct FixedArray256<T> where T : unmanaged
{
	private T _element0;
	public const int Length = 256;
	public readonly Span<T> Span => MemoryMarshal.CreateSpan(ref Unsafe.AsRef(in _element0), Length);
	public readonly ReadOnlySpan<T> ReadOnlySpan => MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(in _element0), Length);
}

[InlineArray(Length)]
public struct FixedArray512<T> where T : unmanaged
{
	private T _element0;
	public const int Length = 512;
	public readonly Span<T> Span => MemoryMarshal.CreateSpan(ref Unsafe.AsRef(in _element0), Length);
	public readonly ReadOnlySpan<T> ReadOnlySpan => MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(in _element0), Length);
}