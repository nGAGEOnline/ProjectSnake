using Snake.Library.Enums;

namespace Snake.Library.Interfaces
{
	public interface ISnakeGameInput
	{
		Direction Direction { get; }
		void Listen();
		void Reset();
	}
}
