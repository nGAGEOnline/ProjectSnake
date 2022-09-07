using Snake.Console;
using Snake.Library.Enums;

var game = new ConsoleGame(80, 20, Difficulty.Hard);
await game.Run();