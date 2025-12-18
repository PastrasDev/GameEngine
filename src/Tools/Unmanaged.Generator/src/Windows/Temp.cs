using System.Text;
using ClangSharp.Interop;

namespace Unmanaged.Generator.Windows;

internal sealed class D3DRecordCollector(CXTranslationUnit tu, string rootPath) : Collector
{
    public override bool Enabled => true;

    public IReadOnlyList<RecordDecl> Records { get; private set; } = [];

    private string _rootPath = rootPath;

    private static readonly HashSet<string> _targetNames =
    [
        "D3D11_RENDER_TARGET_VIEW_DESC",
    ];

    public override unsafe void Execute()
    {
        var root = GetRootCursor(tu, ref _rootPath);
        var result = new List<RecordDecl>();
        var seenNames = new HashSet<string>(StringComparer.Ordinal);

        root.VisitChildren((cursor, parent, clientData) =>
        {
            // Only handle typedefs that come from our Windows Kits root.
            if (cursor.Kind == CXCursorKind.CXCursor_TypedefDecl && IsCursorInRoot(cursor, _rootPath))
            {
                HandleRecordTypedef(cursor, _rootPath, result, seenNames);
            }

            // Always recurse so we see typedefs inside included headers.
            return CXChildVisitResult.CXChildVisit_Recurse;
        }, new CXClientData(nint.Zero));

        Log($"Collected {result.Count} records (structs/unions).");
        Records = result;
    }

    private static void HandleRecordTypedef(CXCursor typedefCursor, string rootPath, List<RecordDecl> records, HashSet<string> seenNames)
    {
        var typedefName = typedefCursor.Spelling.ToString();
        if (string.IsNullOrWhiteSpace(typedefName))
            return;

        if (!_targetNames.Contains(typedefName))
            return;

        if (!IsCursorInRoot(typedefCursor, rootPath))
            return;

        var underlying = typedefCursor.TypedefDeclUnderlyingType;
        var canonical = underlying.CanonicalType;

        if (canonical.kind != CXTypeKind.CXType_Record)
            return;

        if (!seenNames.Add(typedefName))
            return;

        var recordDecl = canonical.Declaration;
        if (recordDecl.IsNull)
            return;

        BuildRecordAndNestedUnions(typedefName, recordDecl, rootPath, records);
    }

    private static unsafe void BuildRecordAndNestedUnions(string typedefName, CXCursor recordDecl, string rootPath, List<RecordDecl> records)
    {
        var main = new RecordDecl { Name = typedefName, SourceFile = GetCursorFilePath(recordDecl), Kind = RecordKind.Struct, };

        var unionIndex = 0;

        recordDecl.VisitChildren((child, _, _) =>
        {
            switch (child.Kind)
            {
                case CXCursorKind.CXCursor_FieldDecl:
                {
                    // Normal field on the main struct
                    var fieldName = child.Spelling.ToString();
                    if (string.IsNullOrWhiteSpace(fieldName))
                        fieldName = "field_" + main.Fields.Count;

                    var type = BuildTypeSymbol(child.Type);
                    var fieldInfo = new FieldDecl { Name = fieldName, SourceFile = GetCursorFilePath(child), Type = type, };

                    main.Fields.Add(fieldInfo);
                    break;
                }

                case CXCursorKind.CXCursor_UnionDecl:
                {
                    var nestedName = $"UNION{unionIndex++}";

                    var union = new RecordDecl
                    {
                        Name = nestedName, SourceFile = GetCursorFilePath(child), Kind = RecordKind.Union, ParentName = typedefName,
                    };

                    child.VisitChildren((unionChild, _, _) =>
                    {
                        if (unionChild.Kind != CXCursorKind.CXCursor_FieldDecl)
                            return CXChildVisitResult.CXChildVisit_Continue;

                        var ufName = unionChild.Spelling.ToString();
                        if (string.IsNullOrWhiteSpace(ufName))
                            ufName = "field_" + union.Fields.Count;

                        var uType = BuildTypeSymbol(unionChild.Type);

                        union.Fields.Add(new FieldDecl { Name = ufName, SourceFile = GetCursorFilePath(unionChild), Type = uType, });

                        return CXChildVisitResult.CXChildVisit_Continue;
                    }, new CXClientData(nint.Zero));

                    if (union.Fields.Count > 0)
                    {
                        records.Add(union);

                        main.Fields.Add(new FieldDecl { Name = "Anonymous", SourceFile = GetCursorFilePath(child), Type = new NamedTypeSymbol(nestedName), });
                    }

                    break;
                }
            }

            return CXChildVisitResult.CXChildVisit_Continue;
        }, new CXClientData(nint.Zero));

        if (main.Fields.Count > 0)
            records.Add(main);
    }
}

internal sealed class D3DRecordEmitter(IReadOnlyList<RecordDecl> records, string outputDir, string @namespace, string fileName) : Emitter
{
    public override bool Enabled => true;

    public override void Execute()
    {
        ArgumentNullException.ThrowIfNull(records);

        if (string.IsNullOrWhiteSpace(outputDir))
            throw new ArgumentException("Output directory must not be null or empty.", nameof(outputDir));

        if (string.IsNullOrWhiteSpace(@namespace))
            throw new ArgumentException("Namespace must not be null or empty.", nameof(@namespace));

        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("File name must not be null or empty.", nameof(fileName));

        Directory.CreateDirectory(outputDir);
        var path = Path.Combine(outputDir, fileName);

        var sb = new StringBuilder(128 * 1024);

        sb.AppendLine("// <auto-generated />");
        sb.AppendLine("// D3D11/DXGI POD structs and unions.");
        sb.AppendLine();
        sb.AppendLine($"{Keyword.Using} System;");
        sb.AppendLine($"{Keyword.Using} System.Runtime.InteropServices;");
        sb.AppendLine();
        sb.AppendLine($"{Keyword.Namespace} {@namespace};");
        sb.AppendLine();

        // Split into parent structs vs union records.
        var parentStructs = new List<RecordDecl>();
        var unionsByParent = new Dictionary<string, List<RecordDecl>>(StringComparer.Ordinal);

        foreach (var r in records)
        {
            if (r is { IsUnion: true, ParentName: { } parentName })
            {
                if (!unionsByParent.TryGetValue(parentName, out var list))
                {
                    list = [];
                    unionsByParent[parentName] = list;
                }

                list.Add(r);
            }
            else
            {
                parentStructs.Add(r);
            }
        }

        parentStructs.Sort((x, y) => string.CompareOrdinal(x.Name, y.Name));

        for (var i = 0; i < parentStructs.Count; i++)
        {
            var s = parentStructs[i];
            EmitRecord(sb, s, unionsByParent);

            if (i < parentStructs.Count - 1)
                sb.AppendLine();
        }

        File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
    }

    private static void EmitRecord(StringBuilder sb, RecordDecl s, IReadOnlyDictionary<string, List<RecordDecl>> unionsByParent)
    {
        sb.AppendLine("/// <summary>");
        sb.AppendLine($"/// Generated from C struct '{s.Name}'.");
        sb.AppendLine("/// </summary>");

        if (s.IsUnion)
            sb.AppendLine("[StructLayout(LayoutKind.Explicit)]");
        else
            sb.AppendLine("[StructLayout(LayoutKind.Sequential)]");

        sb.AppendLine($"{Keyword.Public} {Keyword.Unsafe} {Keyword.Struct} {s.Name}");
        sb.AppendLine("{");

        // Fields of the parent struct
        foreach (var f in s.Fields)
        {
            var managedType = GetManagedTypeName(f.Type);
            var escapedName = EscapeIdentifier(f.Name);

            // Parent should not be union (by construction), but keep the branch for completeness
            if (s.IsUnion)
                sb.Append("    [FieldOffset(0)] ");

            sb.AppendLine($"    {Keyword.Public} {managedType} {escapedName};");
        }

        // Nested unions, if any
        if (unionsByParent.TryGetValue(s.Name, out var unions))
        {
            unions.Sort((x, y) => string.CompareOrdinal(x.Name, y.Name));

            sb.AppendLine();

            foreach (var u in unions)
            {
                EmitNestedUnion(sb, u);
                sb.AppendLine();
            }
        }

        sb.AppendLine("}");
    }

    private static void EmitNestedUnion(StringBuilder sb, RecordDecl u)
    {
        // Local nested name: strip "<Parent>_" prefix if present to keep it shorter.
        // Example: D3D11_RENDER_TARGET_VIEW_DESC_UNION0 -> UNION0
        var nestedName = u.Name;
        var idx = nestedName.LastIndexOf('_');
        if (idx > 0 && idx < nestedName.Length - 1)
            nestedName = nestedName[(idx + 1)..];

        sb.AppendLine("    /// <summary>");
        sb.AppendLine($"    /// Generated from C union '{u.Name}'.");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    [StructLayout(LayoutKind.Explicit)]");
        sb.AppendLine($"    {Keyword.Public} {Keyword.Unsafe} {Keyword.Struct} {nestedName}");
        sb.AppendLine("    {");

        foreach (var f in u.Fields)
        {
            var managedType = GetManagedTypeName(f.Type);
            var escapedName = EscapeIdentifier(f.Name);

            sb.Append("        [FieldOffset(0)] ");
            sb.AppendLine($"{Keyword.Public} {managedType} {escapedName};");
        }

        sb.AppendLine("    }");
    }
}