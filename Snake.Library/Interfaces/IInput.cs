using Snake.Library.Enums;

namespace Snake.Library.Interfaces;

public interface IInput
{
	Direction Direction { get; }
	// event Action<Direction> OnInput;
	void Listen();
}
