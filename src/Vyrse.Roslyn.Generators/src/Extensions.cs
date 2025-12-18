using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Vyrse.Roslyn.Generators;

#region INamedTypeSymbol

public static class INamedTypeSymbolExtensions
{
	internal static string HintName(this INamedTypeSymbol type) => type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat).Replace("global::", "").Replace('<', '[').Replace('>', ']') + ".Generated.cs";
	internal static bool HasPrimaryConstructor(this INamedTypeSymbol type) => type.DeclaringSyntaxReferences.Select(r => r.GetSyntax()).OfType<ClassDeclarationSyntax>().Any(cd => cd.ParameterList is { Parameters.Count: > 0 });
	internal static bool HasInstanceConstructor(this INamedTypeSymbol type) => type.InstanceConstructors.Any(c => !c.IsImplicitlyDeclared);
}

#endregion

#region IdentifierNameSyntax

public static class IdentifierNameSyntaxExtensions
{
	internal static IdentifierNameSyntax LowerCase(this IdentifierNameSyntax identifierName)
	{
		var name = identifierName.Identifier.Text;
		return string.IsNullOrEmpty(name) ? identifierName : SyntaxFactory.IdentifierName(char.ToLowerInvariant(name[0]) + name[1..]);
	}
}

#endregion

#region SyntaxToken

public static class SyntaxTokenExtensions
{
	internal static SyntaxToken LowerCase(this SyntaxToken identifier)
	{
		var name = identifier.Text;
		return string.IsNullOrEmpty(name) ? identifier : SyntaxFactory.Identifier(char.ToLowerInvariant(name[0]) + name[1..]);
	}
}

#endregion

#region Type

public static class TypeExtensions
{
	internal static INamedTypeSymbol NamedTypeSymbol(this Type type, Compilation compilation) => compilation.GetTypeByMetadataName(type.FullName!) ?? throw new InvalidOperationException($"Type '{type.FullName}' not found in compilation.");

	internal static string AttributeShortName(this Type type)
	{
		const string suffix = "Attribute";
		var name = type.Name;
		return name.EndsWith(suffix, StringComparison.Ordinal) ? name[..^suffix.Length] : name;
	}
}

#endregion