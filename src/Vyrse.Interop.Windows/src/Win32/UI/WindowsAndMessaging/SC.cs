namespace Windows.Win32.UI.WindowsAndMessaging;

/// <summary>
/// System command identifiers sent in <see cref="WM.SYSCOMMAND"/> messages.
///
/// <para>
/// These correspond to operations invoked through the system menu (Alt+Space) or by window
/// management actions such as minimize, maximize, move, or close.
/// </para>
/// <para>
/// Note: The four low-order bits of <c>wParam</c> in <c>WM_SYSCOMMAND</c> are reserved by Windows.
/// Mask them off with <c>wParam &amp; 0xFFF0</c> before comparing against these values.
/// </para>
/// </summary>
public enum SC : uint
{
	/// <summary>
	/// Sizes the window (initiates a resize via the system menu).
	/// </summary>
	SIZE = 0xF000,

	/// <summary>
	/// Moves the window (initiates move/drag via the system menu).
	/// </summary>
	MOVE = 0xF010,

	/// <summary>
	/// Minimizes the window.
	/// </summary>
	MINIMIZE = 0xF020,

	/// <summary>
	/// Maximizes the window.
	/// </summary>
	MAXIMIZE = 0xF030,

	/// <summary>
	/// Switches to the next top-level window in Z order.
	/// </summary>
	NEXTWINDOW = 0xF040,

	/// <summary>
	/// Switches to the previous top-level window in Z order.
	/// </summary>
	PREVWINDOW = 0xF050,

	/// <summary>
	/// Closes the window (same as choosing “Close” from the system menu).
	/// </summary>
	CLOSE = 0xF060,

	/// <summary>
	/// Scrolls vertically within the window’s client area.
	/// </summary>
	VSCROLL = 0xF070,

	/// <summary>
	/// Scrolls horizontally within the window’s client area.
	/// </summary>
	HSCROLL = 0xF080,

	/// <summary>
	/// Invokes the system menu using the mouse.
	/// </summary>
	MOUSEMENU = 0xF090,

	/// <summary>
	/// Invokes the system menu using the keyboard (for example, Alt+Space).
	/// </summary>
	KEYMENU = 0xF100,

	/// <summary>
	/// Arranges all minimized windows (Tile / Cascade operation).
	/// </summary>
	ARRANGE = 0xF110,

	/// <summary>
	/// Restores a minimized or maximized window to its normal size.
	/// </summary>
	RESTORE = 0xF120,

	/// <summary>
	/// Opens the Task List or Task Manager (legacy behavior).
	/// </summary>
	TASKLIST = 0xF130,

	/// <summary>
	/// Activates the screen saver.
	/// </summary>
	SCREENSAVE = 0xF140,

	/// <summary>
	/// Executes a system hotkey associated with the window.
	/// </summary>
	HOTKEY = 0xF150,

	/// <summary>
	/// Performs the default system command for the window.
	/// Rarely used directly by applications.
	/// </summary>
	DEFAULT = 0xF160,

	/// <summary>
	/// Controls monitor power state:
	/// <list type="bullet">
	/// <item><description>–1: power on</description></item>
	/// <item><description>1: low power</description></item>
	/// <item><description>2: shut off</description></item>
	/// </list>
	/// </summary>
	MONITORPOWER = 0xF170,

	/// <summary>
	/// Invokes context help (used with the “?” title-bar button).
	/// </summary>
	CONTEXTHELP = 0xF180,

	/// <summary>
	/// Inserts a separator in the system menu (used internally by Windows).
	/// </summary>
	SEPARATOR = 0xF00F,

	/// <summary>
	/// Indicates a secure screen saver (used in conjunction with <see cref="SC.SCREENSAVE"/>).
	/// </summary>
	SCF_ISSECURE = 0x00000001
}