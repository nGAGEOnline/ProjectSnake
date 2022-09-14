using Snake.Library.Enums;

namespace Snake.Library.Interfaces;

public interface IFruit
{
	Coord Coord { get; }
	
	void Render(IRenderer renderer);
}
