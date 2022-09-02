using Snake.Library.Abstractions;
using Snake.Library.Enums;
using Snake.Library.Interfaces;

namespace Snake.Library
{
	public class Board : IBoard
	{
		public int Width { get; }
		public int Height { get; }
		public GridValue[,] Grid { get; }
		public Coord Fruit { get; private set; }
		
		public Board(int size) 
			: this(size, size) { }

		public Board(int width, int height)
		{
			Width = width;
			Height = height;
			Grid = new GridValue[Width, Height];
		}
		
		public void AddFruit()
		{
			var emptyPositions = new List<Coord>(EmptyPositions());
			if (emptyPositions.Count == 0)
				return;

			Fruit = emptyPositions[SnakeGame.Rng.Next(emptyPositions.Count)];
			Grid[Fruit.X, Fruit.Y] = GridValue.Fruit;
		}

		private IEnumerable<Coord> EmptyPositions()
		{
			for (var y = 0; y < Height; y++)
				for (var x = 0; x < Width; x++)
					if (Grid[x, y] == GridValue.Empty)
						yield return new Coord(x, y);
		}
	}
}