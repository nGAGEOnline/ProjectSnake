using Snake.Console.Abstractions;
using Snake.Library;
using Snake.Library.Abstractions;
using Snake.Library.Enums;
using Snake.Library.Interfaces;

namespace Snake.Console;

public class ConsoleGame
{
	private readonly IInput _input;
	private readonly IRenderer _renderer;
	private readonly Settings _settings;

	public ConsoleGame(int width, int height, Difficulty difficulty, int startingLength = 5)
	{
		_input = new ConsoleInput(); // IInput
		_renderer = new ConsoleRenderer(); // IRenderer
		_settings = new Settings(width, height, difficulty, startingLength);
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
			var snakeGame = new SnakeGame(_input, _renderer, _settings);

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