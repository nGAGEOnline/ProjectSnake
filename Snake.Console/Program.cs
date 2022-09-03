using Snake.Library;
using Snake.Library.Abstractions;
using Snake.Library.Enums;

// TODO: Consider adding simple sound (Windows only)
// TODO: Add sound-playback to API for other platforms
// TODO: Remove any Console.Writes from anything other than the renderer
while (true)
{
	// TODO: Add Start-Menu
	// TODO: - Simple instructions
	// TODO: Add Settings-Menu for choosing difficulty
	// TODO: -- Settings for customizing key-binds?
	var exit = false;
	var input = new ConsoleInput();
	var renderer = new ConsoleRenderer();
	var gameSettings = new GameSettings(60, 25, Difficulty.Beginner);
	var snakeGame = new SnakeGame(gameSettings, renderer, input);

	// TODO: Add short count-down before start 
	// Game Loop
	await snakeGame.GameLoop();

	// GameOver Screen
	while (!exit)
	{
		var key = Console.ReadKey().Key;
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
