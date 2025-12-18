// using System;
// using Windows.Win32.Foundation;
// using Windows.Win32.UI.Input;
// using Windows.Win32.UI.Input.KeyboardAndMouse;
// using Vyrse.Collections;
// using Vyrse.Core;
// using Vyrse.Interop.Windows.Win32;
//
// namespace Vyrse.Platform.Windows.Win32;
//
// using static Winuser;
//
// [Discoverable]
// public sealed class Input_Deprecated : Component<Input_Deprecated>, IComponent
// {
// 	internal Queue<Keyboard.RawEvent>.DoubleBuffered KeyboardEventQueue { get; } = new(512);
// 	internal Queue<Mouse.RawEvent>.DoubleBuffered MouseEventQueue { get; } = new(1024, Queue.Policies.Capacity.DropNewest);
//
// 	public static Keyboard Keyboard { get; } = new();
// 	public static Mouse Mouse { get; } = new();
//
// 	public void Initialize() { }
//
// 	public void Start()
// 	{
// 		var window = (Window)Owner;
// 		var hwnd = window.Hwnd;
//
// 		ReadOnlySpan<RAWINPUTDEVICE> rid =
// 		[
// 			new()
// 			{
// 				usUsagePage = 0x01,
// 				usUsage = 0x02,
// 				dwFlags = RAWINPUTDEVICE_FLAGS.RIDEV_DEVNOTIFY,
// 				hwndTarget = hwnd
// 			},
// 			new()
// 			{
// 				usUsagePage = 0x01,
// 				usUsage = 0x06,
// 				dwFlags = RAWINPUTDEVICE_FLAGS.RIDEV_DEVNOTIFY | RAWINPUTDEVICE_FLAGS.RIDEV_NOLEGACY,
// 				hwndTarget = hwnd
// 			}
// 		];
//
// 		RegisterRawInputDevices(rid).ThrowIfFalse();
// 	}
//
// 	public unsafe LRESULT ProcessRawInput(in LPARAM lParam)
// 	{
// 		var hRaw = new HRAWINPUT(lParam);
//
// 		Span<byte> buf = stackalloc byte[512];
// 		if (!hRaw.GetDataBuffer(ref buf))
// 		{
// 			Console.WriteLine("Failed to get raw input data.");
// 			return 0;
// 		}
//
// 		ref readonly var header = ref RAWINPUTHEADER.AsRef(buf);
// 		var info = RID_DEVICE_INFO.GetDeviceInfo(header.hDevice);
//
// 		switch (info.GetMessageType())
// 		{
// 			case RIM.MOUSE:
// 			{
// 				RAWMOUSE mouse = RAWMOUSE.Read(buf);
//
// 				MouseEventQueue.Push(new Mouse.RawEvent
// 				{
//
// 				});
//
// 				break;
// 			}
// 			case RIM.KEYBOARD:
// 			{
// 				RAWKEYBOARD keyboard = RAWKEYBOARD.Read(buf);
//
// 				if (keyboard.IsSynthetic)
// 					break;
//
// 				ushort extended = keyboard.MakeCode;
//
// 				if (keyboard.IsE0)
// 					extended |= (ushort)ScanCode.E0;
// 				else if (keyboard.IsE1)
// 					extended |= (ushort)ScanCode.E1;
//
// 				var scancode = (ScanCode)extended;
//
// 				Console.WriteLine($"Key = {scancode}, MakeCode = {keyboard.MakeCode}, IsE0 = {keyboard.IsE0}, IsE1 = {keyboard.IsE1}, IsMake = {keyboard.IsMake}, IsBreak = {keyboard.IsBreak}");
//
//
// 				KeyboardEventQueue.Push(new Keyboard.RawEvent
// 				{
//
// 				});
//
// 				break;
// 			}
// 			case RIM.HID:
// 			{
//
// 				Console.WriteLine("HID input received.");
//
// 				break;
// 			}
// 		}
//
// 		return 0;
// 	}
//
// 	public void PreUpdate() { }
//
// 	public void Update()
// 	{
// 		Keyboard.Update(KeyboardEventQueue.SwapAndRead());
// 		Mouse.Update(MouseEventQueue.SwapAndRead());
// 	}
//
// 	public void PostUpdate() { }
//
// 	public void FixedUpdate() { }
//
// 	public void Shutdown() { }
// }
//
// public sealed class Keyboard
// {
// 	internal readonly struct RawEvent
// 	{
//
// 	}
//
// 	internal void Update(ReadOnlySpan<RawEvent> events)
// 	{
//
// 	}
//
// }
//
// public sealed class Mouse
// {
// 	internal readonly struct RawEvent
// 	{
//
// 	}
//
// 	internal void Update(ReadOnlySpan<RawEvent> events)
// 	{
//
// 	}
// }