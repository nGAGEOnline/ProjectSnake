using Snake.Library;
using Snake.Library.Abstractions;

while (true)
{
	var exit = false;
	var input = new ConsoleInput();
	var renderer = new ConsoleRenderer();
	var snakeGame = new SnakeGame(input, renderer, 60, 25);
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
