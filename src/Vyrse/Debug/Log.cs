using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Vyrse.Debug;

public readonly record struct CallerInfo([CallerFilePath] string File = "", [CallerLineNumber] int Line = 0, [CallerMemberName] string Member = "")
{
	public static implicit operator string(CallerInfo ci) => $"{Path.GetFileName(ci.File)}:{ci.Line} :: {ci.Member}";
}

public static class Log
{
	public enum Level
	{
		Trace = 0,
		Debug = 1,
		Info = 2,
		Warn = 3,
		Error = 4,
		Fatal = 5
	}

	public interface ISink
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		void Write(in Event evt);
	}

	public sealed class ConsoleSink : ISink
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Write(in Event evt)
		{
			var s = evt.ToString();
			if (evt.Level >= Level.Warn) Console.Error.WriteLine(s);
			else Console.WriteLine(s);
		}
	}

	public sealed class FileSink(string path) : ISink, IDisposable
	{
		private readonly object _lock = new();
		private readonly StreamWriter _w = new(path, append:true) { AutoFlush = true };

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Write(in Event e)
		{
			lock (_lock)
			{
				_w.WriteLine(e.ToString());
			}
		}
		public void Dispose() => _w.Dispose();
	}

	public readonly struct Event(DateTime utc, Level level, string message, Exception? exception)
	{
		public readonly DateTime Utc = utc;
		public readonly Level Level = level;
		public readonly string Message = message;
		public readonly Exception? Exception = exception;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString() => $"[{Utc:O}][{Level}] {Message}" + (Exception is null ? "" : Environment.NewLine + Exception);
	}

	internal static class Core
	{
		public static Level Minimum = Level.Info;
		public static ISink Sink { get; set; } = new ConsoleSink();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Enabled(Level level) => level >= Minimum;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Write(Level level, string message, Exception? ex = null)
		{
			if (!Enabled(level)) return;
			var ev = new Event(DateTime.UtcNow, level, message, ex);
			try { Sink.Write(in ev); }
			catch (Exception sinkEx)
			{
				Console.Error.WriteLine("[LogSink ERROR] " + sinkEx);
				Console.Error.WriteLine(ev.ToString());
			}
		}
	}

	public interface ILevelTag
	{
		static abstract Level Level { get; }
	}

	public readonly struct TraceTag : ILevelTag { public static Level Level => Level.Trace; }
	public readonly struct DebugTag : ILevelTag { public static Level Level => Level.Debug; }
	public readonly struct InfoTag : ILevelTag { public static Level Level => Level.Info; }
	public readonly struct WarnTag : ILevelTag { public static Level Level => Level.Warn; }
	public readonly struct ErrorTag : ILevelTag { public static Level Level => Level.Error; }
	public readonly struct FatalTag : ILevelTag { public static Level Level => Level.Fatal; }


	[InterpolatedStringHandler]
	public readonly ref struct Handler<TLevel> where TLevel : struct, ILevelTag
	{
		private readonly StringBuilder? _sb;

		public bool Enabled { get; }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Handler(int literalLength, int formattedCount, out bool shouldAppend)
		{
			var level = TLevel.Level;
			Enabled = shouldAppend = Core.Enabled(level);
			_sb = Enabled ? new StringBuilder(literalLength + formattedCount * 8) : null;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)] public void AppendLiteral(string s) { if (Enabled) _sb!.Append(s); }
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public void AppendFormatted(ReadOnlySpan<char> s) { if (Enabled) _sb!.Append(s); }
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public void AppendFormatted(string? s) { if (Enabled) _sb!.Append(s); }
		[MethodImpl(MethodImplOptions.AggressiveInlining)] public void AppendFormatted<T>(T v) { if (Enabled) _sb!.Append(v); }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AppendFormatted<T>(T v, string fmt)
		{
			if (!Enabled) return;
			if (v is IFormattable f) _sb!.Append(f.ToString(fmt, CultureInfo.InvariantCulture));
			else _sb!.Append(v);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AppendFormatted<T>(T v, int align)
		{
			if (!Enabled) return;

			string s = v switch
			{
				IFormattable f => f.ToString(null, CultureInfo.InvariantCulture),
				null => string.Empty,
				_ => v.ToString() ?? string.Empty
			};

			if (align > 0) _sb!.Append(s.PadLeft(align));
			else if (align < 0) _sb!.Append(s.PadRight(-align));
			else _sb!.Append(s);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void AppendFormatted<T>(T v, int align, string fmt)
		{
			if (!Enabled) return;

			string s = v is IFormattable f ? f.ToString(fmt, CultureInfo.InvariantCulture) : v?.ToString() ?? string.Empty;

			if (align > 0) _sb!.Append(s.PadLeft(align));
			else if (align < 0) _sb!.Append(s.PadRight(-align));
			else _sb!.Append(s);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString() => Enabled ? _sb!.ToString() : string.Empty;
	}

	public static Level Minimum { get => Core.Minimum; set => Core.Minimum = value; }

	[Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsEnabled(Level level) => Core.Enabled(level);

	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static void Info(string message)  => Core.Write(Level.Info,  message);
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static void Info([InterpolatedStringHandlerArgument] Handler<InfoTag> message) { if (message.Enabled) Core.Write(Level.Info, message.ToString()); }

	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static void Warn(string message)  => Core.Write(Level.Warn,  message);
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static void Warn([InterpolatedStringHandlerArgument] Handler<WarnTag> message) { if (message.Enabled) Core.Write(Level.Warn, message.ToString()); }
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static void Warn(Exception ex, [InterpolatedStringHandlerArgument] Handler<WarnTag> message) { if (message.Enabled) Core.Write(Level.Warn, message.ToString(), ex); }

	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static void Error(string message) => Core.Write(Level.Error, message);
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static void Error([InterpolatedStringHandlerArgument] Handler<ErrorTag> message) { if (message.Enabled) Core.Write(Level.Error, message.ToString()); }
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static void Error(Exception ex, [InterpolatedStringHandlerArgument] Handler<ErrorTag> message) { if (message.Enabled) Core.Write(Level.Error, message.ToString(), ex); }
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static void Error(Exception ex) => Core.Write(Level.Error, ex.ToString(), ex);

	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static void Fatal(string message) => Core.Write(Level.Fatal, message);
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static void Fatal([InterpolatedStringHandlerArgument] Handler<FatalTag> message) { if (message.Enabled) Core.Write(Level.Fatal, message.ToString()); }
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static void Fatal(Exception ex, [InterpolatedStringHandlerArgument] Handler<FatalTag> message) { if (message.Enabled) Core.Write(Level.Fatal, message.ToString(), ex); }
	[MethodImpl(MethodImplOptions.AggressiveInlining)] public static void Fatal(Exception ex) => Core.Write(Level.Fatal, ex.ToString(), ex);

	#if DEBUG
	[Conditional("DEBUG"), MethodImpl(MethodImplOptions.AggressiveInlining)] public static void Debug([InterpolatedStringHandlerArgument] Handler<DebugTag> message) { if (message.Enabled) Core.Write(Level.Debug, message.ToString()); }
	[Conditional("DEBUG"), MethodImpl(MethodImplOptions.AggressiveInlining)] public static void Trace([InterpolatedStringHandlerArgument] Handler<TraceTag> message) { if (message.Enabled) Core.Write(Level.Trace, message.ToString()); }
	#endif
}