using Snake.Library.Enums;
using Snake.Library.Structs;

namespace Snake.Library
{
	public sealed class Snake
	{
		private LinkedList<Coord> Coords { get; } = new ();

		public Coord Coord => Coords.First();
		private Coord Neck => Coords.ElementAt(1);
		private Coord Tail => Coords.Last();
		public int Length => Coords.Count;

		internal event Action? OnEat;
		internal event Action? OnDie;

		private readonly SnakeSettings _settings;
		private readonly Grid _grid;

		internal Snake(SnakeSettings settings, Grid grid)
		{
			_settings = settings;
			_grid = grid;

			var horizontalRange = _settings.Width / 3;
			var verticalRange = _settings.Height / 3;
			var x = Math.Clamp(new Random().Next(horizontalRange) + horizontalRange - _settings.StartingLength, 0, _settings.Width - 1);
			var y = Math.Clamp(new Random().Next(verticalRange) + verticalRange, 0, _settings.Height - 1);

			for (var i = 0; i < _settings.StartingLength; i++)
				Coords.AddFirst(new Coord(x + i, y));
		}

		internal void Move(Direction direction)
		{
			if (direction == Direction.None)
				return;

			var nextCoord = GetNextCoord(direction);
			switch (NextGridValue(nextCoord))
			{
				case ObjectType.Border:
					if (_settings.WallKills)
						OnDie?.Invoke();
					break;
				case ObjectType.Snake:
					OnDie?.Invoke();
					break;
				case ObjectType.Bomb:
					break;
				case ObjectType.Fruit:
					AddFirst(nextCoord);
					OnEat?.Invoke();
					break;
				case ObjectType.Empty:
					AddFirst(nextCoord);
					RemoveLast();
					break;
				case ObjectType.Grid:
					break;
				case ObjectType.Text:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void AddFirst(Coord coord)
		{
			Coords.AddFirst(coord);
			_grid.Update(new [] { Coord, Neck }, ObjectType.Snake);
		}
		private void RemoveLast()
		{
			_grid.Update(new [] { Tail }, ObjectType.Empty);
			Coords.RemoveLast();
		}

		private Coord GetNextCoord(Direction direction)
		{
			var nextCoord = Coord + direction switch
			{
				Direction.Up => Coord.Up,
				Direction.Down => Coord.Down,
				Direction.Left => Coord.Left,
				Direction.Right => Coord.Right,
				_ => Coord.Zero
			};

			if (_settings.CanWrap)
				nextCoord = direction switch
				{
					Direction.Up => new Coord(nextCoord.X, (nextCoord.Y % _settings.Height + _settings.Height) % _settings.Height),
					Direction.Down => new Coord(nextCoord.X, nextCoord.Y % _settings.Height),
					Direction.Left => new Coord((nextCoord.X % _settings.Width + _settings.Width) % _settings.Width, nextCoord.Y),
					Direction.Right => new Coord(nextCoord.X % _settings.Width, nextCoord.Y),
					_ => Coord
				};

			return nextCoord;
		}
		private ObjectType NextGridValue(Coord nextCoord)
		{
			if (!_settings.CanWrap && !_grid.IsInsideGrid(nextCoord))
				return ObjectType.Border;

			return nextCoord == Tail
				? ObjectType.Empty
				: _grid.Value(nextCoord);
		}
	}
}