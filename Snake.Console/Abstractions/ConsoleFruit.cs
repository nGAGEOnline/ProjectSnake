using Snake.Library;
using Snake.Library.Enums;
using Snake.Library.Interfaces;

namespace Snake.Console.Abstractions;

public class ConsoleFruit : IFruit
{
	#region CONSTS

	private const char FRUIT_SYMBOL = '■';
	private const ConsoleColor FRUIT_COLOR = ConsoleColor.Red;

	#endregion
	
	public Coord Coord  { get; }

	public ConsoleFruit(Coord coord)
		=> Coord = coord;

	public void Render(IRenderer renderer) 
		=> renderer.Render(Coord, RenderType.Fruit);
}
