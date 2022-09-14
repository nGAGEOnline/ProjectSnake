using Snake.Library.Enums;

namespace Snake.Library.Interfaces;

public interface ITextField
{
	string Text { get; set; }

	ColorType ForegroundColor { get; }
	ColorType BackgroundColor { get; }
}
