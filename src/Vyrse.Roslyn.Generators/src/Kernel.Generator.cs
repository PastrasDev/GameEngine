using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

using static Utility;
using static Utility.Syntax;
using static Utility.Enums;
using static Utility.Naming;

namespace Vyrse.Roslyn.Generators;

[Generator]
internal class KernelGenerator : IncrementalGenerator<KernelGenerator.Data>
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
        public INamedTypeSymbol DependencyAttribute { get; init; } = null!;
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
            usings.Add(DependencyAttribute);
            usings.Add(EnabledAttribute);
            usings.Add(DisabledAttribute);
            return usings;
        }
    }

    protected override IncrementalValuesProvider<Data> Initialize(Data data) => data.CompilationProvider.Combine(FindSymbolsFromAttribute("Vyrse.Engine.Core.KernelAttribute")).SelectMany((pair, _) =>
    {
        var right = pair.Right;
        var set = new HashSet<INamedTypeSymbol>(SymbolEqualityComparer.Default);
        foreach (var k in right)
        {
            if (k is not null) set.Add(k);
        }

        var kernels = new INamedTypeSymbol[set.Count];
        set.CopyTo(kernels);

        Array.Sort(kernels, static (a, b) => string.Compare(FullName(a, includeGlobalPrefix: true), FullName(b, includeGlobalPrefix: true), StringComparison.Ordinal));

        if (kernels.Length == 0) return [];

        var iModule = GetNamedTypeSymbol("Vyrse.Engine.Core.IModule");
        var phase = GetNamedTypeSymbol("Vyrse.Engine.Core.IPhase+Phase");
        var iPhase = GetNamedTypeSymbol("Vyrse.Engine.Core.IPhase");
        var phases = GetEnumMemberNames(phase);
        var contextType = GetNamedTypeSymbol("Vyrse.Engine.Core.Context");
        var dependencyAttribute = GetNamedTypeSymbol("Vyrse.Engine.Core.DependencyAttribute`1");
        var enabledAttribute = GetNamedTypeSymbol("Vyrse.Engine.Core.EnabledAttribute");
        var disabledAttribute = GetNamedTypeSymbol("Vyrse.Engine.Core.DisabledAttribute");

        var results = new List<Data>(kernels.Length);
        foreach (var kernel in kernels)
        {
            results.Add(new Data
            {
                Compilation = data.Compilation,
                IncrementalGeneratorInitializationContext = data.IncrementalGeneratorInitializationContext,
                CompilationProvider = data.CompilationProvider,
                SourceProductionContext = data.SourceProductionContext,
                Type = kernel,
                IModule = iModule,
                IPhase = iPhase,
                Phase = phase,
                Phases = phases,
                Context = contextType,
                DependencyAttribute = dependencyAttribute,
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

        var attrList = AttributeList(SingletonSeparatedList(Attribute<MethodImplAttribute>().WithArgumentList(AttributeArgumentList(SingletonSeparatedList(AttributeArgument(MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, IdentifierName<MethodImplOptions>(), IdentifierName(nameof(MethodImplOptions.AggressiveInlining)))))))));

        foreach (var phase in data.Phases)
        {
            var method = Method.Declare(new Method.Description
            {
                Name = phase,
                ReturnType = SyntaxKind.VoidKeyword,
                Accessibility = Microsoft.CodeAnalysis.Accessibility.Private,
                Attributes = attrList,
                IsPartial = true,
            });

            usings.Add(GetNamedTypeSymbol<MethodImplAttribute>());
            AddMembers(ref classDecl, method);
        }

        usings.Remove(data.Type);
        var unit = CompilationUnit().AddUsings(usings).WithLeadingTrivia(Comment("// <auto-generated />\n"));
        var final = unit.AddMembers(FileScopedNamespaceDeclaration(ParseName(Namespace(data.Type))).AddMembers(classDecl));
        data.SourceProductionContext.AddSource(HintName(data.Type), final.NormalizeWhitespace().ToFullString());
    }
}