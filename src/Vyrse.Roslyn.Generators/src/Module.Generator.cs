using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

using static Utility;
using static Utility.Symbols;
using static Utility.Syntax;
using static Utility.Attributes;
using static Utility.Enums;
using static Utility.Naming;

namespace Vyrse.Roslyn.Generators;

[Generator]
internal sealed class ModuleGenerator : IncrementalGenerator<ModuleGenerator.Data>
{
	public sealed class Data : IData
	{
		public Compilation Compilation { get; set; } = null!;
		public IncrementalGeneratorInitializationContext IncrementalGeneratorInitializationContext { get; init; }
		public IncrementalValueProvider<Compilation> CompilationProvider { get; set; }
		public SourceProductionContext SourceProductionContext { get; set; }
		public ImmutableArray<string> Phases { get; init; }
		public INamedTypeSymbol Type { get; set; } = null!;
		public INamedTypeSymbol IModule { get; init; } = null!;
		public INamedTypeSymbol IPhase { get; init; } = null!;
		public INamedTypeSymbol Phase { get; init; } = null!;
		public INamedTypeSymbol Context { get; init; } = null!;
		public INamedTypeSymbol EnabledAttribute { get; init; } = null!;
		public INamedTypeSymbol DisabledAttribute { get; init; } = null!;

		public Usings GetAllUsings()
		{
			var usings = new Usings();
			usings.Add(Type);
			usings.Add(IModule);
			usings.Add(IPhase);
			usings.Add(Phase);
			usings.Add(Context);
			usings.Add(EnabledAttribute);
			usings.Add(DisabledAttribute);
			return usings;
		}
	}

	protected override IncrementalValuesProvider<Data> Initialize(Data data) => data.CompilationProvider.Combine(FindSymbolsFromAttribute("Vyrse.Engine.Core.ModuleAttribute")).SelectMany((pair, _) =>
	{
        var right = pair.Right;
        var set = new HashSet<INamedTypeSymbol>(SymbolEqualityComparer.Default);
        foreach (var m in right)
        {
            if (m is not null) set.Add(m);
        }

        var modules = new INamedTypeSymbol[set.Count];
        set.CopyTo(modules);

        Array.Sort(modules, static (a, b) => string.Compare(FullName(a, includeGlobalPrefix: true), FullName(b, includeGlobalPrefix: true), StringComparison.Ordinal));

        if (modules.Length == 0) return [];

        var iModule = GetNamedTypeSymbol("Vyrse.Engine.Core.IModule");
        var iPhase = GetNamedTypeSymbol("Vyrse.Engine.Core.IPhase");
        var phase = GetNamedTypeSymbol("Vyrse.Engine.Core.IPhase+Phase");
        var phases = GetEnumMemberNames(phase);
        var contextType = GetNamedTypeSymbol("Vyrse.Engine.Core.Context");
        var enabledAttribute = GetNamedTypeSymbol("Vyrse.Engine.Core.EnabledAttribute");
        var disabledAttribute = GetNamedTypeSymbol("Vyrse.Engine.Core.DisabledAttribute");

        var results = new List<Data>(modules.Length);
        foreach (var module in modules)
        {
            results.Add(new Data
            {
                Compilation = data.Compilation,
                IncrementalGeneratorInitializationContext = data.IncrementalGeneratorInitializationContext,
                CompilationProvider = data.CompilationProvider,
                SourceProductionContext = data.SourceProductionContext,
                Type = module,
                IModule = iModule,
                IPhase = iPhase,
                Phase = phase,
                Phases = phases,
                Context = contextType,
                EnabledAttribute = enabledAttribute,
                DisabledAttribute = disabledAttribute
            });
        }

        return results;
	});

	protected override void Generate(Data data)
	{
        var usings = data.GetAllUsings();

		var classDecl = ClassDeclaration(Name(data.Type)).AddModifiers(Token(Accessibility(data.Type.DeclaredAccessibility)), Token(SyntaxKind.PartialKeyword));

        var tps = data.Type.TypeParameters;
        if (tps.Length > 0)
        {
            var items = new TypeParameterSyntax[tps.Length];
            for (var i = 0; i < tps.Length; i++) items[i] = TypeParameter(Name(tps[i]));
            classDecl = classDecl.WithTypeParameterList(TypeParameterList(SeparatedList(items)));
        }

		if (!ImplementsInterface(data.Type, FullName(data.IModule)))
		{
			classDecl = classDecl.AddBaseListTypes(SimpleBaseType(ParseTypeName(Name(data.IModule))));
		}

		AddMembers(ref classDecl, FieldDeclaration(VariableDeclaration(ParseTypeName(Name(data.Context))).WithVariables(SingletonSeparatedList(VariableDeclarator(Identifier(data.Context))))).AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.ReadOnlyKeyword)));

		IEnumerable<ParameterSyntax> parameters =
		[
			Parameter(Identifier(data.Context).LowerCase()).WithType(ParseTypeName(Name(data.Context))),
		];

		StatementSyntax[] bodyStatements =
		[
			ExpressionStatement(AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, ThisExpression(), IdentifierName(data.Context)), IdentifierName(LowerCaseName(data.Context)))),
		];

		AddMembers(ref classDecl, ConstructorDeclaration(Identifier(Name(data.Type))).AddModifiers(Token(Accessibility(data.Type.DeclaredAccessibility))).WithParameterList(ParameterList(SeparatedList(parameters))).WithBody(Block(bodyStatements)));

		var attrList = AttributeList(SingletonSeparatedList(Attribute<MethodImplAttribute>().WithArgumentList(AttributeArgumentList(SingletonSeparatedList(AttributeArgument(MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, IdentifierName<MethodImplOptions>(), IdentifierName(nameof(MethodImplOptions.AggressiveInlining)))))))));

		foreach (var phase in data.Phases)
		{
			var methodSymbol = GetMethodSymbol(data.Type, phase);
			if (methodSymbol is null) continue;

			List<StatementSyntax> statements = [];
			if (!HasAttribute(methodSymbol, data.DisabledAttribute))
			{
				statements = [ExpressionStatement(InvocationExpression(IdentifierName(phase)))];
			}

			if (statements.Count == 0) continue;

			usings.Add(GetNamedTypeSymbol<MethodImplAttribute>());

			var method = Method.Declare(new Method.Description
			{
				Name = phase,
				ReturnType = SyntaxKind.VoidKeyword,
				Accessibility = Microsoft.CodeAnalysis.Accessibility.Public,
				Attributes = attrList,
				Statements = statements,
				Prefix = "__"
			});

			AddMembers(ref classDecl, method);
		}

		usings.Remove(data.Type);
		var unit = CompilationUnit().AddUsings(usings).WithLeadingTrivia(Comment("// <auto-generated />\n"));
		var final = unit.AddMembers(FileScopedNamespaceDeclaration(ParseName(Namespace(data.Type))).AddMembers(classDecl));
        data.SourceProductionContext.AddSource(HintName(data.Type), final.NormalizeWhitespace().ToFullString());
	}
}