namespace Windows.Win32.UI.WindowsAndMessaging;

/// <summary>
/// Window activation codes for WM_ACTIVATE (LOWORD(wParam)).
/// HIWORD(wParam) is nonzero if the window is minimized.
/// lParam is the HWND being activated/deactivated.
/// </summary>
public enum WA : ushort
{
	/// <summary>Window is being deactivated.</summary>
	INACTIVE = 0,

	/// <summary>Activated by something other than a mouse click (e.g., Alt+Tab, SetActiveWindow, focus change).</summary>
	ACTIVE = 1,

	/// <summary>Activated by a mouse click.</summary>
	CLICKACTIVE = 2
}