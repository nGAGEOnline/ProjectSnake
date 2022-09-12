namespace Snake.Library.Interfaces;

public interface IBomb
{
	Library.Coord Coord { get; }
	int DetonationTime { get; }

	event Action OnExplode;
	
	Task Activate();
}
