using System.Runtime.InteropServices;
using Snake.Console.Game.UI;
using Snake.Library;
using Snake.Library.Enums;
using Snake.Library.Interfaces;
using Snake.Library.Interfaces.UI;
using Snake.Library.Structs;
using static System.Console;

namespace Snake.Console.Game;

public class ConsoleSnakeGame : ISnakeGame
{
	public SnakeGame Game { get; private set; }
	public IInputProvider InputProvider { get; private set; }
	public IGameRenderer Renderer { get; private set; }

	private ITextField? CoordText { get; set; }
	private ITextField? TitleText { get; set; }
	private ITextField? ScoreText { get; set; }
	private ITextField? MovesCountText { get; set; }
	private ITextField? LengthText { get; set; }
	private ITextField? DifficultyText { get; set; }
	private ITextField? MoveDirectionText { get; set; }

	public event Action<int>? OnScoreChanged;
	public event Action? OnSnakeMoved;

	private int _moves = 0;
	private readonly SnakeSettings _settings;
	private string SnakeCoordText => $"{(Game.Snake.Coord).ToString()}";

	public ConsoleSnakeGame(SnakeSettings settings)
	{
		_settings = settings;

		Title = SnakeGame.TITLE;
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
		{
			SetWindowSize(settings.Width + 4, settings.Height + 5);
			SetBufferSize(settings.Width + 4, settings.Height + 5);
		}
		InputProvider = new ConsoleInputProvider();
		Renderer = new ConsoleRenderer(_settings);

		Game = new SnakeGame(_settings, Renderer, InputProvider);
		Game.OnLengthChanged += UpdateSnakeLength;
		Game.OnScoreChanged += (score) => OnScoreChanged?.Invoke(score);
		Game.OnScoreChanged += UpdateScore;
		Game.OnSnakeMoved += SnakeMoved;
	}
	~ConsoleSnakeGame()
	{
		Game.OnLengthChanged -= UpdateSnakeLength;
		Game.OnScoreChanged -= OnScoreChanged;
		Game.OnScoreChanged -= UpdateScore;
		Game.OnSnakeMoved -= SnakeMoved;
	}

	public void SetupGame()
	{
		Game.DrawBorder();
		Game.DrawGrid();
		Game.SpawnFruit();
		SetupTextFields();
	}

	private void SetupTextFields()
	{
		const string scoreText = "Score:";
		const string movesText = "Steps:";
		const string lengthText = "Length:";
		const string directionPrefixText = "Direction:";
		var difficultyText = $"Difficulty: {_settings.Difficulty.ToString()}";
		var directionText = $"{difficultyText} {DifficultyText}";
		
		CoordText = new TextField(new Coord((_settings.Width / 2 - SnakeCoordText.Length / 2) - 2, _settings.Height), SnakeCoordText, ObjectType.Text);
		TitleText = new TextField(new Coord((_settings.Width / 2 - SnakeGame.TITLE.Length / 2) - 2, -2), SnakeGame.TITLE, ObjectType.Text);
		ScoreText = new TextField(new Coord((_settings.Width / 2 - (scoreText.Length + 2) /2) - 2, _settings.Height + 1), scoreText, "0", ObjectType.Text);
		LengthText = new TextField(new Coord(0, -2), lengthText, Game.Snake.Length.ToString(), ObjectType.Text);
		MovesCountText = new TextField(new Coord(0, _settings.Height + 1), movesText, "0", ObjectType.Text);
		DifficultyText = new TextField(new Coord(_settings.Width - (difficultyText.Length + 1) - 2, _settings.Height), difficultyText, ObjectType.Text);
		MoveDirectionText = new TextField(new Coord(_settings.Width - directionText.Length, _settings.Height + 1), directionPrefixText, $"{InputProvider.Direction}", ObjectType.Text);
		
		Renderer.Render(ScoreText, TextStyle.Score);
		Renderer.Render(TitleText, TextStyle.Title);
		Renderer.Render(MovesCountText, TextStyle.Moves);
		Renderer.Render(LengthText, TextStyle.Length);
		Renderer.Render(DifficultyText, TextStyle.Difficulty);
	}

	public async Task PlayAsync(int refreshDelay)
	{
		while (!Game.GameOver)
		{
			await Task.Run(() => InputProvider.Listen());
			await Task.Run(() => Game.Move());
			await Task.Delay(GetDirectionAdjustedDelay(refreshDelay));
		}
	}

	public void Play(int refreshDelay)
	{
		while (!Game.GameOver)
		{
			InputProvider.Listen();
			Game.Move();
			Thread.Sleep(GetDirectionAdjustedDelay(refreshDelay));
		}
	}

	private void SnakeMoved()
	{
		UpdateCoordText();
		UpdateDirectionText();
		UpdateMovesCountText();
		OnSnakeMoved?.Invoke();
	}

	private void UpdateDirectionText()
	{
		if (MoveDirectionText is null)
			return;
		var text = $"{InputProvider.Direction} ";
		if (InputProvider.Direction == Direction.Up)
			text += "  ";
		MoveDirectionText.UpdateText($"{text}");
		Renderer.Render(MoveDirectionText, TextStyle.Moves);
	}
	private void UpdateMovesCountText()
	{
		if (MovesCountText is null)
			return;
		MovesCountText.UpdateText($"{++_moves}");
		Renderer.Render(MovesCountText, TextStyle.Moves);
	}
	private void UpdateCoordText()
	{
		if (CoordText is null)
			return;
		CoordText.UpdateText($"{SnakeCoordText}");
		Renderer.Render(CoordText, TextStyle.Coords);
	}
	private void UpdateScore(int score)
	{
		if (ScoreText is null)
			return;
		ScoreText.UpdateText($"{score}");
		Renderer.Render(ScoreText, TextStyle.Score);
	}
	private void UpdateSnakeLength(int length)
	{
		if (LengthText is null)
			return;
		LengthText.UpdateText($"{length}");
		Renderer.Render(LengthText, TextStyle.Length);
	}

	public void Reset() 
		=> Game = new SnakeGame(_settings, Renderer, InputProvider);

	private int GetDirectionAdjustedDelay(int baseDelay) 
		=> InputProvider.Direction is Direction.Up or Direction.Down 
			? (int)(baseDelay * _settings.VerticalSpeedAdjustment) 
			: baseDelay;
}
