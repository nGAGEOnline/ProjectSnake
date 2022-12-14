using Snake.Library.Enums;

namespace Snake.Library.Interfaces
{
	public interface IInputProvider
	{
		Direction Direction { get; }
		void Listen();
		void Reset();
	}
}
