using Snake.Library.Enums;

namespace Snake.Library.Interfaces;

public interface ISnake
{
	IEnumerable<Coord> Coords { get; }
	
	Coord Head { get; }
	Coord Tail { get; }

	event Action<Coord, Direction, Coord> DebugDataPositions;
	event Action<Coord> RemoveTail;
	event Action EatFruit;
	event Action Die;

	void Move(Direction direction);
}
