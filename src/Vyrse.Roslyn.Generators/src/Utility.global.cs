using System.Buffers;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Vyrse.Roslyn.Generators;

[assembly: SuppressMessage("RoslynDiagnostics", "RS1041:Do not use obsoleted APIs", Justification = "Rider-only; no .NET Framework support required.")]

[PublicAPI]
public static partial class Utility
{
    public static partial class Enums
    {
        public static partial bool IsEnum(INamedTypeSymbol symbol);
        public static partial bool HasFlags(INamedTypeSymbol enumType);
        public static partial SpecialType GetUnderlyingSpecialType(INamedTypeSymbol enumType);
        public static partial ImmutableArray<IFieldSymbol> GetDeclaredFields(INamedTypeSymbol enumType);
        public static partial ImmutableArray<string> GetEnumMemberNames(INamedTypeSymbol enumType);
    }

    public static partial class Attributes
    {
        public static partial bool HasAttribute<T>(INamedTypeSymbol symbol) where T : Attribute;
        public static partial bool HasAttribute(ISymbol symbol, string attributeFullName);
        public static partial bool HasAttribute(ISymbol symbol, INamedTypeSymbol attributeSymbol);
        public static partial string GetAttributeShortName<T>() where T : Attribute;
    }

    public static partial class Types
    {
        public static partial bool IsUnmanaged(INamedTypeSymbol type);
        public static partial bool IsString(INamedTypeSymbol type);
        public static partial bool IsScalar(INamedTypeSymbol type);
        public static partial bool IsNullableRefType(INamedTypeSymbol type);
        public static partial bool IsSpan(INamedTypeSymbol type);
        public static partial bool IsReadOnlySpan(INamedTypeSymbol type);
        public static partial ITypeSymbol GetSpanElementType(INamedTypeSymbol type);
        public static partial ITypeSymbol? GetReadOnlySpanElementType(INamedTypeSymbol type);
    }

    public static partial class Symbols
    {
        public static partial INamedTypeSymbol GetNamedTypeSymbol<T>(Compilation compilation);
        public static partial INamedTypeSymbol GetNamedTypeSymbol(Compilation compilation, string fullName);
        public static partial INamedTypeSymbol GetNamedTypeSymbol(Compilation compilation, Type type);
        public static partial IncrementalValueProvider<ImmutableArray<INamedTypeSymbol>> FindSymbolsFromAttribute<T>(scoped in IncrementalGeneratorInitializationContext context) where T : Attribute;
        public static partial IncrementalValueProvider<ImmutableArray<INamedTypeSymbol>> FindSymbolsFromAttribute(scoped in IncrementalGeneratorInitializationContext context, string fullName);
        public static partial bool HasPrimaryConstructor(INamedTypeSymbol type);
        public static partial bool HasInstanceConstructor(INamedTypeSymbol type);
        public static partial bool ImplementsInterface(ITypeSymbol type, string interfaceFullName);
        public static partial bool DerivesFrom(ITypeSymbol type, string name);
        public static partial bool IsMethodDeclared(INamedTypeSymbol type, string name);
        public static partial IMethodSymbol? GetMethodSymbol(INamedTypeSymbol type, string name);
    }

    public static partial class Naming
    {
        public static partial string Name(ISymbol symbol);
        public static partial string FullName(INamedTypeSymbol symbol, bool includeGlobalPrefix = false);
        public static partial string Namespace(INamedTypeSymbol symbol, bool includeGlobalPrefix = false);
        public static partial string HintName(INamedTypeSymbol symbol, bool fullyQualified = false);
        public static partial string LowerCaseName(ISymbol symbol);
    }

    public static partial class Syntax
    {
        public static partial SyntaxToken Identifier<T>();
        public static partial SyntaxToken Identifier(ISymbol symbol);
        // public static partial SyntaxToken Identifier(string name);
        public static partial IdentifierNameSyntax IdentifierName<T>();
        public static partial IdentifierNameSyntax IdentifierName(ISymbol symbol);
        // public static partial IdentifierNameSyntax IdentifierName(string name);
        public static partial AttributeSyntax Attribute<T>() where T : Attribute;
        public static partial SyntaxKind Accessibility(Accessibility accessibility);
        public static partial void AddMembers(ref ClassDeclarationSyntax classDecl, params MemberDeclarationSyntax[] members);
    }

    public static partial class Usings
    {
        public static partial UsingDirectiveSyntax[] CreateUsingDirectiveSyntaxArray(HashSet<INamespaceSymbol?> usings, INamedTypeSymbol selfSymbol);
        public static partial INamedTypeSymbol AddUsing(HashSet<INamespaceSymbol?> usings, INamedTypeSymbol symbol);
        public static partial bool RemoveUsing(HashSet<INamespaceSymbol?> usings, INamedTypeSymbol symbol);
    }
}

public static partial class Utility
{
    public static partial class Enums
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial bool IsEnum(INamedTypeSymbol symbol) => symbol.TypeKind == TypeKind.Enum;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial bool HasFlags(INamedTypeSymbol enumType)
        {
            var full = typeof(FlagsAttribute).FullName;
            if (full is null) return false;

            foreach (var a in enumType.GetAttributes())
                if (a.AttributeClass?.ToDisplayString() == full)
                    return true;

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial SpecialType GetUnderlyingSpecialType(INamedTypeSymbol enumType) => enumType.TypeKind != TypeKind.Enum ? throw new InvalidOperationException($"Type '{enumType.ToDisplayString()}' is not an enum.") : enumType.EnumUnderlyingType!.SpecialType;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial ImmutableArray<IFieldSymbol> GetDeclaredFields(INamedTypeSymbol enumType)
        {
            if (enumType.TypeKind != TypeKind.Enum)
                throw new InvalidOperationException($"Type '{enumType.ToDisplayString()}' is not an enum.");

            var members = enumType.GetMembers();
            var builder = ImmutableArray.CreateBuilder<IFieldSymbol>(members.Length);

            foreach (var m in members)
                if (m is IFieldSymbol { IsImplicitlyDeclared: false, HasConstantValue: true } f)
                    builder.Add(f);

            return builder.ToImmutable();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial ImmutableArray<string> GetEnumMemberNames(INamedTypeSymbol enumType)
        {
            if (enumType.TypeKind != TypeKind.Enum)
                throw new InvalidOperationException($"Type '{enumType.ToDisplayString()}' is not an enum.");

            var members = enumType.GetMembers();
            var builder = ImmutableArray.CreateBuilder<string>(members.Length);

            foreach (var m in members)
                if (m is IFieldSymbol { IsImplicitlyDeclared: false, HasConstantValue: true } f)
                    builder.Add(f.Name);

            return builder.ToImmutable();
        }
    }

    public static partial class Attributes
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial bool HasAttribute<T>(INamedTypeSymbol symbol) where T : Attribute
        {
            var fullName = typeof(T).FullName;
            if (fullName is null) return false;
            foreach (var a in symbol.GetAttributes())
            {
                if (a.AttributeClass?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) == "global::" + fullName) return true;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial bool HasAttribute(ISymbol symbol, string attributeFullName)
        {
            foreach (var a in symbol.GetAttributes())
            {
                if (a.AttributeClass?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) == attributeFullName) return true;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial bool HasAttribute(ISymbol symbol, INamedTypeSymbol attributeSymbol)
        {
            foreach (var a in symbol.GetAttributes())
            {
                if (SymbolEqualityComparer.Default.Equals(a.AttributeClass, attributeSymbol)) return true;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial string GetAttributeShortName<T>() where T : Attribute
        {
            ReadOnlySpan<char> name = typeof(T).Name;
            const string suffix = "Attribute";
            return name.EndsWith(suffix, StringComparison.Ordinal) ? new string(name[..^suffix.Length]) : new string(name);
        }
    }

    public static partial class Types
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial bool IsUnmanaged(INamedTypeSymbol type) => type.IsUnmanagedType;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial bool IsString(INamedTypeSymbol type) => type.SpecialType == SpecialType.System_String;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial bool IsScalar(INamedTypeSymbol type) => type.SpecialType != SpecialType.None || type.TypeKind == TypeKind.Enum;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial bool IsNullableRefType(INamedTypeSymbol type) => type is { IsReferenceType: true, NullableAnnotation: NullableAnnotation.Annotated };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial bool IsSpan(INamedTypeSymbol type) => type is { Name: nameof(Span<>), ContainingNamespace: { } ns } && ns.ToDisplayString() == typeof(Span<>).Namespace;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial bool IsReadOnlySpan(INamedTypeSymbol type) => type is { Name: nameof(ReadOnlySpan<>), ContainingNamespace: { } ns } && ns.ToDisplayString() == typeof(ReadOnlySpan<>).Namespace;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial ITypeSymbol GetSpanElementType(INamedTypeSymbol type) => IsSpan(type) ? type.TypeArguments[0] : throw new InvalidOperationException($"Type '{type.ToDisplayString()}' is not a Span<>.");

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial ITypeSymbol? GetReadOnlySpanElementType(INamedTypeSymbol type) => IsReadOnlySpan(type) ? type.TypeArguments[0] : throw new InvalidOperationException($"Type '{type.ToDisplayString()}' is not a ReadOnlySpan<>.");
    }

    public static partial class Symbols
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial INamedTypeSymbol GetNamedTypeSymbol<T>(Compilation compilation) => compilation.GetTypeByMetadataName(typeof(T).FullName ?? throw new InvalidOperationException($"Type '{typeof(T)}' does not have a full name.")) ?? throw new InvalidOperationException($"Type '{typeof(T).FullName}' could not be found in the compilation.");

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial INamedTypeSymbol GetNamedTypeSymbol(Compilation compilation, string fullName) => compilation.GetTypeByMetadataName(fullName) ?? throw new InvalidOperationException($"Type '{fullName}' could not be found in the compilation.");

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial INamedTypeSymbol GetNamedTypeSymbol(Compilation compilation, Type type)
        {
            return compilation.GetTypeByMetadataName(type.FullName ?? throw new InvalidOperationException($"Type '{type}' does not have a full name.")) ?? throw new InvalidOperationException($"Type '{type.FullName}' could not be found in the compilation.");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial IncrementalValueProvider<ImmutableArray<INamedTypeSymbol>> FindSymbolsFromAttribute<T>(scoped in IncrementalGeneratorInitializationContext context) where T : Attribute
        {
            var attributeName = typeof(T).FullName;
            return attributeName is null ? throw new InvalidOperationException($"Type '{typeof(T)}' does not have a full name.") : FindSymbolsFromAttribute(context, attributeName);
        }

        public static partial IncrementalValueProvider<ImmutableArray<INamedTypeSymbol>> FindSymbolsFromAttribute(scoped in IncrementalGeneratorInitializationContext context, string fullName)
        {
            return context.SyntaxProvider.ForAttributeWithMetadataName(fullyQualifiedMetadataName: fullName, predicate: static (_, _) => true, transform: static (ctx, ct) =>
            {
                if (ctx.TargetSymbol is not INamedTypeSymbol type) return null;
                if (type.IsAbstract) return null;

                foreach (var r in type.DeclaringSyntaxReferences)
                {
                    if (r.GetSyntax(ct) is not ClassDeclarationSyntax cls)
                        continue;

                    var modifiers = cls.Modifiers;
                    for (var i = 0; i < modifiers.Count; i++)
                        if (modifiers[i].IsKind(SyntaxKind.PartialKeyword))
                            return type;
                }

                return null;
            }).Where(static t => t is not null).Select(static (t, _) => t!).Collect();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial bool HasPrimaryConstructor(INamedTypeSymbol type)
        {
            foreach (var r in type.DeclaringSyntaxReferences)
            {
                if (r.GetSyntax() is ClassDeclarationSyntax { ParameterList.Parameters.Count: > 0 }) return true;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial bool HasInstanceConstructor(INamedTypeSymbol type)
        {
            foreach (var c in type.InstanceConstructors)
            {
                if (!c.IsImplicitlyDeclared) return true;
            }

            return false;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial bool ImplementsInterface(ITypeSymbol type, string interfaceFullName)
        {
            foreach (var iface in type.AllInterfaces)
            {
                if (iface.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) == interfaceFullName) return true;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial bool DerivesFrom(ITypeSymbol type, string name)
        {
            for (var x = type as INamedTypeSymbol; x != null; x = x.BaseType)
                if (x.Name == name) return true;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial bool IsMethodDeclared(INamedTypeSymbol type, string name)
        {
            var members = type.GetMembers(name);
            foreach (var t in members)
            {
                if (t is not IMethodSymbol m) continue;
                if (m.IsImplicitlyDeclared) continue;
                if (m.MethodKind != MethodKind.Ordinary) continue;
                if (m.Parameters.Length != 0) continue;
                if (!m.ReturnsVoid) continue;
                if (m.DeclaredAccessibility != Accessibility.Public) continue;
                if (!SymbolEqualityComparer.Default.Equals(m.ContainingType, type)) continue;
                return true;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial IMethodSymbol? GetMethodSymbol(INamedTypeSymbol type, string name)
        {
            var members = type.GetMembers(name);
            foreach (var t in members)
            {
                if (t is not IMethodSymbol m) continue;
                if (m.IsImplicitlyDeclared) continue;
                if (m.MethodKind != MethodKind.Ordinary) continue;
                if (m.Parameters.Length != 0) continue;
                if (!m.ReturnsVoid) continue;
                if (m.DeclaredAccessibility != Accessibility.Public) continue;
                if (!SymbolEqualityComparer.Default.Equals(m.ContainingType, type)) continue;
                return m;
            }

            return null;
        }
    }

    public static partial class Naming
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial string Name(ISymbol symbol) => symbol.Name;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial string FullName(INamedTypeSymbol symbol, bool includeGlobalPrefix) => Namespace(symbol, includeGlobalPrefix) is { Length: > 0 } ns ? $"{ns}.{symbol.Name}" : symbol.Name;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial string Namespace(INamedTypeSymbol symbol, bool includeGlobalPrefix) => symbol.ContainingNamespace is { IsGlobalNamespace: false } ns ? ns.ToDisplayString(includeGlobalPrefix ? SymbolDisplayFormat.FullyQualifiedFormat : SymbolDisplayFormat.FullyQualifiedFormat.WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.Omitted)) : string.Empty;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial string HintName(INamedTypeSymbol symbol, bool fullyQualified)
        {
            var parts = new Stack<string>();
            for (var t = symbol; t is not null; t = t.ContainingType)
                parts.Push(t.MetadataName);

            var typePath = string.Join(".", parts);

            if (!fullyQualified) return $"{symbol.MetadataName}.g.cs";

            var ns = Namespace(symbol, includeGlobalPrefix: true);
            var fullPath = ns.Length > 0 ? $"{ns}.{typePath}" : typePath;
            return fullPath + ".g.cs";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial string LowerCaseName(ISymbol symbol)
        {
            var name = symbol.Name;
            if (string.IsNullOrEmpty(name) || char.IsLower(name[0]))
                return name;

            return string.Create(name.Length, name, static (span, src) =>
            {
                span[0] = char.ToLowerInvariant(src[0]);
                src.AsSpan(1).CopyTo(span[1..]);
            });
        }
    }

    public static partial class Syntax
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial SyntaxToken Identifier<T>() => SyntaxFactory.Identifier(typeof(T).Name);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial SyntaxToken Identifier(ISymbol symbol) => SyntaxFactory.Identifier(symbol.Name);

        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public static partial SyntaxToken Identifier(string name) => SyntaxFactory.Identifier(name);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial IdentifierNameSyntax IdentifierName<T>() => SyntaxFactory.IdentifierName(typeof(T).Name);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial IdentifierNameSyntax IdentifierName(ISymbol symbol) => SyntaxFactory.IdentifierName(symbol.Name);

        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        // public static partial IdentifierNameSyntax IdentifierName(string name) => SyntaxFactory.IdentifierName(name);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial AttributeSyntax Attribute<T>() where T : Attribute => SyntaxFactory.Attribute(SyntaxFactory.IdentifierName(typeof(T).AttributeShortName()));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial SyntaxKind Accessibility(Accessibility accessibility) => accessibility switch
        {
            Microsoft.CodeAnalysis.Accessibility.Public => SyntaxKind.PublicKeyword,
            Microsoft.CodeAnalysis.Accessibility.Internal => SyntaxKind.InternalKeyword,
            Microsoft.CodeAnalysis.Accessibility.Private => SyntaxKind.PrivateKeyword,
            Microsoft.CodeAnalysis.Accessibility.Protected => SyntaxKind.ProtectedKeyword,
            Microsoft.CodeAnalysis.Accessibility.ProtectedAndInternal => SyntaxKind.ProtectedKeyword,
            Microsoft.CodeAnalysis.Accessibility.ProtectedOrInternal => SyntaxKind.PrivateKeyword,
            _ => SyntaxKind.InternalKeyword
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial void AddMembers(ref ClassDeclarationSyntax classDecl, params MemberDeclarationSyntax[] members) => classDecl = classDecl.AddMembers(members);
    }

    public static partial class Usings
    {
        public static partial UsingDirectiveSyntax[] CreateUsingDirectiveSyntaxArray(HashSet<INamespaceSymbol?> usings, INamedTypeSymbol selfSymbol)
        {
            var selfNs = selfSymbol.ContainingNamespace is { IsGlobalNamespace: false } s ? s.ToDisplayString() : null;
            var list = new List<string>(usings.Count);
            foreach (var n in usings)
            {
                if (n is null || n.IsGlobalNamespace) continue;
                var name = n.ToDisplayString();
                if (selfNs is not null && name == selfNs) continue;
                list.Add(name);
            }

            list.Sort(StringComparer.Ordinal);
            var result = new UsingDirectiveSyntax[list.Count];
            for (var i = 0; i < list.Count; i++)
            {
                var parsed = SyntaxFactory.ParseName("global::" + list[i]);
                result[i] = SyntaxFactory.UsingDirective(parsed);
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial INamedTypeSymbol AddUsing(HashSet<INamespaceSymbol?> usings, INamedTypeSymbol symbol)
        {
            var ns = symbol.ContainingNamespace;
            if (ns is not null && !ns.IsGlobalNamespace) usings.Add(ns);
            return symbol;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static partial bool RemoveUsing(HashSet<INamespaceSymbol?> usings, INamedTypeSymbol symbol)
        {
            var ns = symbol.ContainingNamespace;
            if (ns is not null && !ns.IsGlobalNamespace) return usings.Remove(ns);
            return false;
        }
    }

    [PublicAPI]
    public static class Method
    {
        public readonly struct Description()
        {
            public required string Name { get; init; }
            public required SyntaxKind ReturnType { get; init; }
            public required Accessibility Accessibility { get; init; }
            public AttributeListSyntax? Attributes { get; init; } = null;
            public List<StatementSyntax>? Statements { get; init; } = null;
            public string? Prefix { get; init; } = null;
            public string? Suffix { get; init; } = null;
            public bool IsPartial { get; init; } = false;
            public bool IsStatic { get; init; } = false;
            public bool IsAsync { get; init; } = false;
            public bool IsUnsafe { get; init; } = false;
            public bool IsExtern { get; init; } = false;
            public bool IsOverride { get; init; } = false;
            public bool IsVirtual { get; init; } = false;
            public bool IsAbstract { get; init; } = false;
            public bool IsSealed { get; init; } = false;
            public bool IsReadOnly { get; init; } = false;
            public bool IsDeclarationOnly { get; init; } = false;
        }

        public static MethodDeclarationSyntax Declare(scoped in Description options)
        {
            var name = (options.Prefix ?? string.Empty) + options.Name + (options.Suffix ?? string.Empty);
            var method = SyntaxFactory.MethodDeclaration(SyntaxFactory.PredefinedType(SyntaxFactory.Token(options.ReturnType)), name);
            var mods = new List<SyntaxToken>(8) { SyntaxFactory.Token(Syntax.Accessibility(options.Accessibility)) };

            if (options.IsStatic) mods.Add(SyntaxFactory.Token(SyntaxKind.StaticKeyword));
            if (options.IsAsync) mods.Add(SyntaxFactory.Token(SyntaxKind.AsyncKeyword));
            if (options.IsUnsafe) mods.Add(SyntaxFactory.Token(SyntaxKind.UnsafeKeyword));
            if (options.IsExtern) mods.Add(SyntaxFactory.Token(SyntaxKind.ExternKeyword));
            if (options.IsOverride) mods.Add(SyntaxFactory.Token(SyntaxKind.OverrideKeyword));
            if (options.IsVirtual) mods.Add(SyntaxFactory.Token(SyntaxKind.VirtualKeyword));
            if (options.IsAbstract) mods.Add(SyntaxFactory.Token(SyntaxKind.AbstractKeyword));
            if (options.IsSealed) mods.Add(SyntaxFactory.Token(SyntaxKind.SealedKeyword));
            if (options.IsReadOnly) mods.Add(SyntaxFactory.Token(SyntaxKind.ReadOnlyKeyword));
            if (options.IsPartial) mods.Add(SyntaxFactory.Token(SyntaxKind.PartialKeyword));

            method = method.WithModifiers(SyntaxFactory.TokenList(mods));

            if (options.Attributes is not null)
            {
                method = method.AddAttributeLists(options.Attributes);
            }

            if (options.IsDeclarationOnly || options.IsExtern || options.IsAbstract)
            {
                return method.WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken));
            }

            return method.WithBody(SyntaxFactory.Block(options.Statements is { Count: > 0 } ? options.Statements : []));
        }
    }
}