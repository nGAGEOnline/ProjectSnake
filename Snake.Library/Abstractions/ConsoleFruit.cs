using Snake.Library.Interfaces;

namespace Snake.Library.Abstractions;

public class ConsoleFruit : IFruit
{
	#region CONSTS

	private const char EMPTY_SYMBOL = ' ';
	private const char FRUIT_SYMBOL = '■';
	private const ConsoleColor FRUIT_COLOR = ConsoleColor.Red;

	#endregion

	public Coord Coord { get; set; }
	public IRenderer Renderer { get; }

	public ConsoleFruit(Coord coord, IRenderer renderer)
	{
		Coord = coord;
		Renderer = renderer;
	}
	public void Render()
	{
		Renderer.Render(this);
	}
}
