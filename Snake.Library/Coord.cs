namespace Snake.Library
{
	public readonly struct Coord
	{
		#region STATICS

		public static Coord Zero => new(0, 0);
		public static Coord Up => new(0, -1);
		public static Coord Down => new(0, 1);
		public static Coord Left => new(-1, 0);
		public static Coord Right => new(1, 0);
		public static Coord One => new(1, 1);

		#endregion

		public int X { get; }
		public int Y { get; }

		public Coord(int x, int y)
		{
			X = x;
			Y = y;
		}

		private bool Equals(Coord other)
			=> X == other.X && Y == other.Y;

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;
			return obj.GetType() == GetType() && Equals((Coord)obj);
		}

		public override int GetHashCode()
			=> HashCode.Combine(X, Y);

		public static bool operator ==(Coord left, Coord right)
			=> Equals(left, right);

		public static bool operator !=(Coord left, Coord right)
			=> !Equals(left, right);

		public static Coord operator -(Coord left, Coord right)
			=> new(left.X - right.X, left.Y - right.Y);

		public static Coord operator +(Coord left, Coord right)
			=> new(left.X + right.X, left.Y + right.Y);
	}
}