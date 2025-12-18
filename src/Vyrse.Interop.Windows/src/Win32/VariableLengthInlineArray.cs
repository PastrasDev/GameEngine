using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Windows.Win32;

[StructLayout(LayoutKind.Sequential)]
public struct VariableLengthInlineArray<T> where T : unmanaged
{
	public T E0;
	
	public ref T this[int index]
	{
		[UnscopedRef]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref Unsafe.Add(ref E0, index);
	}
	
	[UnscopedRef]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Span<T> AsSpan(int length)
	{
		return MemoryMarshal.CreateSpan(ref E0, length);
	}
}

[StructLayout(LayoutKind.Sequential)]
public struct VariableLengthInlineArray<T, TBlittable> where T : unmanaged where TBlittable : unmanaged
{
	private TBlittable _e0;
	
	[UnscopedRef]
	public ref T E0 => ref Unsafe.As<TBlittable, T>(ref _e0);
	
	public ref T this[int index]
	{
		[UnscopedRef]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ref Unsafe.Add(ref E0, index);
	}
	
	[UnscopedRef]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Span<T> AsSpan(int length) => MemoryMarshal.CreateSpan(ref E0, length);
}