using Snake.Console.Game;
using Snake.Library;
using Snake.Library.Enums;

namespace Snake.Console;

public static class GameLauncher
{
	private const bool USE_ASYNC = true;

	public static async Task Start()
	{
		var settings = new SnakeSettings(80, 25, Difficulty.Hard);
		var game = new ConsoleSnakeGame(settings);
		game.SetupGame();

		// =================================================

		var refreshDelay = (int)(settings.GetDelayByDifficulty() * 0.5f);
		if (USE_ASYNC)
			await game.PlayAsync(refreshDelay);
		else
			game.Play(refreshDelay);

		System.Console.ReadKey();
	}
}
