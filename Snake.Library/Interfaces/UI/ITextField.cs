using Snake.Library.Structs;

namespace Snake.Library.Interfaces.UI;

public interface ITextField
{
	Coord Coord { get; }
	string Text { get; }
	string FullText { get; }

	void UpdateText(string text);
}
