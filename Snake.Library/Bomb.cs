using Snake.Library.Interfaces;

namespace Snake.Library;

public sealed class Bomb : IBomb
{
	private const int BLINK_TIME = 250;
	private readonly Coord[] _explosionCoords;

	public Coord Coord { get; }
	public int DetonationTime { get; }
	public bool IsBlinkOn { get; private set; }
	public IEnumerable<Coord> ExplosionCoords => _explosionCoords;

	public event Action<IBomb> OnToggleBlink;
	public event Action<IBomb> OnExplosion;

	private int _timeRemaining;

	private readonly IRenderer _renderer;

	public Bomb(IRenderer renderer, Coord coord, float detonationTime) 
		: this(renderer, coord, (int)(detonationTime * 1000)) {}
	public Bomb(IRenderer renderer, Coord coord, int detonationTime = 10000)
	{
		_renderer = renderer;
		OnToggleBlink += _renderer.Render;
		OnExplosion += _renderer.RenderExplosion;
		DetonationTime = detonationTime;
		Coord = coord;
		_explosionCoords = new[] { Coord + Coord.Up, Coord + Coord.Down, Coord + Coord.Left, Coord + Coord.Right };
		Activate();
	}
	~Bomb()
	{
		OnToggleBlink -= _renderer.Render;
		OnExplosion -= _renderer.RenderExplosion;
	}

	public async Task Activate()
	{
		_timeRemaining = DetonationTime;
		await StartTimer();
	}

	private async Task StartTimer()
	{
		while (_timeRemaining >= 3000)
			await Blinking(BLINK_TIME * 4);
		
		while (_timeRemaining >= 0)
			await Blinking(BLINK_TIME);
		
		OnExplosion?.Invoke(this);
	}
	private async Task Blinking(int delay)
	{
		IsBlinkOn = !IsBlinkOn;
		_timeRemaining -= delay;
		OnToggleBlink?.Invoke(this);
		await Task.Delay(delay);
	}
}
