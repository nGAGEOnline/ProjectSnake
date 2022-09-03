using Snake.Library.Enums;
using Snake.Library.Helpers;
using Snake.Library.Interfaces;

namespace Snake.Library.Abstractions;

public class ConsoleInput : IInput
{
	// public event Action<Direction>? OnInput;
	public Direction Direction { get; private set; }

	public ConsoleInput()
	{
		//Direction = (Direction)SnakeGame.Rng.Next(1, Enum.GetValues<Direction>().Length);
		Direction = Direction.Right;
	}
	private readonly Queue<Direction> _directionChanges = new();

	public void Listen()
	{
		var direction = GetDirectionFromInput();
		ChangeDirection(direction);
		
		if (_directionChanges.Count > 0)
			Direction = _directionChanges.Dequeue();
	}

	private Direction GetDirectionFromInput()
	{
		if (!Console.KeyAvailable)
			return Direction;
				
		return Console.ReadKey(true).Key switch
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
		if (_directionChanges.Count == 1)
			return false;

		var lastDirection = GetLastDirection();
		return newDirection != lastDirection && newDirection != lastDirection.Opposite();
	}

	private Direction GetLastDirection()
		=> _directionChanges.Count == 0
			? Direction
			: _directionChanges.Last();

}