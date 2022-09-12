using Snake.Library.Enums;

namespace Snake.Library.Interfaces;

public interface ISnake
{
	IEnumerable<Library.Coord> Coords { get; }
	
	Library.Coord Head { get; }
	Library.Coord Body { get; } // Just the 2nd element (old head on next frame)
	Library.Coord Tail { get; }

	event Action<Library.Coord> RemoveTail;
	event Action EatFruit;
	event Action Die;

	void Move(Direction direction);
}
