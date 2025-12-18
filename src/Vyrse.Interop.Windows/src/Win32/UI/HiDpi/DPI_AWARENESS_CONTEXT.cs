namespace Windows.Win32.UI.HiDpi;

public readonly unsafe struct DPI_AWARENESS_CONTEXT : IEquatable<DPI_AWARENESS_CONTEXT>
{
	internal readonly void* Value;

	internal DPI_AWARENESS_CONTEXT(void* value) => Value = value;

	internal DPI_AWARENESS_CONTEXT(IntPtr value) : this((void*)value) { }

	internal static DPI_AWARENESS_CONTEXT Null => default;

	internal bool IsNull => Value == null;

	public static implicit operator void*(DPI_AWARENESS_CONTEXT value) => value.Value;

	public static explicit operator DPI_AWARENESS_CONTEXT(void* value) => new(value);

	public static bool operator ==(DPI_AWARENESS_CONTEXT left, DPI_AWARENESS_CONTEXT right) => left.Value == right.Value;

	public static bool operator !=(DPI_AWARENESS_CONTEXT left, DPI_AWARENESS_CONTEXT right) => !(left == right);

	public bool Equals(DPI_AWARENESS_CONTEXT other) => Value == other.Value;

	public override bool Equals(object? obj) => obj is DPI_AWARENESS_CONTEXT other && Equals(other);

	public override int GetHashCode() => unchecked((int)Value);

	public override string ToString() => $"0x{(nuint)Value:x}";

	public static implicit operator IntPtr(DPI_AWARENESS_CONTEXT value) => new(value.Value);

	public static explicit operator DPI_AWARENESS_CONTEXT(IntPtr value) => new(value.ToPointer());

	public static explicit operator DPI_AWARENESS_CONTEXT(UIntPtr value) => new(value.ToPointer());

	internal static readonly DPI_AWARENESS_CONTEXT DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2 = (DPI_AWARENESS_CONTEXT)(IntPtr)(-4);

	internal static readonly DPI_AWARENESS_CONTEXT DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE = (DPI_AWARENESS_CONTEXT)(IntPtr)(-3);
}