using Snake.Library.Enums;
using Snake.Library.Interfaces;

namespace Snake.Library;

public class Snake : ISnake
{
	public IEnumerable<Coord> Coords => _coords;

	public event Action? EatFruit;
	public event Action? Die;
	public event Action<Coord>? RemoveTail;
	public event Action<Coord, Direction, Coord> DebugDataPositions;

	public Coord Head => _coords.First.Value;
	public Coord Tail => _coords.Last.Value;

	public Direction CurrentDirection { get; private set; }
	
	private readonly LinkedList<Coord> _coords = new();
	private readonly IBoard _board;
	private Coord _head;

	public Snake(IBoard board, int startingLength = 3)
	{
		_board = board;

		var x = SnakeGame.Rng.Next(board.Width / 2); // + board.Width / 4;
		var y = SnakeGame.Rng.Next(board.Height / 2); // + board.Height / 4;
		for (var i = 0; i < startingLength; i++)
			Add(new Coord(x + i, y));
	}

	public void Move(Direction direction)
	{
		CurrentDirection = direction;
		
		if (direction == Direction.None)
			return;

		var nextCoord = _head + direction switch
		{
			Direction.Up => Coord.Up,
			Direction.Down => Coord.Down,
			Direction.Left => Coord.Left,
			Direction.Right => Coord.Right,
		};

		switch (NextGridValue(nextCoord))
		{
			case GridValue.Border:
				if (SnakeGame.CanDie)
					Die?.Invoke();
				break;
			case GridValue.Snake:
				Die?.Invoke();
				break;
			case GridValue.Empty:
				Add(nextCoord);
				Remove();
				break;
			case GridValue.Fruit:
				Add(nextCoord);
				EatFruit?.Invoke();
				break;
		}
		if (SnakeGame.IsDebugMode)
			DebugDataPositions?.Invoke(Head, direction, nextCoord);
	}

	private void Add(Coord coord)
	{
		_board.Grid[coord.X, coord.Y] = GridValue.Snake;
		_coords.AddFirst(coord);
		_head = coord;
	}

	private void Remove()
	{
		_board.Grid[Tail.X, Tail.Y] = GridValue.Empty;
		RemoveTail?.Invoke(Tail);
		_coords.RemoveLast();
	}

	private GridValue NextGridValue(Coord nextCoord)
	{
		if (IsOutsideGrid(nextCoord))
			return GridValue.Border;

		return nextCoord == Tail
			? GridValue.Empty 
			: _board.Grid[nextCoord.X, nextCoord.Y];
	}

	private bool IsOutsideGrid(Coord coord) 
		=> coord.X < 0 || coord.X >= _board.Width || coord.Y < 0 || coord.Y >= _board.Height;
}
