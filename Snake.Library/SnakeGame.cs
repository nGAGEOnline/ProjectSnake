﻿using Snake.Library.Abstractions;
using Snake.Library.Enums;
using Snake.Library.Interfaces;

namespace Snake.Library;

public class SnakeGame
{
	public static readonly Random Rng = new ();
	
	public static bool CanDie { get; private set; } = true;

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
		_snake = new Snake(_board, settings.StartingLength);
		_settings = settings;
		_input = input;
		_renderer = renderer;
		
		_snake.RemoveTail += OnRemoveTail;
		_snake.EatFruit += OnEatFruit;
		_snake.Die += OnDie;

		Init();
	}
	~SnakeGame()
	{
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

		_renderer.Render(_board);

		AddNewFruit();
		UpdateScore(_score);
	}

	public async Task GameLoop()
	{
		var delay = _settings.GetDelayByDifficulty();
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
		
		await Task.Delay(_input.Direction is Direction.Left or Direction.Right ? delay : (int)(delay * 1.5f));
	}

	private void OnRemoveTail(Coord coord) 
		=> _renderer.Clear(coord);

	private void OnEatFruit()
	{
		IncreaseScoreByDifficulty();
		UpdateScore(_score);
		AddNewFruit();
	}

	private void OnDie()
	{
		// TODO: Add player-death animation?
		_gameOver = true;
		
		// TODO: Move render-code to a Display/Screen class
		var text = "!!! THE SNAKE DIED !!!";
		_renderer.Render(new Coord(_board.Width / 2 - text.Length / 2, _board.Height / 2 - 2), text, ColorType.PlayerDeathText);
		text = $"TOTAL SCORE: {_score}";
		_renderer.Render(new Coord(_board.Width / 2 - text.Length / 2, _board.Height / 2), text, ColorType.Score);
		text = "Press SPACE To Restart or ESC to Exit";
		_renderer.Render(new Coord(_board.Width / 2 - text.Length / 2, _board.Height / 2 + 2), text, ColorType.RestartText);
	}

	private void UpdateScore(int score)
	{
		var text = $"Score: {score}";
		var x = _board.Width / 2 - text.Length / 2;
		var y = _board.Height + 1;

		_renderer.Render(new Coord(x, y), text, ColorType.Score);
	}

	private void AddNewFruit()
	{
		_board.SpawnFruit();
		_renderer.RenderFruit(_board.FruitCoord);
	}

	private void IncreaseScoreByDifficulty() 
		=> _score += _settings.GetPointsByDifficulty();
}