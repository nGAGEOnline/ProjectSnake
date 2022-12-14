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
		public float VerticalSpeedAdjustment { get; set; } = 1.85f;
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
				Difficulty.Beginner => 250,
				Difficulty.Easy => 190,
				Difficulty.Normal => 140,
				Difficulty.Hard => 100,
				Difficulty.Insane => 70,
				Difficulty.Nightmare => 50,
				_ => 300
			};
	}
}
