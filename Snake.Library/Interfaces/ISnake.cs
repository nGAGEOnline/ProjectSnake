using Snake.Library.Enums;

namespace Snake.Library.Interfaces;

public interface ISnake
{
	public IEnumerable<Coord> Coords { get; }
	public Direction CurrentDirection { get; }
	
	public Coord Head { get; }
	public Coord Tail { get; }

	event Action<Coord, Direction, Coord> DebugDataPositions;
	event Action<Coord> RemoveTail;
	event Action EatFruit;
	event Action Die;

	void Move(Direction direction);
}
