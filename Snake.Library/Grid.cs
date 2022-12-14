using System.Buffers;
using Snake.Library.Enums;
using Snake.Library.Structs;

namespace Snake.Library
{
	internal sealed class Grid
	{
		private readonly Random _rng = new Random();

		internal event SpanAction<Coord, ObjectType>? OnGridValueChanged;
		internal event Action? OnSnakeMoved;
		
		private readonly ObjectType[,] _values;
		private readonly SnakeSettings _settings;

		internal Grid(SnakeSettings settings)
		{
			_settings = settings;
			_values = new ObjectType[settings.Width, settings.Height];
		}

		internal void Update(IEnumerable<Coord> coords, ObjectType objectType)
		{
			var array = coords as Coord[] ?? coords.ToArray();
			var coord = array[0];
			_values[coord.X, coord.Y] = objectType;
			
			OnGridValueChanged?.Invoke(array, objectType);
			if (objectType == ObjectType.Snake)
				OnSnakeMoved?.Invoke();
		}

		internal bool SpawnFruit()
		{
			var empties = new Span<Coord>(GetEmptyCoords().ToArray());
			if (empties.Length == 0)
				return false;

			var coord = empties[_rng.Next(empties.Length)];
			Update(new [] { coord }, ObjectType.Fruit);
			return true;
		}
		
		internal ObjectType Value(Coord coord) 
			=> _values[coord.X, coord.Y];

		internal bool IsInsideGrid(Coord coord)
			=> coord.X >= 0 && coord.X < _settings.Width && coord.Y >= 0 && coord.Y < _settings.Height;
		
		private IEnumerable<Coord> GetEmptyCoords()
		{
			for (var y = 0; y < _settings.Height; y++)
				for (var x = 0; x < _settings.Width; x++)
					if (_values[x, y] == ObjectType.Empty)
						yield return new Coord(x, y);
		}
	}
}
