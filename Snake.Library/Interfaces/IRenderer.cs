using Snake.Library.Enums;

namespace Snake.Library.Interfaces;

public interface IRenderer
{
	void Render(IBoard board);
	void Render(ISnake snake);
	void Render(Coord coord, RenderType renderType);
	void RenderFruit(Coord coord);
	void Render(Coord coord, string text, ColorType colorType);
	void Clear(Coord coord);
}
