using Snake.Library.Enums;

namespace Snake.Library.Helpers
{
	public static class Extensions
	{
		public static Direction Opposite(this Direction direction)
		{
			return direction switch
			{
				Direction.Up => Direction.Down,
				Direction.Down => Direction.Up,
				Direction.Left => Direction.Right,
				Direction.Right => Direction.Left,
				Direction.None => Direction.None,
				_ => Direction.None
			};
		}
	}
}