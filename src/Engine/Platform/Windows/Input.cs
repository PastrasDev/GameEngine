using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Win32.UI.Input;
using Windows.Win32.UI.Input.KeyboardAndMouse;
using Engine.Core;
using Engine.Core.Lifecycle;
using Engine.Core.Threading.Messaging;
//using Engine.Core.Threading.Messaging.Messages;
using Engine.Core.Threading.Messaging.Messages;
using static Windows.Win32.PInvoke;

namespace Engine.Platform.Windows;

[Discoverable]
public sealed class Input : Module<Input>, IConfigure
{
	//private IReadPort<InputSnapshot> InputReader { get; set; } = null!;
	private InputSnapshot _snapshot;
	private long _lastProcessedInputFrame = -1;

	private Keyboard _keyboard = null!;
	private Mouse _mouse = null!;

	public static void Configure()
	{
		Enabled = true;
		AllowMultiple = false;
		Affinity = Affinity.Game;
	}

	public override void Initialize()
	{
		base.Initialize();

		_keyboard = Context.Keyboard;
		_mouse = Context.Mouse;

		//InputReader = Context.MessageBus.Claim<InputSnapshot>().Read!;
	}

	public override void PreUpdate()
	{

	}

	public override void Update()
	{
		_snapshot = Receive<InputSnapshot, RawInput>();

		//while (InputReader.TryRead(out var snapshot))
			//_snapshot = snapshot;


		if (_snapshot.FrameId != _lastProcessedInputFrame)
		{
			_keyboard.Update(_snapshot.KState, _snapshot.Timestamp);
			_mouse.Update(_snapshot.MState, _snapshot.Timestamp);
			_lastProcessedInputFrame = _snapshot.FrameId;
		}
	}

	public override void Shutdown()
	{
		base.Shutdown();
	}
}

public sealed class Keyboard
{
	private FrameState _state;
	private KeyBits _prevHeld;
	private long _lastEventTimestamp;

	public bool Enabled { get; set; } = true;

	public double LastLatencyMs { get; private set; }
	public double AverageLatencyMs { get; private set; }

	private readonly Dictionary<(Key, Event), Action> _handlers = new();
	private readonly Dictionary<Event, Action<Key>> _anyKeyHandlers = new();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ref Action OnEvent(Key key, Event type) =>
		ref CollectionsMarshal.GetValueRefOrAddDefault(_handlers, (key, type), out _)!;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ref Action<Key> OnAny(Event type) =>
		ref CollectionsMarshal.GetValueRefOrAddDefault(_anyKeyHandlers, type, out _)!;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Invoke(Key key, Event type)
	{
		if (_handlers.TryGetValue((key, type), out var h)) h?.Invoke();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void InvokeAny(Key key, Event type)
	{
		if (_anyKeyHandlers.TryGetValue(type, out var h)) h?.Invoke(key);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool IsKeyHeld(Key key) => _state.Held.IsSet((int)key);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool WasKeyPressed(Key key) => _state.Pressed.IsSet((int)key);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool WasKeyReleased(Key key) => _state.Released.IsSet((int)key);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool IsChord(Modifiers modifier, Key key) => (_state.Modifiers & modifier) == modifier && IsKeyHeld(key);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool PressedChord(Modifiers modifier, Key key) =>
		(_state.Modifiers & modifier) == modifier && WasKeyPressed(key);

	internal void Update(in FrameState state, long timestamp)
	{
		_state = state;

		if (state.EventsThisFrame > 0)
		{
			var endToEndMs = (timestamp - state.EventTimestamp) * Time.ToMilliseconds;
			LastLatencyMs = endToEndMs;
			AverageLatencyMs = AverageLatencyMs == 0 ? endToEndMs : AverageLatencyMs * 0.9 + endToEndMs * 0.1;
		}

		var heldNow = state.Held;
		var pressedMain = state.Pressed;
		var releasedMain = state.Released;

		bool edgesAreFresh = state.EventTimestamp != _lastEventTimestamp;

		for (int i = 0; i < 256; i++)
		{
			var key = (Key)i;

			if (edgesAreFresh && pressedMain.IsSet(i))
			{
				Invoke(key, Event.Down);
				InvokeAny(key, Event.Down);
			}

			if (edgesAreFresh && releasedMain.IsSet(i))
			{
				Invoke(key, Event.Up);
				InvokeAny(key, Event.Up);
			}

			if (heldNow.IsSet(i) && !pressedMain.IsSet(i))
			{
				Invoke(key, Event.Held);
				InvokeAny(key, Event.Held);
			}
		}

		_prevHeld = heldNow;
		_lastEventTimestamp = state.EventTimestamp;
	}

	public readonly record struct FrameState
	{
		public required KeyBits Held { get; init; }
		public required KeyBits Pressed { get; init; }
		public required KeyBits Released { get; init; }
		public required Modifiers Modifiers { get; init; }
		public required long EventTimestamp { get; init; }
		public required long ProducedTimestamp { get; init; }
		public required int EventsThisFrame { get; init; }
	}

	internal sealed class State
	{
		private KeyBits _current;
		private KeyBits _framePressed;
		private KeyBits _frameReleased;
		private Modifiers _modifiers;
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

			UpdateModifiers((Key)vk, make);
		}

		public FrameState BuildFrameState(long timestamp) =>
			new()
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

		private void UpdateModifiers(Key vk, bool down)
		{
			switch (vk)
			{
				case Key.LShift:
					_modifiers = down ? _modifiers | Modifiers.LShift : _modifiers & ~Modifiers.LShift; break;
				case Key.RShift:
					_modifiers = down ? _modifiers | Modifiers.RShift : _modifiers & ~Modifiers.RShift; break;
				case Key.LControl:
					_modifiers = down ? _modifiers | Modifiers.LControl : _modifiers & ~Modifiers.LControl;
					break;
				case Key.RControl:
					_modifiers = down ? _modifiers | Modifiers.RControl : _modifiers & ~Modifiers.RControl;
					break;
				case Key.LAlt:
					_modifiers = down ? _modifiers | Modifiers.LAlt : _modifiers & ~Modifiers.LAlt; break;
				case Key.RAlt:
					_modifiers = down ? _modifiers | Modifiers.RAlt : _modifiers & ~Modifiers.RAlt; break;
				case Key.LWin:
					_modifiers = down ? _modifiers | Modifiers.LWin : _modifiers & ~Modifiers.LWin; break;
				case Key.RWin:
					_modifiers = down ? _modifiers | Modifiers.RWin : _modifiers & ~Modifiers.RWin; break;

				// Lock keys toggle on key "make" (ignore break).
				case Key.CapsLock:
					if (down) _modifiers ^= Modifiers.CapsLock;
					break;
				case Key.NumLock:
					if (down) _modifiers ^= Modifiers.NumLock;
					break;
				case Key.ScrollLock:
					if (down) _modifiers ^= Modifiers.ScrollLock;
					break;
			}

			// AltGr heuristic: commonly reported as RAlt + LCtrl together.
			if ((_modifiers & Modifiers.RAlt) != 0 && (_modifiers & Modifiers.LControl) != 0)
				_modifiers |= Modifiers.AltGr;
			else
				_modifiers &= ~Modifiers.AltGr;
		}
	}

	public readonly record struct KeyBits(ulong A, ulong B, ulong C, ulong D)
	{
		public static KeyBits Zero => default;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsSet(int vk) =>
			(vk >> 6) switch
			{
				0 => (A & (1UL << (vk & 63))) != 0,
				1 => (B & (1UL << (vk & 63))) != 0,
				2 => (C & (1UL << (vk & 63))) != 0,
				_ => (D & (1UL << (vk & 63))) != 0
			};

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static KeyBits operator |(KeyBits x, KeyBits y) => new(x.A | y.A, x.B | y.B, x.C | y.C, x.D | y.D);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static KeyBits operator &(KeyBits x, KeyBits y) => new(x.A & y.A, x.B & y.B, x.C & y.C, x.D & y.D);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static KeyBits operator ~(KeyBits x) => new(~x.A, ~x.B, ~x.C, ~x.D);

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

	public enum Event
	{
		Up,
		Down,
		Held
	}

	[Flags]
	public enum Modifiers : ushort
	{
		None = 0,
		LShift = 1 << 0,
		RShift = 1 << 1,
		LControl = 1 << 2,
		RControl = 1 << 3,
		LAlt = 1 << 4,
		RAlt = 1 << 5,
		LWin = 1 << 6,
		RWin = 1 << 7,
		CapsLock = 1 << 8,
		NumLock = 1 << 9,
		ScrollLock = 1 << 10,
		AltGr = 1 << 11
	}

	public enum Key
	{
		None = 0x00,
		Backspace = 0x08,
		Tab = 0x09,
		Enter = 0x0D,
		Pause = 0x13,
		CapsLock = 0x14,
		Escape = 0x1B,
		Space = 0x20,
		PageUp = 0x21,
		PageDown = 0x22,
		End = 0x23,
		Home = 0x24,
		LArrow = 0x25,
		UArrow = 0x26,
		RArrow = 0x27,
		DArrow = 0x28,
		Insert = 0x2D,
		Delete = 0x2E,
		Zero = 0x30,
		One = 0x31,
		Two = 0x32,
		Three = 0x33,
		Four = 0x34,
		Five = 0x35,
		Six = 0x36,
		Seven = 0x37,
		Eight = 0x38,
		Nine = 0x39,
		A = 0x41,
		B = 0x42,
		C = 0x43,
		D = 0x44,
		E = 0x45,
		F = 0x46,
		G = 0x47,
		H = 0x48,
		I = 0x49,
		J = 0x4A,
		K = 0x4B,
		L = 0x4C,
		M = 0x4D,
		N = 0x4E,
		O = 0x4F,
		P = 0x50,
		Q = 0x51,
		R = 0x52,
		S = 0x53,
		T = 0x54,
		U = 0x55,
		V = 0x56,
		W = 0x57,
		X = 0x58,
		Y = 0x59,
		Z = 0x5A,
		NumPadZero = 0x60,
		NumPadOne = 0x61,
		NumPadTwo = 0x62,
		NumPadThree = 0x63,
		NumPadFour = 0x64,
		NumPadFive = 0x65,
		NumPadSix = 0x66,
		NumPadSeven = 0x67,
		NumPadEight = 0x68,
		NumPadNine = 0x69,
		Multiply = 0x6A,
		Add = 0x6B,
		Subtract = 0x6D,
		Decimal = 0x6E,
		Divide = 0x6F,
		F1 = 0x70,
		F2 = 0x71,
		F3 = 0x72,
		F4 = 0x73,
		F5 = 0x74,
		F6 = 0x75,
		F7 = 0x76,
		F8 = 0x77,
		F9 = 0x78,
		F10 = 0x79,
		F11 = 0x7A,
		F12 = 0x7B,
		F13 = 0x7C,
		F14 = 0x7D,
		F15 = 0x7E,
		F16 = 0x7F,
		F17 = 0x80,
		F18 = 0x81,
		F19 = 0x82,
		F20 = 0x83,
		F21 = 0x84,
		F22 = 0x85,
		F23 = 0x86,
		F24 = 0x87,
		LShift = 0xA0,
		RShift = 0xA1,
		LControl = 0xA2,
		RControl = 0xA3,
		LAlt = 0xA4,
		RAlt = 0xA5,
		PrintScreen = 0x2C,
		ScrollLock = 0x91,
		NumLock = 0x90,
		Menu = 0x5D,
		LWin = 0x5B,
		RWin = 0x5C,
		Semicolon = 0xBA,
		Plus = 0xBB,
		Comma = 0xBC,
		Minus = 0xBD,
		Period = 0xBE,
		Slash = 0xBF,
		Tilde = 0xC0,
		LeftBracket = 0xDB,
		Backslash = 0xDC,
		RightBracket = 0xDD,
		Quote = 0xDE
	}
}

public sealed class Mouse
{
	private FrameState _state;
	private Button _prevDown;
	private long _lastEventTimestamp;

	public bool Enabled { get; set; } = true;

	public double LastLatencyMs { get; private set; }
	public double AverageLatencyMs { get; private set; }

	private readonly Dictionary<(Button, Event), Action> _handlers = new();

	public (int x, int y) Delta => _state.Delta;
	public double ScrollDelta => _state.ScrollDelta;
	public (int x, int y) Position => _state.Position;
	public int Wheel => _state.Wheel;
	public int HWheel => _state.HWheel;

	public Button Held => _state.Held;
	public Button Pressed => _state.Pressed;
	public Button Released => _state.Released;

	public ref Action OnMove => ref OnEvent(Button.None, Event.Move);
	public ref Action OnWheel => ref OnEvent(Button.None, Event.Wheel);
	public ref Action OnHWheel => ref OnEvent(Button.None, Event.HWheel);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ref Action OnEvent(Button button, Event type) =>
		ref CollectionsMarshal.GetValueRefOrAddDefault(_handlers, (button, type), out _)!;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Invoke(Button b, Event e)
	{
		if (_handlers.TryGetValue((b, e), out var h)) h?.Invoke();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool IsButtonHeld(Button button) => (Held & button) != 0;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool WasButtonPressed(Button button) => (Pressed & button) != 0;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool WasButtonReleased(Button button) => (Released & button) != 0;

	internal void Update(in FrameState state, long timestamp)
	{
		_state = state;

		if (state.EventsThisFrame > 0)
		{
			var endToEndMs = (timestamp - state.EventTimestamp) * Time.ToMilliseconds;
			LastLatencyMs = endToEndMs;
			AverageLatencyMs = AverageLatencyMs == 0 ? endToEndMs : (AverageLatencyMs * 0.9 + endToEndMs * 0.1);
		}

		bool edgesAreFresh = state.EventTimestamp != _lastEventTimestamp;

		Dispatch(Button.Left, state, edgesAreFresh);
		Dispatch(Button.Right, state, edgesAreFresh);
		Dispatch(Button.Middle, state, edgesAreFresh);
		Dispatch(Button.XButton1, state, edgesAreFresh);
		Dispatch(Button.XButton2, state, edgesAreFresh);

		if (edgesAreFresh && ((state.Delta.X | state.Delta.Y) != 0))
			Invoke(Button.None, Event.Move);
		if (edgesAreFresh && state.Wheel != 0)
			Invoke(Button.None, Event.Wheel);
		if (edgesAreFresh && state.HWheel != 0)
			Invoke(Button.None, Event.HWheel);

		_prevDown = state.Held;
		_lastEventTimestamp = state.EventTimestamp;

		return;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		void Dispatch(Button b, in FrameState s, bool fresh)
		{
			bool pressed = (s.Pressed & b) != 0;
			bool released = (s.Released & b) != 0;
			bool held = (s.Held & b) != 0;

			if (fresh && pressed) Invoke(b, Event.Down);
			if (fresh && released) Invoke(b, Event.Up);
			if (held && !pressed) Invoke(b, Event.Held);
		}
	}

	public readonly record struct FrameState
	{
		public required (int X, int Y) Delta { get; init; }
		public required (int X, int Y) Position { get; init; }
		public required int Wheel { get; init; }
		public required int HWheel { get; init; }
		public required double ScrollDelta { get; init; }
		public required Button Held { get; init; }
		public required Button Pressed { get; init; }
		public required Button Released { get; init; }
		public required long EventTimestamp { get; init; }
		public required long ProducedTimestamp { get; init; }
		public required int EventsThisFrame { get; init; }
	}

	internal sealed class State
	{
		private (int x, int y) _lastDelta;
		private int _wheel;
		private int _hWheel;
		private Button _held;
		private Button _pressed;
		private Button _released;

		public long LastEventTimestamp { get; set; }
		public int EventsThisFrame { get; set; }

		private long _lastWheelTimestamp;
		private double _scrollDelta;

		public void ProcessState(RAWMOUSE mouse, long timestamp)
		{
			LastEventTimestamp = timestamp;
			EventsThisFrame++;

			_lastDelta.x += mouse.lLastX;
			_lastDelta.y += mouse.lLastY;

			var flags = mouse.Anonymous.Anonymous.usButtonFlags;
			if ((flags & RI_MOUSE_LEFT_BUTTON_DOWN) != 0)
			{
				_held |= Button.Left;
				_pressed |= Button.Left;
			}

			if ((flags & RI_MOUSE_LEFT_BUTTON_UP) != 0)
			{
				_held &= ~Button.Left;
				_released |= Button.Left;
			}

			if ((flags & RI_MOUSE_RIGHT_BUTTON_DOWN) != 0)
			{
				_held |= Button.Right;
				_pressed |= Button.Right;
			}

			if ((flags & RI_MOUSE_RIGHT_BUTTON_UP) != 0)
			{
				_held &= ~Button.Right;
				_released |= Button.Right;
			}

			if ((flags & RI_MOUSE_MIDDLE_BUTTON_DOWN) != 0)
			{
				_held |= Button.Middle;
				_pressed |= Button.Middle;
			}

			if ((flags & RI_MOUSE_MIDDLE_BUTTON_UP) != 0)
			{
				_held &= ~Button.Middle;
				_released |= Button.Middle;
			}

			if ((flags & RI_MOUSE_BUTTON_4_DOWN) != 0)
			{
				_held |= Button.XButton1;
				_pressed |= Button.XButton1;
			}

			if ((flags & RI_MOUSE_BUTTON_4_UP) != 0)
			{
				_held &= ~Button.XButton1;
				_released |= Button.XButton1;
			}

			if ((flags & RI_MOUSE_BUTTON_5_DOWN) != 0)
			{
				_held |= Button.XButton2;
				_pressed |= Button.XButton2;
			}

			if ((flags & RI_MOUSE_BUTTON_5_UP) != 0)
			{
				_held &= ~Button.XButton2;
				_released |= Button.XButton2;
			}

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

		public FrameState BuildFrameState(long timestamp) =>
			new()
			{
				Delta = _lastDelta,
				Position = (0, 0), //Window.GetCursorPosition(),
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

	[Flags]
	public enum Button : uint
	{
		None = 0,
		Left = 1u << 0,
		Right = 1u << 1,
		Middle = 1u << 2,
		XButton1 = 1u << 3,
		XButton2 = 1u << 4,
	}

	public enum Event
	{
		Up,
		Down,
		Held,
		Wheel,
		HWheel,
		Move
	}
}