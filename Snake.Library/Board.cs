using Snake.Library.Enums;
using Snake.Library.Interfaces;

namespace Snake.Library;

public class Board : IBoard
{
	public int Width => _settings.Width;
	public int Height => _settings.Height;
	public Coord FruitCoord { get; private set; }
	
	private readonly Settings _settings;

	public Board(Settings settings) 
		=> _settings = settings;

	public void SpawnFruit()
	{
		var emptyPositions = EmptyPositions().ToList();
		if (emptyPositions.Count == 0)
			return;

		FruitCoord = emptyPositions[SnakeGame.Rng.Next(emptyPositions.Count)];
		_settings.Grid[FruitCoord.X, FruitCoord.Y] = GridValue.Fruit;
	}

	private IEnumerable<Coord> EmptyPositions()
	{
		for (var y = 0; y < Height; y++)
			for (var x = 0; x < Width; x++)
				if (_settings.Grid[x, y] == GridValue.Empty)
					yield return new Coord(x, y);
	}
}
