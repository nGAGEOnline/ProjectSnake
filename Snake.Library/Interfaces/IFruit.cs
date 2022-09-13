using Snake.Library.Enums;

namespace Snake.Library.Interfaces;

public interface IFruit
{
	Coord Coord { get; }
	ColorType ColorType { get; set; }
	string FruitSymbol { get; set; }
	void Render(IRenderer renderer);
}
