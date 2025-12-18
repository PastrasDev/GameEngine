using System.Reflection;
using System.Runtime.Loader;

namespace Vyrse;

public sealed class Session : IDisposable
{
    private readonly byte[] _image;
    private readonly string _entryTypeFullName;
    private readonly string _entryMethodName;
    private readonly string[] _args;

    private Thread? _thread;
    private readonly ManualResetEventSlim _finished = new(false);

    private LoadContext _context = null!;
    private object _instance = null!;
    private MethodInfo _entryMethod = null!;

    public bool IsRunning => _thread is { IsAlive: true };
    public int ExitCode { get; private set; }

    public Session(byte[] image, string entryTypeFullName, string entryMethodName, string[]? args)
    {
        if (image.NullOrEmpty) throw new ArgumentNullException(nameof(image));
        if (entryTypeFullName.NullOrEmpty) throw new ArgumentNullException(nameof(entryTypeFullName));
        if (entryMethodName.NullOrEmpty) throw new ArgumentNullException(nameof(entryMethodName));

        _image = image;
        _entryTypeFullName = entryTypeFullName;
        _entryMethodName = entryMethodName;
        _args = args ?? [];
    }

    public void Start()
    {
        if (_thread is not null)
            throw new InvalidOperationException("Session has already been started.");

        _thread = new Thread(ThreadMain) { IsBackground = false, Name = "Vyrse.EngineThread" };

        _thread.SetApartmentState(ApartmentState.STA);

        _thread.Start();
    }

    private void ThreadMain()
    {
        try
        {
            _context = new LoadContext(AppContext.BaseDirectory);
            using var memoryStream = new MemoryStream(_image, writable: false);
            var assembly = _context.LoadFromStream(memoryStream)!.Validate();
            var entryType = assembly.GetType(_entryTypeFullName, throwOnError: false, ignoreCase: false).Validate();
            _instance = Activator.CreateInstance(entryType).Validate();
            _entryMethod = entryType.GetMethod(_entryMethodName, [typeof(string[])]).Validate();
            ExitCode = (int)_entryMethod.Invoke(_instance, [_args])!;
            this.Log("Engine exited with code " + ExitCode + ".");
        }
        finally
        {
            _finished.Set();
        }
    }

    public bool Wait(int millisecondsTimeout) => _finished.Wait(millisecondsTimeout);
    public void Wait() => _finished.Wait();

    public void Dispose()
    {
        _finished.Dispose();

        if (_instance is IDisposable disposable)
            disposable.Dispose();

        _context?.Unload();
    }

    private sealed class LoadContext(string? resolverBasePath) : AssemblyLoadContext(isCollectible: true)
    {
        private readonly AssemblyDependencyResolver? _resolver = !string.IsNullOrWhiteSpace(resolverBasePath) ? new AssemblyDependencyResolver(resolverBasePath) : null;

        protected override Assembly? Load(AssemblyName name)
        {
            var path = _resolver?.ResolveAssemblyToPath(name);
            return path != null ? LoadFromAssemblyPath(path) : null;
        }
    }
}