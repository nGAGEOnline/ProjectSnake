using Snake.Library.Enums;
using Snake.Library.Interfaces;

namespace Snake.Library;

public class Snake : ISnake
{
	public IEnumerable<Coord> Coords => _coords;

	public event Action? EatFruit;
	public event Action? Die;
	public event Action<Coord>? RemoveTail;

	public Coord Head => _coords.ElementAt(0);
	public Coord Body => _coords.ElementAt(1);
	public Coord Tail => _coords.Last();
	
	private readonly LinkedList<Coord> _coords = new();
	private readonly IBoard _board;

	public Snake(IBoard board, int startingLength = 3)
	{
		_board = board;

		var x = SnakeGame.Rng.Next(board.Width / 2);
		var y = SnakeGame.Rng.Next(board.Height / 2);
		for (var i = 0; i < startingLength; i++)
			Add(new Coord(x + i, y));
	}

	public void Move(Direction direction)
	{
		if (direction == Direction.None)
			return;

		var nextCoord = direction switch
		{
			Direction.Up => Head + Coord.Up,
			Direction.Down => Head + Coord.Down,
			Direction.Left => Head + Coord.Left,
			Direction.Right => Head + Coord.Right,
			_ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
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
			case GridValue.Bomb:
				// TODO: Decide if snake should eat bomb, or die
				break;
			case GridValue.Fruit:
				Add(nextCoord);
				EatFruit?.Invoke();
				break;
			case GridValue.Empty:
				Add(nextCoord);
				Remove();
				break;
		}
	}

	private void Add(Coord coord)
	{
		UpdateGrid(coord, GridValue.Snake);
		_coords.AddFirst(coord);
	}

	private void Remove()
	{
		UpdateGrid(Tail, GridValue.Empty);
		RemoveTail?.Invoke(Tail);
		_coords.RemoveLast();
	}
	
	private void UpdateGrid(Coord coord, GridValue gridValue)
		=> _board.Grid[coord.X, coord.Y] = gridValue; 

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
