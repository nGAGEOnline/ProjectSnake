using Snake.Library.Enums;
using Snake.Library.Interfaces;

namespace Snake.Library.Abstractions;

public enum MessageType
{
	Score,
	DebugPositions,
	PlayerDeath,
	Restart
}
public class ConsoleRenderer : IRenderer
{
	#region CONSTS

	private const char EMPTY_SYMBOL = ' ';
	private const char BORDER_SYMBOL = '█';
	private const char SNAKE_SYMBOL = '▒';
	private const char SNAKE_HEAD_SYMBOL = '▓';
	private const char FRUIT_SYMBOL = '■';
	private const ConsoleColor DEFAULT_COLOR = ConsoleColor.White;
	private const ConsoleColor SNAKE_COLOR = ConsoleColor.Green;
	private const ConsoleColor DEAD_SNAKE_COLOR = ConsoleColor.DarkGreen;
	private const ConsoleColor FRUIT_COLOR = ConsoleColor.Red;
	private const ConsoleColor SCORE_COLOR = ConsoleColor.Yellow;
	private const ConsoleColor POSITIONS_COLOR = ConsoleColor.DarkCyan;
	private const ConsoleColor PLAYER_DEATH_COLOR = ConsoleColor.DarkRed;

	#endregion

	public ConsoleRenderer() 
		=> Console.CursorVisible = false;

	public void Render(IBoard board)
	{
		Console.Clear();
		for (var y = 0; y < board.Height + 2; y++)
		{
			for (var x = 0; x < board.Width + 2; x++)
				if (x == 0 || x == board.Width + 1 ||
				    y == 0 || y == board.Height + 1)
					Print(new Coord(x - 1, y - 1), BORDER_SYMBOL);
			
			if (SnakeGame.IsDebugMode && y > 0 && y <= board.Height)
				Print(new Coord(board.Width + 8 - (y - 1).ToString().Length, y - 1), (y - 1).ToString());
		}
	}

	public void Render(ISnake snake)
	{
		var points = snake.Coords.ToList();
		for (var i = 0; i < points.Count; i++)
			Print(points[i], i == 0 ? SNAKE_HEAD_SYMBOL : SNAKE_SYMBOL, SNAKE_COLOR);
	}

	public void RenderFruit(Coord coord) 
		=> Print(coord, FRUIT_SYMBOL, FRUIT_COLOR);

	public void RenderText(Coord coord, string text, MessageType messageType) 
		=> Print(coord, text, messageType switch{
			MessageType.Score => SCORE_COLOR,
			MessageType.DebugPositions => POSITIONS_COLOR,
			MessageType.PlayerDeath => PLAYER_DEATH_COLOR,
			MessageType.Restart => DEFAULT_COLOR,
			_ => throw new ArgumentOutOfRangeException(nameof(messageType), messageType, null)
		});

	private static void Print(Coord coord, char character, ConsoleColor color = DEFAULT_COLOR)
		=> Print(coord, $"{character}", color);
	private static void Print(Coord coord, string text, ConsoleColor color = DEFAULT_COLOR)
	{
		var currentColor = Console.ForegroundColor;
		Console.ForegroundColor = color;
		Console.SetCursorPosition(coord.X + 1, coord.Y + 1);
		Console.Write(text);
		Console.ForegroundColor = currentColor;
	}

	public void Clear(Coord coord) 
		=> Print(coord, EMPTY_SYMBOL);
}
