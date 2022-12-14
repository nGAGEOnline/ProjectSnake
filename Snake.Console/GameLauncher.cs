using Snake.Console.Game;
using Snake.Library;
using Snake.Library.Enums;

namespace Snake.Console;

public static class GameLauncher
{
	public static async Task Start(bool useAsync = true)
	{
		var settings = new SnakeSettings(80, 25, Difficulty.Hard);
		var game = new ConsoleSnakeGame(settings);
		game.SetupGame();

		// =================================================

		var refreshDelay = settings.GetDelayByDifficulty();
		if (useAsync)
			await game.PlayAsync(refreshDelay);
		else
			game.Play(refreshDelay);
	}
}
