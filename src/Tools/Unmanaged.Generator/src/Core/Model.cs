using System.Runtime.CompilerServices;
using ClangSharp.Interop;

namespace Unmanaged.Generator;

/// <summary>Root node for all C/C++ model constructs.</summary>
public abstract class Node;

/// <summary>Base for all declarations (enums, records, fields, etc.).</summary>
public abstract class Decl : Node
{
    public abstract string Name { get; init; }
    public abstract string? SourceFile { get; init; }
}

/// <summary>Represents a typedef'd enum like "cef_state_t".</summary>
public class EnumDecl : Decl
{
    public override required string Name { get; init; }
    public override string? SourceFile { get; init; }
    public List<EnumMemberDecl> Members { get; } = [];
}

public sealed class EnumMemberDecl : Node
{
    public required string Name { get; init; }
    public required long Value { get; init; }
}

public class RecordDecl : Decl
{
    public override required string Name { get; init; }
    public override string? SourceFile { get; init; }
    public string? ParentName { get; init; }
    public List<FieldDecl> Fields { get; } = [];
    public RecordKind Kind { get; init; } = RecordKind.Struct;
    public bool IsUnion => Kind == RecordKind.Union;
}

public sealed class FieldDecl : Decl
{
    public override required string Name { get; init; }
    public override string? SourceFile { get; init; }
    public required TypeSymbol Type { get; init; }
}

public sealed class ParameterDecl : Decl
{
    public override required string Name { get; init; }
    public override string? SourceFile { get; init; }
    public required TypeSymbol Type { get; init; }
}

/// <summary>Represents a free C function like "cef_initialize".</summary>
public class FunctionDecl : Decl
{
    public override required string Name { get; init; }
    public override string? SourceFile { get; init; }
    public required TypeSymbol ReturnType { get; init; }
    public required List<ParameterDecl> Parameters { get; init; }
    public required CXCallingConv CallingConvention { get; init; }
}

/// <summary>
/// COM-style interface declaration (D3D11/DXGI, CEF C++ wrappers, etc.).
/// This is the high-level representation; the generator can project this to partial classes.
/// </summary>
public sealed class InterfaceDecl : Decl
{
    /// <summary>Managed name of the interface/class (e.g. "D3D11Device").</summary>
    public override required string Name { get; init; }
    /// <summary>Native name as spelled in the headers (e.g. "ID3D11Device").</summary>
    public required string NativeName { get; init; }
    /// <summary>Optional native name of the base interface (e.g. "IUnknown", "ID3D11DeviceChild").</summary>
    public string? BaseInterfaceNativeName { get; init; }
    /// <summary>Optional managed name of the base interface (e.g. "Unknown", "D3D11DeviceChild").</summary>
    public string? BaseInterfaceName { get; set; }
    /// <summary>Methods in vtable order.</summary>
    public List<FunctionDecl> Methods { get; } = [];
    /// <summary>Header file this interface was declared in.</summary>
    public override string? SourceFile { get; init; }

    public Guid? Iid { get; set; }
}

/// <summary>Base for all C/C++ types we care about for CEF CAPI.</summary>
public abstract class TypeSymbol : Node;

/// <summary>Built-in scalar types we map to primitive C# types.</summary>
public enum BuiltinTypeKind
{
    Void,
    Bool,
    Char,
    SChar,
    UChar,
    Short,
    UShort,
    Int,
    UInt,
    Long,
    ULong,
    LongLong,
    ULongLong,
    SizeT,
    Char16,
}

public enum RecordKind
{
    Class,
    Struct,
    Union,
}

/// <summary>Built-in type: int, unsigned int, size_t, etc.</summary>
public sealed class BuiltinTypeSymbol() : TypeSymbol
{
    public BuiltinTypeSymbol (BuiltinTypeKind kind) : this() => Kind = kind;
    public BuiltinTypeKind Kind { get; init; }
}

/// <summary>Named type: typedefs, structs, enums, etc. (cef_string_t, cef_app_t).</summary>
public sealed class NamedTypeSymbol() : TypeSymbol
{
    public NamedTypeSymbol(string name) : this() => Name = name;

    public string Name { get; init => field = Normalize(value); } = string.Empty;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static string Normalize(string spelling)
    {
        if (string.IsNullOrWhiteSpace(spelling)) return spelling;
        spelling = spelling.Trim();

        ReadOnlySpan<string> qualifiers = ["const ", "volatile "];
        foreach (var q in qualifiers)
        {
            if (!spelling.StartsWith(q, StringComparison.Ordinal)) continue;
            spelling = spelling[q.Length..].TrimStart();
            break;
        }

        ReadOnlySpan<string> prefixes = ["struct ", "union ", "enum "];
        foreach (var p in prefixes)
        {
            if (!spelling.StartsWith(p, StringComparison.Ordinal)) continue;
            var tag = spelling[p.Length..];
            if (tag.Length > 1 && tag[0] == '_' && tag.EndsWith("_t", StringComparison.Ordinal)) return tag[1..];
            return tag;
        }

        if (spelling.Length > 1 && spelling[0] == '_' && spelling.EndsWith("_t", StringComparison.Ordinal)) return spelling[1..];
        return spelling;
    }
}

/// <summary>Pointer type: T*, including function pointers (whose element is FunctionTypeSymbol).</summary>
public sealed class PointerTypeSymbol() : TypeSymbol
{
    public PointerTypeSymbol(TypeSymbol elementType) : this() => ElementType = elementType;
    public TypeSymbol ElementType { get; init; } = null!;
}

/// <summary>Function type: return + parameter types + calling convention.</summary>
public sealed class FunctionTypeSymbol : TypeSymbol
{
    public required TypeSymbol ReturnType { get; init; }
    public required List<TypeSymbol> ParameterTypes { get; init; }
    public required CXCallingConv CallingConvention { get; init; }
}

internal sealed class InterfaceCollector(CXTranslationUnit tu, string rootPath) : Collector
{
    public override bool Enabled => true;

    private string _rootPath = rootPath ?? throw new ArgumentNullException(nameof(rootPath));

    public override unsafe void Execute()
    {
        var cursor = GetRootCursor(tu, ref _rootPath);

        Log("=== InterfaceCollector PROBE START ===");

        cursor.VisitChildren((decl, parent, data) =>
        {
            // We care about top-level struct/class declarations in SDK headers
            if (decl.Kind is not (CXCursorKind.CXCursor_StructDecl or CXCursorKind.CXCursor_ClassDecl))
                return CXChildVisitResult.CXChildVisit_Recurse;

            if (!IsCursorInRoot(decl, _rootPath))
                return CXChildVisitResult.CXChildVisit_Continue;

            var native = decl.Spelling.ToString();
            if (string.IsNullOrWhiteSpace(native))
                return CXChildVisitResult.CXChildVisit_Recurse;

            if (native != "ID3D11Device")
                return CXChildVisitResult.CXChildVisit_Recurse;

            if (!decl.IsDefinition)
            {
                Log($"[PROBE] Saw non-definition of {native}, Kind={decl.Kind}. Skipping.");
                return CXChildVisitResult.CXChildVisit_Continue;
            }

            var sourceFile = GetCursorFilePath(decl);
            Log($"[PROBE] Decl: {native}, Kind={decl.Kind}, File={sourceFile}");

            // Dump all children so we can see whether we got methods or lpVtbl fields.
            decl.VisitChildren((child, _, _) =>
            {
                var childName = child.Spelling.ToString();
                var childType = child.Type.Spelling.ToString();

                Log($"  Child: Kind={child.Kind}, Name={childName}, Type={childType}", namePrefix: false);
                return CXChildVisitResult.CXChildVisit_Continue;
            }, default);

            // Now compute the "mode" verdict.
            var hasCxxMethods = false;
            var hasLpVtblField = false;

            decl.VisitChildren((child, _, _) =>
            {
                if (child.Kind == CXCursorKind.CXCursor_CXXMethod)
                    hasCxxMethods = true;

                if (child.Kind == CXCursorKind.CXCursor_FieldDecl && child.Spelling.ToString() == "lpVtbl")
                    hasLpVtblField = true;

                return CXChildVisitResult.CXChildVisit_Continue;
            }, default);

            if (hasCxxMethods)
            {
                Log("[PROBE RESULT] ID3D11Device is a C++ COM interface (has CXXMethod children).");
            }
            else if (hasLpVtblField)
            {
                Log("[PROBE RESULT] ID3D11Device is C-style COM (struct with lpVtbl field).");
            }
            else
            {
                Log("[PROBE RESULT] ID3D11Device has neither CXXMethod nor lpVtbl; unexpected shape.");
            }

            Log("=== InterfaceCollector PROBE END ===");

            // We've seen what we needed; no need to keep walking.
            return CXChildVisitResult.CXChildVisit_Break;
        }, default);
    }
}