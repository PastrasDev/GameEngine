using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Vyrse.Tools;

public static partial class ComGuidHarvester
{
	[GeneratedRegex(@"DEFINE_GUID\s*\(\s*(?<name>[A-Za-z_][A-Za-z0-9_]+)\s*,\s*0x(?<d1>[0-9A-Fa-f]+)\s*,\s*0x(?<d2>[0-9A-Fa-f]+)\s*,\s*0x(?<d3>[0-9A-Fa-f]+)\s*,\s*0x(?<b1>[0-9A-Fa-f]+)\s*,\s*0x(?<b2>[0-9A-Fa-f]+)\s*,\s*0x(?<b3>[0-9A-Fa-f]+)\s*,\s*0x(?<b4>[0-9A-Fa-f]+)\s*,\s*0x(?<b5>[0-9A-Fa-f]+)\s*,\s*0x(?<b6>[0-9A-Fa-f]+)\s*,\s*0x(?<b7>[0-9A-Fa-f]+)\s*,\s*0x(?<b8>[0-9A-Fa-f]+)\s*\)", RegexOptions.Compiled)]
	private static partial Regex DEFINE_GUID_Regex();

	[GeneratedRegex("""MIDL_INTERFACE\s*\(\s*"(?<guid>[0-9A-Fa-f\-]{36})"\s*\)\s*(?<name>[A-Za-z_][A-Za-z0-9_]*)""", RegexOptions.Compiled)]
	private static partial Regex MIDL_INTERFACE_Regex();

	[GeneratedRegex("""__declspec\s*\(\s*uuid\s*\(\s*"(?<guid>[0-9A-Fa-f\-]{36})"\s*\)\s*\)\s*(?:struct|class|interface)\s+(?<name>[A-Za-z_][A-Za-z0-9_]*)""", RegexOptions.Compiled)]
	private static partial Regex __declspec_Regex();

	[GeneratedRegex("""DECLARE_INTERFACE_IID_\s*\(\s*(?<name>[A-Za-z_][A-Za-z0-9_]*)\s*,\s*[A-Za-z_][A-Za-z0-9_]*\s*,\s*"(?<guid>[0-9A-Fa-f\-]{36})"\s*\)""", RegexOptions.Compiled)]
	private static partial Regex DECLARE_INTERFACE_IID_Regex();

	[GeneratedRegex(@"/\*.*?\*/", RegexOptions.Singleline)]
	private static partial Regex MyRegex4();

	[GeneratedRegex(@"//.*?$", RegexOptions.Multiline)]
	private static partial Regex MyRegex5();

	[GeneratedRegex(@"\[\s*(?:[^\]]*?,\s*)*uuid\s*\(\s*(?<guid>[0-9A-Fa-f\-]{36})\s*\)\s*(?:,[^\]]*)*\]\s*interface\s+(?<name>[A-Za-z_][A-Za-z0-9_]*)", RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-US")]
	private static partial Regex MyRegex6();

	private static readonly Regex _idlUuidInterfaceRx = MyRegex6();

	public readonly struct Config
	{
		public required IReadOnlyList<Root> Root { get; init; }
	}

	public readonly struct Root
	{
		public required string Path { get; init; }
		public required string Namespace { get; init; }
		public required string ClassName { get; init; }
		public required string OutputDirectory { get; init; }
		public required IReadOnlyList<Header> Headers { get; init; }
	}

	public readonly struct Header()
	{
		public required string Path { get; init; }
		public string ClassName { get; init; } = string.Empty;
		public IReadOnlyList<string>? Prefixes { get; init; }
		public EmitMode Mode { get; init; } = EmitMode.ConstString;
		public bool Overwrite { get; init; } = true;
		public bool EnableGlobbing { get; init; } = true;
	}

	public enum EmitMode
	{
		ConstString,
		ReadonlyGuidField,
		Both
	}

	public static string Generate(Config cfg)
	{
		// buckets: (ns, outDir, className, mode) -> map[name] = guid
		var buckets = new Dictionary<(string ns, string outDir, string className, EmitMode mode), Dictionary<string, string>>();

		// 1) Parse & collect
		foreach (var root in cfg.Root)
		{
			foreach (var hdr in root.Headers)
			{
				var files = ResolveHeaders(root.Path, [hdr.Path], hdr.EnableGlobbing);
				if (files.Count == 0) continue;

				// Effective class name for this header within this root
				var effClass = string.IsNullOrWhiteSpace(hdr.ClassName) ? root.ClassName : hdr.ClassName;
				var key = (root.Namespace, root.OutputDirectory, effClass, hdr.Mode);

				if (!buckets.TryGetValue(key, out var found))
				{
					found = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
					buckets.Add(key, found);
				}

				var prefixes = hdr.Prefixes?.ToArray() ?? [];

				foreach (var file in files)
				{
					var text = File.ReadAllText(file);
					text = NormalizeHeader(text);

					// DEFINE_GUID(NAME, 0x..., ...)
					foreach (Match m in DEFINE_GUID_Regex().Matches(text))
					{
						var rawName = m.Groups["name"].Value;
						var name = StripIidPrefix(rawName);
						if (!IncludeName(name, prefixes)) continue;
						var guid = AssembleGuidFromHex(m);
						TryAdd(found, name, guid);
					}

					// MIDL_INTERFACE("...") InterfaceName
					foreach (Match m in MIDL_INTERFACE_Regex().Matches(text))
					{
						var guid = CanonicalizeGuid(m.Groups["guid"].Value);
						var name = m.Groups["name"].Value;
						if (!IncludeName(name, prefixes)) continue;
						TryAdd(found, name, guid);
					}

					// __declspec(uuid("...")) struct/class/interface Name
					foreach (Match m in __declspec_Regex().Matches(text))
					{
						var guid = CanonicalizeGuid(m.Groups["guid"].Value);
						var name = m.Groups["name"].Value;
						if (!IncludeName(name, prefixes)) continue;
						TryAdd(found, name, guid);
					}

					// [uuid(...)] interface Name   (IDL form)
					foreach (Match m in _idlUuidInterfaceRx.Matches(text))
					{
						var guid = CanonicalizeGuid(m.Groups["guid"].Value);
						var name = m.Groups["name"].Value;
						if (!IncludeName(name, prefixes)) continue;
						TryAdd(found, name, guid);
					}

					// DECLARE_INTERFACE_IID_(Name, Base, "GUID")  (GameInput/XAudio-style)
					foreach (Match m in DECLARE_INTERFACE_IID_Regex().Matches(text))
					{
						var guid = CanonicalizeGuid(m.Groups["guid"].Value);
						var name = m.Groups["name"].Value;
						if (!IncludeName(name, prefixes)) continue;
						TryAdd(found, name, guid);
					}
				}
			}
		}

		if (buckets.Count == 0)
			throw new InvalidOperationException("No matching interface GUIDs discovered across all roots/headers.");

		string lastPath = string.Empty;

		// 2) Emit: one file per Root, with nested static classes by Header.ClassName
		foreach (var root in cfg.Root)
		{
			// Gather all buckets that belong to this root (by ns + outDir)
			var rootBuckets = buckets.Where(b => b.Key.ns == root.Namespace && b.Key.outDir == root.OutputDirectory).ToList();

			if (rootBuckets.Count == 0) continue;

			Directory.CreateDirectory(root.OutputDirectory);
			var outPath = Path.Combine(root.OutputDirectory, $"{root.ClassName}.g.cs");

			using var sw = new StreamWriter(outPath, false);
			sw.WriteLine("// <auto-generated />");
			sw.WriteLine($"namespace {root.Namespace};");
			sw.WriteLine();
			sw.WriteLine($"public static class {root.ClassName}");
			sw.WriteLine("{");

			// Split into: entries targeting the root class vs. nested classes
			var (rootClassEntries, nestedEntries) = SplitRootVsNested(root, rootBuckets);

			// Emit root-class members (headers with empty or matching ClassName)
			foreach (var ((_, _, _, mode), map) in rootClassEntries)
			{
				foreach (var kv in map.OrderBy(k => k.Key, StringComparer.OrdinalIgnoreCase))
				{
					switch (mode)
					{
						case EmitMode.ConstString:
							sw.WriteLine($"    public const string {kv.Key} = \"{kv.Value}\";");
							break;
						case EmitMode.ReadonlyGuidField:
							sw.WriteLine($"    public static readonly System.Guid {kv.Key} = new(\"{kv.Value}\");");
							break;
						case EmitMode.Both:
							sw.WriteLine($"    public const string IID_{kv.Key} = \"{kv.Value}\";");
							sw.WriteLine($"    public static readonly System.Guid {kv.Key} = new(IID_{kv.Key});");
							break;
					}
				}
			}

			if (rootClassEntries.Any() && nestedEntries.Any())
				sw.WriteLine();

			// Emit nested static classes (group by className, but there can be different modes per bucket)
			// We merge maps per (className, mode) so the mode is preserved; multiple headers with same className/mode merge.
			foreach (var group in nestedEntries.GroupBy(e => (e.Key.className)))
			{
				var className = group.Key;
				sw.WriteLine($"    public static class {className}");
				sw.WriteLine("    {");

				// Within a given nested class, we may have multiple modes (ConstString/ReadonlyGuidField/Both)
				foreach (var ((_, _, _, mode), map) in group.OrderBy(g => g.Key.mode))
				{
					foreach (var kv in map.OrderBy(k => k.Key, StringComparer.OrdinalIgnoreCase))
					{
						switch (mode)
						{
							case EmitMode.ConstString:
								sw.WriteLine($"        public const string {kv.Key} = \"{kv.Value}\";");
								break;
							case EmitMode.ReadonlyGuidField:
								sw.WriteLine($"        public static readonly System.Guid {kv.Key} = new(\"{kv.Value}\");");
								break;
							case EmitMode.Both:
								sw.WriteLine($"        public const string IID_{kv.Key} = \"{kv.Value}\";");
								sw.WriteLine($"        public static readonly System.Guid {kv.Key} = new(IID_{kv.Key});");
								break;
						}
					}
				}

				sw.WriteLine("    }");
				sw.WriteLine();
			}

			sw.WriteLine("}");
			Console.WriteLine($"Generated nested IID file: {outPath}");
			lastPath = outPath;
		}

		return lastPath;

		// ---- local helpers ----
		static (IEnumerable<KeyValuePair<(string ns, string outDir, string className, EmitMode mode), Dictionary<string, string>>> rootClass, IEnumerable<KeyValuePair<(string ns, string outDir, string className, EmitMode mode), Dictionary<string, string>>> nested) SplitRootVsNested(Root root, IEnumerable<KeyValuePair<(string ns, string outDir, string className, EmitMode mode), Dictionary<string, string>>> entries)
		{
			var rootClass = new List<KeyValuePair<(string, string, string, EmitMode), Dictionary<string, string>>>();
			var nested = new List<KeyValuePair<(string, string, string, EmitMode), Dictionary<string, string>>>();

			foreach (var kvp in entries)
			{
				var (_, _, className, _) = kvp.Key;
				bool goesInRoot = string.IsNullOrWhiteSpace(className) || string.Equals(className, root.ClassName, StringComparison.OrdinalIgnoreCase);

				if (goesInRoot) rootClass.Add(kvp);
				else nested.Add(kvp);
			}

			return (rootClass, nested);
		}
	}

	private static string NormalizeHeader(string text)
	{
		text = text.Replace("\\\r\n", "").Replace("\\\n", "");
		text = MyRegex4().Replace(text, "");
		text = MyRegex5().Replace(text, "");
		return text;
	}

	private static bool IncludeName(string ifaceName, IReadOnlyList<string> prefixes)
	{
		if (prefixes.Count == 0) return true;

		// Strip IID_ prefix before matching
		if (ifaceName.StartsWith("IID_", StringComparison.OrdinalIgnoreCase))
			ifaceName = ifaceName[4..];

		foreach (var p in prefixes)
		{
			if (ifaceName.StartsWith(p, StringComparison.OrdinalIgnoreCase))
				return true;
		}

		return false;
	}

	private static string StripIidPrefix(string raw) => raw.StartsWith("IID_", StringComparison.OrdinalIgnoreCase) ? raw[4..] : raw;

	private static void TryAdd(Dictionary<string, string> map, string name, string guid)
	{
		map.TryAdd(name, guid);
	}

	private static string AssembleGuidFromHex(Match m)
	{
		uint d1 = ParseHex32(m.Groups["d1"].Value);
		ushort d2 = (ushort)ParseHex32(m.Groups["d2"].Value);
		ushort d3 = (ushort)ParseHex32(m.Groups["d3"].Value);

		Span<byte> b = stackalloc byte[8];
		for (int i = 1; i <= 8; i++)
			b[i - 1] = (byte)ParseHex32(m.Groups[$"b{i}"].Value);

		var g = new Guid((int)d1, (short)d2, (short)d3, b[0], b[1], b[2], b[3], b[4], b[5], b[6], b[7]);
		return CanonicalizeGuid(g.ToString("D"));
	}

	private static string CanonicalizeGuid(string s) => Guid.TryParse(s, out var g) ? g.ToString("D").ToUpperInvariant() : throw new FormatException($"Invalid GUID literal: {s}");

	private static uint ParseHex32(string s) => uint.Parse(s, NumberStyles.HexNumber, CultureInfo.InvariantCulture);

	private static List<string> ResolveHeaders(string root, IReadOnlyList<string> entries, bool glob)
	{
		var result = new List<string>();
		foreach (var entry in entries)
		{
			string p = entry;
			if (!Path.IsPathRooted(p) && !string.IsNullOrEmpty(root))
				p = Path.Combine(root, p);

			if (glob && (p.Contains('*') || p.Contains('?')))
			{
				var dir = Path.GetDirectoryName(p)!;
				var pat = Path.GetFileName(p);
				if (Directory.Exists(dir))
					result.AddRange(Directory.EnumerateFiles(dir, pat, SearchOption.TopDirectoryOnly));
				continue;
			}

			if (Directory.Exists(p))
			{
				result.AddRange(Directory.EnumerateFiles(p, "*.h", SearchOption.AllDirectories));
				result.AddRange(Directory.EnumerateFiles(p, "*.idl", SearchOption.AllDirectories));
				continue;
			}

			if (File.Exists(p))
				result.Add(p);
		}

		return result.Distinct(StringComparer.OrdinalIgnoreCase).OrderBy(s => s, StringComparer.OrdinalIgnoreCase).ToList();
	}
}