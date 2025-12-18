using System.Collections.Concurrent;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Host;

namespace Vyrse;

internal static class Compiler
{
    public static byte[] GetOrRebuildImage(Environment environment, ProjectId id)
    {
        var solution = environment.Solution;
        var project = solution.GetProject(id).Validate();

        environment.ProjectCache.TryGetValue(project.Id, out var cacheEntry);

        if (!cacheEntry.IsEmpty && project.DependentSemanticVersion.Equals(cacheEntry.Version))
        {
            Log($"Project '{project.Name}' is up to date. Using cached image.");
            return cacheEntry.Image!;
        }

        Log($"Compiling project '{project.Name}'...");
        var compilationOptions = (CSharpCompilationOptions)project.CompilationOptions!;
        var parseOptions = (CSharpParseOptions)project.ParseOptions!;
        var documents = project.Documents.ToArray();
        var count = documents.Length;
        var trees = new ConcurrentBag<SyntaxTree>();

        Parallel.For(0, count, i =>
        {
            var doc = documents[i];
            var text = doc.GetTextAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var tree = CSharpSyntaxTree.ParseText(text, parseOptions, doc.FilePath ?? string.Empty);
            trees.Add(tree);
        });

        var references = project.MetadataReferences;
        var compilation = CSharpCompilation.Create(project.AssemblyName, syntaxTrees: trees, references: references, options: compilationOptions);

        Log("Analyzing syntax trees...");
        var treeCount = compilation.SyntaxTrees.Count;
        Log($"Total syntax trees: {treeCount}");

        // var diagnostics = compilation.Diagnostics;
        // var diagnosticsCount = diagnostics.Length;
        // var errors = compilation.Errors.ToArray();
        // var errorCount = errors.Count;
        // if (errorCount > 0)
        // {
        //     Log("Compilation failed due to errors:");
        //     foreach (var error in errors)
        //         Log(error.ToString());
        //
        //     throw new InvalidOperationException("Engine compilation failed.");
        // }
        //
        // Log($"Diagnostics: {diagnosticsCount}, Errors: {errorCount}");

        Log($"Emitting assembly for project '{project.Name}'...");
        using var peStream = new MemoryStream();
        var emitResult = compilation.Emit(peStream);
        if (!emitResult.Success)
        {
            var emitDiagnostics = emitResult.Diagnostics;
            var emitCount = emitDiagnostics.Length;
            var emitErrors = 0;

            for (var i = 0; i < emitCount; i++)
                if (emitDiagnostics[i].Severity == DiagnosticSeverity.Error)
                    emitErrors++;

            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"[{nameof(Compiler)}] Emission failed with " + emitErrors + " error(s).");

            var printed = 0;
            for (var i = 0; i < emitCount && printed < 20; i++)
            {
                var d = emitDiagnostics[i];
                if (d.Severity != DiagnosticSeverity.Error) continue;
                sb.AppendLine(d.ToString());
                printed++;
            }

            throw new InvalidOperationException(sb.ToString());
        }

        var image = peStream.ToArray().ThrowIfNullOrEmpty();

        Log("Updating cached image and version...");

        environment.ProjectCache[project.Id] = new Cache.Project
        {
            Image = image,
            Version = project.DependentSemanticVersion,
            Compilation = compilation
        };

        Log($"Compilation succeeded with an image size of: {image.Length} bytes.");
        return image;
    }

    [Conditional("DEBUG")]
    private static void Log(string message) => Console.WriteLine($"[{nameof(Compiler)}][{DateTime.Now:HH:mm:ss.fff}] {message}");
}

public sealed class Workspace : Microsoft.CodeAnalysis.Workspace
{
    public Workspace(HostServices host, string? workspaceKind) : base(host, workspaceKind)
    {

    }
}