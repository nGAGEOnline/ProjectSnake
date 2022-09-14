using Snake.Library.Interfaces;

namespace Snake.Library;

public sealed class Fruit : IFruit
{
	public Coord Coord { get; }

	public Fruit(Coord coord) 
		=> Coord = coord;

	public void Render(IRenderer renderer) 
		=> renderer.Render(this);
}
