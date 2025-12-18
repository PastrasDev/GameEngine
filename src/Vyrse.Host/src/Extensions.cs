using System.Collections;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.MSBuild;

namespace Vyrse;

internal static class Extensions
{
    extension<T>(IEnumerable<T> source)
    {
        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                switch (source)
                {
                    case ICollection<T> colT: return colT.Count;
                    case ICollection col: return col.Count;
                }

                var count = 0;
                using var e = source.GetEnumerator();
                while (e.MoveNext())
                {
                    count++;
                }

                return count;
            }
        }
    }

    extension<T>(T obj)
    {
        [Conditional("DEBUG")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Log(string message) => Console.WriteLine($"[{typeof(T).Name}][{DateTime.Now:HH:mm:ss.fff}] {message}");
    }

    extension(object obj)
    {
        [Conditional("DEBUG")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Log<T>(string message) => Console.WriteLine($"[{typeof(T).Name}][{DateTime.Now:HH:mm:ss.fff}] {message}");
    }

    extension<T>(T? obj) where T : class
    {
        public bool Null => obj is null;

        public bool Empty => obj switch
        {
            string s => s.Length == 0,
            Array a => a.Length == 0,
            ICollection c => c.Count == 0,
            _ => false
        };

        public bool NullOrEmpty => obj!.Null || obj!.Empty;
    }

    [return: NotNull][MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Validate<T>([NotNull] this T? obj, string? message = null, [CallerFilePath] string file = "", [CallerLineNumber] int line = 0, [CallerMemberName] string member = "") where T : class
    {
        if (obj is not null) return obj;
        message ??= $"Object of type '{typeof(T).Name}' is null";
        throw new InvalidOperationException($"{message}: [File: {file}], [Line: {line}], [Member: {member}()]");
    }

    [return: NotNull][MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T ThrowIfNullOrEmpty<T>([NotNull] this T? obj, string? message = null, [CallerFilePath] string file = "", [CallerLineNumber] int line = 0, [CallerMemberName] string member = "") where T : class
    {
        if (obj is not null && !obj.Empty) return obj;
        message ??= $"Object of type '{typeof(T).Name}' is null or empty";
        throw new InvalidOperationException($"{message}: [File: {file}], [Line: {line}], [Member: {member}()]");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ThrowIf(this bool condition, bool value, string? message = null, [CallerArgumentExpression("condition")] string? expr = null, [CallerFilePath] string file = "", [CallerLineNumber] int line = 0, [CallerMemberName] string member = "")
    {
        if (condition != value) return value;
        message ??= $"Condition was {value}: '{expr}' at {file}:{line} in {member}(): Message: {message}";
        throw new InvalidOperationException(message);
    }

    extension(MSBuildWorkspace workspace)
    {
        public FileInfo FindSolution(string name, string directory)
        {
            name.ThrowIfNullOrEmpty();
            directory.ThrowIfNullOrEmpty();
            var dir = directory;
            while (!string.IsNullOrEmpty(dir))
            {
                var candidate = Path.Combine(dir, name);
                if (File.Exists(candidate))
                    return new FileInfo(candidate);

                dir = Path.GetDirectoryName(dir);
            }

            throw new FileNotFoundException("Solution file not found.", name);
        }

        public Solution OpenSolution(FileInfo fileInfo, IProgress<ProjectLoadProgress>? progress = null)
        {
            fileInfo.Exists.ThrowIf(false);
            return workspace.OpenSolutionAsync(fileInfo.FullName, progress).ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }

    extension(CSharpCompilation compilation)
    {
        public IEnumerable<Diagnostic> Errors
        {
            get
            {
                foreach (var diagnostic in compilation.GetDiagnostics())
                    if (diagnostic.Severity >= DiagnosticSeverity.Error)
                        yield return diagnostic;
            }
        }

        public ImmutableArray<Diagnostic> Diagnostics => compilation!.GetDiagnostics();
    }

    extension(Solution solution)
    {
        public Project? GetProject(string name)
        {
            name.ThrowIfNullOrEmpty();
            foreach (var project in solution.Projects)
                if (string.Equals(project.Name, name, StringComparison.OrdinalIgnoreCase))
                    return project;

            return null;
        }
    }

    extension(Project project)
    {
        public VersionStamp DependentSemanticVersion => project.GetDependentSemanticVersionAsync().ConfigureAwait(false).GetAwaiter().GetResult();
    }
}