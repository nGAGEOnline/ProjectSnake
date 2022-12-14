using Snake.Library.Enums;
using Snake.Library.Interfaces.UI;

namespace Snake.Console.Game.UI;

public class TextStyle : ITextStyle<ConsoleColor> 
{
	#region STATICS

	public static ITextStyle<ConsoleColor> Default { get; }
	public static ITextStyle<ConsoleColor> Border { get; }
	public static ITextStyle<ConsoleColor> Grid { get; }
	public static ITextStyle<ConsoleColor> Snake { get; }
	public static ITextStyle<ConsoleColor> Fruit { get; }
	public static ITextStyle<ConsoleColor> Bomb { get; }
	public static ITextStyle<ConsoleColor> Score { get; }
	public static ITextStyle<ConsoleColor> Title { get; }
	public static ITextStyle<ConsoleColor> Moves { get; }
	public static ITextStyle<ConsoleColor> Length { get; }
	public static ITextStyle<ConsoleColor> Difficulty { get; }
	public static ITextStyle<ConsoleColor> Coords { get; }

	#endregion

	public ConsoleColor Foreground { get; }
	public ConsoleColor Background { get; }
	public ObjectType ObjectType { get; }

	public TextStyle(ConsoleColor foreground, ConsoleColor background, ObjectType objectType = ObjectType.Text)
	{
		Foreground = foreground;
		Background = background;
		ObjectType = objectType;
	}
	static TextStyle()
	{
		Default = new TextStyle(ConsoleColor.Gray, ConsoleColor.Black);
		Border = new TextStyle(ConsoleColor.DarkGray, ConsoleColor.Gray, ObjectType.Border);
		Grid = new TextStyle(ConsoleColor.DarkGray, ConsoleColor.Black, ObjectType.Grid);
		Snake = new TextStyle(ConsoleColor.Black, ConsoleColor.Green, ObjectType.Snake);
		Fruit = new TextStyle(ConsoleColor.Black, ConsoleColor.Red, ObjectType.Fruit);
		Bomb = new TextStyle(ConsoleColor.Black, ConsoleColor.Yellow, ObjectType.Bomb);
		Score = new TextStyle(ConsoleColor.Cyan, ConsoleColor.Black);
		Title = new TextStyle(ConsoleColor.Green, ConsoleColor.Black);
		Moves = new TextStyle(ConsoleColor.DarkGray, ConsoleColor.Black);
		Coords = new TextStyle(ConsoleColor.Black, ConsoleColor.DarkGray);
		Length = new TextStyle(ConsoleColor.DarkGreen, ConsoleColor.Black);
		Difficulty = new TextStyle(ConsoleColor.Black, ConsoleColor.DarkGray);
	}
}
