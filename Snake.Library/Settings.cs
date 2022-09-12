using Snake.Library.Enums;

namespace Snake.Library;

public struct Settings
{
	public int Width { get; private set; }
	public int Height { get; private set; }
	public int StartingLength { get; private set; }
	
	public Difficulty Difficulty { get; private set; }

	public Settings(int width, int height, Difficulty difficulty, int startingLength = 3)
	{
		Width = width;
		Height = height;
		Difficulty = difficulty;
		StartingLength = startingLength;
	}

	public readonly int GetPointsByDifficulty()
		=> Difficulty switch
		{
			Difficulty.Beginner => 1,
			Difficulty.Easy => 2,
			Difficulty.Normal => 3,
			Difficulty.Hard => 4,
			Difficulty.Insane => 5,
			_ => 0
		};

	public readonly int GetDelayByDifficulty() 
		=> Difficulty switch
		{
			Difficulty.Beginner => 200,
			Difficulty.Easy => 150,
			Difficulty.Normal => 100,
			Difficulty.Hard => 60,
			Difficulty.Insane => 40,
			_ => 500
		};
}
