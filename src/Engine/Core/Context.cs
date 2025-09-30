using Engine.Core.Registries;
using Engine.Core.Threading.Messaging;
using Engine.Platform.Windows;

namespace Engine.Core
{
	public sealed class Context
	{
		private readonly Keyboard _keyboard = new();
		private readonly Mouse _mouse = new();

		public Affinity Affinity { get; internal set; }
		public Time Time { get; } = new();
		public Keyboard Keyboard => Affinity != Affinity.Game ? throw new InvalidOperationException("Keyboard can only be accessed from the Game thread.") : _keyboard;
		public Mouse Mouse => Affinity != Affinity.Game ? throw new InvalidOperationException("Mouse can only be accessed from the Game thread.") : _mouse;

		//public MessageBus MessageBus => Registry.Global.Get<MessageBus>();
		public required Registry Registry { get; init; }
	}
}