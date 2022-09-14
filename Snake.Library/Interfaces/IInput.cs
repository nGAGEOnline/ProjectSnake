using Snake.Library.Enums;

namespace Snake.Library.Interfaces;

public interface IInput
{
	int BufferSize { get; }
	Direction Direction { get; }
	void Listen();
	void Reset();
}
