namespace Snake.Library;

public readonly struct Coord
{
	public int X { get; }
	public int Y { get; }
	
	public static Coord Zero => new(0, 0);
	public static Coord Up => new(0, -1);
	public static Coord Down => new(0, 1);
	public static Coord Left => new(-1, 0);
	public static Coord Right => new(1, 0);
	public static Coord One => new(1, 1);

	public Coord(int x, int y)
	{
		X = x;
		Y = y;
	}

	public bool Equals(Coord other)
	{
		return X == other.X && Y == other.Y;
	}

	public override bool Equals(object? obj)
	{
		if (ReferenceEquals(null, obj))
			return false;
		if (ReferenceEquals(this, obj))
			return true;
		if (obj.GetType() != this.GetType())
			return false;
		return Equals((Coord)obj);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(X, Y);
	}

	public static bool operator ==(Coord? left, Coord? right)
	{
		return Equals(left, right);
	}

	public static bool operator !=(Coord? left, Coord? right)
	{
		return !Equals(left, right);
	}

	public static Coord operator -(Coord? left, Coord? right)
	{
		return new Coord(left.Value.X - right.Value.X, left.Value.Y - right.Value.Y);
	}
	public static Coord operator +(Coord? left, Coord? right)
	{
		return new Coord(left.Value.X + right.Value.X, left.Value.Y + right.Value.Y);
	}
}
