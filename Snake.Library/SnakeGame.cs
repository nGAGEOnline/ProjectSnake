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
	private readonly Settings _settings;

	private int _score = 0;
	private bool _gameOver;

	public SnakeGame(IInput input, IRenderer renderer, Settings settings)
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

		Init();
	}
	~SnakeGame()
	{
		_snake.DebugDataPositions -= OnDebugDataPositions;
		_snake.RemoveTail -= OnRemoveTail;
		_snake.EatFruit -= OnEatFruit;
		_snake.Die -= OnDie;
	}

	private void Init()
	{
		// TODO: Add random, timed bombs (5sec timer) for Insane difficulty
		// TODO: Consider adding Nightmare difficulty
		// TODO: Consider adding move-speed acceleration on higher difficulty (upto a max speed)
		// Beginner & Easy difficulty allows player to not die when hitting the walls, colliding with the snake still kills the player
		CanDie = _settings.Difficulty != Difficulty.Beginner && _settings.Difficulty != Difficulty.Easy;
		IsDebugMode = _settings.DebugMode;

		_renderer.Render(_board);

		AddNewFruit();
		RenderScore(_score);
	}

	public async Task GameLoop()
	{
		var delay = _settings.GetDelayByDifficulty();
		while (!_gameOver)
		{
			await Update();
			await Task.Delay(_input.Direction is Direction.Left or Direction.Right ? delay : (int)(delay * 1.5f));
		}
	}

	private async Task Update()
	{
		_input.Listen();
		_snake.Move(_input.Direction);
		_renderer.Render(_snake);
	}

	private void OnRemoveTail(Coord coord) 
		=> _renderer.Clear(coord);

	private void OnDebugDataPositions(Coord head, Direction direction, Coord nextCoord)
	{
		var text = $"Head: [{head.X}, {head.Y}]\n" +
		           $" Direction: {direction}\n" +
		           $" Next Coord: [{nextCoord.X}, {nextCoord.Y}]";
		_renderer.RenderText(new Coord(0, _board.Height + 4), text, MessageType.DebugPositions);
	}

	private void OnEatFruit()
	{
		IncreaseScoreByDifficulty();
		RenderScore(_score);
		AddNewFruit();
	}

	private void OnDie()
	{
		// TODO: Add player-death animation?
		_gameOver = true;
		
		// TODO: Move render-code to a Display/Screen class
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

	private void AddNewFruit()
	{
		_board.AddFruit();
		_renderer.RenderFruit(_board.Fruit);
	}

	private void IncreaseScoreByDifficulty() 
		=> _score += _settings.GetPointsByDifficulty();

}
