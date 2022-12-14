using Snake.Library.Interfaces;

namespace Snake.Library
{
	public sealed class SnakeGame
	{
		public const string TITLE = $"{(SnakeSettings.USE_ASYNC ? "[ASYNC] " : "")}SuperSnake (by nGAGEOnline)";

		public bool IsPaused { get; }
		public bool GameOver { get; private set; } = false;
		public bool UltimateWinner { get; private set; }

		public event Action? OnSnakeMoved;
		public event Action<int>? OnScoreChanged;
		public event Action<int>? OnLengthChanged;

		private Grid Grid { get; }
		public Snake Snake { get; }

		private int _score = 0;
		private int _collectedFruits = 0;

		private readonly SnakeSettings _settings;
		private readonly ISnakeGameRenderer _renderer;
		private readonly ISnakeGameInput _input;

		public SnakeGame(SnakeSettings settings, ISnakeGameRenderer renderer, ISnakeGameInput input)
		{
			_settings = settings;
			_renderer = renderer;
			_input = input;
			
			Grid = new Grid(settings);
			Grid.OnGridValueChanged += _renderer.Render;
			Grid.OnSnakeMoved += () => OnSnakeMoved?.Invoke();

			Snake = new Snake(settings, Grid);
			Snake.OnEat += Eat;
			Snake.OnDie += Die;
		}
		~SnakeGame()
		{
			Grid.OnGridValueChanged -= _renderer.Render;
			Grid.OnSnakeMoved -= OnSnakeMoved;
			Snake.OnEat -= Eat;
			Snake.OnDie -= Die;
		}

		public void DrawBorder() => _renderer.RenderBorder();
		public void DrawGrid() => _renderer.RenderGrid();
		public void SpawnFruit() => Grid.SpawnFruit();

		public void Move() => Snake.Move(_input.Direction);
		private void Die() => GameOver = true;
		private void Eat()
		{
			_collectedFruits++;
			_score = _collectedFruits * _settings.GetPointsByDifficulty();
			OnScoreChanged?.Invoke(_score);
			var success = Grid.SpawnFruit();
			if (!success) 
				UltimateWinner = true;
			
			OnLengthChanged?.Invoke(Snake.Length);
		}
	}
}
