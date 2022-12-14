using Snake.Library.Enums;

namespace Snake.Library.Interfaces.UI;

public interface ITextStyle
{
	ObjectType ObjectType { get; }
}
public interface ITextStyle<out T> : ITextStyle
{
	T Foreground { get; }
	T Background { get; }
}

