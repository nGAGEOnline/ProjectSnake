using Snake.Library.Enums;
using Snake.Library.Interfaces;

namespace Snake.Library
{
	public class TextField : ITextField
	{
		public string Text { get; set; }
		public ColorType ForegroundColor { get; }
		public ColorType BackgroundColor { get; }

		public TextField(string text, ColorType foregroundColor, ColorType backgroundColor)
		{
			Text = text;
			ForegroundColor = foregroundColor;
			BackgroundColor = backgroundColor;
		}
	}
}