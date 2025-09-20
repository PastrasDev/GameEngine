using Engine.Core;

namespace Engine;

public interface IEngine
{
	static readonly Runtime Runtime  = new();

	static int Start(string[]? args) => (int)Runtime.Run(args);
	static void Shutdown() => Runtime.Shutdown();
}