using Snake.Library.Enums;

namespace Snake.Library.Interfaces;

public interface IInput
{
	Direction Direction { get; }
	void Listen();
	void Reset();
}
