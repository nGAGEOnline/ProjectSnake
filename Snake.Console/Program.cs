using Snake.Console;
using Snake.Library;
using Snake.Library.Enums;

const int width = 120;
const int height = 30;
Console.SetWindowSize(width + 4,height + 4);
Console.SetBufferSize(width + 4, height + 4);
Console.Title = "SnakeGame (Console)";

// Console.ReadKey();

var settings = new Settings(width, height, Difficulty.Insane, false, 15, false, true);
var game = new ConsoleGame(settings);
await game.Run();
