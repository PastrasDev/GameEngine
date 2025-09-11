using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
using Engine.Core;

namespace Engine.Platform.Windows;

using static PInvoke;
using static Marshal;
using static Utilities;

[Affinity(Threads.Main)]
public sealed class Window : EngineModule<Window>
{
	private const string Title = "GameEngine";
	private const string ClassName = "MyWindowClass";
	private static nint _classNamePtr;
	private readonly Color _titleBarColor = Color.Black;

	private HWND _hwnd;
	private HINSTANCE _hInstance;
	private WNDPROC _wndProc = null!;

	public override void Load()
	{
		Console.WriteLine("Window loaded");
		Enabled = true;
	}

	public override unsafe void Initialize()
	{
		_wndProc = WNDPROC;
		_hInstance = GetModuleHandle(default(PCWSTR));

		_classNamePtr = StringToHGlobalUni(ClassName);

		var wc = new WNDCLASSEXW
		{
			cbSize = (uint)sizeof(WNDCLASSEXW),
			lpfnWndProc = _wndProc,
			hInstance = _hInstance,
			lpszClassName = new PCWSTR((char*)_classNamePtr),
		};

		require(RegisterClassEx(wc) == 0, false);

		Console.WriteLine("Window initialized");
	}

	public override unsafe void Start()
	{
		_hwnd = CreateWindowEx
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

		require(_hwnd.IsNull, false);

		SetTitleBarColor(_hwnd, _titleBarColor);
		CenterWindow(_hwnd);
		ShowWindow(_hwnd, SHOW_WINDOW_CMD.SW_SHOW);
		SetForegroundWindow(_hwnd);

		Console.WriteLine("Window started");
	}

	public override unsafe void Update()
	{
		MSG msg = default;
		while (PeekMessage(&msg, default, 0, 0, PEEK_MESSAGE_REMOVE_TYPE.PM_REMOVE) != 0)
		{
			if (msg.message == WM_QUIT)
			{
				Context.TokenSrc.Cancel();
				return;
			}

			TranslateMessage(&msg);
			DispatchMessage(&msg);
		}
	}

	public override void LateUpdate() { }

	public override void Shutdown()
	{
		if (!_hwnd.IsNull)
		{
			DestroyWindow(_hwnd);
			_hwnd = default;
		}

		require(UnregisterClass(ClassName, _hInstance) == 0, false);

		if (_classNamePtr != 0)
		{
			FreeHGlobal(_classNamePtr);
			_classNamePtr = 0;
		}

		Console.WriteLine("Window shutdown");
	}

	private LRESULT WNDPROC(HWND hwnd, uint msg, WPARAM wParam, LPARAM lParam)
	{
		switch (msg)
		{
			case WM_DESTROY:
			{
				PostQuitMessage(0);
				return new LRESULT(0);
			}
			case WM_SIZE:
			{
				// Handle window resizing if needed
				return new LRESULT(0);
			}
		}

		return DefWindowProc(hwnd, msg, wParam, lParam);
	}

}