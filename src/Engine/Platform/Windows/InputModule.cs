using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Windows.Win32;
using Windows.Win32.UI.Input;
using Engine.Core;
using Windows.Win32.UI.Input.KeyboardAndMouse;
using Engine.Messaging;
using Engine.Utilities;

namespace Engine.Platform.Windows;

using static PInvoke;

[DependsOn<WindowModule>]
[Affinity(Threads.Main)]
internal sealed class InputModule : EngineModule<InputModule>
{
	private static readonly unsafe uint RawInputDeviceSize = (uint)sizeof(RAWINPUTDEVICE);
	private static readonly unsafe uint RawInputHeaderSize = (uint)sizeof(RAWINPUTHEADER);

	private readonly IDeviceState _keyboardState = new KeyboardState();
	private readonly IDeviceState _mouseState = new MouseState();

	private IMainChannels _channels = null!;
	private CancellationToken _token;
	private long _frameId;

	private enum RimType : uint
	{
		Mouse = 0,
		Keyboard = 1,
		Hid = 2
	}

	protected override void Load()
	{
		base.Load();
		Enabled = true;
	}

	protected override void Initialize()
	{
		base.Initialize();
	}

	protected override void Start()
	{
		base.Start();

		_channels = require(Context.Registry.Get<IMainChannels>());
		_token = require(Context.TokenSrc).Token;
		var window = require(Context.Registry.Get<WindowModule>());
		window.RawInput += ProcessRawInput;
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

		require(RegisterRawInputDevices(rid, RawInputDeviceSize), true);
	}

	private unsafe void ProcessRawInput(nint lParam, long timestamp)
	{
		uint dwSize = 0;
		if (GetRawInputData((HRAWINPUT)lParam, RAW_INPUT_DATA_COMMAND_FLAGS.RID_INPUT, null, ref dwSize, RawInputHeaderSize) != 0 || dwSize == 0) return;

		byte* buf = stackalloc byte[(int)dwSize];
		if (GetRawInputData((HRAWINPUT)lParam, RAW_INPUT_DATA_COMMAND_FLAGS.RID_INPUT, buf, ref dwSize, RawInputHeaderSize) == unchecked((uint)-1)) return;

		var raw = *(RAWINPUT*)buf;
		switch ((RimType)raw.header.dwType)
		{
			case RimType.Keyboard:
			{
				_keyboardState.ProcessState(raw.data.keyboard, timestamp);
				break;
			}
			case RimType.Mouse:
			{
				_mouseState.ProcessState(raw.data.mouse, timestamp);
				break;
			}
			case RimType.Hid:
			{
				break;
			}
		}
	}

	protected override void Update()
	{
		var now = Time.Now();
		var kState = _keyboardState.BuildFrameState<KeyboardFrameState>(now);
		var mState = _mouseState.BuildFrameState<MouseFrameState>(now);
		long timestamp = (kState.EventsThisFrame == 0 && mState.EventsThisFrame == 0) ? now : max(kState.ProducedTimestamp, mState.ProducedTimestamp);
		_channels.InputWriter.Write(new InputSnapshot(_frameId++, timestamp, kState, mState), _token);
	}

	protected override void LateUpdate()
	{
		_keyboardState.Reset();
		_mouseState.Reset();
	}

	protected override void Shutdown()
	{
		base.Shutdown();
	}
}

public readonly record struct InputSnapshot(long FrameId, long Timestamp, KeyboardFrameState KeyboardState, MouseFrameState MouseState);

internal interface IDeviceState
{
	long LastEventTimestamp { get; set; }
	int EventsThisFrame { get; set; }

	void ProcessState(RAWKEYBOARD keyboard, long timestamp) { }
	void ProcessState(RAWMOUSE mouse, long timestamp) { }

	T BuildFrameState<T>(long timestamp) where T : struct;

	void Reset();
}

public readonly record struct KeyboardFrameState
{
	public required KeyBits Held { get; init; }
	public required KeyBits Pressed { get; init; }
	public required KeyBits Released { get; init; }
	public required Keyboard.Modifiers Modifiers { get; init; }
	public required long EventTimestamp { get; init; }
	public required long ProducedTimestamp { get; init; }
	public required int EventsThisFrame { get; init; }
}

internal sealed class KeyboardState : IDeviceState
{
	private KeyBits _current;
	private KeyBits _framePressed;
	private KeyBits _frameReleased;
	private Keyboard.Modifiers _modifiers;
	public long LastEventTimestamp { get; set; }
	public int EventsThisFrame { get; set; }

	public void ProcessState(RAWKEYBOARD keyboard, long timestamp)
	{
		LastEventTimestamp = timestamp;
		EventsThisFrame++;

		var flags = keyboard.Flags;
		bool make = (flags & RI_KEY_BREAK) == 0;

		ushort vKey = keyboard.VKey;
		if (vKey is 0x10 or 0x11 or 0x12)
			vKey = ResolveModifierKey(vKey, keyboard.MakeCode, flags);

		int vk = vKey & 0xFF;

		bool wasDown = _current.IsSet(vk);

		_current = _current.WithBit(vk, make);

		if (make && !wasDown) _framePressed = _framePressed.WithBit(vk, true);
		else if (!make && wasDown) _frameReleased = _frameReleased.WithBit(vk, true);

		UpdateModifiers((Keyboard.Key)vk, make);
	}

	public T BuildFrameState<T>(long timestamp) where T : struct => (T)(object)new KeyboardFrameState
	{
		Held = _current,
		Pressed = _framePressed,
		Released = _frameReleased,
		Modifiers = _modifiers,
		EventTimestamp = LastEventTimestamp,
		ProducedTimestamp = EventsThisFrame == 0 ? timestamp : LastEventTimestamp,
		EventsThisFrame = EventsThisFrame
	};

	public void Reset()
	{
		_framePressed = default;
		_frameReleased = default;
		EventsThisFrame = 0;
	}

	private static ushort ResolveModifierKey(ushort vKey, ushort makeCode, ushort flags)
	{
		uint scanCode = makeCode;
		if ((flags & RI_KEY_E0) != 0) scanCode |= 0xE000;
		if ((flags & RI_KEY_E1) != 0) scanCode |= 0xE100;
		return (ushort)MapVirtualKey(scanCode, MAP_VIRTUAL_KEY_TYPE.MAPVK_VSC_TO_VK_EX);
	}

	private void UpdateModifiers(Keyboard.Key vk, bool down)
	{
		switch (vk)
		{
			case Keyboard.Key.LShift: _modifiers = down ? _modifiers | Keyboard.Modifiers.LShift : _modifiers & ~Keyboard.Modifiers.LShift; break;
			case Keyboard.Key.RShift: _modifiers = down ? _modifiers | Keyboard.Modifiers.RShift : _modifiers & ~Keyboard.Modifiers.RShift; break;
			case Keyboard.Key.LControl:
				_modifiers = down ? _modifiers | Keyboard.Modifiers.LControl : _modifiers & ~Keyboard.Modifiers.LControl; break;
			case Keyboard.Key.RControl:
				_modifiers = down ? _modifiers | Keyboard.Modifiers.RControl : _modifiers & ~Keyboard.Modifiers.RControl; break;
			case Keyboard.Key.LAlt: _modifiers = down ? _modifiers | Keyboard.Modifiers.LAlt : _modifiers & ~Keyboard.Modifiers.LAlt; break;
			case Keyboard.Key.RAlt: _modifiers = down ? _modifiers | Keyboard.Modifiers.RAlt : _modifiers & ~Keyboard.Modifiers.RAlt; break;
			case Keyboard.Key.LWin: _modifiers = down ? _modifiers | Keyboard.Modifiers.LWin : _modifiers & ~Keyboard.Modifiers.LWin; break;
			case Keyboard.Key.RWin: _modifiers = down ? _modifiers | Keyboard.Modifiers.RWin : _modifiers & ~Keyboard.Modifiers.RWin; break;

			// Lock keys toggle on key "make" (ignore break).
			case Keyboard.Key.CapsLock:
				if (down) _modifiers ^= Keyboard.Modifiers.CapsLock;
				break;
			case Keyboard.Key.NumLock:
				if (down) _modifiers ^= Keyboard.Modifiers.NumLock;
				break;
			case Keyboard.Key.ScrollLock:
				if (down) _modifiers ^= Keyboard.Modifiers.ScrollLock;
				break;
		}

		// AltGr heuristic: commonly reported as RAlt + LCtrl together.
		if ((_modifiers & Keyboard.Modifiers.RAlt) != 0 && (_modifiers & Keyboard.Modifiers.LControl) != 0) _modifiers |= Keyboard.Modifiers.AltGr;
		else _modifiers &= ~Keyboard.Modifiers.AltGr;
	}
}

public readonly record struct MouseFrameState
{
	public required (int X, int Y) Delta { get; init; }
	public required (int X, int Y) Position { get; init; }
	public required int Wheel { get; init; }
	public required int HWheel { get; init; }
	public required double ScrollDelta { get; init; }
	public required Mouse.Button Held { get; init; }
	public required Mouse.Button Pressed { get; init; }
	public required Mouse.Button Released { get; init; }
	public required long EventTimestamp { get; init; }
	public required long ProducedTimestamp { get; init; }
	public required int EventsThisFrame { get; init; }
}

internal sealed class MouseState : IDeviceState
{
	private (int x, int y) _lastDelta;
	private int _wheel;
	private int _hWheel;
	private Mouse.Button _held;
	private Mouse.Button _pressed;
	private Mouse.Button _released;

	public long LastEventTimestamp { get; set; }
	public int  EventsThisFrame { get; set; }

	private long _lastWheelTimestamp;
	private double _scrollDelta;

	public void ProcessState(RAWMOUSE mouse, long timestamp)
	{
		LastEventTimestamp = timestamp;
		EventsThisFrame++;

		_lastDelta.x += mouse.lLastX;
		_lastDelta.y += mouse.lLastY;

		var flags = mouse.Anonymous.Anonymous.usButtonFlags;
		if ((flags & RI_MOUSE_LEFT_BUTTON_DOWN) != 0) { _held |= Mouse.Button.Left; _pressed |= Mouse.Button.Left; }
		if ((flags & RI_MOUSE_LEFT_BUTTON_UP) != 0) { _held &= ~Mouse.Button.Left; _released |= Mouse.Button.Left; }
		if ((flags & RI_MOUSE_RIGHT_BUTTON_DOWN) != 0) { _held |= Mouse.Button.Right; _pressed |= Mouse.Button.Right; }
		if ((flags & RI_MOUSE_RIGHT_BUTTON_UP) != 0) { _held &= ~Mouse.Button.Right; _released |= Mouse.Button.Right; }
		if ((flags & RI_MOUSE_MIDDLE_BUTTON_DOWN) != 0) { _held |= Mouse.Button.Middle; _pressed |= Mouse.Button.Middle; }
		if ((flags & RI_MOUSE_MIDDLE_BUTTON_UP) != 0) { _held &= ~Mouse.Button.Middle; _released |= Mouse.Button.Middle; }
		if ((flags & RI_MOUSE_BUTTON_4_DOWN) != 0) { _held |= Mouse.Button.XButton1; _pressed |= Mouse.Button.XButton1; }
		if ((flags & RI_MOUSE_BUTTON_4_UP) != 0) { _held &= ~Mouse.Button.XButton1; _released |= Mouse.Button.XButton1; }
		if ((flags & RI_MOUSE_BUTTON_5_DOWN) != 0) { _held |= Mouse.Button.XButton2; _pressed |= Mouse.Button.XButton2; }
		if ((flags & RI_MOUSE_BUTTON_5_UP) != 0) { _held &= ~Mouse.Button.XButton2; _released |= Mouse.Button.XButton2; }

		var data = mouse.Anonymous.Anonymous.usButtonData;
		if ((flags & RI_MOUSE_WHEEL) != 0)
		{
			short step = (short)data;
			_wheel += step;

			if (_lastWheelTimestamp != 0)
			{
				var dtMs = (timestamp - _lastWheelTimestamp) * Time.ToMilliseconds;
				if (dtMs > 0)
					_scrollDelta = (step / 120.0) / (dtMs / 1000.0);
			}

			_lastWheelTimestamp = timestamp;
		}

		if ((flags & RI_MOUSE_HWHEEL) != 0) _hWheel += (short)data;
	}

	public T BuildFrameState<T>(long timestamp) where T : struct => (T)(object)new MouseFrameState
	{
		Delta = _lastDelta,
		Position = Window.GetCursorPosition(),
		Wheel = _wheel,
		HWheel = _hWheel,
		ScrollDelta = _scrollDelta,
		Held = _held,
		Pressed = _pressed,
		Released = _released,
		EventTimestamp = LastEventTimestamp,
		ProducedTimestamp = EventsThisFrame == 0 ? timestamp : LastEventTimestamp,
		EventsThisFrame = EventsThisFrame
	};

	public void Reset()
	{
		_lastDelta = (0, 0);
		_wheel = 0;
		_hWheel = 0;
		_pressed = 0;
		_released = 0;
		_scrollDelta = 0;
		EventsThisFrame = 0;
	}
}

public readonly record struct KeyBits(ulong A, ulong B, ulong C, ulong D)
{
	public static KeyBits Zero => default;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool IsSet(int vk) => (vk >> 6) switch
		{
			0 => (A & (1UL << (vk & 63))) != 0,
			1 => (B & (1UL << (vk & 63))) != 0,
			2 => (C & (1UL << (vk & 63))) != 0,
			_ => (D & (1UL << (vk & 63))) != 0
		};

	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static KeyBits operator |(KeyBits x, KeyBits y) => new(x.A | y.A, x.B | y.B, x.C | y.C, x.D | y.D);
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static KeyBits operator &(KeyBits x, KeyBits y) => new(x.A & y.A, x.B & y.B, x.C & y.C, x.D & y.D);
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static KeyBits operator ~(KeyBits x) => new(~x.A, ~x.B, ~x.C, ~x.D);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public KeyBits WithBit(int vk, bool down)
	{
		int lane = vk >> 6; // 0..3
		int off = vk & 63;  // 0..63
		ulong m = 1UL << off;

		return lane switch
		{
			0 => this with { A = down ? (A | m) : (A & ~m) },
			1 => this with { B = down ? (B | m) : (B & ~m) },
			2 => this with { C = down ? (C | m) : (C & ~m) },
			_ => this with { D = down ? (D | m) : (D & ~m) },
		};
	}
}