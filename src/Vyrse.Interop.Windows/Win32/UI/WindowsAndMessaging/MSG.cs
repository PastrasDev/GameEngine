using System.Drawing;
using Windows.Win32.Foundation;

namespace Windows.Win32.UI.WindowsAndMessaging;

public struct MSG
{
	/// <summary>
	/// <para>Type: <b>HWND</b> A handle to the window whose window procedure receives the message. This member is <b>NULL</b> when the message is a thread message.</para>
	/// <para><see href="https://learn.microsoft.com/windows/win32/api/winuser/ns-winuser-msg#members">Read more on learn.microsoft.com</see>.</para>
	/// </summary>
	public HWND hwnd;

	/// <summary>
	/// <para>Type: <b>UINT</b> The message identifier. Applications can only use the low word; the high word is reserved by the system.</para>
	/// <para><see href="https://learn.microsoft.com/windows/win32/api/winuser/ns-winuser-msg#members">Read more on learn.microsoft.com</see>.</para>
	/// </summary>
	public uint message;

	/// <summary>
	/// <para>Type: <b>WPARAM</b> Additional information about the message. The exact meaning depends on the value of the <b>message</b> member.</para>
	/// <para><see href="https://learn.microsoft.com/windows/win32/api/winuser/ns-winuser-msg#members">Read more on learn.microsoft.com</see>.</para>
	/// </summary>
	public WPARAM wParam;

	/// <summary>
	/// <para>Type: <b>LPARAM</b> Additional information about the message. The exact meaning depends on the value of the <b>message</b> member.</para>
	/// <para><see href="https://learn.microsoft.com/windows/win32/api/winuser/ns-winuser-msg#members">Read more on learn.microsoft.com</see>.</para>
	/// </summary>
	public LPARAM lParam;

	/// <summary>
	/// <para>Type: <b>DWORD</b> The time at which the message was posted.</para>
	/// <para><see href="https://learn.microsoft.com/windows/win32/api/winuser/ns-winuser-msg#members">Read more on learn.microsoft.com</see>.</para>
	/// </summary>
	public uint time;

	/// <summary>
	/// <para>Type: <b><a href="https://docs.microsoft.com/windows/win32/api/windef/ns-windef-point">POINT</a></b> The cursor position, in screen coordinates, when the message was posted.</para>
	/// <para><see href="https://learn.microsoft.com/windows/win32/api/winuser/ns-winuser-msg#members">Read more on learn.microsoft.com</see>.</para>
	/// </summary>
	public Point pt;
}