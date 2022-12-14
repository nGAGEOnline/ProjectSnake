using System.Numerics;

namespace Snake.Library.Structs
{
	public readonly struct Coord
	{
		#region STATICS

		internal static Coord Zero => new Coord(0, 0);
		internal static Coord Up => new Coord(0, -1);
		internal static Coord Down => new Coord(0, 1);
		internal static Coord Left => new Coord(-1, 0);
		internal static Coord Right => new Coord(1, 0);
		internal static Coord One => new Coord(1, 1);

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
			=> new Coord(left.X - right.X, left.Y - right.Y);

		public static Coord operator +(Coord left, Coord right)
			=> new Coord(left.X + right.X, left.Y + right.Y);

		public static implicit operator Vector2(Coord coord) 
			=> new Vector2(coord.X, coord.Y);
		public static implicit operator Coord(Vector2 vector) 
			=> new Coord((int)vector.X, (int)vector.Y);

		public override string ToString() 
			=> $"[{X}, {Y}]";
	}
}