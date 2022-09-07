namespace Snake.Library.Interfaces;

public interface IFruit
{
	Coord Coord { get; set; }
	IRenderer Renderer { get; }

	void Render();
}
