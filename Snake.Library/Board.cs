using Snake.Library.Enums;
using Snake.Library.Interfaces;

namespace Snake.Library;

public sealed class Board
{
	public int Width => Grid.GetLength(0);
	public int Height => Grid.GetLength(1);

	public IFruit? Fruit { get; private set; }
	public IBomb? Bomb { get; private set; }
	
	public event Action<IBomb> OnExplosion;
	
	public GridValue[,] Grid { get; }

	public Board(int width, int height) 
		=> Grid = new GridValue[width, height];

	public void SpawnFruit()
	{
		var emptyPositions = EmptyPositions().ToList();
		if (emptyPositions.Count == 0)
		{
			Fruit = null;
			return;
		}

		var coord = emptyPositions[SnakeGame.Rng.Next(emptyPositions.Count)];
		Fruit = new Fruit(coord);
		Grid[coord.X, coord.Y] = GridValue.Fruit;
	}

	public void SpawnBomb(IRenderer renderer)
	{
		var emptyPositions = EmptyPositions().ToList();
		if (emptyPositions.Count == 0)
		{
			Bomb = null;
			return;
		}

		var coord = emptyPositions[SnakeGame.Rng.Next(emptyPositions.Count)];
		Bomb = new Bomb(renderer, coord);
		Bomb.OnExplosion += Explode;
		Grid[coord.X, coord.Y] = GridValue.Bomb;
	}

	private void Explode(IBomb? bomb)
	{
		if (bomb == null)
			return;
		
		OnExplosion?.Invoke(bomb);
		ClearBombFromGrid(bomb);
	}
	public void ClearBombFromGrid(IBomb bomb)
	{
		Grid[bomb.Coord.X, bomb.Coord.Y] = GridValue.Empty;
		bomb.OnExplosion -= ClearBombFromGrid;
		Bomb = null;
	}

	private IEnumerable<Coord> EmptyPositions()
	{
		for (var y = 0; y < Height; y++)
			for (var x = 0; x < Width; x++)
				if (Grid[x, y] == GridValue.Empty)
					yield return new Coord(x, y);
	}
}