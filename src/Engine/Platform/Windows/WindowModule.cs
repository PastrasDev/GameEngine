using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;
using Engine.Core;
using Engine.Utilities;

namespace Engine.Platform.Windows;

using static PInvoke;
using static Marshal;
using static Window;

[Affinity(Threads.Main)]
internal sealed class WindowModule : EngineModule<WindowModule>
{
	private const string Title = "GameEngine";
	private const string ClassName = "MyWindowClass";
	private static nint _classNamePtr;
	private readonly Color _titleBarColor = Color.Black;

	internal HWND Hwnd { get; private set; }
	private HINSTANCE _hInstance;
	private WNDPROC _wndProc = null!;

	internal event Action<nint, long>? RawInput;

	protected override void Load()
	{
		base.Load();
		Enabled = true;
	}

	protected override unsafe void Initialize()
	{
		base.Initialize();

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

	protected override unsafe void Start()
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

	protected override unsafe void Update()
	{
		MSG msg = default;
		while (PeekMessage(&msg, default, 0, 0, PEEK_MESSAGE_REMOVE_TYPE.PM_REMOVE) != 0)
		{
			if (msg.message == WM_QUIT)
			{
				IEngine.Shutdown();
				return;
			}

			DispatchMessage(&msg);
		}
	}

	protected override void LateUpdate() { }

	protected override void Shutdown()
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
				RawInput?.Invoke(lParam.Value, Time.Now());
				return new LRESULT(0);
			}
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