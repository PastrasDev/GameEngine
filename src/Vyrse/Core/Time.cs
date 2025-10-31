using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Vyrse.Core;

public sealed partial class Time
{
	/// Delta time in seconds, unscaled and not accounting for pause state
	public double UnscaledDelta { get; private set; }

	/// Delta time in seconds, scaled and accounting for pause state
	public double Delta => Paused ? 0.0 : UnscaledDelta * Scale;

	/// Is the time progression paused
	public bool Paused { get; set; }

	/// Time scale factor, default is 1.0
	public double Scale { get; set; } = 1.0;

	/// Current frame index since start
	public ulong FrameIndex { get; private set; }

	/// Total elapsed time since start in seconds
	public double Elapsed { get; private set; }

	/// Amount of frames in one second
	public double Fps { get; private set; }

	/// Average frame time in milliseconds
	public double AverageFrameTime { get; private set; }

	private long _lastUpdateTimestamp = Now();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal void Update()
	{
		var now = Now();
		UnscaledDelta = (now - _lastUpdateTimestamp) * ToSeconds;
		_lastUpdateTimestamp = now;

		Elapsed += UnscaledDelta;
		FrameIndex++;

		if (!(UnscaledDelta > double.Epsilon)) return;

		double alpha = saturate(1.0 - exponent(-UnscaledDelta * 8.0));
		var frameTimeMs = UnscaledDelta * 1000.0;

		AverageFrameTime = AverageFrameTime == 0 ? frameTimeMs : (1 - alpha) * AverageFrameTime + alpha * frameTimeMs;
		Fps = Fps == 0 ? 1.0 / UnscaledDelta : (1 - alpha) * Fps + alpha * (1.0 / UnscaledDelta);
	}
}

public sealed partial class Time
{
	/// Fixed time step in seconds, default is 1/120 (120 Hz)
	public double FixedStep { get; set; } = 1.0 / 60.0;

	/// Maximum number of fixed steps to process per frame, default is 8
	public int MaxFixedSteps { get; set; } = 8;

	/// Maximum delta time in seconds to consider for fixed update accumulation, default is 0.25 (250 ms)
	public double MaxFrameDelta { get; set; } = 0.25;

	/// Interpolation alpha value between the last and the next fixed update, 0..1
	public double FixedAlpha { get; private set; }

	/// Current fixed frame index since start
	public ulong FixedFrameIndex { get; private set; }

	private double _fixedAccum;
	private long _lastFixedTimestamp = Now();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal bool FixedUpdate()
	{
		var now = Now();
		var dt = (now - _lastFixedTimestamp) * ToSeconds;
		_lastFixedTimestamp = now;

		if (dt > MaxFrameDelta) dt = MaxFrameDelta;
		var maxAccum = FixedStep * MaxFixedSteps;

		_fixedAccum = Math.Min(_fixedAccum + (Paused ? 0.0 : dt * Scale), maxAccum);

		if (_fixedAccum >= FixedStep)
		{
			_fixedAccum -= FixedStep;
			FixedFrameIndex++;
			FixedAlpha = _fixedAccum / FixedStep;
			return true;
		}

		FixedAlpha = _fixedAccum / FixedStep;
		return false;
	}
}

public sealed partial class Time
{
	public enum Units
	{
		Years,
		Days,
		Hours,
		Minutes,
		Seconds,
		Milliseconds,
		Microseconds,
		Nanoseconds
	}

	public static readonly double ToYears = 1.0 / (Stopwatch.Frequency * 60 * 60 * 24 * 365.25);
	public static readonly double ToDays = 1.0 / (Stopwatch.Frequency * 60 * 60 * 24);
	public static readonly double ToHours = 1.0 / (Stopwatch.Frequency * 60 * 60);
	public static readonly double ToMinutes = 1.0 / (Stopwatch.Frequency * 60);
	public static readonly double ToSeconds = 1.0 / Stopwatch.Frequency;
	public static readonly double ToMilliseconds = 1000.0 / Stopwatch.Frequency;
	public static readonly double ToMicroseconds = 1_000_000.0 / Stopwatch.Frequency;
	public static readonly double ToNanoseconds = 1_000_000_000.0 / Stopwatch.Frequency;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static long Now() => Stopwatch.GetTimestamp();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double TimeSince(long start, Units units)
	{
		return (Now() - start) * units switch
		{
			Units.Years => ToYears,
			Units.Days => ToDays,
			Units.Hours => ToHours,
			Units.Minutes => ToMinutes,
			Units.Seconds => ToSeconds,
			Units.Milliseconds => ToMilliseconds,
			Units.Microseconds => ToMicroseconds,
			Units.Nanoseconds => ToNanoseconds,
			_ => throw new ArgumentOutOfRangeException(nameof(units), units, null)
		};
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static long From(double value, Units units) => (long)(value / (units switch
	{
		Units.Years        => ToYears,
		Units.Days         => ToDays,
		Units.Hours        => ToHours,
		Units.Minutes      => ToMinutes,
		Units.Seconds      => ToSeconds,
		Units.Milliseconds => ToMilliseconds,
		Units.Microseconds => ToMicroseconds,
		Units.Nanoseconds  => ToNanoseconds,
		_ => throw new ArgumentOutOfRangeException(nameof(units))
	}));

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double To(long ticks, Units units) => ticks * (units switch
	{
		Units.Years        => ToYears,
		Units.Days         => ToDays,
		Units.Hours        => ToHours,
		Units.Minutes      => ToMinutes,
		Units.Seconds      => ToSeconds,
		Units.Milliseconds => ToMilliseconds,
		Units.Microseconds => ToMicroseconds,
		Units.Nanoseconds  => ToNanoseconds,
		_ => throw new ArgumentOutOfRangeException(nameof(units))
	});
}