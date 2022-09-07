using Snake.Library;
using Snake.Library.Interfaces;

namespace Snake.Console.Abstractions;

public class ConsoleFruit : IFruit
{
	#region CONSTS

	private const char EMPTY_SYMBOL = ' ';
	private const char FRUIT_SYMBOL = '■';
	private const ConsoleColor FRUIT_COLOR = ConsoleColor.Red;

	#endregion

	public Coord Coord { get; set; }

	public ConsoleFruit(Coord coord) 
		=> Coord = coord;
}
