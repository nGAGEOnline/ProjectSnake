using Snake.Library;
using Snake.Library.Enums;
using Snake.Library.Interfaces;

namespace Snake.Console.Abstractions;

public class ConsoleFruit
{
	#region CONSTS

	private const char FRUIT_SYMBOL = '■';
	private const ConsoleColor FRUIT_COLOR = ConsoleColor.Red;

	#endregion
	
	public void Render(IRenderer renderer, IFruit fruit)
	{
		renderer.Render(fruit.Coord, $"{fruit.FruitSymbol}", fruit.ColorType);
	}

}
