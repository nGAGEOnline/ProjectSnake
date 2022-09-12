using Snake.Library.Enums;

namespace Snake.Library;

public class Board
{
	public int Width => Grid.GetLength(0);
	public int Height => Grid.GetLength(1);
	public Coord FruitCoord { get; private set; }
	
	public GridValue[,] Grid { get; }

	public Board(int width, int height) 
		=> Grid = new GridValue[width, height];

	public void SpawnFruit()
	{
		var emptyPositions = EmptyPositions().ToList();
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
