using System;
using System.Threading.Tasks;
using Snake.Library.Enums;
using Snake.Library.Interfaces;

namespace Snake.Library
{
	public sealed class SnakeGame
	{
		public static bool WallKills { get; private set; }
		public static bool CanWrap { get; private set; }
		public static bool CanEatBomb { get; private set; }

		private Board _board;
		private ISnake _snake;

		private readonly IInput _input;
		private readonly IRenderer _renderer;
		private readonly Settings _settings;

		private readonly TextPrinter _scoreText;
		private readonly TextPrinter _titleText;

		private int _score = 0;
		private int _fruitsCollected = 0;
		private bool _gameOver;

		public SnakeGame(IInput input, IRenderer renderer, Settings settings)
		{
			_input = input;
			_renderer = renderer;
			_settings = settings;
			_board = new Board(_settings.Width, _settings.Height);
			_snake = new Snake(_board, _settings.StartingLength);

			_titleText = new TextPrinter(_board, new TextField("SnakeGame by nGAGEOnline", ColorType.Green, ColorType.Black), -2);
			_scoreText = new TextPrinter(_board, new TextField($"Score: {_score}", ColorType.DarkGrey, ColorType.Black), _settings.Height);

			_snake.RemoveTail += OnRemoveTail;
			_snake.EatFruit += OnEatFruit;
			_snake.EatBomb += OnEatBomb;
			_snake.Die += OnDie;
			_board.OnExplosion += _snake.CheckForDamage;

			WallKills = _settings.WallKills;
			CanWrap = _settings.CanWrap;
			CanEatBomb = _settings.CanEatBomb;
		}

		~SnakeGame()
		{
			_snake.RemoveTail -= OnRemoveTail;
			_snake.EatFruit -= OnEatFruit;
			_snake.EatBomb -= OnEatBomb;
			_snake.Die -= OnDie;
			_board.OnExplosion -= _snake.CheckForDamage;
		}

		public void Reset()
		{
			_board = new Board(_settings.Width, _settings.Height);
			_snake = new Snake(_board);
			_input.Reset();
			_fruitsCollected = 0;
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
			await _renderer.Render(_snake, _gameOver);

			await Task.Delay(_input.Direction is Direction.Left or Direction.Right ? delay : (int)(delay * 1.6f));
		}

		private async Task BombSpawner()
		{
			var bombSpawningDelay = 15000;
			while (!_gameOver)
			{
				await Task.Delay(bombSpawningDelay);
				if (_gameOver)
					return;

				_board.SpawnBomb(_renderer);

				bombSpawningDelay -= 1500;
				bombSpawningDelay = Math.Clamp(bombSpawningDelay, 3000, 15000);
			}
		}

		private void OnRemoveTail(Coord coord)
			=> _renderer.Clear(coord);

		private void OnEatBomb()
		{
			if (_board.Bomb != null)
				_board.ClearBombFromGrid(_board.Bomb);
		}

		private void OnEatFruit()
		{
			IncreaseScoreByDifficulty();
			_scoreText.Update($"Score: {_score}");
			_scoreText.Render(_renderer);
			AddNewFruit();
		}

		private async Task OnDie()
		{
			// TODO: Add player-death animation?
			await _renderer.Render(_snake, _gameOver);
			_gameOver = true;

			// TODO: Move render-code to a Display/Screen class
			var text = new TextPrinter[]
			{
				new(_board, new TextField("!!! THE SNAKE DIED !!!", ColorType.White, ColorType.DarkRed), _settings.Height / 2 - 2),
				new(_board, new TextField($"TOTAL SCORE: {_score}", ColorType.DarkCyan, ColorType.Black), _settings.Height / 2),
				new(_board, new TextField("Press SPACE To Restart or ESC to Exit", ColorType.Cyan, ColorType.Black), _settings.Height / 2 + 2),
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
		{
			var points = _settings.GetPointsByDifficulty();
			if (_settings.DynamicDifficulty)
				points *= 2;

			_score += points;
			_fruitsCollected++;
		}
	}
}