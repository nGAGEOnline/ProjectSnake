using Snake.Library.Abstractions;
using Snake.Library.Enums;

namespace Snake.Library.Interfaces;

public interface IRenderer
{
	void Render(IBoard board);
	void Render(ISnake snake);
	// void Render(IFruit fruit);
	void RenderFruit(Coord coord);
	void Render(Coord coord, string text, MessageType messageType);
	void Clear(Coord coord);
}
