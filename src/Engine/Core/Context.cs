using Engine.Core.Registries;
using Engine.Core.Threading.Communication;
using Engine.Platform.Windows;

namespace Engine.Core
{
	public sealed class Context
	{
		private readonly Input _input = new();

		public Affinity Affinity { get; internal set; }
		public Time Time { get; } = new();
		public Input Input => Affinity != Affinity.Game ? throw new InvalidOperationException("Input can only be accessed from the Game thread.") : _input;
		public MessageBus MessageBus => Registry.Global.Get<MessageBus>();

		public required Registry Registry { get; init; }
	}
}