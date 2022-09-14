using Snake.Library.Enums;

namespace Snake.Library.Interfaces;

public interface ISnake
{
	IEnumerable<Coord> Coords { get; }
	
	Coord Head { get; }
	Coord Body { get; } // Just the 2nd element (old head on next frame)
	Coord Tail { get; }

	event Action<Coord> RemoveTail;
	event Action? EatBomb;
	event Action EatFruit;
	event Func<Task> Die;

	void Move(Direction direction);
	void CheckForDamage(IBomb bomb);
}
