namespace Engine.Abstractions;

public interface IRegistry
{
    bool Add<T>(T service) where T : class => false;
    void Replace<T>(T service) where T : class { }
    bool Remove<T>() where T : class => false;
    T Get<T>() where T : class => null!;
    bool TryGet<T>(out T? service) where T : class { service = null; return false; }
}