using Snake.Library.Enums;

namespace Snake.Library.Interfaces;

public interface IRenderer
{
	void Render(Board board);
	void Render(ISnake snake);
	void Render(IFruit fruit);
	void Render(ITextPrinter textPrinter);
	
	void Render(IBomb bomb);
	void RenderExplosion(IBomb bomb);
	void Render(Coord coord, char symbol, ConsoleColor color, ConsoleColor bgColor = ConsoleColor.Black);
	
	void Clear(Coord coord);
	void RenderExplosion();
}