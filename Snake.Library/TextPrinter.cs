using Snake.Library.Enums;
using Snake.Library.Interfaces;

namespace Snake.Library;

public class TextPrinter : ITextPrinter
{
	public Coord Coord { get; private set; }
	public ITextField TextField { get; private set; }

	private readonly Board _board;

	public TextPrinter(Board board, ITextField textField) 
		: this(board, textField, new Coord(board.Width / 2 - textField.Text.Length / 2, board.Height / 2)) { }
	public TextPrinter(Board board, ITextField textField, int lineHeight) 
		: this(board, textField, new Coord(board.Width / 2 - textField.Text.Length / 2, lineHeight)) { }
	public TextPrinter(Board board, ITextField textField, Coord coord)
	{
		_board = board;
		TextField = textField;
		Coord = coord;
	}

	public void Update(string text, HorizontalAlign horizontalAlign = HorizontalAlign.Center)
	{
		TextField.Text = text;
		var x = horizontalAlign switch
		{
			HorizontalAlign.Center => _board.Width / 2 - text.Length / 2,
			HorizontalAlign.Left => 0,
			HorizontalAlign.Right => _board.Width - 1,
			_ => _board.Width / 2 - text.Length / 2
		};
		Coord = new Coord(x, Coord.Y);
	}
	public void Render(IRenderer renderer)
		=> renderer.Render(this);
}
