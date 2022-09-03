using Snake.Library.Abstractions;

namespace Snake.Library.Interfaces;

public interface IRenderer
{
	void Render(IBoard board);
	void Render(ISnake snake);
	void RenderFruit(Coord coord);
	void RenderText(Coord coord, string text, MessageType messageType);
	void Clear(Coord coord);
}
