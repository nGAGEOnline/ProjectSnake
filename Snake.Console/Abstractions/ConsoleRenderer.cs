using Snake.Library;
using Snake.Library.Enums;
using Snake.Library.Interfaces;
using static System.Console;

namespace Snake.Console.Abstractions;

// ═║╒╓╔╕╖╗╘╙╚╛╜╝╞╟╠╡╢╣╤╥╦╧╨╩╪╫╬
// ─│┌┐└┘├┬┴┼
// ♦◊◌●☼
// █▓▒░
// ■□▪▫
// ▲►▼◄

public class ConsoleRenderer : IRenderer
{
	#region CONSTS
	
	private const char EMPTY_SYMBOL = ' ';
	private const char BORDER_SYMBOL = '█';
	private const char SNAKE_SYMBOL = '▒';
	private const char SNAKE_HEAD_SYMBOL = '▓';
	private const char FRUIT_SYMBOL = '■';
	private const char BOMB_SYMBOL = '●';

	private const ConsoleColor DEFAULT_COLOR = ConsoleColor.White;
	private const ConsoleColor SNAKE_COLOR = ConsoleColor.Green;
	private const ConsoleColor SNAKE_DEAD_COLOR = ConsoleColor.DarkGreen;
	private const ConsoleColor FRUIT_COLOR = ConsoleColor.Red;
	private const ConsoleColor BOMB_COLOR = ConsoleColor.Yellow;
	private const ConsoleColor BORDER_COLOR = ConsoleColor.Gray;
	private const ConsoleColor SCORE_COLOR = ConsoleColor.DarkCyan;
	private const ConsoleColor PLAYER_DEATH_COLOR = ConsoleColor.DarkRed;
	private const ConsoleColor RESTART_TEXT_COLOR = ConsoleColor.Cyan;
	private const ConsoleColor BOMB_ON_COLOR = ConsoleColor.Yellow;
	private const ConsoleColor BOMB_OFF_COLOR = ConsoleColor.DarkYellow;

	#endregion

	public ConsoleRenderer() 
		=> CursorVisible = false;

	public void Render(Board board)
	{
		System.Console.Clear();
		for (var y = 0; y < board.Height + 2; y++)
			for (var x = 0; x < board.Width + 2; x++)
				if (x == 0 || x == board.Width + 1 ||
				    y == 0 || y == board.Height + 1)
					Print(new Coord(x - 1, y - 1), BORDER_SYMBOL);
	}

	public void Render(Library.Snake snake)
	{
		for (var i = 0; i < 2; i++)
			Print(snake.Coords.ElementAt(i), i == 0 ? SNAKE_HEAD_SYMBOL : SNAKE_SYMBOL, SNAKE_COLOR);
	}

	public void Render(IFruit fruit) 
		=> new ConsoleFruit().Render(this, fruit);

	public void RenderFruit(Coord coord) 
		=> Print(coord, FRUIT_SYMBOL, FRUIT_COLOR);

	public void Render(Coord coord, RenderType renderType)
	{
		switch (renderType)
		{
			case RenderType.SnakeHead: Print(coord, SNAKE_HEAD_SYMBOL, SNAKE_COLOR); break;
			case RenderType.SnakeBody: Print(coord, SNAKE_SYMBOL, SNAKE_COLOR); break;
			case RenderType.Fruit: Print(coord, FRUIT_SYMBOL, FRUIT_COLOR); break;
			case RenderType.Bomb: Print(coord, BOMB_SYMBOL, BOMB_COLOR); break;
			case RenderType.Border: Print(coord, BORDER_SYMBOL, BORDER_COLOR); break;
			case RenderType.Empty: Print(coord, ' ', DEFAULT_COLOR); break;
			default: Print(coord, '?', DEFAULT_COLOR); break;
		};
	}

	public void Render(Coord coord, string text, ColorType colorType) 
		=> Print(coord, text, GetColorByType(colorType));
	protected static void Print(Coord coord, char character, ConsoleColor color = DEFAULT_COLOR)
		=> Print(coord, $"{character}", color);
	protected static void Print(Coord coord, string text, ConsoleColor color = DEFAULT_COLOR)
	{
		var currentColor = ForegroundColor;
		ForegroundColor = color;
		SetCursorPosition(coord.X + 1, coord.Y + 1);
		Write(text);
		ForegroundColor = currentColor;
	}

	public void Clear(Coord coord) 
		=> Print(coord, EMPTY_SYMBOL);

	private static ConsoleColor GetColorByType(ColorType type)
		=> type switch
		{
			ColorType.Default => DEFAULT_COLOR,
			ColorType.Snake => SNAKE_COLOR,
			ColorType.SnakeDead => SNAKE_DEAD_COLOR,
			ColorType.Fruit => FRUIT_COLOR,
			ColorType.BombOn => BOMB_ON_COLOR,
			ColorType.BombOff => BOMB_OFF_COLOR,
			ColorType.Score => SCORE_COLOR,
			ColorType.Wall => DEFAULT_COLOR,
			ColorType.PlayerDeathText => PLAYER_DEATH_COLOR,
			ColorType.RestartText => RESTART_TEXT_COLOR,
			_ => DEFAULT_COLOR
		};
}