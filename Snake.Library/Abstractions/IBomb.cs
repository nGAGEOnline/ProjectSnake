namespace Snake.Library.Abstractions;

public interface IBomb
{
	Coord Coord { get; }
	int DetonationTime { get; }

	Task Activate();
}
