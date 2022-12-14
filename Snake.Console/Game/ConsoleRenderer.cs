using Snake.Console.Game.UI;
using Snake.Library;
using Snake.Library.Enums;
using Snake.Library.Interfaces;
using Snake.Library.Interfaces.UI;
using Snake.Library.Structs;

namespace Snake.Console.Game;

public class ConsoleRenderer : IGameRenderer
{
	public Coord Offset => new Coord(OFFSET_X, OFFSET_Y);
	private const int OFFSET_X = 3;
	private const int OFFSET_Y = 2;
	
	#region CONST SYMBOLS 
	
	// ░▒▓█■∙
	private const char GRID_SYMBOL = '∙';
	private const char EMPTY_SYMBOL = ' ';
	private const char BORDER_SYMBOL = '█';
	private const char SNAKE_SYMBOL = '▒';
	private const char SNAKE_HEAD_SYMBOL = 'S';
	private const char FRUIT_SYMBOL = 'F';
	private const char BOMB_SYMBOL = 'B';
	
	#endregion

	private readonly char[,] _screenBuffer;
	private readonly SnakeSettings _settings;

	public ConsoleRenderer(SnakeSettings settings, bool clear = true)
	{
		_screenBuffer = new char[settings.Width + OFFSET_X * 2, settings.Height + OFFSET_Y * 2];
		_settings = settings;
		System.Console.CursorVisible = false;
		if (clear)
			System.Console.Clear();
	}

	public void RenderBorder()
	{
		for (var y = -1; y <= _settings.Height; y++)
			for (var x = -1; x <= _settings.Width; x++)
				if (x == -1 || x == _settings.Width || y == -1 || y == _settings.Height)
					Print(new Coord(x, y), $"{BORDER_SYMBOL}", TextStyle.Border);
	}
	public void RenderGrid()
	{
		for (var y = 0; y < _settings.Height; y++)
			for (var x = 0; x < _settings.Width; x++)
				Print(new Coord(x, y), $"{GRID_SYMBOL}", TextStyle.Grid);
	}

	public void Render(Span<Coord> coords, ObjectType objectType)
	{
		var symbol = SymbolFrom(objectType);
		for (var i = 0; i < coords.Length; i++)
		{
			if (objectType == ObjectType.Snake)
				symbol = i == 0 ? SNAKE_HEAD_SYMBOL : SNAKE_SYMBOL;

			Print(coords[i], $"{symbol}", TextStyleFrom(objectType));
		}
	}

	public char[,] GetCharacters(Coord coord, int width, int height = 1)
	{
		var chars = new char[width, height];
		for (var y = 0; y < height; y++)
			for (var x = 0; x < width; x++)
				chars[x, y] = _screenBuffer[coord.X + x, coord.Y + y];
		return chars;
	}
	public void Render<T>(ITextField textField, T textStyle) where T : ITextStyle
		=> Print(textField.Coord, textField.FullText, textStyle);
	private void Print(Coord coord, string text, ITextStyle textStyle)
	{
		for (var i = 0; i < text.Length; i++)
			_screenBuffer[coord.X + OFFSET_X, coord.Y + OFFSET_Y] = text[i];

		System.Console.SetCursorPosition(coord.X + OFFSET_X, coord.Y + OFFSET_Y);
		if (textStyle is ITextStyle<ConsoleColor> style)
		{
			System.Console.ForegroundColor = style.Foreground;
			System.Console.BackgroundColor = style.Background;
		}
		else
		{
			var oStyle = TextStyleFrom(textStyle.ObjectType);
			System.Console.ForegroundColor = oStyle.Foreground;
			System.Console.BackgroundColor = oStyle.Background;
		}
		System.Console.Write($"{text}");
		System.Console.ResetColor();
	}

	private static char SymbolFrom(ObjectType type)
		=> RenderDetails(type).Item1;
	private static ITextStyle<ConsoleColor> TextStyleFrom(ObjectType type)
		=> RenderDetails(type).Item2;
	private static (char, ITextStyle<ConsoleColor>) RenderDetails(ObjectType objectType)
	{
		return objectType switch
		{
			ObjectType.Border => (BORDER_SYMBOL, TextStyle.Border),
			ObjectType.Empty => (GRID_SYMBOL, TextStyle.Grid),
			ObjectType.Snake => (SNAKE_SYMBOL, TextStyle.Snake),
			ObjectType.Fruit => (FRUIT_SYMBOL, TextStyle.Fruit),
			ObjectType.Bomb => (BOMB_SYMBOL, TextStyle.Bomb),
			ObjectType.Grid => (GRID_SYMBOL, TextStyle.Grid),
			_ => (EMPTY_SYMBOL, new TextStyle(ConsoleColor.White, ConsoleColor.Black))
		};
	}
}
