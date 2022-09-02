using Snake.Library.Abstractions;
using Snake.Library.Enums;
using Snake.Library.Interfaces;

namespace Snake.Library;

public class SnakeGame
{
	public static readonly Random Rng = new ();
	public static bool CanDie { get; private set; } = true;
	public static bool IsDebugMode { get; private set; }

	private readonly IInput _input;
	private readonly IBoard _board;
	private readonly ISnake _snake;
	private readonly IRenderer _renderer;
	private readonly GameSettings _settings;

	private int _score = 0;
	private bool _gameOver;
	private int _delay;

	public SnakeGame(GameSettings settings, IRenderer renderer, IInput input)
	{
		_board = new Board(settings.Width, settings.Height);
		_snake = new Snake(_board);
		_settings = settings;
		_input = input;
		_renderer = renderer;
		
		_snake.DebugDataPositions += OnDebugDataPositions;
		_snake.RemoveTail += OnRemoveTail;
		_snake.EatFruit += OnEatFruit;
		_snake.Die += OnDie;
	}
	~SnakeGame()
	{
		_snake.DebugDataPositions -= OnDebugDataPositions;
		_snake.RemoveTail -= OnRemoveTail;
		_snake.EatFruit -= OnEatFruit;
		_snake.Die -= OnDie;
	}

	public void Init()
	{
		IsDebugMode = _settings.DebugMode;
		_delay = GetDelayByDifficulty(_settings.Difficulty);
		_renderer.Render(_board);
		
		AddNewFruit();
		RenderScore(_score);
	}

	public async Task GameLoop()
	{
		while (!_gameOver)
		{
			await Update();
			await Task.Delay(_input.Direction is Direction.Left or Direction.Right ? _delay : (int)(_delay * 1.5f));
		}
	}

	private async Task Update()
	{
		_input.Listen();
		_snake.Move(_input.Direction);
		_renderer.Render(_snake);
	}

	private static int GetDelayByDifficulty(Difficulty difficulty)
	{
		// Beginner & Easy difficulty allows player to not die when hitting the walls
		// However, colliding with the snake still kills the player
		CanDie = difficulty != Difficulty.Beginner && difficulty != Difficulty.Easy;
		
		return difficulty switch
		{
			Difficulty.Beginner => 200,
			Difficulty.Easy => 150,
			Difficulty.Normal => 100,
			Difficulty.Hard => 60,
			Difficulty.Insane => 40,
			_ => throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null)
		};
	}
	private void OnRemoveTail(Coord coord) 
		=> _renderer.Clear(coord);

	private void OnEatFruit()
	{
		RenderScore(IncreaseScore());
		AddNewFruit();
	}

	private int IncreaseScore()
	{
		_score += _settings.Difficulty switch
		{
			Difficulty.Beginner => 1,
			Difficulty.Easy => 2,
			Difficulty.Normal => 3,
			Difficulty.Hard => 4,
			Difficulty.Insane => 5,
			_ => 0
		};
		return _score;
	}
	private void AddNewFruit()
	{
		_board.AddFruit();
		_renderer.RenderFruit(_board.Fruit);
	}

	private void OnDebugDataPositions(Coord head, Direction direction, Coord nextCoord)
	{
		var text = $"Head: [{head.X}, {head.Y}]\n" +
		           $" Direction: {direction}\n" +
		           $" Next Coord: [{nextCoord.X}, {nextCoord.Y}]";
		_renderer.RenderText(new Coord(0, _board.Height + 4), text, MessageType.DebugPositions);
	}

	private void OnDie()
	{
		_gameOver = true;
		
		var text = "!!! THE SNAKE DIED !!!";
		_renderer.RenderText(new Coord(_board.Width / 2 - text.Length / 2, _board.Height / 2 - 2), text, MessageType.PlayerDeath);
		text = $"TOTAL SCORE: {_score}";
		_renderer.RenderText(new Coord(_board.Width / 2 - text.Length / 2, _board.Height / 2), text, MessageType.Score);
		text = "Press SPACE To Restart or ESC to Exit";
		_renderer.RenderText(new Coord(_board.Width / 2 - text.Length / 2, _board.Height / 2 + 2), text, MessageType.Restart);
	}

	private void RenderScore(int score)
	{
		var text = $"Score: {score}";
		var x = _board.Width / 2 - text.Length / 2;
		var y = _board.Height + 1;

		_renderer.RenderText(new Coord(x, y), text, MessageType.Score);
	}
}

