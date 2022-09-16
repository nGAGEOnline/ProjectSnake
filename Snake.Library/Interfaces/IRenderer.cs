using System.Threading.Tasks;

namespace Snake.Library.Interfaces
{
	public interface IRenderer
	{
		void Render(Board board);
		Task Render(ISnake snake, bool gameOver);
		void Render(IFruit fruit);
		void Render(ITextPrinter textPrinter);

		void Render(IBomb bomb);
		void RenderExplosion(IBomb bomb);
		// void Render(Coord coord, char symbol, ConsoleColor color, ConsoleColor bgColor = ConsoleColor.Black);

		void Clear(Coord coord);
		void RenderExplosion();
	}
}