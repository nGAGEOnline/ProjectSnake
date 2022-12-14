using Snake.Library.Enums;
using Snake.Library.Interfaces.UI;
using Snake.Library.Structs;

namespace Snake.Console.Game.UI;

public class TextField : ITextField
{
	public Coord Coord { get; private set; }
	public string Prefix { get; }
	public string Text { get; private set; }
	public string FullText => $" {Prefix} {Text} ";
	
	public ObjectType ObjectType { get; }

	public TextField(Coord coord, char symbol, ObjectType objectType) 
		: this(coord, $"{symbol}", objectType) { }
	public TextField(Coord coord, string text, ObjectType objectType)
		: this(coord, string.Empty, text, objectType) { }
	public TextField(Coord coord, string prefix, string text, ObjectType objectType)
	{
		ObjectType = objectType;
		Coord = coord;
		Prefix = prefix;
		Text = text;
	}

	public void UpdateText(string text)
	{
		Text = text;
	}
}
