using Snake.Library.Enums;
using Snake.Library.Interfaces;

namespace Snake.Library;

public class Board
{
	public int Width => Grid.GetLength(0);
	public int Height => Grid.GetLength(1);
	//public Coord FruitCoord { get; private set; }
	public IFruit Fruit { get; private set; }
	
	public GridValue[,] Grid { get; }

	public Board(int width, int height) 
		=> Grid = new GridValue[width, height];
	
	public void SpawnBomb(IRenderer renderer)
	{
		var emptyPositions = EmptyPositions().ToList();
		if (emptyPositions.Count == 0)
			return;

		var coord = emptyPositions[SnakeGame.Rng.Next(emptyPositions.Count)];

		var bomb = new Bomb(renderer, coord);
		bomb.OnExplosion += ClearBombFromGrid;
		Grid[coord.X, coord.Y] = GridValue.Bomb;
	}

	private void ClearBombFromGrid(IBomb bomb) 
		=> Grid[bomb.Coord.X, bomb.Coord.Y] = GridValue.Empty;

	public void SpawnFruit()
	{
		var emptyPositions = EmptyPositions().ToList();
		if (emptyPositions.Count == 0)
			return;

		var coord = emptyPositions[SnakeGame.Rng.Next(emptyPositions.Count)];
		Fruit = new Fruit(coord);
		Grid[coord.X, coord.Y] = GridValue.Fruit;
	}

	private IEnumerable<Coord> EmptyPositions()
	{
		for (var y = 0; y < Height; y++)
			for (var x = 0; x < Width; x++)
				if (Grid[x, y] == GridValue.Empty)
					yield return new Coord(x, y);
	}
}