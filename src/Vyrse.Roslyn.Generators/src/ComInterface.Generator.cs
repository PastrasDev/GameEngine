using System.Runtime.InteropServices;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Vyrse.Generators;

using static Utilities;
using static SyntaxFactory;
using static Keyword;
using static Op;

[Generator]
public class ComInterfaceGenerator : IIncrementalGenerator
{
	public const string InterfaceAttribute = "Windows.Win32.System.Com.InterfaceAttribute";
	public const string MethodAttribute = "Windows.Win32.System.Com.MethodAttribute";

	private enum PKind
	{
		InBlit,           // in T (unmanaged struct)
		RefBlit,          // ref T (unmanaged struct)
		OutBlit,          // out T (unmanaged struct)
		ReadOnlySpanBlit, // ReadOnlySpan<T> (unmanaged T) -> (T* data, int length)
		SpanBlit,         // Span<T> (unmanaged T)         -> (T* data, int length)
		Utf16,            // string or ReadOnlySpan<char>  -> (char*)
		Interface,        // Unknown-derived input         -> (nint)
		OutInterface,     // out Unknown-derived           -> (nint*)
		Scalar            // unmanaged scalars (incl. enums), bool => int
	}

	private sealed record PSpec(PKind Kind, ITypeSymbol Type, string Name, ITypeSymbol? Elem = null, bool NeedsCount = false);

	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		var targets = FindSymbolsFromAttribute(context, InterfaceAttribute);

		context.RegisterSourceOutput(targets, static (spc, symbols) =>
		{
			foreach (var symbol in symbols.Distinct<INamedTypeSymbol>(SymbolEqualityComparer.Default))
			{
				try
				{
					var unit = EmitForClass(symbol);
					if (unit is null) continue;
					var text = unit.NormalizeWhitespace().ToFullString();
					spc.AddSource(symbol.HintName(), text);
				}
				catch (Exception ex)
				{
					spc.AddSource($"{symbol.Name}.error.generated.cs", $"// {ex}");
				}
			}
		});
	}

	private static CompilationUnitSyntax? EmitForClass(INamedTypeSymbol cls)
	{
		// Pick up: [Method]-tagged OR public partial instance methods (no attribute)
		var methods = GetLocalComMethodsInSourceOrder(cls).ToArray();
		if (methods.Length == 0)
			return null;

		var ns = cls.ContainingNamespace?.IsGlobalNamespace == true ? null : cls.ContainingNamespace!.ToDisplayString();

		// We’ll collect in separate buckets to control the order:
		// fields -> ctor -> methods
		var fields = new List<MemberDeclarationSyntax>(methods.Length);
		var impls = new List<MemberDeclarationSyntax>(methods.Length);

		// Collect namespaces from return types and parameters
		var namespaces = new HashSet<string?>();
		foreach (var m in methods)
		{
			if (!string.IsNullOrEmpty(m.ReturnType.ContainingNamespace?.ToDisplayString()))
				namespaces.Add(m.ReturnType.ContainingNamespace?.ToDisplayString());

			foreach (var p in m.Parameters)
			{
				if (!string.IsNullOrEmpty(p.Type.ContainingNamespace?.ToDisplayString()))
					namespaces.Add(p.Type.ContainingNamespace?.ToDisplayString());
			}
		}

		// Infer exactly which BCL namespaces are needed by the bodies we emit
		bool needsUnsafe = false;
		bool needsMemoryMarshal = false;

		foreach (var m in methods)
		{
			var ps = Classify(m, out _);
			if (ps.Any(p => p.Kind is PKind.InBlit or PKind.RefBlit))
				needsUnsafe = true;

			if (ps.Any(p => p.Kind is PKind.ReadOnlySpanBlit or PKind.SpanBlit))
				needsMemoryMarshal = true;
		}

		if (needsUnsafe) namespaces.Add("System.Runtime.CompilerServices");
		if (needsMemoryMarshal) namespaces.Add("System.Runtime.InteropServices");

		// Emit members (per-instance field + impl, or stub on failure)
		foreach (var m in methods)
		{
			try
			{
				EmitMethod(cls, m, out var field, out var impl);
				fields.Add(field);
				impls.Add(impl);
			}
			catch (Exception ex)
			{
				var detail = ex.Message.Replace("-->", "→").Replace("<", "&lt;").Replace(">", "&gt;");
				var xmlComment = "/// <summary>\n" + "/// ⚠️ Auto-stubbed: generation failed.\n" + "/// </summary>\n" + "/// <remarks>\n" + $"/// <para>{EscapeXml(detail).Replace("\n", "\n/// ")}</para>\n" + "/// </remarks>\n";

				var stub = EmitStubMethod(m).WithLeadingTrivia(TriviaList(Comment(xmlComment), CarriageReturnLineFeed));
				impls.Add(stub);
			}
		}

		// Build constructor (optional)
		ConstructorDeclarationSyntax? ctorNode = null;

		if (ShouldEmitCtor(cls))
		{
			var fqThis = cls.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
			bool isUnknown = fqThis == "global::Windows.Win32.System.Com.Unknown";

			// Signature (protected for Unknown, public for others)
			var ctor = ConstructorDeclaration(Identifier(cls.Name)).AddModifiers(Token(isUnknown ? SyntaxKind.ProtectedKeyword : SyntaxKind.PublicKeyword)).WithParameterList(ParameterList(SeparatedList([Parameter(Identifier("raw")).WithType(ParseTypeName("nint")), Parameter(Identifier("addRef")).WithType(PredefinedType(Token(SyntaxKind.BoolKeyword))).WithDefault(EqualsValueClause(LiteralExpression(SyntaxKind.FalseLiteralExpression)))])));

			// Only derived classes call base(raw, addRef)
			if (!isUnknown)
			{
				ctor = ctor.WithInitializer(ConstructorInitializer(SyntaxKind.BaseConstructorInitializer, ArgumentList(SeparatedList([Argument(IdentifierName("raw")), Argument(IdentifierName("addRef"))]))));
			}

			// Build body statements
			var bindStmts = new List<StatementSyntax>();

			if (isUnknown)
			{
				// Fully-qualify to avoid depending on a 'using System;' from other analysis
				bindStmts.Add(ParseStatement("global::System.ArgumentNullException.ThrowIfNull((void*)raw);"));
				bindStmts.Add(ParseStatement("Raw = (void*)raw;"));
			}

			bindStmts.Add(ParseStatement("nint* vt = Vtbl;"));
			foreach (var m in methods)
			{
				var slot = ComputeAbsoluteSlot(m);
				bindStmts.Add(ParseStatement($"_fp_{m.Name} = vt[{slot}];"));
			}

			if (isUnknown)
			{
				bindStmts.Add(ParseStatement("if (addRef && !IsNull) AddRef();"));
			}

			// Optional user hook
			if (HasOnCreate(cls))
			{
				bindStmts.Add(ParseStatement("OnCreate(raw, addRef);"));
			}

			ctorNode = ctor.WithBody(Block(UnsafeStatement(Block(bindStmts))));
		}

		// Assemble in the order: fields, ctor, methods
		var classMembers = new List<MemberDeclarationSyntax>(fields.Count + (ctorNode is null ? 0 : 1) + impls.Count);
		classMembers.AddRange(fields);
		if (ctorNode is not null) classMembers.Add(ctorNode);
		classMembers.AddRange(impls);

		var classDecl = ClassDeclaration(cls.Name).AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.PartialKeyword)).WithMembers(List(classMembers));

		MemberDeclarationSyntax container = classDecl;
		if (ns is not null)
			container = NamespaceDeclaration(ParseName(ns)).WithMembers(SingletonList(container));

		// Build: using global::<ns>;
		var usingDirectives = namespaces.Where(n => !string.IsNullOrWhiteSpace(n)).Where(n => n != ns && n != "global").Distinct().OrderBy(n => n).Select(n => UsingDirective(ParseName("global::" + n!))).ToList();

		// Create the compilation unit
		var unit = CompilationUnit().WithUsings(List(usingDirectives)).WithMembers(SingletonList(container)).WithEndOfFileToken(Token(SyntaxKind.EndOfFileToken));

		// --- Nullable detection and header composition ---
		bool needsNullable = NeedsNullableContext(methods);

		var trivia = new List<SyntaxTrivia> { Comment("// <auto-generated />\n"), CarriageReturnLineFeed };

		if (needsNullable)
		{
			trivia.Add(Comment("#nullable enable\n"));
			trivia.Add(CarriageReturnLineFeed);
		}

		trivia.Add(CarriageReturnLineFeed);

		// Return the final compilation unit
		return unit.WithLeadingTrivia(TriviaList(trivia));
	}

	private static MethodDeclarationSyntax EmitStubMethod(IMethodSymbol m)
	{
		// Build the signature
		var retType = ParseTypeName(m.ReturnType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat));

		var paramList = ParameterList(SeparatedList(m.Parameters.Select(p =>
		{
			var name = p.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
			var mods = p.RefKind switch
			{
				RefKind.In => TokenList(Token(SyntaxKind.InKeyword)),
				RefKind.Ref => TokenList(Token(SyntaxKind.RefKeyword)),
				RefKind.Out => TokenList(Token(SyntaxKind.OutKeyword)),
				_ => default
			};
			return Parameter(Identifier(p.Name)).WithType(ParseTypeName(name)).WithModifiers(mods);
		})));

		var inside = new List<StatementSyntax>();

		foreach (var p in m.Parameters)
		{
			inside.Add(p.RefKind == RefKind.Out ? ParseStatement((p.Name + Assign + @default) | Semicolon) : ParseStatement("_ = " + p.Name + Semicolon));
		}

		var unsafeStmt = UnsafeStatement(Block(inside));

		// If non-void, return default;
		var outer = new List<StatementSyntax> { unsafeStmt };

		if (m.ReturnType.SpecialType != SpecialType.System_Void)
		{
			outer.Add(ParseStatement((@return + @default) | Semicolon));
		}

		return MethodDeclaration(retType, m.Name).AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.PartialKeyword)).WithParameterList(paramList).WithBody(Block(outer));
	}

	private static void EmitMethod(INamedTypeSymbol cls, IMethodSymbol m, out MemberDeclarationSyntax cacheField, out MemberDeclarationSyntax impl)
	{
		var parms = Classify(m, out var returnsHr);

		// delegate* unmanaged signature
		var delegateArgs = new List<string> { "nint" }; // 'this'
		delegateArgs.AddRange(parms.Select(AbiTypeFor));
		var retAbi = AbiReturnType(m.ReturnType, returnsHr);
		var delegateSig = $"delegate* unmanaged[Stdcall]<{string.Join(", ", delegateArgs.Append(retAbi))}>";

		// per-instance cache (raw function pointer) — no 'static'
		var cacheName = $"_fp_{m.Name}";
		cacheField = FieldDeclaration(VariableDeclaration(ParseTypeName("nint")).WithVariables(SingletonSeparatedList(VariableDeclarator(cacheName)))).AddModifiers(Token(SyntaxKind.PrivateKeyword));

		// public partial signature
		var retType = ParseTypeName(m.ReturnType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat));
		var paramList = ParameterList(SeparatedList(m.Parameters.Select(p =>
		{
			var tname = p.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
			var mods = p.RefKind switch
			{
				RefKind.In => TokenList(Token(SyntaxKind.InKeyword)),
				RefKind.Ref => TokenList(Token(SyntaxKind.RefKeyword)),
				RefKind.Out => TokenList(Token(SyntaxKind.OutKeyword)),
				_ => default
			};
			return Parameter(Identifier(p.Name)).WithType(ParseTypeName(tname)).WithModifiers(mods);
		})));

		// ===== inside unsafe { … } =====
		var inner = new List<StatementSyntax>();

		var abiArgs = new List<string> { "(nint)Raw" };
		var fixedBlockCloses = new Stack<string>();

		foreach (var p in parms)
		{
			switch (p.Kind)
			{
				case PKind.InBlit:
				{
					var t = p.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
					inner.Add(ParseStatement($"{t}* __{p.Name} = ({t}*)Unsafe.AsPointer(ref Unsafe.AsRef(in {p.Name}));"));
					abiArgs.Add($"__{p.Name}");
					break;
				}
				case PKind.RefBlit:
				{
					var t = p.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
					inner.Add(ParseStatement($"{t}* __{p.Name} = ({t}*)Unsafe.AsPointer(ref {p.Name});"));
					abiArgs.Add($"__{p.Name}");
					break;
				}
				case PKind.OutBlit:
				{
					var t = p.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
					inner.Add(ParseStatement($"{t} __{p.Name} = default;"));
					inner.Add(ParseStatement($"{t}* __p_{p.Name} = &__{p.Name};"));
					abiArgs.Add($"__p_{p.Name}");
					break;
				}
				case PKind.ReadOnlySpanBlit:
				{
					var e = p.Elem!.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
					inner.Add(ParseStatement($"fixed ({e}* __{p.Name} = {p.Name}.IsEmpty ? null : &MemoryMarshal.GetReference({p.Name})) {{"));
					abiArgs.Add($"__{p.Name}");
					abiArgs.Add($"{p.Name}.Length");
					fixedBlockCloses.Push("}");
					break;
				}
				case PKind.SpanBlit:
				{
					var e = p.Elem!.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
					inner.Add(ParseStatement($"fixed ({e}* __{p.Name} = {p.Name}.IsEmpty ? null : &MemoryMarshal.GetReference({p.Name})) {{"));
					abiArgs.Add($"__{p.Name}");
					abiArgs.Add($"{p.Name}.Length");
					fixedBlockCloses.Push("}");
					break;
				}
				case PKind.Utf16:
				{
					inner.Add(ParseStatement($"fixed (char* __{p.Name} = {p.Name}) {{"));
					abiArgs.Add($"__{p.Name}");
					fixedBlockCloses.Push("}");
					break;
				}
				case PKind.Interface:
				{
					abiArgs.Add($"{p.Name} == null ? 0 : (nint){p.Name}.Raw");
					break;
				}
				case PKind.OutInterface:
				{
					inner.Add(ParseStatement("nint __out_ptr = 0;"));
					abiArgs.Add("&__out_ptr");
					break;
				}
				case PKind.Scalar:
				{
					if (p.Type.SpecialType == SpecialType.System_Boolean)
					{
						inner.Add(ParseStatement($"int __{p.Name}_abi = {p.Name} ? 1 : 0;"));
						abiArgs.Add($"__{p.Name}_abi");
					}
					else
					{
						abiArgs.Add(p.Name);
					}

					break;
				}
			}
		}

		// inline function-pointer call via prebound per-instance field
		inner.Add(retAbi == "void" ? ParseStatement($"(({delegateSig}){cacheName})({string.Join(", ", abiArgs)});") : ParseStatement($"{retAbi} __ret = (({delegateSig}){cacheName})({string.Join(", ", abiArgs)});"));

		// post-call copies / wraps
		foreach (var p in parms)
		{
			switch (p.Kind)
			{
				case PKind.OutBlit:
					inner.Add(ParseStatement($"{p.Name} = __{p.Name};"));
					break;
				case PKind.OutInterface:
					inner.Add(ParseStatement($"{p.Name} = __out_ptr != 0 ? Create<{p.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}>(__out_ptr) : null!;"));
					break;
			}
		}

		// close any fixed blocks
		while (fixedBlockCloses.Count > 0)
			inner.Add(ParseStatement(fixedBlockCloses.Pop()));

		if (retAbi != "void")
			inner.Add(ParseStatement("return __ret;"));

		impl = MethodDeclaration(retType, m.Name).AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.PartialKeyword)).WithParameterList(paramList).WithBody(Block(UnsafeStatement(Block(inner))));
	}

	private static List<PSpec> Classify(IMethodSymbol m, out bool returnsHresult)
	{
		returnsHresult = m.ReturnType.Name == "HRESULT";
		var list = new List<PSpec>();

		foreach (var p in m.Parameters)
		{
			var rk = p.RefKind;
			var t = p.Type;

			if (rk == RefKind.In && IsScalar(t))
			{
				list.Add(new PSpec(PKind.Scalar, t, p.Name));
				continue;
			}

			// ref/out/in unmanaged structs
			switch (rk)
			{
				case RefKind.In when IsUnmanaged(t):
					list.Add(new PSpec(PKind.InBlit, t, p.Name));
					continue;
				case RefKind.Ref when IsUnmanaged(t):
					list.Add(new PSpec(PKind.RefBlit, t, p.Name));
					continue;
				case RefKind.Out when IsUnmanaged(t):
					list.Add(new PSpec(PKind.OutBlit, t, p.Name));
					continue;
			}

			// Spans of unmanaged
			if (IsReadOnlySpan(t, out var re) && IsUnmanaged(re))
			{
				list.Add(new PSpec(PKind.ReadOnlySpanBlit, re, p.Name, re, NeedsCount: true));
				continue;
			}

			if (IsSpan(t, out var se) && IsUnmanaged(se))
			{
				list.Add(new PSpec(PKind.SpanBlit, se, p.Name, se, NeedsCount: true));
				continue;
			}

			// Strings (UTF-16) or ReadOnlySpan<char>
			if (IsString(t) || (IsReadOnlySpan(t, out var ch) && ch.SpecialType == SpecialType.System_Char))
			{
				list.Add(new PSpec(PKind.Utf16, t, p.Name));
				continue;
			}

			// COM interfaces
			if (rk == RefKind.Out && DerivesFrom(t, "Unknown"))
			{
				list.Add(new PSpec(PKind.OutInterface, t, p.Name));
				continue;
			}

			if (DerivesFrom(t, "Unknown"))
			{
				list.Add(new PSpec(PKind.Interface, t, p.Name));
				continue;
			}

			// Scalar unmanaged (incl. enums)
			if (IsUnmanaged(t))
			{
				list.Add(new PSpec(PKind.Scalar, t, p.Name));
				continue;
			}

			throw new NotSupportedException($"Unsupported parameter '{p.Name}' of type '{t.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}'.\n" + GetUnmanagedReason(t));
		}

		return list;
	}

	private static string GetUnmanagedReason(ITypeSymbol root)
	{
		var sb = new System.Text.StringBuilder();
		var visited = new HashSet<ITypeSymbol>(SymbolEqualityComparer.Default);

		try
		{
			Walk(root, 0);
		}
		catch (Exception ex)
		{
			sb.AppendLine($"(diagnostic traversal failed: {ex.Message})");
		}

		return sb.ToString();

		void Walk(ITypeSymbol t, int depth)
		{
			string indent = new string(' ', depth * 2);
			bool isUnmanaged = t.IsUnmanagedType;
			sb.AppendLine($"{indent}- {t.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)} " + $"(IsValueType={t.IsValueType}, Unmanaged={isUnmanaged})");

			if (!visited.Add(t))
			{
				sb.AppendLine($"{indent}  (cycle detected)");
				return;
			}

			if (t is not INamedTypeSymbol nt || nt.TypeKind != TypeKind.Struct) return;
			var layout = nt.GetAttributes().FirstOrDefault(a => a.AttributeClass?.Name == nameof(StructLayoutAttribute));
			if (layout is not null && layout.ConstructorArguments.Length > 0)
				sb.AppendLine($"{indent}  Layout={layout.ConstructorArguments[0].Value}");

			foreach (var field in nt.GetMembers().OfType<IFieldSymbol>())
			{
				if (field.IsStatic) continue;
				sb.AppendLine($"{indent}  Field: {field.Name} -> {field.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}");
				Walk(field.Type, depth + 1);
			}
		}
	}

	private static string AbiTypeFor(PSpec p) =>
		p.Kind switch
		{
			PKind.InBlit or PKind.RefBlit or PKind.OutBlit => $"{p.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}*",
			PKind.ReadOnlySpanBlit or PKind.SpanBlit => $"{p.Elem!.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)}*",
			PKind.Utf16 => "char*",
			PKind.Interface => "nint",
			PKind.OutInterface => "nint*",
			PKind.Scalar => p.Type.SpecialType switch
			{
				SpecialType.System_Boolean => "int",
				SpecialType.System_IntPtr => "nint",
				_ => p.Type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat)
			},
			_ => "nint"
		};

	private static string AbiReturnType(ITypeSymbol ret, bool treatIntAsHresult)
	{
		if (ret.SpecialType == SpecialType.System_Void) return "void";
		if (treatIntAsHresult || ret.Name == "HRESULT") return "int";
		if (ret.SpecialType == SpecialType.System_Boolean) return "int";
		if (ret.SpecialType == SpecialType.System_IntPtr) return "nint";
		if (ret.IsUnmanagedType) return ret.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
		throw new NotSupportedException($"Unsupported return type '{ret}'.");
	}

	// Count local COM methods for a type (candidates)
	private static int CountLocalComMethods(INamedTypeSymbol type)
	{
		return type.GetMembers().OfType<IMethodSymbol>().Count(m => IsComMethodCandidate(type, m));
	}

	// Enumerate local COM methods in source order (candidates)
	private static IEnumerable<IMethodSymbol> GetLocalComMethodsInSourceOrder(INamedTypeSymbol type)
	{
		return type.GetMembers().OfType<IMethodSymbol>().Where(m => IsComMethodCandidate(type, m)).OrderBy(m => m.Locations.FirstOrDefault()?.SourceTree?.FilePath ?? "").ThenBy(m => m.Locations.FirstOrDefault()?.SourceSpan.Start ?? 0);
	}

	// Try read an explicit local index from [Method(...)]
	private static bool TryGetExplicitLocalIndex(IMethodSymbol m, out int idx)
	{
		var attr = m.GetAttributes().FirstOrDefault(a => a.AttributeClass?.ToDisplayString() == MethodAttribute);
		idx = -1;
		if (attr is null) return false;

		// ctor arg
		if (attr.ConstructorArguments.Length > 0 && attr.ConstructorArguments[0].Value is int c and >= 0)
		{
			idx = c;
			return true;
		}

		// named arg Index=...
		foreach (var kv in attr.NamedArguments)
		{
			if (kv.Key != "Index" || kv.Value.Value is not int j || j < 0) continue;
			idx = j;
			return true;
		}

		return false;
	}

	// Compute local index: explicit from [Method(idx)] OR implicit by source order
	private static int GetLocalIndex(IMethodSymbol m)
	{
		if (TryGetExplicitLocalIndex(m, out var idx)) return idx;

		var type = m.ContainingType;
		var locals = GetLocalComMethodsInSourceOrder(type).ToList();
		for (int i = 0; i < locals.Count; i++)
		{
			if (SymbolEqualityComparer.Default.Equals(locals[i], m))
				return i;
		}

		throw new InvalidOperationException($"Failed to compute local index for {type.Name}.{m.Name}.");
	}

	// Sum base local counts (excluding current type)
	private static int SumBaseLocalCounts(INamedTypeSymbol type)
	{
		int sum = 0;
		for (var t = type.BaseType; t is not null; t = t.BaseType)
		{
			// stop when we reach System.Object (no COM)
			if (t.SpecialType == SpecialType.System_Object) break;
			sum += CountLocalComMethods(t);
		}

		return sum;
	}

	// Absolute slot = 3 (IUnknown) + Σ base local counts + local index in this type
	private static int ComputeAbsoluteSlot(IMethodSymbol m)
	{
		var declaringType = m.ContainingType;
		int baseCount = SumBaseLocalCounts(declaringType);
		int localIndex = GetLocalIndex(m);
		return baseCount + localIndex;
	}

	private static bool IsComMethodCandidate(INamedTypeSymbol type, IMethodSymbol m)
	{
		if (m.MethodKind != MethodKind.Ordinary) return false;
		if (m.IsStatic) return false;
		if (!SymbolEqualityComparer.Default.Equals(m.ContainingType, type)) return false;
		if (m.OverriddenMethod is not null) return false;
		if (HasAttribute(m, MethodAttribute)) return true;
		var isPartialDecl = m is { IsPartialDefinition: true, PartialImplementationPart: null };
		return isPartialDecl && m.DeclaredAccessibility == Accessibility.Public;
	}

	private static string EscapeXml(string value) => string.IsNullOrEmpty(value) ? value : value.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;");

	private static bool NeedsNullableContext(IEnumerable<IMethodSymbol> methods)
	{
		foreach (var m in methods)
		{
			// Check return type
			if (IsNullableRefType(m.ReturnType))
				return true;

			// Check parameters
			foreach (var p in m.Parameters)
			{
				if (IsNullableRefType(p.Type))
					return true;
			}
		}

		return false;
	}

	private static bool ShouldEmitCtor(INamedTypeSymbol cls)
	{
		if (cls.IsAbstract) return false;

		var fq = cls.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
		bool isUnknown = fq == "global::Windows.Win32.System.Com.Unknown";

		// For Unknown, we want the generator-owned (nint,bool) ctor if the user didn't already define one.
		if (isUnknown)
		{
			return !HasInstanceCtor(cls, "global::System.IntPtr", "global::System.Boolean");
		}

		// For derived types, keep the existing duplicate checks.
		if (HasInstanceCtor(cls, "global::System.IntPtr", "global::System.Boolean")) return false;
		return !HasInstanceCtor(cls, "global::System.IntPtr");
	}

	private static bool HasInstanceCtor(INamedTypeSymbol t, params string[] paramTypes)
	{
		foreach (var m in t.InstanceConstructors)
		{
			if (m.DeclaredAccessibility == Accessibility.Private) continue;
			if (m.Parameters.Length != paramTypes.Length) continue;

			bool all = true;
			for (int i = 0; i < paramTypes.Length; i++)
			{
				var pi = m.Parameters[i].Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
				if (pi == paramTypes[i]) continue;
				all = false;
				break;
			}

			if (all) return true;
		}

		return false;
	}

	private static bool HasOnCreate(INamedTypeSymbol cls)
	{
		return cls.GetMembers().OfType<IMethodSymbol>().Any(m => m.Name == "OnCreate" && m is { IsStatic: false, ReturnsVoid: true, Parameters.Length: 2 } && m.Parameters[0].Type.SpecialType == SpecialType.System_IntPtr && m.Parameters[1].Type.SpecialType == SpecialType.System_Boolean);
	}
}