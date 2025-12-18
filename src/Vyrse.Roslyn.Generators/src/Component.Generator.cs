// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Microsoft.CodeAnalysis;
// using Microsoft.CodeAnalysis.CSharp;
// using Microsoft.CodeAnalysis.CSharp.Syntax;
// using Vyrse.Engine.Core;
//
// namespace Vyrse.Roslyn.Generators.Generators;
//
// using static Utilities;
// using static SyntaxFactory;
//
// [Generator]
// public sealed class ComponentGenerator : IIncrementalGenerator
// {
// 	private readonly struct Data()
// 	{
// 		public INamedTypeSymbol Component { get; init; } = null!;
//
// 		public Compilation Compilation { get; init; } = null!;
// 		public INamedTypeSymbol IComponentSymbol { get; init; } = null!;
// 		public INamedTypeSymbol IModuleSymbol { get; init; } = null!;
// 		public INamedTypeSymbol ContextSymbol { get; init; } = null!;
//
// 	}
//
// 	public void Initialize(IncrementalGeneratorInitializationContext context)
// 	{
// 		var shared = context.CompilationProvider.Select(static (compilation, _) => new
// 		{
// 			Compilation = compilation,
// 			IComponentSymbol = GetNamedTypeSymbol<IComponent>(compilation),
// 			IModuleSymbol = GetNamedTypeSymbol<IModule>(compilation),
// 			ContextSymbol = GetNamedTypeSymbol<Context>(compilation)
// 		});
//
// 		var component = shared.Combine(FindSymbolsFromAttribute<ComponentAttribute>(context)).SelectMany(static (pair, _) =>
// 		{
// 			var left = pair.Left;
// 			return pair.Right.Distinct<INamedTypeSymbol>(SymbolEqualityComparer.Default).Select(component => new Data
// 			{
// 				Component = component,
// 				Compilation = left.Compilation,
// 				IComponentSymbol = left.IComponentSymbol,
// 				IModuleSymbol = left.IModuleSymbol,
// 				ContextSymbol = left.ContextSymbol
// 			});
// 		});
//
// 		context.RegisterSourceOutput(component, static (spc, data) =>
// 		{
// 			try
// 			{
// 				Generate(in data, in spc);
// 			}
// 			catch (Exception ex)
// 			{
// 				var descriptor = new DiagnosticDescriptor("VYRSEGEN001", "Vyrse Generator Crash", "Generator failed for '{0}': {1}", "Vyrse.Roslyn.Generators", DiagnosticSeverity.Error, isEnabledByDefault: true);
// 				spc.ReportDiagnostic(Diagnostic.Create(descriptor, Location.None, data.Component.ToDisplayString(), ex.Message));
// 			}
// 		});
// 	}
//
// 	private static void Generate(in Data data, in SourceProductionContext spc)
// 	{
// 		var usings = new HashSet<INamespaceSymbol?>(SymbolEqualityComparer.Default);
// 		var component = data.Component;
// 		var compilation = data.Compilation;
// 		var iComponentSymbol = AddNamespace(usings, data.IComponentSymbol);
// 		var iModuleSymbol = AddNamespace(usings, data.IModuleSymbol);
// 		var contextSymbol = AddNamespace(usings, data.ContextSymbol);
//
// 		if (HasPrimaryConstructor(component) || HasInstanceConstructor(component))
// 			return;
//
// 		var classDecl = ClassDeclaration(component.Name).AddModifiers(Token(AccessibilityToToken(component.DeclaredAccessibility)), Token(SyntaxKind.PartialKeyword));
//
// 		if (component.TypeParameters.Length > 0)
// 		{
// 			classDecl = classDecl.WithTypeParameterList(TypeParameterList(SeparatedList(component.TypeParameters.Select(tp => TypeParameter(tp.Name)))));
// 		}
//
// 		if (!ImplementsInterface(component, typeof(IComponent).FullName!))
// 		{
// 			classDecl = classDecl.AddBaseListTypes(SimpleBaseType(ParseTypeName(iComponentSymbol.Name)));
// 		}
//
// 		AddMembers(ref classDecl, FieldDeclaration(VariableDeclaration(ParseTypeName(contextSymbol.Name)).WithVariables(SingletonSeparatedList(VariableDeclarator(Identifier<Context>())))).AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.ReadOnlyKeyword)));
// 		AddMembers(ref classDecl, FieldDeclaration(VariableDeclaration(ParseTypeName(iModuleSymbol.Name)).WithVariables(SingletonSeparatedList(VariableDeclarator(Identifier("Owner"))))).AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.ReadOnlyKeyword)));
//
// 		IEnumerable<ParameterSyntax> parameters =
// 		[
// 			Parameter(Identifier<Context>().LowerCase()).WithType(ParseTypeName(contextSymbol.Name)),
// 			Parameter(Identifier("owner")).WithType(ParseTypeName(iModuleSymbol.Name))
// 		];
//
// 		StatementSyntax[] bodyStatements =
// 		[
// 			ExpressionStatement(AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, ThisExpression(), IdentifierName<Context>()), IdentifierName<Context>().LowerCase())),
// 			ExpressionStatement(AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, ThisExpression(), IdentifierName("Owner")), IdentifierName("owner")))
// 		];
//
// 		AddMembers(ref classDecl, ConstructorDeclaration(Identifier(component.Name)).AddModifiers(Token(AccessibilityToToken(component.DeclaredAccessibility))).WithParameterList(ParameterList(SeparatedList(parameters))).WithBody(Block(bodyStatements)));
//
// 		var unit = CompilationUnit().AddUsings(CreateUsingDirectiveSyntaxArray(usings, component)).WithLeadingTrivia(Comment("// <auto-generated />\n"));
// 		var @namespace = FileScopedNamespaceDeclaration(ParseName(component.ContainingNamespace!.ToDisplayString()));
// 		var final = unit.AddMembers(@namespace.AddMembers(classDecl));
// 		spc.AddSource(component.HintName(), final.NormalizeWhitespace().ToFullString());
// 	}
// }