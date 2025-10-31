using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Dwm;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.UI.HiDpi;
using Windows.Win32.UI.Input;
using Windows.Win32.UI.Input.KeyboardAndMouse;
using Windows.Win32.UI.WindowsAndMessaging;

namespace Vyrse.Interop.Windows
{
	namespace Win32
	{
		[UnmanagedFunctionPointer(CallingConvention.Winapi)]
		public delegate LRESULT WNDPROC(HWND hwnd, uint msg, WPARAM wParam, LPARAM lParam);

		public static partial class Winuser
		{
			/// <include file="Win32.xml" path="doc/members/member[@name='M:Vyrse.Interop.Windows.Win32.Winuser.SetProcessDpiAwarenessContext(Windows.Win32.UI.HiDpi.DPI_AWARENESS_CONTEXT)']/*" />
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool SetProcessDpiAwarenessContext(DPI_AWARENESS_CONTEXT value) => Unmanaged.SetProcessDpiAwarenessContext(value);

			/// <summary>
			/// <see href="https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getwindowdpiawarenesscontext">GetWindowDpiAwarenessContext</see>
			/// Returns the <see cref="DPI_AWARENESS_CONTEXT"/> associated with a window.
			/// </summary>
			/// <param name="hwnd">The window to query.</param>
			/// <returns> <c>true</c> if the operation succeeds; otherwise, <c>false</c>. To obtain extended error information, call <see cref="Marshal.GetLastWin32Error"/>. </returns>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static DPI_AWARENESS_CONTEXT GetWindowDpiAwarenessContext(HWND hwnd) => Unmanaged.GetWindowDpiAwarenessContext(hwnd);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static DPI_AWARENESS GetAwarenessFromDpiAwarenessContext(DPI_AWARENESS_CONTEXT value) => Unmanaged.GetAwarenessFromDpiAwarenessContext(value);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static uint GetDpiForWindow(HWND hwnd) => Unmanaged.GetDpiForWindow(hwnd);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool AreDpiAwarenessContextsEqual(DPI_AWARENESS_CONTEXT dpiContextA, DPI_AWARENESS_CONTEXT dpiContextB) => Unmanaged.AreDpiAwarenessContextsEqual(dpiContextA, dpiContextB);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static HWND CreateWindow(WINDOW_EX_STYLE dwExStyle, string className, string windowName, WINDOW_STYLE dwStyle, int x, int y, int nWidth, int nHeight, HWND hWndParent, HMENU hMenu, HINSTANCE hInstance, nint lpParam) => Unmanaged.CreateWindowExW(dwExStyle, className, windowName, dwStyle, x, y, nWidth, nHeight, hWndParent, hMenu, hInstance, lpParam);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool ShowWindow(HWND hWnd, SHOW_WINDOW_CMD nCmdShow) => Unmanaged.ShowWindow(hWnd, nCmdShow);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool UpdateWindow(HWND hWnd) => Unmanaged.UpdateWindow(hWnd);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool DestroyWindow(HWND hWnd) => Unmanaged.DestroyWindow(hWnd);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static LRESULT DefWindowProc(HWND hWnd, uint msg, WPARAM wParam, LPARAM lParam) => Unmanaged.DefWindowProcW(hWnd, msg, wParam, lParam);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool PeekMessage(out MSG msg, HWND hWnd, uint min, uint max, PEEK_MESSAGE_REMOVE_TYPE remove) => Unmanaged.PeekMessageW(out msg, hWnd, min, max, remove);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool TranslateMessage(in MSG msg, uint flags) => Unmanaged.TranslateMessageEx(in msg, flags);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static LRESULT DispatchMessage(in MSG msg) => Unmanaged.DispatchMessageW(in msg);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void PostQuitMessage(int exitCode) => Unmanaged.PostQuitMessage(exitCode);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static ATOM RegisterClass(in WNDCLASSEXW wcx) => Unmanaged.RegisterClassExW(in wcx);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool UnregisterClass(string className, HINSTANCE hInstance = default) => Unmanaged.UnregisterClassW(className, hInstance);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static long GetWindowLongPtr(HWND hWnd, WINDOW_LONG_PTR_INDEX nIndex) => Unmanaged.GetWindowLongPtrW(hWnd, nIndex);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool GetWindowRect(HWND hWnd, out RECT rect) => Unmanaged.GetWindowRect(hWnd, out rect);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool GetClientRect(HWND hWnd, out RECT rect) => Unmanaged.GetClientRect(hWnd, out rect);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static int GetSystemMetrics(SYSTEM_METRICS_INDEX nIndex) => Unmanaged.GetSystemMetrics(nIndex);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool SetWindowPos(HWND hWnd, HWND hWndInsertAfter, int x, int y, int cx, int cy, SET_WINDOW_POS_FLAGS uFlags) => Unmanaged.SetWindowPos(hWnd, hWndInsertAfter, x, y, cx, cy, uFlags);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool ClientToScreen(HWND hWnd, out Point point) => Unmanaged.ClientToScreen(hWnd, out point);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static int ShowCursor(bool bShow) => Unmanaged.ShowCursor(bShow);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool LockCursor(RECT? rect) { var r = rect.GetValueOrDefault(); unsafe { return Unmanaged.ClipCursor(rect.HasValue ? &r : null); } }

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool SetWindowText(HWND hwnd, string title) => Unmanaged.SetWindowTextW(hwnd, title);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool GetCursorPosition(out Point point) => Unmanaged.GetCursorPos(out point);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool SetForegroundWindow(HWND hwnd) => Unmanaged.SetForegroundWindow(hwnd);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void ForceHideCursor(int cycles = 32) { for (int i = 0; i < cycles && ShowCursor(false) >= 0; i++) ; }

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void ForceShowCursor(int cycles = 32) { for (int i = 0; i < cycles && ShowCursor(true) < 0; i++) ; }

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool LockCursor(HWND hwnd) => hwnd.IsNull ? LockCursor(null) : GetWindowRect(hwnd, out var rect) && LockCursor(rect);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool UnlockCursor() => LockCursor(null);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static uint GetWindowsStyle(HWND hwnd) => (uint)GetWindowLongPtr(hwnd, WINDOW_LONG_PTR_INDEX.GWL_STYLE);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void SetTitleBarColor(HWND hwnd, Color color) => DwmApi.DwmSetWindowAttribute(hwnd, DWMWINDOWATTRIBUTE.DWMWA_CAPTION_COLOR, (COLORREF)color);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void CenterWindow(HWND hwnd)
			{
				if (!GetWindowRect(hwnd, out var rect)) return;
				var x = (GetSystemMetrics(SYSTEM_METRICS_INDEX.SM_CXSCREEN) - (rect.Right - rect.Left)) / 2;
				var y = (GetSystemMetrics(SYSTEM_METRICS_INDEX.SM_CYSCREEN) - (rect.Bottom - rect.Top)) / 2;
				SetWindowPos(hwnd, default, x, y, 0, 0, SET_WINDOW_POS_FLAGS.SWP_NOSIZE | SET_WINDOW_POS_FLAGS.SWP_NOZORDER);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static void SetClientSize(HWND hwnd, int width, int height)
			{
				GetWindowRect(hwnd, out var rect);
				GetClientRect(hwnd, out RECT clientRect);
				int borderX = rect.Width - clientRect.Width;
				int borderY = rect.Height - clientRect.Height;
				const SET_WINDOW_POS_FLAGS flags = SET_WINDOW_POS_FLAGS.SWP_NOZORDER | SET_WINDOW_POS_FLAGS.SWP_NOACTIVATE;
				SetWindowPos(hwnd, default, rect.Left, rect.Top, width + borderX, height + borderY, flags);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static unsafe void ApplySuggestedDpiRect(HWND hwnd, LPARAM lParam)
			{
				var prc = (RECT*)lParam.Value;
				SetWindowPos(hwnd, default, prc->Left, prc->Top, prc->Right - prc->Left, prc->Bottom - prc->Top, SET_WINDOW_POS_FLAGS.SWP_NOZORDER | SET_WINDOW_POS_FLAGS.SWP_NOACTIVATE);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static unsafe bool RegisterRawInputDevices(ReadOnlySpan<RAWINPUTDEVICE> devices) => Unmanaged.RegisterRawInputDevices(devices, (uint)devices.Length, (uint)sizeof(RAWINPUTDEVICE));

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static uint GetRawInputData(HRAWINPUT hRawInput, RAW_INPUT_DATA_COMMAND_FLAGS uiCommand, nint pData, ref uint size) => Unmanaged.GetRawInputData(hRawInput, uiCommand, pData, ref size, (uint)Unsafe.SizeOf<RAWINPUTHEADER>());

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static uint GetRawInputData(HRAWINPUT hRawInput, RAW_INPUT_DATA_COMMAND_FLAGS uiCommand, Span<byte> buffer, ref uint size) => Unmanaged.GetRawInputData(hRawInput, uiCommand, buffer, ref size, (uint)Unsafe.SizeOf<RAWINPUTHEADER>());

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static uint GetRawInputBuffer(Span<byte> data, ref uint pcbSize) => Unmanaged.GetRawInputBuffer(data, ref pcbSize, (uint)Unsafe.SizeOf<RAWINPUTHEADER>());

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static uint GetRawInputDeviceInfo(HANDLE hDevice, RAW_INPUT_DEVICE_INFO_COMMAND uiCommand, ref uint pcbSize, nint pData = 0) => Unmanaged.GetRawInputDeviceInfoW(hDevice, uiCommand, pData, ref pcbSize);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static unsafe uint GetRegisteredRawInputDevices(ref uint deviceCount, Span<RAWINPUTDEVICE> devices = default) => Unmanaged.GetRegisteredRawInputDevices(devices, ref deviceCount, (uint)sizeof(RAWINPUTDEVICE));

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static unsafe uint GetRawInputDeviceList(ref uint count, Span<RAWINPUTDEVICELIST> list = default) => Unmanaged.GetRawInputDeviceList(list, ref count, (uint)sizeof(RAWINPUTDEVICELIST));

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static unsafe LRESULT DefRawInputProc(Span<byte> buffer, int nInput) => Unmanaged.DefRawInputProc(buffer, nInput, (uint)sizeof(RAWINPUTHEADER));

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static HKL GetKeyboardLayout(uint idThread) => Unmanaged.GetKeyboardLayout(idThread);

			internal static partial class Unmanaged
			{
				private const string Library = "user32.dll";

				[LibraryImport(Library, EntryPoint = "SetProcessDpiAwarenessContext", SetLastError = true)]
				internal static partial BOOL SetProcessDpiAwarenessContext(DPI_AWARENESS_CONTEXT value);

				[LibraryImport(Library, EntryPoint = "GetWindowDpiAwarenessContext")]
				internal static partial DPI_AWARENESS_CONTEXT GetWindowDpiAwarenessContext(HWND hwnd);

				[LibraryImport(Library, EntryPoint = "GetAwarenessFromDpiAwarenessContext")]
				internal static partial DPI_AWARENESS GetAwarenessFromDpiAwarenessContext(DPI_AWARENESS_CONTEXT value);

				[LibraryImport(Library, EntryPoint = "GetDpiForWindow")]
				internal static partial uint GetDpiForWindow(HWND hwnd);

				[LibraryImport(Library, EntryPoint = "AreDpiAwarenessContextsEqual")]
				internal static partial BOOL AreDpiAwarenessContextsEqual(DPI_AWARENESS_CONTEXT dpiContextA, DPI_AWARENESS_CONTEXT dpiContextB);

				[LibraryImport(Library, EntryPoint = "CreateWindowExW", StringMarshalling = StringMarshalling.Utf16, SetLastError = true)]
				public static partial HWND CreateWindowExW(WINDOW_EX_STYLE dwExStyle, string? lpClassName, string? lpWindowName, WINDOW_STYLE dwStyle, int x, int y, int nWidth, int nHeight, HWND hWndParent, HMENU hMenu, HINSTANCE hInstance, nint lpParam);

				[LibraryImport(Library, EntryPoint = "ShowWindow")]
				internal static partial BOOL ShowWindow(HWND hWnd, SHOW_WINDOW_CMD nCmdShow);

				[LibraryImport(Library, EntryPoint = "UpdateWindow")]
				internal static partial BOOL UpdateWindow(HWND hWnd);

				[LibraryImport(Library, EntryPoint = "DestroyWindow", SetLastError = true)]
				internal static partial BOOL DestroyWindow(HWND hWnd);

				[LibraryImport(Library, EntryPoint = "DefWindowProcW")]
				internal static partial LRESULT DefWindowProcW(HWND hWnd, uint msg, WPARAM wParam, LPARAM lParam);

				[LibraryImport(Library, EntryPoint = "PeekMessageW")]
				internal static partial BOOL PeekMessageW(out MSG msg, HWND hWnd, uint wMsgFilterMin, uint wMsgFilterMax, PEEK_MESSAGE_REMOVE_TYPE wRemoveMsg);

				[LibraryImport(Library, EntryPoint = "TranslateMessageEx", SetLastError = true)]
				internal static partial BOOL TranslateMessageEx(in MSG msg, uint flags);

				[LibraryImport(Library, EntryPoint = "DispatchMessageW")]
				internal static partial LRESULT DispatchMessageW(in MSG msg);

				[LibraryImport(Library, EntryPoint = "PostQuitMessage")]
				internal static partial void PostQuitMessage(int exitCode);

				[LibraryImport(Library, EntryPoint = "RegisterClassExW", SetLastError = true)]
				internal static partial ATOM RegisterClassExW(in WNDCLASSEXW wcx);

				[LibraryImport(Library, EntryPoint = "UnregisterClassW", StringMarshalling = StringMarshalling.Utf16, SetLastError = true)]
				internal static partial BOOL UnregisterClassW(string className, HINSTANCE hInstance = default);

				[LibraryImport(Library, EntryPoint = "GetWindowLongPtrW")]
				internal static partial long GetWindowLongPtrW(HWND hWnd, WINDOW_LONG_PTR_INDEX nIndex);

				[LibraryImport(Library, EntryPoint = "GetWindowRect")]
				internal static partial BOOL GetWindowRect(HWND hWnd, out RECT rect);

				[LibraryImport(Library, EntryPoint = "GetClientRect")]
				internal static partial BOOL GetClientRect(HWND hWnd, out RECT rect);

				[LibraryImport(Library, EntryPoint = "GetSystemMetrics")]
				internal static partial int GetSystemMetrics(SYSTEM_METRICS_INDEX index);

				[LibraryImport(Library, EntryPoint = "SetWindowPos")]
				internal static partial BOOL SetWindowPos(HWND hWnd, HWND hWndInsertAfter, int x, int y, int cx, int cy, SET_WINDOW_POS_FLAGS uFlags);

				[LibraryImport(Library, EntryPoint = "ClientToScreen")]
				internal static partial BOOL ClientToScreen(HWND hWnd, out Point point);

				[LibraryImport(Library, EntryPoint = "ShowCursor")]
				internal static partial int ShowCursor([MarshalAs(UnmanagedType.Bool)] bool bShow);

				[LibraryImport(Library, EntryPoint = "ClipCursor", SetLastError = true)]
				public static unsafe partial BOOL ClipCursor(RECT* lpRect);

				[LibraryImport(Library, EntryPoint = "SetWindowTextW", StringMarshalling = StringMarshalling.Utf16, SetLastError = true)]
				internal static partial BOOL SetWindowTextW(HWND hWnd, string? value);

				[LibraryImport(Library, EntryPoint = "GetCursorPos", SetLastError = true)]
				internal static unsafe partial BOOL GetCursorPos(out Point point);

				[LibraryImport(Library, EntryPoint = "SetForegroundWindow")]
				internal static partial BOOL SetForegroundWindow(HWND hWnd);

				[LibraryImport(Library, EntryPoint = "RegisterRawInputDevices", SetLastError = true)]
				internal static partial BOOL RegisterRawInputDevices(ReadOnlySpan<RAWINPUTDEVICE> pRawInputDevices, uint uiNumDevices, uint cbSize);

				[LibraryImport(Library, EntryPoint = "GetRawInputData", SetLastError = true)]
				internal static partial uint GetRawInputData(HRAWINPUT hRawInput, RAW_INPUT_DATA_COMMAND_FLAGS uiCommand, nint pData, ref uint pcbSize, uint cbSizeHeader);

				[LibraryImport(Library, EntryPoint = "GetRawInputData", SetLastError = true)]
				internal static partial uint GetRawInputData(HRAWINPUT hRawInput, RAW_INPUT_DATA_COMMAND_FLAGS uiCommand, Span<byte> buffer, ref uint pcbSize, uint cbSizeHeader);

				[LibraryImport(Library, EntryPoint = "GetRawInputBuffer", SetLastError = true)]
				internal static unsafe partial uint GetRawInputBuffer(Span<byte> data, ref uint pcbSize, uint cbSizeHeader);

				[LibraryImport(Library, EntryPoint = "GetRawInputDeviceInfoW", SetLastError = true)]
				internal static partial uint GetRawInputDeviceInfoW(HANDLE hDevice, RAW_INPUT_DEVICE_INFO_COMMAND uiCommand, nint pData, ref uint pcbSize);

				[LibraryImport(Library, EntryPoint = "GetRegisteredRawInputDevices", SetLastError = true)]
				internal static partial uint GetRegisteredRawInputDevices(Span<RAWINPUTDEVICE> pRawInputDevices, ref uint puiNumDevices, uint cbSize);

				[LibraryImport(Library, EntryPoint = "GetRawInputDeviceList", SetLastError = true)]
				internal static unsafe partial uint GetRawInputDeviceList(Span<RAWINPUTDEVICELIST> devices, ref uint deviceCount, uint cbSize);

				[LibraryImport(Library, EntryPoint = "DefRawInputProc", SetLastError = true)]
				internal static unsafe partial LRESULT DefRawInputProc(Span<byte> buffer, int nInput, uint cbSizeHeader);

				[LibraryImport(Library, EntryPoint = "GetKeyboardLayout", SetLastError = true)]
				internal static partial HKL GetKeyboardLayout(uint idThread);
			}
		}
	}

	namespace Win32
	{
		public static partial class Kernel32
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool GetModuleHandle(uint dwFlags, string? moduleName, out HMODULE hModule) => Unmanaged.GetModuleHandleExW(dwFlags, moduleName, out hModule);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static HMODULE GetModuleHandle(string? moduleName) => Unmanaged.GetModuleHandleW(moduleName);

			internal static partial class Unmanaged
			{
				private const string Library = "kernel32.dll";

				[LibraryImport(Library, EntryPoint = "GetModuleHandleExW", StringMarshalling = StringMarshalling.Utf16)]
				internal static partial BOOL GetModuleHandleExW(uint dwFlags, string? moduleName, out HMODULE hModule);

				[LibraryImport(Library, EntryPoint = "GetModuleHandleW", StringMarshalling = StringMarshalling.Utf16)]
				internal static partial HMODULE GetModuleHandleW(string? moduleName);
			}
		}
	}

	namespace Win32
	{
		public static partial class DwmApi
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static unsafe void DwmSetWindowAttribute<T>(HWND hwnd, DWMWINDOWATTRIBUTE attribute, in T value) where T : unmanaged
			{
				fixed (T* p = &value) Unmanaged.DwmSetWindowAttribute(hwnd, attribute, (nint)p, (uint)sizeof(T)).ThrowOnFailure();
			}

			internal static partial class Unmanaged
			{
				private const string Library = "dwmapi.dll";

				[LibraryImport(Library, EntryPoint = "DwmSetWindowAttribute", SetLastError = true)]
				public static partial HRESULT DwmSetWindowAttribute(HWND hwnd, DWMWINDOWATTRIBUTE attribute, nint value, uint size);
			}

		}
	}

	namespace Win32
	{
		public static partial class Gdi32
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static HBRUSH CreateSolidBrush(Color color) => Unmanaged.CreateSolidBrush(color);

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static bool DeleteObject(HGDIOBJ hObject) => Unmanaged.DeleteObject(hObject);

			internal static partial class Unmanaged
			{
				private const string Library = "gdi32.dll";

				[LibraryImport(Library, EntryPoint = "CreateSolidBrush", SetLastError = true)]
				public static partial HBRUSH CreateSolidBrush(COLORREF color);

				[LibraryImport(Library, EntryPoint = "DeleteObject", SetLastError = true)]
				public static partial BOOL DeleteObject(HGDIOBJ hObject);
			}
		}
	}

	namespace Win32
	{
		public static partial class D3d11
		{
			// [MethodImpl(MethodImplOptions.AggressiveInlining)]
			// public static HRESULT D3D11CreateDevice(IDXGIAdapter? pAdapter, D3D_DRIVER_TYPE driverType, HMODULE software, D3D11_CREATE_DEVICE_FLAG flags, ReadOnlySpan<D3D_FEATURE_LEVEL> pFeatureLevels, uint sdkVersion, out ID3D11Device device, out D3D_FEATURE_LEVEL pFeatureLevel, out ID3D11DeviceContext ppImmediateContext) => Unmanaged.D3D11CreateDevice(pAdapter, driverType, software, flags, pFeatureLevels, (uint)pFeatureLevels.Length, sdkVersion, out device, out pFeatureLevel, out ppImmediateContext);
			//
			// internal static partial class Unmanaged
			// {
			// 	private const string Library = "d3d11.dll";
			//
			// 	[LibraryImport(Library, EntryPoint = "D3D11CreateDevice")]
			// 	internal static partial HRESULT D3D11CreateDevice(IDXGIAdapter? pAdapter, D3D_DRIVER_TYPE driverType, HMODULE software, D3D11_CREATE_DEVICE_FLAG flags, ReadOnlySpan<D3D_FEATURE_LEVEL> pFeatureLevels, uint featureLevels, uint sdkVersion, out ID3D11Device device, out D3D_FEATURE_LEVEL pFeatureLevel, out ID3D11DeviceContext ppImmediateContext);
			// }
		}
	}
}