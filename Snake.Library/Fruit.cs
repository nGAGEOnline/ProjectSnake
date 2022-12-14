using Snake.Library.Enums;
using Snake.Library.Interfaces;
using Snake.Library.Structs;

namespace Snake.Library;

internal sealed class Fruit : IGridObject
{
	public Coord[] Coords { get; set; }
	public ObjectType ObjectType { get; set; }
}
