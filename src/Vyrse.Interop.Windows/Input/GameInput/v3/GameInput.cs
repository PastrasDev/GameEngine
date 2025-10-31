using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using Windows.Win32.Foundation;
using Windows.Win32.System.Com;
using Vyrse.Interop.Windows.Primitives;

namespace Windows.Input.GameInput.v3;

using GameInputCallbackToken = ulong;

using unsafe GameInputReadingCallback = delegate* unmanaged[Stdcall]<ulong, nint, nint, void>;
using unsafe GameInputDeviceCallback = delegate* unmanaged[Stdcall]<ulong, nint, nint, ulong, GameInputDeviceStatus, GameInputDeviceStatus, void>;
using unsafe GameInputSystemButtonCallback = delegate* unmanaged[Stdcall]<ulong, nint, nint, ulong, GameInputSystemButtons, GameInputSystemButtons, void>;
using unsafe GameInputKeyboardLayoutCallback = delegate* unmanaged[Stdcall]<ulong, nint, nint, ulong, uint, uint, void>;

public static partial class GameInput
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static HRESULT GameInputCreate(out IGameInput gameInput) => Unmanaged.GameInputCreate(out gameInput);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IGameInput GameInputCreate() { GameInputCreate(out var gameInput).ThrowOnFailure(); return gameInput; }

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static HRESULT GameInputInitialize(in Guid riid, out nint ppv) => Unmanaged.GameInputInitialize(in riid, out ppv);

	internal static partial class Unmanaged
	{
		public const string Library = "gameinput.dll";

		[LibraryImport(Library, EntryPoint = "GameInputCreate")]
		public static unsafe partial HRESULT GameInputCreate(out IGameInput gameInput);

		[LibraryImport(Library, EntryPoint = "GameInputInitialize")]
		public static unsafe partial HRESULT GameInputInitialize(in Guid riid, out nint ppv);
	}

	public const GameInputGamepadButtons GameInputGamepadModuleSystemDuo = GameInputGamepadButtons.GameInputGamepadMenu | GameInputGamepadButtons.GameInputGamepadView;
	public const GameInputGamepadButtons GameInputGamepadModuleDpad = GameInputGamepadButtons.GameInputGamepadDPadUp | GameInputGamepadButtons.GameInputGamepadDPadDown | GameInputGamepadButtons.GameInputGamepadDPadLeft | GameInputGamepadButtons.GameInputGamepadDPadRight;
	public const GameInputGamepadButtons GameInputGamepadModuleShoulders = GameInputGamepadButtons.GameInputGamepadLeftShoulder | GameInputGamepadButtons.GameInputGamepadRightShoulder;
	public const GameInputGamepadButtons GameInputGamepadModuleTriggers = GameInputGamepadButtons.GameInputGamepadLeftTriggerButton | GameInputGamepadButtons.GameInputGamepadRightTriggerButton;
	public const GameInputGamepadButtons GameInputGamepadModuleThumbsticks = GameInputGamepadButtons.GameInputGamepadLeftThumbstickUp | GameInputGamepadButtons.GameInputGamepadLeftThumbstickDown | GameInputGamepadButtons.GameInputGamepadLeftThumbstickLeft | GameInputGamepadButtons.GameInputGamepadLeftThumbstickRight | GameInputGamepadButtons.GameInputGamepadRightThumbstickUp | GameInputGamepadButtons.GameInputGamepadRightThumbstickDown | GameInputGamepadButtons.GameInputGamepadRightThumbstickLeft | GameInputGamepadButtons.GameInputGamepadRightThumbstickRight;
	public const GameInputGamepadButtons GameInputGamepadModulePaddles2 = GameInputGamepadButtons.GameInputGamepadPaddleLeft1 | GameInputGamepadButtons.GameInputGamepadPaddleRight1;
	public const GameInputGamepadButtons GameInputGamepadModulePaddles4 = GameInputGamepadButtons.GameInputGamepadPaddleLeft1 | GameInputGamepadButtons.GameInputGamepadPaddleLeft2 | GameInputGamepadButtons.GameInputGamepadPaddleRight1 | GameInputGamepadButtons.GameInputGamepadPaddleRight2;
	public const GameInputGamepadButtons GameInputGamepadLayoutBasic = GameInputGamepadModuleSystemDuo | GameInputGamepadModuleDpad | GameInputGamepadButtons.GameInputGamepadA | GameInputGamepadButtons.GameInputGamepadB;
	public const GameInputGamepadButtons GameInputGamepadLayoutButtons = GameInputGamepadLayoutBasic | GameInputGamepadButtons.GameInputGamepadX | GameInputGamepadButtons.GameInputGamepadY | GameInputGamepadModuleShoulders;
	public const GameInputGamepadButtons GameInputGamepadLayoutStandard = GameInputGamepadLayoutButtons | GameInputGamepadModuleTriggers | GameInputGamepadModuleThumbsticks | GameInputGamepadButtons.GameInputGamepadLeftThumbstick | GameInputGamepadButtons.GameInputGamepadRightThumbstick;
	public const GameInputGamepadButtons GameInputGamepadLayoutElite = GameInputGamepadLayoutStandard | GameInputGamepadModulePaddles4;

	public const uint GAMEINPUT_HAPTIC_MAX_LOCATIONS = 8;
	public const uint GAMEINPUT_HAPTIC_MAX_AUDIO_ENDPOINT_ID_SIZE = 256;
	public const uint GAMEINPUT_MAX_SWITCH_STATES = 8;
}

#region Enums

public enum GameInputKind
{
	GameInputKindUnknown = 0x00000000,
	GameInputKindRawDeviceReport = 0x00000001,
	GameInputKindControllerAxis = 0x00000002,
	GameInputKindControllerButton = 0x00000004,
	GameInputKindControllerSwitch = 0x00000008,
	GameInputKindController = 0x0000000E,
	GameInputKindKeyboard = 0x00000010,
	GameInputKindMouse = 0x00000020,
	GameInputKindSensors = 0x00000040,
	GameInputKindArcadeStick = 0x00010000,
	GameInputKindFlightStick = 0x00020000,
	GameInputKindGamepad = 0x00040000,
	GameInputKindRacingWheel = 0x00080000,
};

public enum GameInputEnumerationKind
{
	GameInputNoEnumeration = 0,
	GameInputAsyncEnumeration = 1,
	GameInputBlockingEnumeration = 2
};

public enum GameInputFocusPolicy
{
	GameInputDefaultFocusPolicy = 0x00000000,
	GameInputExclusiveForegroundInput = 0x00000002,
	GameInputExclusiveForegroundGuideButton = 0x00000008,
	GameInputExclusiveForegroundShareButton = 0x00000020,
	GameInputEnableBackgroundInput = 0x00000040,
	GameInputEnableBackgroundGuideButton = 0x00000080,
	GameInputEnableBackgroundShareButton = 0x00000100
};

public enum GameInputSwitchKind
{
	GameInputUnknownSwitchKind = -1,
	GameInput2WaySwitch = 0,
	GameInput4WaySwitch = 1,
	GameInput8WaySwitch = 2
};

public enum GameInputSwitchPosition
{
	GameInputSwitchCenter = 0,
	GameInputSwitchUp = 1,
	GameInputSwitchUpRight = 2,
	GameInputSwitchRight = 3,
	GameInputSwitchDownRight = 4,
	GameInputSwitchDown = 5,
	GameInputSwitchDownLeft = 6,
	GameInputSwitchLeft = 7,
	GameInputSwitchUpLeft = 8
};

public enum GameInputKeyboardKind
{
	GameInputUnknownKeyboard = -1,
	GameInputAnsiKeyboard = 0,
	GameInputIsoKeyboard = 1,
	GameInputKsKeyboard = 2,
	GameInputAbntKeyboard = 3,
	GameInputJisKeyboard = 4
};

public enum GameInputMouseButtons
{
	GameInputMouseNone = 0x00000000,
	GameInputMouseLeftButton = 0x00000001,
	GameInputMouseRightButton = 0x00000002,
	GameInputMouseMiddleButton = 0x00000004,
	GameInputMouseButton4 = 0x00000008,
	GameInputMouseButton5 = 0x00000010,
	GameInputMouseWheelTiltLeft = 0x00000020,
	GameInputMouseWheelTiltRight = 0x00000040
};

public enum GameInputMousePositions
{
	GameInputMouseNoPosition = 0x00000000,
	GameInputMouseAbsolutePosition = 0x00000001,
	GameInputMouseRelativePosition = 0x00000002
};

public enum GameInputSensorsKind
{
	GameInputSensorsNone = 0x00000000,
	GameInputSensorsAccelerometer = 0x00000001,
	GameInputSensorsGyrometer = 0x00000002,
	GameInputSensorsCompass = 0x00000004,
	GameInputSensorsOrientation = 0x00000008
};

public enum GameInputSensorAccuracy
{
	GameInputSensorAccuracyUnknown = 0x00000000,
	GameInputSensorAccuracyUnreliable = 0x00000001,
	GameInputSensorAccuracyApproximate = 0x00000002,
	GameInputSensorAccuracyHigh = 0x00000003
};

public enum GameInputArcadeStickButtons
{
	GameInputArcadeStickNone = 0x00000000,
	GameInputArcadeStickMenu = 0x00000001,
	GameInputArcadeStickView = 0x00000002,
	GameInputArcadeStickUp = 0x00000004,
	GameInputArcadeStickDown = 0x00000008,
	GameInputArcadeStickLeft = 0x00000010,
	GameInputArcadeStickRight = 0x00000020,
	GameInputArcadeStickAction1 = 0x00000040,
	GameInputArcadeStickAction2 = 0x00000080,
	GameInputArcadeStickAction3 = 0x00000100,
	GameInputArcadeStickAction4 = 0x00000200,
	GameInputArcadeStickAction5 = 0x00000400,
	GameInputArcadeStickAction6 = 0x00000800,
	GameInputArcadeStickSpecial1 = 0x00001000,
	GameInputArcadeStickSpecial2 = 0x00002000
};

public enum GameInputFlightStickButtons
{
	GameInputFlightStickNone = 0x00000000,
	GameInputFlightStickMenu = 0x00000001,
	GameInputFlightStickView = 0x00000002,
	GameInputFlightStickFirePrimary = 0x00000004,
	GameInputFlightStickFireSecondary = 0x00000008,
	GameInputFlightStickHatSwitchUp = 0x00000010,
	GameInputFlightStickHatSwitchDown = 0x00000020,
	GameInputFlightStickHatSwitchLeft = 0x00000040,
	GameInputFlightStickHatSwitchRight = 0x00000080,
	GameInputFlightStickA = 0x00000100,
	GameInputFlightStickB = 0x00000200,
	GameInputFlightStickX = 0x00000400,
	GameInputFlightStickY = 0x00000800,
	GameInputFlightStickLeftShoulder = 0x00001000,
	GameInputFlightStickRightShoulder = 0x00002000,
};

[Flags]
public enum GameInputGamepadButtons
{
	GameInputGamepadNone = 0x00000000,
	GameInputGamepadMenu = 0x00000001,
	GameInputGamepadView = 0x00000002,
	GameInputGamepadA = 0x00000004,
	GameInputGamepadB = 0x00000008,
	GameInputGamepadC = 0x00004000,
	GameInputGamepadX = 0x00000010,
	GameInputGamepadY = 0x00000020,
	GameInputGamepadZ = 0x00008000,
	GameInputGamepadDPadUp = 0x00000040,
	GameInputGamepadDPadDown = 0x00000080,
	GameInputGamepadDPadLeft = 0x00000100,
	GameInputGamepadDPadRight = 0x00000200,
	GameInputGamepadLeftShoulder = 0x00000400,
	GameInputGamepadRightShoulder = 0x00000800,
	GameInputGamepadLeftTriggerButton = 0x00010000,
	GameInputGamepadRightTriggerButton = 0x00020000,
	GameInputGamepadLeftThumbstick = 0x00001000,
	GameInputGamepadLeftThumbstickUp = 0x00040000,
	GameInputGamepadLeftThumbstickDown = 0x00080000,
	GameInputGamepadLeftThumbstickLeft = 0x00100000,
	GameInputGamepadLeftThumbstickRight = 0x00200000,
	GameInputGamepadRightThumbstick = 0x00002000,
	GameInputGamepadRightThumbstickUp = 0x00400000,
	GameInputGamepadRightThumbstickDown = 0x00800000,
	GameInputGamepadRightThumbstickLeft = 0x01000000,
	GameInputGamepadRightThumbstickRight = 0x02000000,
	GameInputGamepadPaddleLeft1 = 0x04000000,
	GameInputGamepadPaddleLeft2 = 0x08000000,
	GameInputGamepadPaddleRight1 = 0x10000000,
	GameInputGamepadPaddleRight2 = 0x20000000,
};

public enum GameInputRawDeviceReportKind
{
	GameInputRawInputReport = 0,
	GameInputRawOutputReport = 1,
};

public enum GameInputRacingWheelButtons
{
	GameInputRacingWheelNone = 0x00000000,
	GameInputRacingWheelMenu = 0x00000001,
	GameInputRacingWheelView = 0x00000002,
	GameInputRacingWheelPreviousGear = 0x00000004,
	GameInputRacingWheelNextGear = 0x00000008,
	GameInputRacingWheelA = 0x00000100,
	GameInputRacingWheelB = 0x00000200,
	GameInputRacingWheelX = 0x00000400,
	GameInputRacingWheelY = 0x00000800,
	GameInputRacingWheelDpadUp = 0x00000010,
	GameInputRacingWheelDpadDown = 0x00000020,
	GameInputRacingWheelDpadLeft = 0x00000040,
	GameInputRacingWheelDpadRight = 0x00000080,
	GameInputRacingWheelLeftThumbstick = 0x00001000,
	GameInputRacingWheelRightThumbstick = 0x00002000,
};

public enum GameInputSystemButtons
{
	GameInputSystemButtonNone = 0x00000000,
	GameInputSystemButtonGuide = 0x00000001,
	GameInputSystemButtonShare = 0x00000002
};

public enum GameInputFlightStickAxes
{
	GameInputFlightStickAxesNone = 0x00000000,
	GameInputFlightStickRoll = 0x00000010,
	GameInputFlightStickPitch = 0x00000020,
	GameInputFlightStickYaw = 0x00000040,
	GameInputFlightStickThrottle = 0x00000080,
};

public enum GameInputGamepadAxes
{
	GameInputGamepadAxesNone = 0x00000000,
	GameInputGamepadLeftTrigger = 0x00000001,
	GameInputGamepadRightTrigger = 0x00000002,
	GameInputGamepadLeftThumbstickX = 0x00000004,
	GameInputGamepadLeftThumbstickY = 0x00000008,
	GameInputGamepadRightThumbstickX = 0x00000010,
	GameInputGamepadRightThumbstickY = 0x00000020,
};

public enum GameInputRacingWheelAxes
{
	GameInputRacingWheelAxesNone = 0x00000000,
	GameInputRacingWheelSteering = 0x00000100,
	GameInputRacingWheelThrottle = 0x00000200,
	GameInputRacingWheelBrake = 0x00000400,
	GameInputRacingWheelClutch = 0x00000800,
	GameInputRacingWheelHandbrake = 0x00001000,
	GameInputRacingWheelPatternShifter = 0x00002000,
};

public enum GameInputDeviceStatus : uint
{
	GameInputDeviceNoStatus = 0x00000000,
	GameInputDeviceConnected = 0x00000001,
	GameInputDeviceHapticInfoReady = 0x00200000,
	GameInputDeviceAnyStatus = 0xFFFFFFFF
};

public enum GameInputDeviceFamily
{
	GameInputFamilyVirtual = -1,
	GameInputFamilyUnknown = 0,
	GameInputFamilyXboxOne = 1,
	GameInputFamilyXbox360 = 2,
	GameInputFamilyHid = 3,
	GameInputFamilyI8042 = 4,
	GameInputFamilyAggregate = 5,
};

public enum GameInputLabel
{
	GameInputLabelUnknown = -1,
	GameInputLabelNone = 0,
	GameInputLabelXboxGuide = 1,
	GameInputLabelXboxBack = 2,
	GameInputLabelXboxStart = 3,
	GameInputLabelXboxMenu = 4,
	GameInputLabelXboxView = 5,
	GameInputLabelXboxA = 7,
	GameInputLabelXboxB = 8,
	GameInputLabelXboxX = 9,
	GameInputLabelXboxY = 10,
	GameInputLabelXboxDPadUp = 11,
	GameInputLabelXboxDPadDown = 12,
	GameInputLabelXboxDPadLeft = 13,
	GameInputLabelXboxDPadRight = 14,
	GameInputLabelXboxLeftShoulder = 15,
	GameInputLabelXboxLeftTrigger = 16,
	GameInputLabelXboxLeftStickButton = 17,
	GameInputLabelXboxRightShoulder = 18,
	GameInputLabelXboxRightTrigger = 19,
	GameInputLabelXboxRightStickButton = 20,
	GameInputLabelXboxPaddle1 = 21,
	GameInputLabelXboxPaddle2 = 22,
	GameInputLabelXboxPaddle3 = 23,
	GameInputLabelXboxPaddle4 = 24,
	GameInputLabelLetterA = 25,
	GameInputLabelLetterB = 26,
	GameInputLabelLetterC = 27,
	GameInputLabelLetterD = 28,
	GameInputLabelLetterE = 29,
	GameInputLabelLetterF = 30,
	GameInputLabelLetterG = 31,
	GameInputLabelLetterH = 32,
	GameInputLabelLetterI = 33,
	GameInputLabelLetterJ = 34,
	GameInputLabelLetterK = 35,
	GameInputLabelLetterL = 36,
	GameInputLabelLetterM = 37,
	GameInputLabelLetterN = 38,
	GameInputLabelLetterO = 39,
	GameInputLabelLetterP = 40,
	GameInputLabelLetterQ = 41,
	GameInputLabelLetterR = 42,
	GameInputLabelLetterS = 43,
	GameInputLabelLetterT = 44,
	GameInputLabelLetterU = 45,
	GameInputLabelLetterV = 46,
	GameInputLabelLetterW = 47,
	GameInputLabelLetterX = 48,
	GameInputLabelLetterY = 49,
	GameInputLabelLetterZ = 50,
	GameInputLabelNumber0 = 51,
	GameInputLabelNumber1 = 52,
	GameInputLabelNumber2 = 53,
	GameInputLabelNumber3 = 54,
	GameInputLabelNumber4 = 55,
	GameInputLabelNumber5 = 56,
	GameInputLabelNumber6 = 57,
	GameInputLabelNumber7 = 58,
	GameInputLabelNumber8 = 59,
	GameInputLabelNumber9 = 60,
	GameInputLabelArrowUp = 61,
	GameInputLabelArrowUpRight = 62,
	GameInputLabelArrowRight = 63,
	GameInputLabelArrowDownRight = 64,
	GameInputLabelArrowDown = 65,
	GameInputLabelArrowDownLLeft = 66,
	GameInputLabelArrowLeft = 67,
	GameInputLabelArrowUpLeft = 68,
	GameInputLabelArrowUpDown = 69,
	GameInputLabelArrowLeftRight = 70,
	GameInputLabelArrowUpDownLeftRight = 71,
	GameInputLabelArrowClockwise = 72,
	GameInputLabelArrowCounterClockwise = 73,
	GameInputLabelArrowReturn = 74,
	GameInputLabelIconBranding = 75,
	GameInputLabelIconHome = 76,
	GameInputLabelIconMenu = 77,
	GameInputLabelIconCross = 78,
	GameInputLabelIconCircle = 79,
	GameInputLabelIconSquare = 80,
	GameInputLabelIconTriangle = 81,
	GameInputLabelIconStar = 82,
	GameInputLabelIconDPadUp = 83,
	GameInputLabelIconDPadDown = 84,
	GameInputLabelIconDPadLeft = 85,
	GameInputLabelIconDPadRight = 86,
	GameInputLabelIconDialClockwise = 87,
	GameInputLabelIconDialCounterClockwise = 88,
	GameInputLabelIconSliderLeftRight = 89,
	GameInputLabelIconSliderUpDown = 90,
	GameInputLabelIconWheelUpDown = 91,
	GameInputLabelIconPlus = 92,
	GameInputLabelIconMinus = 93,
	GameInputLabelIconSuspension = 94,
	GameInputLabelHome = 95,
	GameInputLabelGuide = 96,
	GameInputLabelMode = 97,
	GameInputLabelSelect = 98,
	GameInputLabelMenu = 99,
	GameInputLabelView = 100,
	GameInputLabelBack = 101,
	GameInputLabelStart = 102,
	GameInputLabelOptions = 103,
	GameInputLabelShare = 104,
	GameInputLabelUp = 105,
	GameInputLabelDown = 106,
	GameInputLabelLeft = 107,
	GameInputLabelRight = 108,
	GameInputLabelLb = 109,
	GameInputLabelLt = 110,
	GameInputLabelLsb = 111,
	GameInputLabelL1 = 112,
	GameInputLabelL2 = 113,
	GameInputLabelL3 = 114,
	GameInputLabelRb = 115,
	GameInputLabelRt = 116,
	GameInputLabelRsb = 117,
	GameInputLabelR1 = 118,
	GameInputLabelR2 = 119,
	GameInputLabelR3 = 120,
	GameInputLabelPaddleLeft1 = 121,
	GameInputLabelPaddleLeft2 = 122,
	GameInputLabelPaddleRight1 = 123,
	GameInputLabelPaddleRight2 = 124,
};

public enum GameInputFeedbackAxes
{
	GameInputFeedbackAxisNone = 0x00000000,
	GameInputFeedbackAxisLinearX = 0x00000001,
	GameInputFeedbackAxisLinearY = 0x00000002,
	GameInputFeedbackAxisLinearZ = 0x00000004,
	GameInputFeedbackAxisAngularX = 0x00000008,
	GameInputFeedbackAxisAngularY = 0x00000010,
	GameInputFeedbackAxisAngularZ = 0x00000020,
	GameInputFeedbackAxisNormal = 0x00000040
};

public enum GameInputFeedbackEffectState
{
	GameInputFeedbackStopped = 0,
	GameInputFeedbackRunning = 1,
	GameInputFeedbackPaused = 2
};

public enum GameInputForceFeedbackEffectKind
{
	GameInputForceFeedbackConstant = 0,
	GameInputForceFeedbackRamp = 1,
	GameInputForceFeedbackSineWave = 2,
	GameInputForceFeedbackSquareWave = 3,
	GameInputForceFeedbackTriangleWave = 4,
	GameInputForceFeedbackSawtoothUpWave = 5,
	GameInputForceFeedbackSawtoothDownWave = 6,
	GameInputForceFeedbackSpring = 7,
	GameInputForceFeedbackFriction = 8,
	GameInputForceFeedbackDamper = 9,
	GameInputForceFeedbackInertia = 10
};

public enum GameInputRumbleMotors
{
	GameInputRumbleNone = 0x00000000,
	GameInputRumbleLowFrequency = 0x00000001,
	GameInputRumbleHighFrequency = 0x00000002,
	GameInputRumbleLeftTrigger = 0x00000004,
	GameInputRumbleRightTrigger = 0x00000008
};

public enum GameInputElementKind
{
	GameInputElementKindNone = 0,
	GameInputElementKindAxis = 1,
	GameInputElementKindButton = 2,
	GameInputElementKindSwitch = 3
};

#endregion

#region Structs

[StructLayout(LayoutKind.Sequential)]
public struct GameInputKeyState
{
	public uint ScanCode;
	public uint CodePoint;
	public byte VirtualKey;
	public bool IsDeadKey;
};

[StructLayout(LayoutKind.Sequential)]
public struct GameInputMouseState
{
	public GameInputMouseButtons Buttons;
	public GameInputMousePositions Positions;
	public long PositionX;
	public long PositionY;
	public long AbsolutePositionX;
	public long AbsolutePositionY;
	public long WheelX;
	public long WheelY;
};

[StructLayout(LayoutKind.Sequential)]
public struct GameInputVersion
{
	public ushort Major;
	public ushort Minor;
	public ushort Build;
	public ushort Revision;
};

[StructLayout(LayoutKind.Sequential)]
public struct GameInputSensorsState
{
	public float AccelerationInGx;
	public float AccelerationInGy;
	public float AccelerationInGz;

	public float AngularVelocityInRadPerSecX;
	public float AngularVelocityInRadPerSecY;
	public float AngularVelocityInRadPerSecZ;

	public float HeadingInDegreesFromMagneticNorth;
	public GameInputSensorAccuracy HeadingAccuracy;

	public float OrientationW;
	public float OrientationX;
	public float OrientationY;
	public float OrientationZ;
};

[StructLayout(LayoutKind.Sequential)]
public struct GameInputArcadeStickState
{
	public GameInputArcadeStickButtons Buttons;
};

[StructLayout(LayoutKind.Sequential)]
public struct GameInputFlightStickState
{
	public GameInputFlightStickButtons Buttons;
	public GameInputSwitchPosition HatSwitch;
	public float Roll;
	public float Pitch;
	public float Yaw;
	public float Throttle;
};

[StructLayout(LayoutKind.Sequential)]
public struct GameInputGamepadState
{
	public GameInputGamepadButtons Buttons;
	public float LeftTrigger;
	public float RightTrigger;
	public float LeftThumbstickX;
	public float LeftThumbstickY;
	public float RightThumbstickX;
	public float RightThumbstickY;
};

[StructLayout(LayoutKind.Sequential)]
public struct GameInputRacingWheelState
{
	public GameInputRacingWheelButtons Buttons;
	public int PatternShifterGear;
	public float Wheel;
	public float Throttle;
	public float Brake;
	public float Clutch;
	public float Handbrake;
};

[StructLayout(LayoutKind.Sequential)]
public struct GameInputUsage
{
	public ushort Page;
	public ushort Id;
};

[StructLayout(LayoutKind.Sequential)]
public struct GameInputControllerSwitchInfo
{
	public FixedArray8<int> Labels;
	public GameInputSwitchKind Kind;
	public readonly Span<GameInputLabel> LabelsSpan => MemoryMarshal.Cast<int, GameInputLabel>(Labels.Span);
};

[StructLayout(LayoutKind.Sequential)]
public struct GameInputControllerInfo
{
	public uint ControllerAxisCount;
	private unsafe GameInputLabel* _controllerAxisLabels;
	public uint ControllerButtonCount;
	private unsafe GameInputLabel* _controllerButtonLabels;
	public uint ControllerSwitchCount;
	private unsafe GameInputControllerSwitchInfo* _controllerSwitchInfo;

	public readonly unsafe ReadOnlySpan<GameInputLabel> ControllerAxisLabels => new(_controllerAxisLabels, (int)ControllerAxisCount);
	public readonly unsafe ReadOnlySpan<GameInputLabel> ControllerButtonLabels => new(_controllerButtonLabels, (int)ControllerButtonCount);
	public readonly unsafe ReadOnlySpan<GameInputControllerSwitchInfo> ControllerSwitchInfo => new(_controllerSwitchInfo, (int)ControllerSwitchCount);
};

[StructLayout(LayoutKind.Sequential)]
public struct GameInputKeyboardInfo
{
	public GameInputKeyboardKind Kind;
	public uint Layout;
	public uint KeyCount;
	public uint FunctionKeyCount;
	public uint MaxSimultaneousKeys;
	public uint PlatformType;
	public uint PlatformSubtype;
};

[StructLayout(LayoutKind.Sequential)]
public struct GameInputMouseInfo
{
	public GameInputMouseButtons SupportedButtons;
	public uint SampleRate;
	public bool HasWheelX;
	public bool HasWheelY;
};

[StructLayout(LayoutKind.Sequential)]
public struct GameInputSensorsInfo
{
	public GameInputSensorsKind SupportedSensors;
};

[StructLayout(LayoutKind.Sequential)]
public struct GameInputArcadeStickInfo
{
	public GameInputLabel MenuButtonLabel;
	public GameInputLabel ViewButtonLabel;
	public GameInputLabel StickUpLabel;
	public GameInputLabel StickDownLabel;
	public GameInputLabel StickLeftLabel;
	public GameInputLabel StickRightLabel;
	public GameInputLabel ActionButton1Label;
	public GameInputLabel ActionButton2Label;
	public GameInputLabel ActionButton3Label;
	public GameInputLabel ActionButton4Label;
	public GameInputLabel ActionButton5Label;
	public GameInputLabel ActionButton6Label;
	public GameInputLabel SpecialButton1Label;
	public GameInputLabel SpecialButton2Label;
	public uint ExtraButtonCount;
	public uint ExtraAxisCount;
};

[StructLayout(LayoutKind.Sequential)]
public struct GameInputFlightStickInfo
{
	public GameInputLabel MenuButtonLabel;
	public GameInputLabel ViewButtonLabel;
	public GameInputLabel FirePrimaryButtonLabel;
	public GameInputLabel FireSecondaryButtonLabel;
	public GameInputLabel HatSwitchUpLabel;
	public GameInputLabel HatSwitchDownLabel;
	public GameInputLabel HatSwitchLeftLabel;
	public GameInputLabel HatSwitchRightLabel;
	public GameInputLabel AButtonLabel;
	public GameInputLabel BButtonLabel;
	public GameInputLabel XButtonLabel;
	public GameInputLabel YButtonLabel;
	public GameInputLabel LeftShoulderButtonLabel;
	public GameInputLabel RightShoulderButtonLabel;
	public uint ExtraButtonCount;
	public uint ExtraAxisCount;
};

[StructLayout(LayoutKind.Sequential)]
public struct GameInputGamepadInfo
{
	public GameInputGamepadButtons SupportedLayout;
	public GameInputLabel MenuButtonLabel;
	public GameInputLabel ViewButtonLabel;
	public GameInputLabel AButtonLabel;
	public GameInputLabel BButtonLabel;
	public GameInputLabel CButtonLabel;
	public GameInputLabel XButtonLabel;
	public GameInputLabel YButtonLabel;
	public GameInputLabel ZButtonLabel;
	public GameInputLabel DpadUpLabel;
	public GameInputLabel DpadDownLabel;
	public GameInputLabel DpadLeftLabel;
	public GameInputLabel DpadRightLabel;
	public GameInputLabel LeftShoulderButtonLabel;
	public GameInputLabel RightShoulderButtonLabel;
	public GameInputLabel LeftThumbstickButtonLabel;
	public GameInputLabel RightThumbstickButtonLabel;
	public uint ExtraButtonCount;
	public uint ExtraAxisCount;
};

[StructLayout(LayoutKind.Sequential)]
public struct GameInputRacingWheelInfo
{
	public GameInputLabel MenuButtonLabel;
	public GameInputLabel ViewButtonLabel;
	public GameInputLabel PreviousGearButtonLabel;
	public GameInputLabel NextGearButtonLabel;
	public GameInputLabel DpadUpLabel;
	public GameInputLabel DpadDownLabel;
	public GameInputLabel DpadLeftLabel;
	public GameInputLabel DpadRightLabel;
	public GameInputLabel AButtonLabel;
	public GameInputLabel BButtonLabel;
	public GameInputLabel XButtonLabel;
	public GameInputLabel YButtonLabel;
	public GameInputLabel LeftThumbstickButtonLabel;
	public GameInputLabel RightThumbstickButtonLabel;
	public bool HasClutch;
	public bool HasHandbrake;
	public bool HasPatternShifter;
	public int MinPatternShifterGear;
	public int MaxPatternShifterGear;
	public float MaxWheelAngle;
	public uint ExtraButtonCount;
	public uint ExtraAxisCount;
};

[StructLayout(LayoutKind.Sequential)]
public struct GameInputForceFeedbackMotorInfo
{
	public GameInputFeedbackAxes SupportedAxes;
	public bool IsConstantEffectSupported;
	public bool IsRampEffectSupported;
	public bool IsSineWaveEffectSupported;
	public bool IsSquareWaveEffectSupported;
	public bool IsTriangleWaveEffectSupported;
	public bool IsSawtoothUpWaveEffectSupported;
	public bool IsSawtoothDownWaveEffectSupported;
	public bool IsSpringEffectSupported;
	public bool IsFrictionEffectSupported;
	public bool IsDamperEffectSupported;
	public bool IsInertiaEffectSupported;
};

[StructLayout(LayoutKind.Sequential)]
public struct GameInputRawDeviceReportInfo
{
	public GameInputRawDeviceReportKind Kind;
	public uint Id;
	public uint Size;
};

[StructLayout(LayoutKind.Sequential)]
public struct GameInputDeviceInfo
{
	public ushort VendorId;
	public ushort ProductId;
	public ushort RevisionNumber;
	public GameInputUsage Usage;
	public GameInputVersion HardwareVersion;
	public GameInputVersion FirmwareVersion;
	public APP_LOCAL_DEVICE_ID DeviceId;
	public APP_LOCAL_DEVICE_ID DeviceRootId;
	public GameInputDeviceFamily DeviceFamily;
	public GameInputKind SupportedInput;
	public GameInputRumbleMotors SupportedRumbleMotors;
	public GameInputSystemButtons SupportedSystemButtons;
	public Guid ContainerId;
	private unsafe byte* _displayName;
	private unsafe byte* _pnpPath;
	private unsafe GameInputKeyboardInfo* _keyboardInfo;
	private unsafe GameInputMouseInfo* _mouseInfo;
	private unsafe GameInputSensorsInfo* _sensorsInfo;
	private unsafe GameInputControllerInfo* _controllerInfo;
	private unsafe GameInputArcadeStickInfo* _arcadeStickInfo;
	private unsafe GameInputFlightStickInfo* _flightStickInfo;
	private unsafe GameInputGamepadInfo* _gamepadInfo;
	private unsafe GameInputRacingWheelInfo* _racingWheelInfo;
	public uint ForceFeedbackMotorCount;
	private unsafe GameInputForceFeedbackMotorInfo* _forceFeedbackMotorInfo;
	public uint InputReportCount;
	private unsafe GameInputRawDeviceReportInfo* _inputReportInfo;
	public uint OutputReportCount;
	private unsafe GameInputRawDeviceReportInfo* _outputReportInfo;
	private static unsafe string? PtrToUtf8(byte* p) => p == null ? null : Marshal.PtrToStringUTF8((nint)p);
	public readonly unsafe string? DisplayName => PtrToUtf8(_displayName);
	public readonly unsafe string? PnpPath => PtrToUtf8(_pnpPath);

	public readonly unsafe ReadOnlySpan<GameInputForceFeedbackMotorInfo> ForceFeedbackMotorInfo => new(_forceFeedbackMotorInfo, checked((int)ForceFeedbackMotorCount));
	public readonly unsafe ReadOnlySpan<GameInputRawDeviceReportInfo> InputReportInfo => new(_inputReportInfo, checked((int)InputReportCount));
	public readonly unsafe ReadOnlySpan<GameInputRawDeviceReportInfo> OutputReportInfo => new(_outputReportInfo, checked((int)OutputReportCount));
	public readonly unsafe ref readonly GameInputKeyboardInfo KeyboardInfo { get { ArgumentNullException.ThrowIfNull(_keyboardInfo); return ref *_keyboardInfo; } }
	public readonly unsafe ref readonly GameInputMouseInfo MouseInfo { get { ArgumentNullException.ThrowIfNull(_mouseInfo); return ref *_mouseInfo; } }
	public readonly unsafe ref readonly GameInputSensorsInfo SensorsInfo { get { ArgumentNullException.ThrowIfNull(_sensorsInfo); return ref *_sensorsInfo; } }
	public readonly unsafe ref readonly GameInputControllerInfo ControllerInfo { get { ArgumentNullException.ThrowIfNull(_controllerInfo); return ref *_controllerInfo; } }
	public readonly unsafe ref readonly GameInputArcadeStickInfo ArcadeStickInfo { get { ArgumentNullException.ThrowIfNull(_arcadeStickInfo); return ref *_arcadeStickInfo; } }
	public readonly unsafe ref readonly GameInputFlightStickInfo FlightStickInfo { get { ArgumentNullException.ThrowIfNull(_flightStickInfo); return ref *_flightStickInfo; } }
	public readonly unsafe ref readonly GameInputGamepadInfo GamepadInfo { get { ArgumentNullException.ThrowIfNull(_gamepadInfo); return ref *_gamepadInfo; } }
	public readonly unsafe ref readonly GameInputRacingWheelInfo RacingWheelInfo { get { ArgumentNullException.ThrowIfNull(_racingWheelInfo); return ref *_racingWheelInfo; } }
};

[StructLayout(LayoutKind.Sequential)]
public struct GameInputHapticInfo
{
	public FixedArray256<char> AudioEndpointId;
	public uint LocationCount;
	public FixedArray8<Guid> Locations;

	public readonly string AudioEndpointIdString
	{
		get
		{
			ReadOnlySpan<char> chars = AudioEndpointId.ReadOnlySpan;
			int nullIndex = chars.IndexOf('\0');
			return new string(nullIndex >= 0 ? chars[..nullIndex] : chars);
		}
	}

	public readonly ReadOnlySpan<Guid> ValidLocations => Locations.ReadOnlySpan[..checked((int)LocationCount)];
};

[StructLayout(LayoutKind.Sequential)]
public struct GameInputForceFeedbackEnvelope
{
	public ulong AttackDuration;
	public ulong SustainDuration;
	public ulong ReleaseDuration;
	public float AttackGain;
	public float SustainGain;
	public float ReleaseGain;
	public uint PlayCount;
	public ulong RepeatDelay;
};

[StructLayout(LayoutKind.Sequential)]
public struct GameInputForceFeedbackMagnitude
{
	public float LinearX;
	public float LinearY;
	public float LinearZ;
	public float AngularX;
	public float AngularY;
	public float AngularZ;
	public float Normal;
};

[StructLayout(LayoutKind.Sequential)]
public struct GameInputForceFeedbackConditionParams
{
	public GameInputForceFeedbackMagnitude Magnitude;
	public float PositiveCoefficient;
	public float NegativeCoefficient;
	public float MaxPositiveMagnitude;
	public float MaxNegativeMagnitude;
	public float DeadZone;
	public float Bias;
};

[StructLayout(LayoutKind.Sequential)]
public struct GameInputForceFeedbackConstantParams
{
	public GameInputForceFeedbackEnvelope Envelope;
	public GameInputForceFeedbackMagnitude Magnitude;
};

[StructLayout(LayoutKind.Sequential)]
public struct GameInputForceFeedbackPeriodicParams
{
	public GameInputForceFeedbackEnvelope Envelope;
	public GameInputForceFeedbackMagnitude Magnitude;
	public float Frequency;
	public float Phase;
	public float Bias;
};

[StructLayout(LayoutKind.Sequential)]
public struct GameInputForceFeedbackRampParams
{
	public GameInputForceFeedbackEnvelope Envelope;
	public GameInputForceFeedbackMagnitude StartMagnitude;
	public GameInputForceFeedbackMagnitude EndMagnitude;
};

[StructLayout(LayoutKind.Explicit)]
public struct GameInputForceFeedbackParams
{
	[FieldOffset(0)] public GameInputForceFeedbackEffectKind Kind;

	// union members (overlap at offset 8)
	[FieldOffset(8)] public GameInputForceFeedbackConstantParams Constant;
	[FieldOffset(8)] public GameInputForceFeedbackRampParams Ramp;
	[FieldOffset(8)] public GameInputForceFeedbackPeriodicParams SineWave;
	[FieldOffset(8)] public GameInputForceFeedbackPeriodicParams SquareWave;
	[FieldOffset(8)] public GameInputForceFeedbackPeriodicParams TriangleWave;
	[FieldOffset(8)] public GameInputForceFeedbackPeriodicParams SawtoothUpWave;
	[FieldOffset(8)] public GameInputForceFeedbackPeriodicParams SawtoothDownWave;
	[FieldOffset(8)] public GameInputForceFeedbackConditionParams Spring;
	[FieldOffset(8)] public GameInputForceFeedbackConditionParams Friction;
	[FieldOffset(8)] public GameInputForceFeedbackConditionParams Damper;
	[FieldOffset(8)] public GameInputForceFeedbackConditionParams Inertia;
};

[StructLayout(LayoutKind.Sequential)]
public struct GameInputRumbleParams
{
	public float LowFrequency;
	public float HighFrequency;
	public float LeftTrigger;
	public float RightTrigger;
};

[StructLayout(LayoutKind.Sequential)]
public struct GameInputAxisMapping
{
	public GameInputElementKind ControllerElementKind;
	public uint ControllerIndex;
	public bool IsInverted;
	public bool FromTwoButtons;
	public uint ButtonMinIndexValue;
	public GameInputSwitchPosition ReferenceDirection;
};

[StructLayout(LayoutKind.Sequential)]
public struct GameInputButtonMapping
{
	public GameInputElementKind ControllerElementKind;
	public uint ControllerIndex;
	public bool IsInverted;
	public GameInputSwitchPosition SwitchPosition;
};

#endregion

#region  Interfaces

[GeneratedComInterface, Guid("20EFC1C7-5D9A-43BA-B26F-B807FA48609C")]
public partial interface IGameInput : IUnknown
{
	[PreserveSig]
	ulong GetCurrentTimestamp();

	[PreserveSig][return: MarshalAs(UnmanagedType.Error)]
	HRESULT GetCurrentReading(GameInputKind inputKind, IGameInputDevice? device, out IGameInputReading reading);

	[PreserveSig][return: MarshalAs(UnmanagedType.Error)]
	HRESULT GetNextReading(IGameInputReading referenceReading, GameInputKind inputKind, IGameInputDevice? device, out IGameInputReading reading);

	[PreserveSig][return: MarshalAs(UnmanagedType.Error)]
	HRESULT GetPreviousReading(IGameInputReading referenceReading, GameInputKind inputKind, IGameInputDevice? device, out IGameInputReading reading);

	[PreserveSig][return: MarshalAs(UnmanagedType.Error)]
	unsafe HRESULT RegisterReadingCallback(IGameInputDevice? device, GameInputKind inputKind, nint context, GameInputReadingCallback callbackFunc, out GameInputCallbackToken callbackToken);

	[PreserveSig][return: MarshalAs(UnmanagedType.Error)]
	unsafe HRESULT RegisterDeviceCallback(IGameInputDevice? device, GameInputKind inputKind, GameInputDeviceStatus statusFilter, GameInputEnumerationKind enumerationKind, nint context, GameInputDeviceCallback callbackFunc, out GameInputCallbackToken callbackToken);

	[PreserveSig][return: MarshalAs(UnmanagedType.Error)]
	unsafe HRESULT RegisterSystemButtonCallback(IGameInputDevice? device, GameInputSystemButtons buttonFilter, nint context, GameInputSystemButtonCallback callbackFunc, out GameInputCallbackToken callbackToken);

	[PreserveSig][return: MarshalAs(UnmanagedType.Error)]
	unsafe HRESULT RegisterKeyboardLayoutCallback(IGameInputDevice? device, nint context, GameInputKeyboardLayoutCallback callbackFunc, out GameInputCallbackToken callbackToken);

	[PreserveSig]
	void StopCallback(GameInputCallbackToken callbackToken);

	[PreserveSig][return: MarshalAs(UnmanagedType.Bool)]
	bool UnregisterCallback(GameInputCallbackToken callbackToken);

	[PreserveSig][return: MarshalAs(UnmanagedType.Error)]
	HRESULT CreateDispatcher(out IGameInputDispatcher dispatcher);

	[PreserveSig][return: MarshalAs(UnmanagedType.Error)]
	HRESULT FindDeviceFromId(in APP_LOCAL_DEVICE_ID value, out IGameInputDevice device);

	[PreserveSig][return: MarshalAs(UnmanagedType.Error)]
	HRESULT FindDeviceFromPlatformString([MarshalAs(UnmanagedType.LPWStr)] string value, out IGameInputDevice device);

	[PreserveSig]
	void SetFocusPolicy(GameInputFocusPolicy policy);

	[PreserveSig][return: MarshalAs(UnmanagedType.Error)]
	HRESULT CreateAggregateDevice(GameInputKind inputKind, out APP_LOCAL_DEVICE_ID deviceId);

	[PreserveSig][return: MarshalAs(UnmanagedType.Error)]
	HRESULT DisableAggregateDevice(in APP_LOCAL_DEVICE_ID deviceId);
}

[GeneratedComInterface, Guid("05A42D89-2CB6-45A3-874D-E635723587AB")]
public partial interface IGameInputRawDeviceReport : IUnknown
{
	[PreserveSig]
	void GetDevice(out IGameInputDevice device);

	[PreserveSig]
	void GetReportInfo(out GameInputRawDeviceReportInfo reportInfo);

	[PreserveSig]
	nuint GetRawDataSize();

	[PreserveSig]
	nuint GetRawData(nuint bufferSize, nint buffer);

	[PreserveSig][return: MarshalAs(UnmanagedType.Bool)]
	bool SetRawData(nuint bufferSize, nint buffer);
}

[GeneratedComInterface, Guid("C81C4CDE-ED1A-4631-A30F-C556A6241A1F")]
public partial interface IGameInputReading : IUnknown
{
	[PreserveSig]
	GameInputKind GetInputKind();

	[PreserveSig]
	ulong GetTimestamp();

	[PreserveSig]
	void GetDevice(out IGameInputDevice device);

	[PreserveSig]
	uint GetControllerAxisCount();

	[PreserveSig]
	uint GetControllerAxisState(uint stateArrayCount, [Out, MarshalUsing(CountElementName = "stateArrayCount")] float[] stateArray);

	[PreserveSig]
	uint GetControllerButtonCount();

	[PreserveSig]
	uint GetControllerButtonState(uint stateArrayCount, [Out, MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I1)] bool[] stateArray);

	[PreserveSig]
	uint GetControllerSwitchCount();

	[PreserveSig]
	uint GetControllerSwitchState(uint stateArrayCount, [Out, MarshalUsing(CountElementName = "stateArrayCount")] GameInputSwitchPosition[] stateArray);

	[PreserveSig]
	uint GetKeyCount();

	[PreserveSig]
	uint GetKeyState(uint stateArrayCount, [Out, MarshalUsing(CountElementName = "stateArrayCount")] GameInputKeyState[] stateArray);

	[PreserveSig][return: MarshalAs(UnmanagedType.Bool)]
	bool GetMouseState(out GameInputMouseState state);

	[PreserveSig][return: MarshalAs(UnmanagedType.Bool)]
	bool GetSensorsState(out GameInputSensorsState state);

	[PreserveSig][return: MarshalAs(UnmanagedType.Bool)]
	bool GetArcadeStickState(out GameInputArcadeStickState state);

	[PreserveSig][return: MarshalAs(UnmanagedType.Bool)]
	bool GetFlightStickState(out GameInputFlightStickState state);

	[PreserveSig][return: MarshalAs(UnmanagedType.Bool)]
	bool GetGamepadState(out GameInputGamepadState state);

	[PreserveSig][return: MarshalAs(UnmanagedType.Bool)]
	bool GetRacingWheelState(out GameInputRacingWheelState state);

	[PreserveSig][return: MarshalAs(UnmanagedType.Bool)]
	bool GetRawReport(out IGameInputRawDeviceReport? report);
}

[GeneratedComInterface, Guid("63E2F38B-A399-4275-8AE7-D4C6E524D12A")]
public partial interface IGameInputDevice : IUnknown
{
	[PreserveSig][return: MarshalAs(UnmanagedType.Error)]
	HRESULT GetDeviceInfo(out nint info);

	[PreserveSig][return: MarshalAs(UnmanagedType.Error)]
	HRESULT GetHapticInfo(out GameInputHapticInfo info);

	[PreserveSig]
	GameInputDeviceStatus GetDeviceStatus();

	[PreserveSig][return: MarshalAs(UnmanagedType.Error)]
	HRESULT CreateForceFeedbackEffect(uint motorIndex, in GameInputForceFeedbackParams @params, out IGameInputForceFeedbackEffect effect);

	[PreserveSig][return: MarshalAs(UnmanagedType.Bool)]
	bool IsForceFeedbackMotorPoweredOn(uint motorIndex);

	[PreserveSig]
	void SetForceFeedbackMotorGain(uint motorIndex, float masterGain);

	[PreserveSig]
	void SetRumbleState(in GameInputRumbleParams @params);

	[PreserveSig][return: MarshalAs(UnmanagedType.Error)]
	HRESULT DirectInputEscape(uint command, IntPtr bufferIn, uint bufferInSize, IntPtr bufferOut, uint bufferOutSize, out uint bufferOutSizeWritten);

	[PreserveSig][return: MarshalAs(UnmanagedType.Error)]
	HRESULT CreateInputMapper(out IGameInputMapper inputMapper);

	[PreserveSig][return: MarshalAs(UnmanagedType.Error)]
	HRESULT GetExtraAxisCount(GameInputKind inputKind, out uint extraAxisCount);

	[PreserveSig][return: MarshalAs(UnmanagedType.Error)]
	HRESULT GetExtraButtonCount(GameInputKind inputKind, out uint extraButtonCount);

	[PreserveSig][return: MarshalAs(UnmanagedType.Error)]
	HRESULT GetExtraAxisIndexes(GameInputKind inputKind, uint extraAxisCount, [Out, MarshalUsing(CountElementName = "extraAxisCount")] byte[] extraAxisIndexes);

	[PreserveSig][return: MarshalAs(UnmanagedType.Error)]
	HRESULT GetExtraButtonIndexes(GameInputKind inputKind, uint extraButtonCount, [Out, MarshalUsing(CountElementName = "extraButtonCount")] byte[] extraButtonIndexes);

	[PreserveSig][return: MarshalAs(UnmanagedType.Error)]
	HRESULT CreateRawDeviceReport(uint reportId, GameInputRawDeviceReportKind reportKind, out IGameInputRawDeviceReport report);

	[PreserveSig][return: MarshalAs(UnmanagedType.Error)]
	HRESULT SendRawDeviceOutput(IGameInputRawDeviceReport report);
}

[GeneratedComInterface, Guid("415EED2E-98CB-42C2-8F28-B94601074E31")]
public partial interface IGameInputDispatcher : IUnknown
{
	[PreserveSig][return: MarshalAs(UnmanagedType.Bool)]
	bool Dispatch(ulong quotaInMicroseconds);

	[PreserveSig][return: MarshalAs(UnmanagedType.Error)]
	HRESULT OpenWaitHandle(out HANDLE waitHandle);
}

[GeneratedComInterface, Guid("FF61096A-3373-4093-A1DF-6D31846B3511")]
public partial interface IGameInputForceFeedbackEffect : IUnknown
{
	[PreserveSig]
	void GetDevice(out IGameInputDevice device);

	[PreserveSig]
	uint GetMotorIndex();

	[PreserveSig]
	float GetGain();

	[PreserveSig]
	void SetGain(float gain);

	[PreserveSig]
	void GetParams(out GameInputForceFeedbackParams @params);

	[PreserveSig][return: MarshalAs(UnmanagedType.Bool)]
	bool SetParams(in GameInputForceFeedbackParams @params);

	[PreserveSig]
	GameInputFeedbackEffectState GetState();

	[PreserveSig]
	void SetState(GameInputFeedbackEffectState state);
}

[GeneratedComInterface, Guid("3C600700-F16C-49CE-9BE6-6A2EF752ED5E")]
public partial interface IGameInputMapper : IUnknown
{
	[PreserveSig][return: MarshalAs(UnmanagedType.Bool)]
	bool GetArcadeStickButtonMappingInfo(GameInputArcadeStickButtons buttonElement, out GameInputButtonMapping mapping);

	[PreserveSig][return: MarshalAs(UnmanagedType.Bool)]
	bool GetFlightStickAxisMappingInfo(GameInputFlightStickAxes axisElement, out GameInputAxisMapping mapping);

	[PreserveSig][return: MarshalAs(UnmanagedType.Bool)]
	bool GetFlightStickButtonMappingInfo(GameInputFlightStickButtons buttonElement, out GameInputButtonMapping mapping);

	[PreserveSig][return: MarshalAs(UnmanagedType.Bool)]
	bool GetGamepadAxisMappingInfo(GameInputGamepadAxes axisElement, out GameInputAxisMapping mapping);

	[PreserveSig][return: MarshalAs(UnmanagedType.Bool)]
	bool GetGamepadButtonMappingInfo(GameInputGamepadButtons buttonElement, out GameInputButtonMapping mapping);

	[PreserveSig][return: MarshalAs(UnmanagedType.Bool)]
	bool GetRacingWheelAxisMappingInfo(GameInputRacingWheelAxes axisElement, out GameInputAxisMapping mapping);

	[PreserveSig][return: MarshalAs(UnmanagedType.Bool)]
	bool GetRacingWheelButtonMappingInfo(GameInputRacingWheelButtons buttonElement, out GameInputButtonMapping mapping);
}

#endregion

#region Extensions

public static class Extensions
{

	#region IGameInput

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IGameInput Create(this IGameInput? _)
	{
		GameInput.GameInputCreate(out var input).ThrowOnFailure();
		return input;
	}

	#endregion

}

#endregion