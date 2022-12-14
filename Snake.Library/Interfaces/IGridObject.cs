using Snake.Library.Enums;
using Snake.Library.Structs;

namespace Snake.Library.Interfaces;

public interface IGridObject
{
	Coord[] Coords { get; }
	ObjectType ObjectType { get; }
}
