using Snake.Library.Enums;

namespace Snake.Library;

public class GameSettings
{
	public int Width { get; }
	public int Height { get; }
	
	public Difficulty Difficulty { get; }
	public bool DebugMode { get; }

	public GameSettings(int width, int height, Difficulty difficulty, bool debugMode = false)
	{
		Difficulty = difficulty;
		DebugMode = debugMode;
		Width = width;
		Height = height;
	}
}
