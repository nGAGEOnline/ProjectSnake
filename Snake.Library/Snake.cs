using Snake.Library.Enums;
using Snake.Library.Interfaces;

namespace Snake.Library;

public sealed class Snake : ISnake
{
	public IEnumerable<Coord> Coords => _coords;

	public event Action? EatBomb;
	public event Action? EatFruit;
	public event Func<Task>? Die;
	public event Action<Coord>? RemoveTail;

	public Coord Head => _coords.ElementAt(0);
	public Coord Body => _coords.ElementAt(1);
	public Coord Tail => _coords.Last();
	
	private readonly LinkedList<Coord> _coords = new();
	private readonly Board _board;

	public Snake(Board board, int startingLength = 5)
	{
		_board = board;

		// Temporary random starting position in the center of the screen
		var aThirdWidth = _board.Width / 3;
		var aThirdHeight = _board.Height / 3;
		var x = SnakeGame.Rng.Next(aThirdWidth) + aThirdWidth - startingLength;
		var y = SnakeGame.Rng.Next(aThirdHeight) + aThirdHeight;
		
		for (var i = 0; i < startingLength; i++)
			Add(new Coord(x + i, y));
	}

	public void Move(Direction direction)
	{
		if (direction == Direction.None)
			return;

		var nextCoord = Head + direction switch
		{
			Direction.Up => Coord.Up,
			Direction.Down => Coord.Down,
			Direction.Left => Coord.Left,
			Direction.Right => Coord.Right,
			_ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
		};

		if (SnakeGame.CanWrap)
			nextCoord = direction switch
			{
				Direction.Up => new Coord(nextCoord.X, (nextCoord.Y % _board.Height + _board.Height) % _board.Height),
				Direction.Down => new Coord(nextCoord.X, nextCoord.Y % _board.Height),
				Direction.Left => new Coord((nextCoord.X % _board.Width + _board.Width) % _board.Width, nextCoord.Y),
				Direction.Right => new Coord(nextCoord.X % _board.Width, nextCoord.Y),
			};
		
		switch (NextGridValue(nextCoord))
		{
			case GridValue.Border:
				if (SnakeGame.WallKills)
					Die?.Invoke();
				break;
			case GridValue.Snake:
				Die?.Invoke();
				break;
			case GridValue.Bomb:
				if (SnakeGame.CanEatBomb)
				{
					Add(nextCoord);
					EatBomb?.Invoke();
				}
				else
					Die?.Invoke();
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

	public void CheckForDamage(IBomb bomb)
	{
		foreach (var coord in bomb.ExplosionCoords.Where(c => !IsOutsideGrid(c)))
			if (_coords.Contains(coord))
				Die?.Invoke();
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
		if (!SnakeGame.CanWrap && IsOutsideGrid(nextCoord))
			return GridValue.Border;

		return nextCoord == Tail
			? GridValue.Empty 
			: _board.Grid[nextCoord.X, nextCoord.Y];
	}

	private bool IsOutsideGrid(Coord coord) 
		=> coord.X < 0 || coord.X >= _board.Width || coord.Y < 0 || coord.Y >= _board.Height;
}
