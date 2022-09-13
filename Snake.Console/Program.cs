using Snake.Console;
using Snake.Library;
using Snake.Library.Enums;

Console.ReadKey();

var settings = new Settings(80, 20, Difficulty.Nightmare, 5, true);
var game = new ConsoleGame(settings);
await game.Run();
