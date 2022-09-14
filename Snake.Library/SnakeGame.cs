using Microsoft.VisualBasic.FileIO;
using Snake.Library.Enums;
using Snake.Library.Interfaces;

namespace Snake.Library;

public class SnakeGame
{
	public static readonly Random Rng = new ();
	
	public static bool CanDie { get; private set; }
	public static bool CanWrap { get; private set; }

	private Board _board;
	private ISnake _snake;
	
	private readonly IInput _input;
	private readonly IRenderer _renderer;
	private readonly Settings _settings;

	private readonly TextPrinter _scoreText;
	private readonly TextPrinter _titleText;

	private int _score = 0;
	private bool _gameOver;

	public SnakeGame(IInput input, IRenderer renderer, Settings settings)
	{
		_input = input;
		_renderer = renderer;
		_settings = settings;
		_board = new Board(_settings.Width, _settings.Height);
		_snake = new Snake(_board);
		
		_titleText = new TextPrinter(_board, new TextField("SnakeGame by nGAGEOnline", ColorType.Green, ColorType.Black), -2);
		_scoreText = new TextPrinter(_board, new TextField($"Score: {_score}", ColorType.DarkGrey, ColorType.Black), _settings.Height);

		_snake.RemoveTail += OnRemoveTail;
		_snake.EatFruit += OnEatFruit;
		_snake.Die += OnDie;

		// Beginner & Easy difficulty allows player to not die when hitting the walls, colliding with the snake still kills the player
		CanDie = _settings.Difficulty != Difficulty.Beginner && _settings.Difficulty != Difficulty.Easy;
		CanWrap = _settings.CanWrap;
	}
	~SnakeGame()
	{
		_snake.RemoveTail -= OnRemoveTail;
		_snake.EatFruit -= OnEatFruit;
		_snake.Die -= OnDie;
	}
	
	public void Reset()
	{
		_board = new Board(_settings.Width, _settings.Height);
		_snake = new Snake(_board);
		_input.Reset();
		_score = 0;
	}
	public async Task GameLoop()
	{
		var delay = _settings.GetDelayByDifficulty();
		_renderer.Render(_board);
		_scoreText.Render(_renderer);
		_titleText.Render(_renderer);
		AddNewFruit();
		BombSpawner();

		while (!_gameOver)
		{
			await Update(delay);
		}
	}

	private async Task Update(int delay)
	{
		_input.Listen();
		_snake.Move(_input.Direction);
		_renderer.Render(_snake);
		
		await Task.Delay(_input.Direction is Direction.Left or Direction.Right ? delay : (int)(delay * 1.6f));
	}

	private async Task BombSpawner()
	{
		while (!_gameOver)
		{
			await Task.Delay(10000);
			_board.SpawnBomb(_renderer);
			await Task.Delay(10000);
		}
	}
	private void OnRemoveTail(Coord coord) 
		=> _renderer.Clear(coord);

	private void OnEatFruit()
	{
		IncreaseScoreByDifficulty();
		_scoreText.Update($"Score: {_score}");
		_scoreText.Render(_renderer);
		AddNewFruit();
	}

	private void OnDie()
	{
		// TODO: Add player-death animation?
		_gameOver = true;
		
		// TODO: Move render-code to a Display/Screen class
		var text = new TextPrinter[]
		{
			new TextPrinter(_board, new TextField("!!! THE SNAKE DIED !!!", ColorType.White, ColorType.DarkRed), _settings.Height / 2 - 2),
			new TextPrinter(_board, new TextField($"TOTAL SCORE: {_score}", ColorType.DarkCyan, ColorType.Black), _settings.Height / 2),
			new TextPrinter(_board, new TextField("Press SPACE To Restart or ESC to Exit", ColorType.Cyan, ColorType.Black), _settings.Height / 2 + 2),
		};
		foreach (var line in text)
			line.Render(_renderer);
	}

	private void AddNewFruit()
	{
		_board.SpawnFruit();
		_renderer.Render(_board.Fruit);
	}

	private void IncreaseScoreByDifficulty() 
		=> _score += _settings.GetPointsByDifficulty();
}