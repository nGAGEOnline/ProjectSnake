using Snake.Library.Enums;
using Snake.Library.Helpers;
using Snake.Library.Interfaces;

namespace Snake.Console.Game;

public class ConsoleInputProvider : IInputProvider
{
	public Direction Direction { get; private set; } = Direction.Right;
	
	private readonly int _bufferSize;

	private readonly Queue<Direction> _directionChanges = new();

	public ConsoleInputProvider(int bufferSize = 2) 
		=> _bufferSize = bufferSize;

	public void Listen()
	{
		var direction = GetDirectionFromInput();
		ChangeDirection(direction);
		
		if (_directionChanges.Count > 0)
			Direction = _directionChanges.Dequeue();
	}

	public void Reset() 
		=> Direction = Direction.Right;

	private Direction GetDirectionFromInput()
	{
		if (!System.Console.KeyAvailable)
			return Direction;
				
		return System.Console.ReadKey(true).Key switch
		{
			ConsoleKey.W or ConsoleKey.UpArrow => Direction.Up,
			ConsoleKey.S or ConsoleKey.DownArrow => Direction.Down,
			ConsoleKey.A or ConsoleKey.LeftArrow => Direction.Left,
			ConsoleKey.D or ConsoleKey.RightArrow => Direction.Right,
			ConsoleKey.Escape => Direction.None,
			_ => Direction
		};
	}

	private void ChangeDirection(Direction direction)
	{
		if (CanChangeDirection(direction))
			_directionChanges.Enqueue(direction);
	}

	private bool CanChangeDirection(Direction newDirection)
	{
		if (_directionChanges.Count == _bufferSize || newDirection == Direction || _directionChanges.Contains(newDirection))
			return false;

		var lastDirection = GetLastDirection();
		return newDirection != lastDirection && newDirection != lastDirection.Opposite();
	}

	private Direction GetLastDirection()
		=> _directionChanges.Count == 0
			? Direction
			: _directionChanges.Last();
}