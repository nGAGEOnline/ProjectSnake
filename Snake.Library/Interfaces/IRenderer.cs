using Snake.Library.Enums;

namespace Snake.Library.Interfaces;

public interface IRenderer
{
	void Render(Snake snake);
	void Render(Board board);
	
	void Render(Coord coord, string text, ColorType colorType);

	void RenderFruit(Coord coord);
	
	void Clear(Coord coord);
}
