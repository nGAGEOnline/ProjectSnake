using Snake.Console;
using Snake.Library.Enums;

Console.ReadKey();

var game = new ConsoleGame(80, 20, Difficulty.Hard);
await game.Run();


