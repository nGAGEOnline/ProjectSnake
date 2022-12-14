using Snake.Library.Enums;

namespace Snake.Library
{
	[Serializable]
	public class SnakeSettings
	{
		public const bool USE_ASYNC = true;
		
		public int Width;
		public int Height;
		public int StartingLength = 4;
		public Difficulty Difficulty;

		public bool WallKills => Difficulty != Difficulty.Beginner && Difficulty != Difficulty.Easy;
		public float VerticalSpeedAdjustment { get; set; } = 1.75f;
		public bool CanWrap = true;

		public SnakeSettings(int width, int height, Difficulty difficulty = Difficulty.Insane)
		{
			Width = width;
			Height = height;
			Difficulty = difficulty;
		}
		
		public int GetPointsByDifficulty()
		{
			return Difficulty switch
			{
				Difficulty.Beginner => 1,
				Difficulty.Easy => 2,
				Difficulty.Normal => 3,
				Difficulty.Hard => 4,
				Difficulty.Insane => 5,
				Difficulty.Nightmare => 6,
				_ => 0
			};
		}

		public int GetDelayByDifficulty()
			=> Difficulty switch
			{
				Difficulty.Beginner => 150,
				Difficulty.Easy => 125,
				Difficulty.Normal => 100,
				Difficulty.Hard => 80,
				Difficulty.Insane => 60,
				Difficulty.Nightmare => 40,
				_ => 200
			};
	}
}
