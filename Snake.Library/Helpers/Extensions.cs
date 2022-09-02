using Snake.Library.Enums;

namespace Snake.Library.Helpers;

public static class Extensions
{
	public static Coord Next(this Direction direction)
	{
		return direction switch
		{
			Direction.None => Coord.Zero,
			Direction.Up => Coord.Up,
			Direction.Down => Coord.Down,
			Direction.Left => Coord.Left,
			Direction.Right => Coord.Right,
			_ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
		};
	}

	public static Direction Opposite(this Direction direction)
	{
		return direction switch
		{
			Direction.Up => Direction.Down,
			Direction.Down => Direction.Up,
			Direction.Left => Direction.Right,
			Direction.Right => Direction.Left,
			Direction.None => Direction.None,
			_ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
		};
	}
}
