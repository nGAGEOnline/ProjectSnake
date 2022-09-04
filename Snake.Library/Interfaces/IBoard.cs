using Snake.Library.Enums;

namespace Snake.Library.Interfaces;

public interface IBoard
{
	int Width { get; }
	int Height { get; }
	Coord FruitCoord { get; }
	
	GridValue[,] Grid { get; }

	void AddFruit();
}
