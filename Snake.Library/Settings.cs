using Snake.Library.Enums;

namespace Snake.Library
{
	[Serializable]
	public struct Settings
	{
		public int Width { get; private set; }
		public int Height { get; private set; }
		public int StartingLength { get; private set; }

		public bool WallKills { get; }
		public bool CanWrap { get; }
		public bool CanEatBomb { get; }
		public bool DynamicDifficulty { get; }

		private Difficulty Difficulty { get; }

		public Settings(int width, int height, Difficulty difficulty, bool dynamicDifficulty = false, int startingLength = 3, bool canWrap = false, bool canEatBomb = false)
		{
			Width = width;
			Height = height;
			Difficulty = difficulty;
			DynamicDifficulty = dynamicDifficulty;
			StartingLength = startingLength;

			CanWrap = canWrap;
			CanEatBomb = canEatBomb;

			// Beginner & Easy difficulty allows player to not die when hitting the walls, colliding with the snake or bomb-explosions still kills the player
			WallKills = Difficulty != Difficulty.Beginner && Difficulty != Difficulty.Easy;
		}

		public readonly int GetPointsByDifficulty()
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

		public readonly int GetDelayByDifficulty()
			=> Difficulty switch
			{
				Difficulty.Beginner => 230,
				Difficulty.Easy => 170,
				Difficulty.Normal => 120,
				Difficulty.Hard => 80,
				Difficulty.Insane => 50,
				Difficulty.Nightmare => 30,
				_ => 300
			};
	}
}