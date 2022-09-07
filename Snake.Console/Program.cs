using Snake.Library;
using Snake.Library.Abstractions;
using Snake.Library.Enums;

Console.ReadKey();

Console.ReadKey();
// TODO: Consider adding simple sound (Windows only)
// TODO: Add sound-playback to API for other platforms
// TODO: Remove any Console.Writes from anything other than the renderer
// TODO: Add (at minimum) local leaderboard
while (true)
{
	// TODO: Add Start-Menu
	// TODO: - Simple instructions
	// TODO: Add Settings-Menu for choosing difficulty
	// TODO: -- Settings for customizing key-binds?
	var exit = false;
	var input = new ConsoleInput(); // IInput
	var renderer = new ConsoleRenderer(); // IRenderer
	var settings = new Settings(80, 20, Difficulty.Hard);
	var snakeGame = new SnakeGame(input, renderer, settings);

	// var bomb = new ConsoleBomb(new Coord(5, 5), renderer);
	// bomb.Activate();

	// Game Loop
	// TODO: Add short count-down before start 
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