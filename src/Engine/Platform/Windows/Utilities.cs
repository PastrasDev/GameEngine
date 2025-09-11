using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Dwm;
using Windows.Win32.UI.WindowsAndMessaging;

namespace Engine.Platform.Windows;

using static PInvoke;

public static class Utilities
{
	public static unsafe void SetClientSize(HWND hwnd, int width, int height)
	{
		RECT rect = new()
		{
			left = 0,
			top = 0,
			right = width,
			bottom = height
		};

		if (!AdjustWindowRect(&rect, (WINDOW_STYLE)GetWindowLong(hwnd, WINDOW_LONG_PTR_INDEX.GWL_STYLE), false))
		{
			throw new Win32Exception(Marshal.GetLastWin32Error());
		}

		var windowWidth = rect.right - rect.left;
		var windowHeight = rect.bottom - rect.top;

		var screenWidth = GetSystemMetrics(SYSTEM_METRICS_INDEX.SM_CXSCREEN);
		var screenHeight = GetSystemMetrics(SYSTEM_METRICS_INDEX.SM_CYSCREEN);

		var x = (screenWidth - windowWidth) / 2;
		var y = (screenHeight - windowHeight) / 2;

		SetWindowPos(hwnd, HWND.Null, x, y, windowWidth, windowHeight, SET_WINDOW_POS_FLAGS.SWP_NOZORDER);
	}

	public static uint GetWindowsStyle(HWND hwnd) => (uint)GetWindowLong(hwnd, WINDOW_LONG_PTR_INDEX.GWL_STYLE);

	public static unsafe void SetTitleBarColor(HWND hwnd, Color color)
	{
		var colorRef = (uint)(color.R | (color.G << 8) | (color.B << 16));
		DwmSetWindowAttribute(hwnd, DWMWINDOWATTRIBUTE.DWMWA_CAPTION_COLOR, &colorRef, sizeof(uint));
	}

	public static unsafe void CenterWindow(HWND hwnd)
	{
		RECT rect;
		if (!GetWindowRect(hwnd, &rect))
		{
			return;
		}

		var x = ((GetSystemMetrics(SYSTEM_METRICS_INDEX.SM_CXSCREEN) - (rect.right - rect.left))) / 2;
		var y = ((GetSystemMetrics(SYSTEM_METRICS_INDEX.SM_CYSCREEN) - (rect.bottom - rect.top))) / 2;
		SetWindowPos(hwnd, HWND.Null, x, y, 0, 0, SET_WINDOW_POS_FLAGS.SWP_NOSIZE | SET_WINDOW_POS_FLAGS.SWP_NOZORDER);
	}

	public static unsafe void LockCursorToWindow(HWND hwnd)
	{
		RECT rect;
		if (!GetClientRect(hwnd, &rect)) return;
		ClientToScreen(hwnd, (Point*)&rect.left);
		ClientToScreen(hwnd, (Point*)&rect.right);
		ClipCursor(&rect);
	}

	public static void UnlockCursor()
	{
		ClipCursor(default(RECT));
	}

	public static void ForceHideCursor()
	{
		while (ShowCursor(false) >= 0) { }
	}

	public static void ForceShowCursor()
	{
		while (ShowCursor(true) < 0) { }
	}

	public static unsafe void SetWindowTitle(HWND hwnd, string title)
	{
		fixed (char* p = title)
		{
			SetWindowText(hwnd, new PCWSTR(p));
		}
	}

	public static unsafe string GetWindowTitle(HWND hwnd)
	{
		const int capacity = 256;
		var buffer = stackalloc char[capacity];
		return new string(buffer, 0, GetWindowText(hwnd, new PWSTR(buffer), capacity));
	}
}