namespace Snake.Library.Interfaces;

public interface IBomb
{
	Coord Coord { get; }
	
	int DetonationTime { get; }
	bool IsBlinkOn { get; }

	event Action<IBomb> OnToggleBlink;
	event Action<IBomb> OnExplosion;
	
	Task Activate();
}
