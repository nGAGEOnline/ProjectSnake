using Snake.Library.Enums;
using Snake.Library.Interfaces.UI;
using Snake.Library.Structs;

namespace Snake.Library.Interfaces
{
	public interface ISnakeGameRenderer
	{
		Coord Offset { get; }
		
		void RenderBorder();
		void RenderGrid();
		void Render(Span<Coord> coords, ObjectType objectType);
		void Render<T>(ITextField textField, T textStyle) where T : ITextStyle;

		char[,] GetCharacters(Coord coord, int width, int height = 1);
	}
}
