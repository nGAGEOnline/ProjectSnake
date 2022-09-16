namespace Snake.Library.Interfaces
{
	public interface ITextPrinter
	{
		Coord Coord { get; }
		ITextField TextField { get; }

		void Render(IRenderer renderer);
	}
}