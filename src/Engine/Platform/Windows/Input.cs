using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Engine.Utilities;

namespace Engine.Platform.Windows;

public sealed class Input
{
	public Keyboard Keyboard { get; } = new();
	public Mouse Mouse { get; } = new();

	internal void Update(in InputSnapshot snapshot)
	{
		Keyboard.Update(snapshot.KeyboardState, snapshot.Timestamp);
		Mouse.Update(snapshot.MouseState, snapshot.Timestamp);
	}
}

public sealed class Keyboard
{
	private KeyboardFrameState _state;
	private KeyBits _prevHeld;

	public bool Enabled { get; set; } = true;

	public double LastLatencyMs { get; private set; }
	public double AverageLatencyMs { get; private set; }

	private readonly Dictionary<(Key, Event), Action> _handlers = new();
	private readonly Dictionary<Event, Action<Key>> _anyKeyHandlers = new();

	[MethodImpl(MethodImplOptions.AggressiveInlining)] public ref Action OnEvent(Key key, Event type) => ref CollectionsMarshal.GetValueRefOrAddDefault(_handlers, (key, type), out _)!;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public ref Action<Key> OnAny(Event type) => ref CollectionsMarshal.GetValueRefOrAddDefault(_anyKeyHandlers, type, out _)!;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] private void Invoke(Key key, Event type) { if (_handlers.TryGetValue((key, type), out var h)) h?.Invoke(); }
	[MethodImpl(MethodImplOptions.AggressiveInlining)] private void InvokeAny(Key key, Event type) { if (_anyKeyHandlers.TryGetValue(type, out var h)) h?.Invoke(key); }

	[MethodImpl(MethodImplOptions.AggressiveInlining)] public bool IsKeyHeld(Key key) => _state.Held.IsSet((int)key);
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public bool WasKeyPressed(Key key) => _state.Pressed.IsSet((int)key);
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public bool WasKeyReleased(Key key) => _state.Released.IsSet((int)key);
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public bool IsChord(Modifiers modifier, Key key) => (_state.Modifiers & modifier) == modifier && IsKeyHeld(key);
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public bool PressedChord(Modifiers modifier, Key key) => (_state.Modifiers & modifier) == modifier && WasKeyPressed(key);

	internal void Update(in KeyboardFrameState state, long timestamp)
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

		for (int i = 0; i < 256; i++)
		{
			var key = (Key)i;

			if (pressedMain.IsSet(i))
			{
				Invoke(key, Event.Down);
				InvokeAny(key, Event.Down);
			}

			if (releasedMain.IsSet(i))
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
		None       = 0,
		LShift     = 1 << 0,
		RShift     = 1 << 1,
		LControl   = 1 << 2,
		RControl   = 1 << 3,
		LAlt       = 1 << 4,
		RAlt       = 1 << 5,
		LWin       = 1 << 6,
		RWin       = 1 << 7,
		CapsLock   = 1 << 8,
		NumLock    = 1 << 9,
		ScrollLock = 1 << 10,
		AltGr      = 1 << 11
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
	private MouseFrameState _state;
	private Button _prevDown;

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

	[MethodImpl(MethodImplOptions.AggressiveInlining)] public ref Action OnEvent(Button button, Event type) => ref CollectionsMarshal.GetValueRefOrAddDefault(_handlers, (button, type), out _)!;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] private void Invoke(Button b, Event e) { if (_handlers.TryGetValue((b, e), out var h)) h?.Invoke(); }
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public bool IsButtonHeld(Button button) => (Held & button) != 0;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public bool WasButtonPressed(Button button) => (Pressed & button) != 0;
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public bool WasButtonReleased(Button button) => (Released & button) != 0;

	internal void Update(in MouseFrameState state, long timestamp)
	{
		_state = state;

		if (state.EventsThisFrame > 0)
		{
			var endToEndMs = (timestamp - state.EventTimestamp) * Time.ToMilliseconds;
			LastLatencyMs = endToEndMs;
			AverageLatencyMs = AverageLatencyMs == 0 ? endToEndMs : (AverageLatencyMs * 0.9 + endToEndMs * 0.1);

		}

		Dispatch(Button.Left,   state);
		Dispatch(Button.Right,  state);
		Dispatch(Button.Middle, state);
		Dispatch(Button.XButton1, state);
		Dispatch(Button.XButton2, state);

		if ((state.Delta.X | state.Delta.Y) != 0)
			Invoke(Button.None, Event.Move);
		if (state.Wheel != 0)
			Invoke(Button.None, Event.Wheel);
		if (state.HWheel != 0)
			Invoke(Button.None, Event.HWheel);

		_prevDown = state.Held;

		return;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		void Dispatch(Button b, in MouseFrameState s)
		{
			bool pressed  = (s.Pressed  & b) != 0;
			bool released = (s.Released & b) != 0;
			bool held     = (s.Held     & b) != 0;

			if (pressed) Invoke(b, Event.Down);
			if (released) Invoke(b, Event.Up);
			if (held && !pressed) Invoke(b, Event.Held);
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