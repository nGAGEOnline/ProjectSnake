using Snake.Library.Enums;

namespace Snake.Library;

public struct Settings
{
	public int Width { get; private set; }
	public int Height { get; private set; }
	public int StartingLength { get; private set; }
	public bool CanWrap { get; set; }
	
	public Difficulty Difficulty { get; private set; }

	public Settings(int width, int height, Difficulty difficulty, int startingLength = 3, bool canWrap = false)
	{
		Width = width;
		Height = height;
		Difficulty = difficulty;
		StartingLength = startingLength;
		CanWrap = canWrap;
	}

	public readonly int GetPointsByDifficulty()
		=> Difficulty switch
		{
			Difficulty.Beginner => 1,
			Difficulty.Easy => 2,
			Difficulty.Normal => 3,
			Difficulty.Hard => 4,
			Difficulty.Insane => 5,
			Difficulty.Nightmare => 6,
			_ => 0
		};

	public readonly int GetDelayByDifficulty() 
		=> Difficulty switch
		{
			Difficulty.Beginner => 200,
			Difficulty.Easy => 150,
			Difficulty.Normal => 110,
			Difficulty.Hard => 80,
			Difficulty.Insane => 60,
			Difficulty.Nightmare => 40,
			_ => 300
		};
}
