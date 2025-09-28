using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Dwm;
using Windows.Win32.UI.Input;
using Windows.Win32.UI.WindowsAndMessaging;
using Engine.Core;
using Engine.Core.Lifecycle;
using Engine.Core.Threading.Communication;
using Engine.Core.Threading.Communication.Messages;

namespace Engine.Platform.Windows;

using static PInvoke;
using static Marshal;

[Discoverable]
public sealed class Window : Module<Window>, IConfigure
{
	private const string Title = "GameEngine";
	private const string ClassName = "MyWindowClass";
	private static nint _classNamePtr;
	private readonly Color _titleBarColor = Color.Black;

	internal HWND Hwnd { get; private set; }
	private HINSTANCE _hInstance;
	private WNDPROC _wndProc = null!;

	private RawInput _rawInput = null!;

	public static void Configure()
	{
		Enabled = true;
		AllowMultiple = false;
		Affinity = Affinity.Main;
		AddComponent<RawInput>();
	}

	public override void Load()
	{
		base.Load();
	}

	public override unsafe void Initialize()
	{
		base.Initialize();

		_rawInput = GetComponent<RawInput>();

		_wndProc = WNDPROC;
		_hInstance = GetModuleHandle(default(PCWSTR));
		_classNamePtr = StringToHGlobalUni(ClassName);

		var wc = new WNDCLASSEXW
		{
			#pragma warning disable CS8500
			cbSize = (uint)sizeof(WNDCLASSEXW),
			#pragma warning restore CS8500
			lpfnWndProc = _wndProc,
			hInstance = _hInstance,
			lpszClassName = new PCWSTR((char*)_classNamePtr),
		};

		require(RegisterClassEx(wc) == 0, false);
	}

	public override unsafe void Start()
	{
		base.Start();

		Hwnd = CreateWindowEx
			(
			 WINDOW_EX_STYLE.WS_EX_APPWINDOW | WINDOW_EX_STYLE.WS_EX_NOREDIRECTIONBITMAP,
			 ClassName,
			 Title,
			 WINDOW_STYLE.WS_OVERLAPPEDWINDOW,
			 CW_USEDEFAULT,
			 CW_USEDEFAULT,
			 800,
			 600,
			 default,
			 default,
			 _hInstance,
			 null
			);

		require(Hwnd.IsNull, false);

		SetTitleBarColor(Hwnd, _titleBarColor);
		CenterWindow(Hwnd);
		ShowWindow(Hwnd, SHOW_WINDOW_CMD.SW_SHOW);
		SetForegroundWindow(Hwnd);
	}

	public override unsafe void Update()
	{
		MSG msg = default;
		while (PeekMessage(&msg, default, 0, 0, PEEK_MESSAGE_REMOVE_TYPE.PM_REMOVE) != 0)
		{
			if (msg.message == WM_QUIT)
			{
				Application.Shutdown();
				return;
			}

			TranslateMessage(&msg);
			DispatchMessage(&msg);
		}
	}

	public override void PostUpdate() { }

	public override void Shutdown()
	{
		base.Shutdown();

		if (!Hwnd.IsNull)
		{
			DestroyWindow(Hwnd);
			Hwnd = default;
		}

		require(UnregisterClass(ClassName, _hInstance) == 0, false);

		if (_classNamePtr != 0)
		{
			FreeHGlobal(_classNamePtr);
			_classNamePtr = 0;
		}
	}

	private LRESULT WNDPROC(HWND hwnd, uint msg, WPARAM wParam, LPARAM lParam)
	{
		switch (msg)
		{
			case WM_INPUT:
			{
				_rawInput.ProcessRawInput(lParam.Value, Time.Now());
				return new LRESULT(0);
			}
			case WM_CLOSE:
			{
				PostQuitMessage(0);
				return new LRESULT(0);
			}
			case WM_SYSCOMMAND:
			{
				if ((wParam.Value & 0xFFF0) == SC_KEYMENU)
				{
					return new LRESULT(0);
				}
				break;
			}
		}

		return DefWindowProc(hwnd, msg, wParam, lParam);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static uint GetWindowsStyle(HWND hwnd) => (uint)GetWindowLong(hwnd, WINDOW_LONG_PTR_INDEX.GWL_STYLE);
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static unsafe void SetTitleBarColor(HWND hwnd, Color color) { var colorRef = (uint)(color.R | (color.G << 8) | (color.B << 16)); DwmSetWindowAttribute(hwnd, DWMWINDOWATTRIBUTE.DWMWA_CAPTION_COLOR, &colorRef, sizeof(uint)); }
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static void UnlockCursor() => ClipCursor(default(RECT));
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static void ForceHideCursor() { while (ShowCursor(false) >= 0) ; }
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static void ForceShowCursor() { while (ShowCursor(true) < 0) ; }
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static unsafe void SetWindowTitle(HWND hwnd, string title) { fixed (char* p = title) { SetWindowText(hwnd, new PCWSTR(p)); } }
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static unsafe string GetWindowTitle(HWND hwnd) { const int capacity = 256; var buffer = stackalloc char[capacity]; return new string(buffer, 0, GetWindowText(hwnd, new PWSTR(buffer), capacity)); }
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static unsafe (int x, int y) GetCursorPosition() { Point p; GetCursorPos(&p); return (p.X, p.Y); }

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
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

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static unsafe void LockCursorToWindow(HWND hwnd)
	{
		RECT rect;
		if (!GetClientRect(hwnd, &rect)) return;
		ClientToScreen(hwnd, (Point*)&rect.left);
		ClientToScreen(hwnd, (Point*)&rect.right);
		ClipCursor(&rect);
	}

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
			throw new Win32Exception(GetLastWin32Error());
		}

		var windowWidth = rect.right - rect.left;
		var windowHeight = rect.bottom - rect.top;

		var screenWidth = GetSystemMetrics(SYSTEM_METRICS_INDEX.SM_CXSCREEN);
		var screenHeight = GetSystemMetrics(SYSTEM_METRICS_INDEX.SM_CYSCREEN);

		var x = (screenWidth - windowWidth) / 2;
		var y = (screenHeight - windowHeight) / 2;

		SetWindowPos(hwnd, HWND.Null, x, y, windowWidth, windowHeight, SET_WINDOW_POS_FLAGS.SWP_NOZORDER);
	}
}

[Discoverable]
public sealed class RawInput : Component<RawInput>, IConfigure
{
	private static readonly unsafe uint s_rawInputDeviceSize = (uint)sizeof(RAWINPUTDEVICE);
	private static readonly unsafe uint s_rawInputHeaderSize = (uint)sizeof(RAWINPUTHEADER);

	private readonly Keyboard.State _kState = new();
	private readonly Mouse.State _mState = new();
	private long _frameId;

	private IWritePort<InputSnapshot> _inputWriter = null!;

	private enum RimType : uint
	{
		Mouse = 0,
		Keyboard = 1,
		Hid = 2
	}

	public static void Configure()
	{
		Enabled = true;
		AllowMultiple = false;
	}

	public override void Load()
	{
		base.Load();
	}

	public override void Initialize()
	{
		base.Initialize();

	}

	public override void Start()
	{
		base.Start();

		var inputEndPoint = Context.MessageBus.Claim<InputSnapshot>();
		_inputWriter = inputEndPoint.Write!;

		var window = (Window)Owner;
		var hwnd = window.Hwnd;

		ReadOnlySpan<RAWINPUTDEVICE> rid =
		[
			new()
			{
				usUsagePage = 0x01,
				usUsage = 0x02,
				dwFlags = RAWINPUTDEVICE_FLAGS.RIDEV_DEVNOTIFY,
				hwndTarget = hwnd
			},
			new()
			{
				usUsagePage = 0x01,
				usUsage = 0x06,
				dwFlags = RAWINPUTDEVICE_FLAGS.RIDEV_DEVNOTIFY | RAWINPUTDEVICE_FLAGS.RIDEV_NOLEGACY,
				hwndTarget = hwnd
			}
		];

		require(RegisterRawInputDevices(rid, s_rawInputDeviceSize), true);
	}

	public unsafe void ProcessRawInput(nint lParam, long timestamp)
	{
		uint dwSize = 0;
		if (GetRawInputData((HRAWINPUT)lParam, RAW_INPUT_DATA_COMMAND_FLAGS.RID_INPUT, null, ref dwSize, s_rawInputHeaderSize) != 0 || dwSize == 0) return;

		byte* buf = stackalloc byte[(int)dwSize];
		if (GetRawInputData((HRAWINPUT)lParam, RAW_INPUT_DATA_COMMAND_FLAGS.RID_INPUT, buf, ref dwSize, s_rawInputHeaderSize) == unchecked((uint)-1)) return;

		var raw = *(RAWINPUT*)buf;
		switch ((RimType)raw.header.dwType)
		{
			case RimType.Keyboard:
			{
				_kState.ProcessState(raw.data.keyboard, timestamp);
				break;
			}
			case RimType.Mouse:
			{
				_mState.ProcessState(raw.data.mouse, timestamp);
				break;
			}
			case RimType.Hid:
			{
				break;
			}
		}
	}

	public override void Update()
	{
		base.Update();

		var now = Time.Now();
		var kState = _kState.BuildFrameState(now);
		var mState = _mState.BuildFrameState(now);
		long timestamp = kState.EventsThisFrame == 0 && mState.EventsThisFrame == 0 ? now : max(kState.ProducedTimestamp, mState.ProducedTimestamp);

		var snapshot = new InputSnapshot
		{
			FrameId = _frameId++,
			Timestamp = timestamp,
			KState = kState,
			MState = mState
		};

		_inputWriter.Write(snapshot, Application.TokenSrc.Token);
	}

	public override void PostUpdate()
	{
		base.PostUpdate();

		_kState.Reset();
		_mState.Reset();
	}

	public override void Shutdown()
	{
		base.Shutdown();
	}
}