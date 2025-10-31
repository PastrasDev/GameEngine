using Vyrse.Interop.Windows.Win32;

namespace Windows.Win32.UI.Input;

/// <summary>Describes the format of the raw input from a Human Interface Device (HID).</summary>
/// <remarks>
/// <para>Each <a href="https://docs.microsoft.com/windows/desktop/inputdev/wm-input">WM_INPUT</a> can indicate several inputs, but all of the inputs come from the same HID. The size of the <b>bRawData</b> array is <b>dwSizeHid</b> *	<b>dwCount</b>. For more information, see <a href="https://docs.microsoft.com/windows-hardware/drivers/hid/interpreting-hid-reports">Interpreting HID Reports</a>.</para>
/// <para><see href="https://learn.microsoft.com/windows/win32/api/winuser/ns-winuser-rawhid#">Read more on learn.microsoft.com</see>.</para>
/// </remarks>
public struct RAWHID
{
	/// <summary>
	/// <para>Type: <b>DWORD</b> The size, in bytes, of each HID input in <b>bRawData</b>.</para>
	/// <para><see href="https://learn.microsoft.com/windows/win32/api/winuser/ns-winuser-rawhid#members">Read more on learn.microsoft.com</see>.</para>
	/// </summary>
	public uint dwSizeHid;

	/// <summary>
	/// <para>Type: <b>DWORD</b> The number of HID inputs in <b>bRawData</b>.</para>
	/// <para><see href="https://learn.microsoft.com/windows/win32/api/winuser/ns-winuser-rawhid#members">Read more on learn.microsoft.com</see>.</para>
	/// </summary>
	public uint dwCount;

	/// <summary>
	/// <para>Type: <b>BYTE[1]</b> The raw input data, as an array of bytes.</para>
	/// <para><see href="https://learn.microsoft.com/windows/win32/api/winuser/ns-winuser-rawhid#members">Read more on learn.microsoft.com</see>.</para>
	/// </summary>
	public VariableLengthInlineArray<byte> bRawData;

	/// <summary>Computes the amount of memory that must be allocated to store this struct, including the specified number of elements in the variable length inline array at the end.</summary>
	public static unsafe int SizeOf(int count)
	{
		int v = sizeof(RAWHID);
		switch (count)
		{
			case > 1:
				v += checked((count - 1) * sizeof(byte));
				break;
			case < 0:
				throw new ArgumentOutOfRangeException();
		}
		return v;
	}
}