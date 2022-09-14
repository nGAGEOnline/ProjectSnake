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
	#region CONST SYMBOLS
	
	private const char EMPTY_SYMBOL = ' ';
	private const char WALL_SYMBOL = '█';
	private const char SNAKE_SYMBOL = '▒';
	private const char SNAKE_HEAD_SYMBOL = '▓';
	private const char FRUIT_SYMBOL = '■';
	private const char BOMB_SYMBOL = '█';

	private static readonly char[] BombExplosionCenterSymbols = new char[]{ '█', '▓', '▒', ' '};
	private static readonly char[] BombExplosionSymbols = new char[]{ ' ', '█', '▓', '▒'};
	private const int ANIMATION_DELAY = 300;

	#endregion

	#region CONST COLORS

	private const ConsoleColor DEFAULT_COLOR = ConsoleColor.White;
	private const ConsoleColor DEFAULT_BACKGROUND_COLOR = ConsoleColor.Black;
	private const ConsoleColor SNAKE_COLOR = ConsoleColor.Green;
	private const ConsoleColor SNAKE_DEAD_COLOR = ConsoleColor.DarkGreen;
	private const ConsoleColor FRUIT_COLOR = ConsoleColor.Red;
	private const ConsoleColor WALL_COLOR = ConsoleColor.Gray;
	private const ConsoleColor WALL_DARK_COLOR = ConsoleColor.DarkGray;
	private const ConsoleColor SCORE_COLOR = ConsoleColor.DarkCyan;
	private const ConsoleColor PLAYER_DEATH_COLOR = ConsoleColor.DarkRed;
	private const ConsoleColor RESTART_TEXT_COLOR = ConsoleColor.Cyan;
	private const ConsoleColor BOMB_ON_COLOR = ConsoleColor.Yellow;
	private const ConsoleColor BOMB_OFF_COLOR = ConsoleColor.DarkYellow;

	#endregion

	private Board _board;
	
	public ConsoleRenderer() 
		=> CursorVisible = false;

	public void Render(Board board)
	{
		_board = board;
		System.Console.Clear();
		for (var y = 1; y < board.Height + 3; y++)
			for (var x = 2; x < board.Width + 4; x++)
				if (x == 2 || x == board.Width + 3 ||
				    y == 1 || y == board.Height + 2)
					Print(new Coord(x - 3, y - 2), WALL_SYMBOL, WALL_DARK_COLOR);
	}

	public async Task Render(ISnake snake, bool gameOver)
	{
		var count = gameOver ? snake.Coords.Reverse().Count() : 2;
		for (var i = 0; i < count; i++)
		{
			Print(snake.Coords.ElementAt(i), i == 0 ? SNAKE_HEAD_SYMBOL : SNAKE_SYMBOL, gameOver ? FRUIT_COLOR : SNAKE_COLOR);
			if (gameOver)
				await Task.Delay(40);
		}
	}

	public void Render(IFruit? fruit)
	{
		if (fruit != null)
			Print(fruit.Coord, FRUIT_SYMBOL, FRUIT_COLOR);
	}

	public void Render(IBomb bomb)
		=> Print(bomb.Coord, BOMB_SYMBOL, bomb.IsBlinkOn ? BOMB_ON_COLOR : BOMB_OFF_COLOR);

	public void RenderExplosion(IBomb bomb) 
		=> Explosion(bomb);

	private async Task Explosion(IBomb bomb)
	{
		var explosionCoord = bomb.ExplosionCoords
			.Where(c => c.X >= 0 && c.X <= _board.Width && c.Y >= 0 && c.Y <= _board.Height)
			.ToArray();
		
		for (var i = 0; i < BombExplosionSymbols.Length; i++)
		{
			Render(bomb.Coord, BombExplosionCenterSymbols[i], BOMB_ON_COLOR);
			foreach (var coord in explosionCoord)
				Render(coord, BombExplosionSymbols[i], BOMB_OFF_COLOR);

			await Task.Delay(ANIMATION_DELAY);
		}
		
		Render(bomb.Coord, EMPTY_SYMBOL, BOMB_ON_COLOR);
		foreach (var coord in explosionCoord)
			Render(coord, EMPTY_SYMBOL, BOMB_OFF_COLOR);
	}
	public void Render(ITextPrinter textPrinter)
		=> Print(textPrinter.Coord, $" {textPrinter.TextField.Text} ", 
			GetColorByType(textPrinter.TextField.ForegroundColor), 
			GetColorByType(textPrinter.TextField.BackgroundColor));

	public void Render(Coord coord, char symbol, ConsoleColor color = DEFAULT_COLOR, ConsoleColor bgColor = ConsoleColor.Black) 
		=> Print(coord, symbol, color, bgColor);

	private void Print(Coord coord, char character, ConsoleColor color = DEFAULT_COLOR, ConsoleColor bgColor = ConsoleColor.Black)
		=> Print(coord, $"{character}", color, bgColor);

	private static void Print(Coord coord, string text, ConsoleColor color = DEFAULT_COLOR, ConsoleColor bgColor = ConsoleColor.Black)
	{
		var foregroundColor = ForegroundColor;
		var backgroundColor = BackgroundColor;
		ForegroundColor = color;
		BackgroundColor = bgColor;
		SetCursorPosition(coord.X + 3, coord.Y + 2);
		Write(text);
		ForegroundColor = foregroundColor;
		BackgroundColor = backgroundColor;
	}

	public void Clear(Coord coord) 
		=> Print(coord, EMPTY_SYMBOL);

	public void RenderExplosion()
	{
		throw new NotImplementedException();
	}

	private static ConsoleColor GetColorByType(ColorType type)
		=> type switch
		{
			ColorType.White => DEFAULT_COLOR,
			ColorType.Black => DEFAULT_BACKGROUND_COLOR,
			ColorType.Green => SNAKE_COLOR,
			ColorType.DarkGreen => SNAKE_DEAD_COLOR,
			ColorType.Red => FRUIT_COLOR,
			ColorType.Yellow => BOMB_ON_COLOR,
			ColorType.DarkYellow => BOMB_OFF_COLOR,
			ColorType.DarkCyan => SCORE_COLOR,
			ColorType.Grey => WALL_COLOR,
			ColorType.DarkGrey => WALL_DARK_COLOR,
			ColorType.DarkRed => PLAYER_DEATH_COLOR,
			ColorType.Cyan => RESTART_TEXT_COLOR,
			_ => DEFAULT_COLOR
		};
}