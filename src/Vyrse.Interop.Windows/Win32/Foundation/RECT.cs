using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Windows.Win32.Foundation;

///<include file="RECT.xml" path="doc/members/member[@name='T:Windows.Win32.Foundation.RECT']/*"/>
[DebuggerDisplay("{Left}, {Top}, {Right}, {Bottom}")]
[StructLayout(LayoutKind.Sequential)]
public struct RECT(int left, int top, int right, int bottom)
{
	public int Left = left;
	public int Top = top;
	public int Right = right;
	public int Bottom = bottom;

	public RECT(Rectangle value) : this(value.Left, value.Top, value.Right, value.Bottom) { }
	public RECT(Point location, Size size) : this(location.X, location.Y, unchecked(location.X + size.Width), unchecked(location.Y + size.Height)) { }
	public static RECT FromXYWH(int x, int y, int width, int height) => new(x, y, unchecked(x + width), unchecked(y + height));
	public readonly int Width => unchecked(Right - Left);
	public readonly int Height => unchecked(Bottom - Top);
	public readonly bool IsEmpty => Left == 0 && Top == 0 && Right == 0 && Bottom == 0;
	public readonly int X => Left;
	public readonly int Y => Top;
	public readonly Size Size => new(Width, Height);
	public static implicit operator Rectangle(RECT value) => new(value.Left, value.Top, value.Width, value.Height);
	public static implicit operator RectangleF(RECT value) => new(value.Left, value.Top, value.Width, value.Height);
	public static implicit operator RECT(Rectangle value) => new(value);
}