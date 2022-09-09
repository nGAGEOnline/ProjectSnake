namespace Snake.Library.Interfaces;

public interface IBomb
{
	Coord Coord { get; }
	int DetonationTime { get; }

	Task Activate();
}
