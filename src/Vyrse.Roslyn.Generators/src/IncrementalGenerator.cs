using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Utility;

namespace Vyrse.Roslyn.Generators;

public sealed class Usings
{
    private readonly HashSet<INamespaceSymbol?> _usings = new(SymbolEqualityComparer.Default);

    public static implicit operator UsingDirectiveSyntax[](Usings usings)
    {
        var set = usings._usings;
        var list = new List<UsingDirectiveSyntax>(set.Count);

        foreach (var n in set)
        {
            if (n is null || n.IsGlobalNamespace) continue;
            list.Add(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("global::" + n.ToDisplayString())));
        }

        list.Sort(static (a, b) => string.Compare(a.Name?.ToString(), b.Name?.ToString(), StringComparison.Ordinal));

        return list.ToArray();
    }

    public INamedTypeSymbol Add(INamedTypeSymbol symbol)
    {
        var ns = symbol.ContainingNamespace;
        if (ns is not null && !ns.IsGlobalNamespace) _usings.Add(ns);
        return symbol;
    }

    public void AddRange(IEnumerable<INamedTypeSymbol> symbols)
    {
        foreach (var symbol in symbols)
        {
            Add(symbol);
        }
    }

    public bool Remove(INamedTypeSymbol symbol)
    {
        var ns = symbol.ContainingNamespace;
        if (ns is not null && !ns.IsGlobalNamespace) return _usings.Remove(ns);
        return false;
    }

    public void Clear() => _usings.Clear();
}

public interface IData
{
    public Compilation Compilation { get; set; }
    public IncrementalGeneratorInitializationContext IncrementalGeneratorInitializationContext { get; init; }
    public IncrementalValueProvider<Compilation> CompilationProvider { get; set; }
    public SourceProductionContext SourceProductionContext { get; set; }
    public INamedTypeSymbol Type { get; set; }

    public Usings GetAllUsings();
}

internal abstract class IncrementalGenerator<TData> : IIncrementalGenerator where TData : class, IData, new()
{
    private TData Data { get; set; } = null!;

    private static readonly DiagnosticDescriptor _error = new(id: "VYRSE000", title: "Vyrse Generator Error", messageFormat: "{0}", category: "Vyrse", defaultSeverity: DiagnosticSeverity.Error, isEnabledByDefault: true);
    private static readonly DiagnosticDescriptor _info = new(id: "VYRSE001", title: "Vyrse Generator Log", messageFormat: "{0}", category: "Vyrse", defaultSeverity: DiagnosticSeverity.Info, isEnabledByDefault: true);

    public virtual void Initialize(IncrementalGeneratorInitializationContext context)
    {
        Data = new TData
        {
            IncrementalGeneratorInitializationContext = context,
            CompilationProvider = context.CompilationProvider.Select((compilation, _) =>
            {
                Data.Compilation = compilation;
                return compilation;
            }),
        };

        var combined = Initialize(Data);

        context.RegisterSourceOutput(combined, (spc, data) =>
        {
            try
            {
                Data = data;
                Data.SourceProductionContext = spc;
                Generate(Data);
            }
            catch (Exception ex)
            {
                var name = Naming.HintName(data.Type);
                spc.AddSource($"{name}.Error.cs", $"// {ex}");
                Log(spc, _error, ex.ToString(), data.Type);
            }
        });
    }

    protected abstract IncrementalValuesProvider<TData> Initialize(TData data);
    protected abstract void Generate(TData data);

    private static void Log(SourceProductionContext context, DiagnosticDescriptor descriptor, string message, ISymbol? symbol = null) => context.ReportDiagnostic(Diagnostic.Create(descriptor, symbol?.Locations.FirstOrDefault(l => l.IsInSource) ?? Location.None, message));

    protected INamedTypeSymbol GetNamedTypeSymbol(Type type) => Symbols.GetNamedTypeSymbol(Data.Compilation, type);
    protected INamedTypeSymbol GetNamedTypeSymbol(string fullName) => Symbols.GetNamedTypeSymbol(Data.Compilation, fullName);
    protected INamedTypeSymbol GetNamedTypeSymbol<T>() => Symbols.GetNamedTypeSymbol<T>(Data.Compilation);
    protected IncrementalValueProvider<ImmutableArray<INamedTypeSymbol>> FindSymbolsFromAttribute(string fullName) => Symbols.FindSymbolsFromAttribute(Data.IncrementalGeneratorInitializationContext, fullName);
}