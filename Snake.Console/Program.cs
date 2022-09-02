using Snake.Library;
using Snake.Library.Abstractions;
using Snake.Library.Enums;

while (true)
{
	var exit = false;
	var input = new ConsoleInput();
	var renderer = new ConsoleRenderer();
	var gameSettings = new GameSettings(60, 25, Difficulty.Beginner);
	var snakeGame = new SnakeGame(gameSettings, renderer, input);
	snakeGame.Init();

	await snakeGame.GameLoop();

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
}
