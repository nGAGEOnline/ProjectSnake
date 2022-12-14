namespace Snake.Library.Interfaces
{
	public interface ISnakeGame
	{
		SnakeGame Game { get; }
		ISnakeGameInput Input { get; }
		ISnakeGameRenderer Renderer { get; }

		event Action<int>? OnScoreChanged;
		
		void SetupGame();
		void Play(int refreshDelay);
		Task PlayAsync(int refreshDelay);
		void Reset();
	}
}
