using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.UI.WindowsAndMessaging;
using Vyrse.Core;

namespace Vyrse.Platform.Windows.Win32;

using static Winuser;
using static Kernel32;
using static Gdi32;

public sealed class Window : IModule
{
	public HWND Hwnd { get; private set; }
	public HINSTANCE HInstance { get; private set; }
	private WNDPROC _wndProc = null!;

	public string Title { get; } = "Vyrse";
	public static string ClassName { get; } = "Vyrse";

	public int Width { get; init; } = 1280;
	public int Height { get; init; } = 720;

	public bool Centered { get; init; } = true;

	public WINDOW_STYLE Style { get; init; } = WINDOW_STYLE.WS_OVERLAPPEDWINDOW;

	private static bool _classRegistered;
	private static nint _pClassName;
	private static HBRUSH _classBrush;
	private static readonly object _classLock = new();

	// private Input _input = null!;

	public event Action? OnCreate;
	public event Action? OnClose;
	public event Action? OnDestroy;
	public event Action? OnEnable;
	public event Action? OnDisable;
	public event Action? OnActivate;
	public event Action? OnDeactivate;
	public event Action? OnSetFocus;
	public event Action? OnKillFocus;
	public event Action<int, int>? OnMove;
	public event Action<int, int>? OnResize;
	public event Action? OnMinimize;
	public event Action? OnMaximize;
	public event Action? OnRestore;
	public event Action? DpiChanged;

	public void Load()
	{
		// _input = AddComponent<Input>();
	}

	public unsafe void Initialize()
	{
		_wndProc = WNDPROC;

		lock (_classLock)
		{
			if (!_classRegistered)
			{
				HInstance = GetModuleHandle(null);
				_pClassName = Marshal.StringToHGlobalUni(ClassName);
				_classBrush = CreateSolidBrush(Color.FromArgb(30, 30, 30));

				var wc = new WNDCLASSEXW
				{
					#pragma warning disable CS8500
					cbSize = (uint)sizeof(WNDCLASSEXW),
					#pragma warning restore CS8500
					lpfnWndProc = (delegate* unmanaged[Stdcall]<HWND, uint, WPARAM, LPARAM, LRESULT>)Marshal.GetFunctionPointerForDelegate(_wndProc),
					hInstance = HInstance,
					lpszClassName = new PCWSTR((char*)_pClassName),
					hbrBackground = _classBrush,
				};

				((bool)RegisterClass(in wc)).ThrowIf(false, "Failed to register window class.");
				_classRegistered = true;
			}
		}

		Hwnd = CreateWindow(WINDOW_EX_STYLE.WS_EX_APPWINDOW, ClassName, Title, Style, 100, 100, Width, Height, default, default, HInstance, 0);
		Hwnd.IsNull.ThrowIf(true, "Failed to create window.");
	}

	public void Start()
	{
		Hwnd.IsNull.ThrowIf(true, "Window handle is null.");
		if (Centered) CenterWindow(Hwnd);
		SetTitleBarColor(Hwnd, Color.FromArgb(30, 30, 30));
		ShowWindow(Hwnd, SHOW_WINDOW_CMD.SW_SHOW);
		UpdateWindow(Hwnd).ThrowIf(false, "Failed to update window.");
		SetForegroundWindow(Hwnd);
	}

	public void PreUpdate()
	{
		while (PeekMessage(out var msg, default, 0, 0, PEEK_MESSAGE_REMOVE_TYPE.PM_REMOVE))
		{
			if ((WM)msg.message == WM.QUIT)
			{
				Engine.Stop();
				return;
			}

			TranslateMessage(in msg, 0);
			DispatchMessage(in msg);
		}
	}

	public void Shutdown()
	{
		if (!Hwnd.IsNull)
		{
			DestroyWindow(Hwnd);
			Hwnd = default;
		}

		Unregister();
	}

	private LRESULT WNDPROC(HWND hwnd, uint msg, WPARAM wParam, LPARAM lParam)
	{
		switch ((WM)msg)
		{
			case WM.CREATE:
			{
				OnCreate?.Invoke();
				return 0;
			}
			case WM.CLOSE:
			{
				OnClose?.Invoke();
				DestroyWindow(hwnd);
				return 0;
			}
			case WM.DESTROY:
			{
				OnDestroy?.Invoke();
				PostQuitMessage(0);
				return 0;
			}
			case WM.ENABLE:
			{
				if (wParam) OnEnable?.Invoke(); else OnDisable?.Invoke();
				return 0;
			}
			case WM.ACTIVATE:
			{
				switch ((WA)wParam.LOWORD)
				{
					case WA.INACTIVE:
					{
						OnDeactivate?.Invoke();
						break;
					}
					case WA.ACTIVE:
					case WA.CLICKACTIVE:
					{
						OnActivate?.Invoke();
						break;
					}
				}

				return 0;
			}
			case WM.SETFOCUS:
			{
				OnSetFocus?.Invoke();
				return 0;
			}
			case WM.KILLFOCUS:
			{
				OnKillFocus?.Invoke();
				return 0;
			}
			case WM.MOVE:
			{
				OnMove?.Invoke(lParam.SLOWORD, lParam.SHIWORD);
				return 0;
			}
			case WM.SIZE:
			{
				int width  = lParam.LOWORD;
				int height = lParam.HIWORD;

				switch ((SIZE)(uint)wParam)
				{
					case SIZE.RESTORED:
					{
						OnRestore?.Invoke();
						OnResize?.Invoke(width, height);
						break;
					}
					case SIZE.MINIMIZED:
					{
						OnMinimize?.Invoke();
						break;
					}
					case SIZE.MAXIMIZED:
					{
						OnMaximize?.Invoke();
						OnResize?.Invoke(width, height);
						break;
					}
				}

				return 0;
			}
			case WM.SYSCOMMAND:
			{
				switch (wParam.SysCommand)
				{
					case SC.KEYMENU: return 0;
				}

				break;
			}
			case WM.DPICHANGED:
			{
				ApplySuggestedDpiRect(hwnd, lParam);
				DpiChanged?.Invoke();
				return 0;
			}
		}

		return DefWindowProc(hwnd, msg, wParam, lParam);
	}

	public static void Unregister()
	{
		lock (_classLock)
		{
			if (!_classRegistered) return;
			UnregisterClass(ClassName, GetModuleHandle(null));
			Marshal.FreeHGlobal(_pClassName);
			DeleteObject(_classBrush);
			_pClassName = nint.Zero;
			_classRegistered = false;
		}
	}
}