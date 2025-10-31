namespace Windows.Win32.UI.Input.KeyboardAndMouse;

[Flags]
public enum RI_KEY : ushort
{
	MAKE               = 0x0000,
	BREAK              = 0x0001,
	E0                 = 0x0002,
	E1                 = 0x0004,
	TERMSRV_SET_LED    = 0x0008,
	TERMSRV_SHADOW     = 0x0010,
}