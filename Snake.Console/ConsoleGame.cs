using Snake.Console.Abstractions;
using Snake.Library;
using Snake.Library.Abstractions;
using Snake.Library.Enums;

namespace Snake.Console;

public class ConsoleGame
{
	private readonly int _width;
	private readonly int _height;

	public ConsoleGame(int width, int height)
	{
		_width = width;
		_height = height;
	}
	
	public async Task Run()
	{
		// TODO: Add Start-Menu
		// TODO: - Simple instructions
		// TODO: Add Settings-Menu for choosing difficulty
		// TODO: -- Settings for customizing key-binds?
		//
		// TODO: Remove any Console.Writes from anything other than the renderer
		// TODO: Add (at minimum) local leaderboard
		// TODO: Consider adding simple sound (Windows only)
		// TODO: Add sound-playback to API for other platforms
		while (true)
		{
			var exit = false;
			var input = new ConsoleInput(); // IInput
			var renderer = new ConsoleRenderer(); // IRenderer
			var settings = new Settings(_width, _height, Difficulty.Hard);
			var snakeGame = new SnakeGame(input, renderer, settings);

			// Game Loop
			// TODO: Add short count-down before start 
			await snakeGame.GameLoop();

			// GameOver Screen
			while (!exit)
			{
				var key = System.Console.ReadKey().Key;
				switch (key)
				{
					case ConsoleKey.Spacebar:
						exit = true;
						break;
					case ConsoleKey.Escape:
						return;
				}
			}
			// TODO: Add credit screen
		}
	}
}