namespace Windows.Win32.UI.Input.KeyboardAndMouse;

[Flags]
public enum RI_MOUSE : uint
{
	LEFT_BUTTON_DOWN    = 0x0001u,
	LEFT_BUTTON_UP      = 0x0002u,
	RIGHT_BUTTON_DOWN   = 0x0004u,
	RIGHT_BUTTON_UP     = 0x0008u,
	MIDDLE_BUTTON_DOWN  = 0x0010u,
	MIDDLE_BUTTON_UP    = 0x0020u,

	BUTTON_4_DOWN       = 0x0040u,
	BUTTON_4_UP         = 0x0080u,
	BUTTON_5_DOWN       = 0x0100u,
	BUTTON_5_UP         = 0x0200u,

	WHEEL               = 0x0400u,
	HWHEEL              = 0x0800u,
}