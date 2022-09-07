using Snake.Library.Abstractions;
using Snake.Library.Enums;
using Snake.Library.Interfaces;

namespace Snake.Library;

public class Board : IBoard
{
	public int Width { get; }
	public int Height { get; }
	public GridValue[,] Grid { get; }
	public Coord FruitCoord { get; private set; }

	public Board(int size = 20)
		: this(size, size) { }
	public Board(int width, int height)
	{
		Width = width;
		Height = height;
		Grid = new GridValue[Width, Height];
	}

	public void SpawnFruit()
	{
		var emptyPositions = new List<Coord>(EmptyPositions());
		if (emptyPositions.Count == 0)
			return;

		FruitCoord = emptyPositions[SnakeGame.Rng.Next(emptyPositions.Count)];
		Grid[FruitCoord.X, FruitCoord.Y] = GridValue.Fruit;
	}

	private IEnumerable<Coord> EmptyPositions()
	{
		for (var y = 0; y < Height; y++)
			for (var x = 0; x < Width; x++)
				if (Grid[x, y] == GridValue.Empty)
					yield return new Coord(x, y);
	}
}
