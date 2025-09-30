using System.Drawing;
using System.Runtime.InteropServices;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Input;
using Windows.Win32.UI.WindowsAndMessaging;
using Engine.Core;
using Engine.Core.Lifecycle;
using Engine.Core.Threading.Messaging;
using Engine.Core.Threading.Messaging;
using Engine.Core.Threading.Messaging.Messages;
using Engine.Core.Threading.Messaging.Policies;
//using Engine.Core.Threading.Messaging.Messages;
using static Windows.Win32.PInvoke;
using static System.Runtime.InteropServices.Marshal;

namespace Engine.Platform.Windows;

[UnmanagedFunctionPointer(CallingConvention.Winapi)]
internal delegate LRESULT WNDPROC(HWND hwnd, uint msg, WPARAM wParam, LPARAM lParam);

[Discoverable]
public sealed partial class Window : Module<Window>, IConfigure
{
	private const string Title = "GameEngine";
	private const string ClassName = "MyWindowClass";
	private nint _pClassName;

	public HWND Hwnd { get; private set; }
	public HINSTANCE HInstance { get; private set; }
	private WNDPROC _wndProc = null!;
	private RawInput _rawInput = null!;

	private readonly Color _titleBarColor = Color.Black;

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
		HInstance = GetModuleHandle(default);
		_pClassName = StringToHGlobalUni(ClassName);

		var wc = new WNDCLASSEXW
		{
			#pragma warning disable CS8500
			cbSize = (uint)sizeof(WNDCLASSEXW),
			#pragma warning restore CS8500
			lpfnWndProc = (delegate* unmanaged[Stdcall]<HWND, uint, WPARAM, LPARAM, LRESULT>)GetFunctionPointerForDelegate(_wndProc),
			hInstance = HInstance,
			lpszClassName = new PCWSTR((char*)_pClassName),
		};

		Require(RegisterClassEx(&wc) == 0, false);
	}

	public override unsafe void Start()
	{
		base.Start();

		fixed (char* pTitle = Title)
		{
			Hwnd = CreateWindowEx
				(WINDOW_EX_STYLE.WS_EX_APPWINDOW | WINDOW_EX_STYLE.WS_EX_NOREDIRECTIONBITMAP,
				 new PCWSTR((char*)_pClassName),
				 new PCWSTR(pTitle),
				 WINDOW_STYLE.WS_OVERLAPPEDWINDOW,
				 CW_USEDEFAULT,
				 CW_USEDEFAULT,
				 1280,
				 720,
				 default,
				 default,
				 HInstance,
				 null);
		}

		// Send(new Snapshot<HWND, Renderer>
		// {
		// 	Data = Hwnd,
		// 	Target = Registry.Instance.Global.Get<Renderer>(),
		// 	Settings = default
		// });

		Require(Hwnd.IsNull, false);
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

	public override unsafe void Shutdown()
	{
		base.Shutdown();

		if (!Hwnd.IsNull)
		{
			DestroyWindow(Hwnd);
			Hwnd = default;
		}

		Require(UnregisterClass(new PCWSTR((char*)_pClassName), HInstance) == 0, false);

		if (_pClassName != 0)
		{
			FreeHGlobal(_pClassName);
			_pClassName = 0;
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


}

[Discoverable]
public sealed class RawInput : Component<RawInput>, IConfigure
{
	private static readonly unsafe uint s_rawInputDeviceSize = (uint)sizeof(RAWINPUTDEVICE);
	private static readonly unsafe uint s_rawInputHeaderSize = (uint)sizeof(RAWINPUTHEADER);

	private readonly Keyboard.State _kState = new();
	private readonly Mouse.State _mState = new();
	private long _frameId;

	//private IWritePort<InputSnapshot> _inputWriter = null!;

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

	public override unsafe void Start()
	{
		base.Start();

		//var inputEndPoint = Context.MessageBus.Claim<InputSnapshot>();
		//_inputWriter = inputEndPoint.Write!;

		var window = (Window)Owner;
		var hwnd = window.Hwnd;

		RAWINPUTDEVICE* rid = stackalloc RAWINPUTDEVICE[2]
		{
			new RAWINPUTDEVICE
			{
				usUsagePage = 0x01,
				usUsage = 0x02,
				dwFlags = RAWINPUTDEVICE_FLAGS.RIDEV_DEVNOTIFY,
				hwndTarget = hwnd
			},
			new RAWINPUTDEVICE
			{
				usUsagePage = 0x01,
				usUsage = 0x06,
				dwFlags = RAWINPUTDEVICE_FLAGS.RIDEV_DEVNOTIFY | RAWINPUTDEVICE_FLAGS.RIDEV_NOLEGACY,
				hwndTarget = hwnd
			}
		};

		Require(RegisterRawInputDevices(rid, 2u, s_rawInputDeviceSize), true);
	}

	public unsafe void ProcessRawInput(nint lParam, long timestamp)
	{
		uint dwSize = 0;
		if (GetRawInputData((HRAWINPUT)lParam, RAW_INPUT_DATA_COMMAND_FLAGS.RID_INPUT, null, &dwSize, s_rawInputHeaderSize) != 0 || dwSize == 0) return;

		byte* buf = stackalloc byte[(int)dwSize];
		if (GetRawInputData((HRAWINPUT)lParam, RAW_INPUT_DATA_COMMAND_FLAGS.RID_INPUT, buf, &dwSize, s_rawInputHeaderSize) == unchecked((uint)-1)) return;

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

	public override void PreUpdate()
	{

	}

	public override void Update()
	{
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

		Send(new Stream<InputSnapshot, Input>
		{
			Data = snapshot,
			Target = Module.Registry.Instance.Global.Get<Input>(),
			Settings = new Settings.Stream
			{
				CapacityPow2 = 1024,
				WaitMode = WaitMode.Hybrid,
				FullMode = FullMode.Wait,
				DropMode = DropMode.None
			}
		});

		//_inputWriter.Write(snapshot, Application.TokenSrc.Token);
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