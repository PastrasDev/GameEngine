/*using Vyrse.Core;

namespace Vyrse.Platform.Windows.Win32;

[Discoverable(Enabled = false)]
public sealed class RawInput : Component<RawInput>, IComponent
{
	private static readonly unsafe uint s_rawInputDeviceSize = (uint)sizeof(RAWINPUTDEVICE);
	private static readonly unsafe uint s_rawInputHeaderSize = (uint)sizeof(RAWINPUTHEADER);

	private readonly Keyboard.State _kState = new();
	private readonly Mouse.State _mState = new();
	private long _frameId;

	private enum RimType : uint
	{
		Mouse = 0,
		Keyboard = 1,
		Hid = 2
	}

	public void Load()
	{

	}

	public void Initialize()
	{

	}

	public unsafe void Start()
	{
		var window = (WindowModule)Owner;
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

		Require(PInvoke.RegisterRawInputDevices(rid, 2u, s_rawInputDeviceSize), true);
	}

	public unsafe void ProcessRawInput(nint lParam, long timestamp)
	{
		uint dwSize = 0;
		if (PInvoke.GetRawInputData((HRAWINPUT)lParam, RAW_INPUT_DATA_COMMAND_FLAGS.RID_INPUT, null, &dwSize, s_rawInputHeaderSize) != 0 || dwSize == 0) return;

		byte* buf = stackalloc byte[(int)dwSize];
		if (PInvoke.GetRawInputData((HRAWINPUT)lParam, RAW_INPUT_DATA_COMMAND_FLAGS.RID_INPUT, buf, &dwSize, s_rawInputHeaderSize) == unchecked((uint)-1)) return;

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

	public void PreUpdate()
	{

	}

	public void Update()
	{
		var now = Time.Now();
		var kState = _kState.BuildFrameState(now);
		var mState = _mState.BuildFrameState(now);
		long timestamp = kState.EventsThisFrame == 0 && mState.EventsThisFrame == 0 ? now : max(kState.ProducedTimestamp, mState.ProducedTimestamp);

		Send(new Stream<InputSnapshot>
		{
			Data = new InputSnapshot
			{
				FrameId = _frameId++,
				Timestamp = timestamp,
				KState = kState,
				MState = mState
			}
		});
	}


	public void PostUpdate()
	{
		_kState.Reset();
		_mState.Reset();
	}

	public void Shutdown()
	{

	}
}*/