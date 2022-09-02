using Snake.Library.Abstractions;
using Snake.Library.Enums;
using Snake.Library.Interfaces;

namespace Snake.Library;

public class SnakeGame
{
	public static readonly Random Rng = new ();
	public static bool CanDie { get; private set; } = true;
	public static bool ShowDebugData { get; } = false;

	public bool GameOver { get; private set; } = false;

	private readonly IInput _input;
	private readonly IBoard _board;
	private readonly ISnake _snake;
	private readonly IRenderer _renderer;

	private int _score = 0;
	private bool _pause;

	public SnakeGame(IInput input, IRenderer renderer, int width = 20, int height = 20)
	{
		_board = new Board(width, height);
		_snake = new Snake(_board);
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
		_renderer.Render(_board);
		AddNewFruit();
		UpdateScore();
	}

	private async Task Update()
	{
		_input.Listen();
		_snake.Move(_input.Direction);
		_renderer.Render(_snake);
	}

	public async Task GameLoop()
	{
		while (!GameOver)
		{
			if (_pause)
				continue;
			
			await Update();
			await Task.Delay(_input.Direction is Direction.Left or Direction.Right ? 100 : 150);
		}
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

	private void OnDie()
	{
		GameOver = true;
		var text = "!!! THE SNAKE DIED !!!";
		_renderer.RenderText(new Coord(_board.Width / 2 - text.Length / 2, _board.Height / 2 - 2), text, MessageType.PlayerDeath);
		text = $"TOTAL SCORE: {_score}";
		_renderer.RenderText(new Coord(_board.Width / 2 - text.Length / 2, _board.Height / 2), text, MessageType.Score);
		text = "Press SPACE To Restart or ESC to Exit";
		_renderer.RenderText(new Coord(_board.Width / 2 - text.Length / 2, _board.Height / 2 + 2), text, MessageType.Restart);
	}

	private void OnEatFruit()
	{
		_score++;
		UpdateScore();
		AddNewFruit();
	}

	private void UpdateScore()
	{
		var text = $"Score: {_score}";
		var x = _board.Width / 2 - text.Length / 2;
		var y = _board.Height + 1;

		_renderer.RenderText(new Coord(x, y), text, MessageType.Score);
	}
	private void AddNewFruit()
	{
		_board.AddFruit();
		_renderer.RenderFruit(_board.Fruit);
	}
}
