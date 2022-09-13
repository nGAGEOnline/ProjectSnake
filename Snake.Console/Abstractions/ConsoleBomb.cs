using Snake.Library;
using Snake.Library.Enums;
using Snake.Library.Interfaces;

namespace Snake.Console.Abstractions;

public class ConsoleBomb // : IBomb
{
	#region CONSTS
	
	private const char EMPTY_SYMBOL = ' ';
	private const char BOMB_SYMBOL = '█';
	private static readonly char[] BombExplosionCenterSymbols = new char[]{ '█', '▓', '▒', ' '};
	private static readonly char[] BombExplosionSymbols = new char[]{ ' ', '█', '▓', '▒'};
	private const int BLINK_TIME = 250;
	private const int ANIMATION_DELAY = 300;
	#endregion

	public event Action? OnExplode;

	public Coord Coord { get; }

	public int DetonationTime { get; }

	private readonly Coord[] _explosionCoords;
	private readonly IRenderer _renderer;
	private int _timeRemaining = 0;
	private bool _blinkOn;

	public ConsoleBomb(Coord coord, IRenderer renderer, int detonationTime = 10000) 
	{
		_renderer = renderer;
		Coord = coord;
		DetonationTime = detonationTime;
		_timeRemaining = DetonationTime;
		_explosionCoords = new[] { Coord + Coord.Up, Coord + Coord.Down, Coord + Coord.Left, Coord + Coord.Right };
	}

	public async Task Activate()
	{
		await StartTimer();
		await Explosion();
	}

	private async Task StartTimer()
	{
		while (_timeRemaining >= 3000)
			await Blinking(BLINK_TIME * 4);
		
		while (_timeRemaining >= 0)
			await Blinking(BLINK_TIME);
	}

	private async Task Blinking(int delay)
	{
		_timeRemaining -= BLINK_TIME;
		_renderer.Render(Coord, $"{BOMB_SYMBOL}", _blinkOn ? ColorType.BombOff : ColorType.BombOn);
		await Task.Delay(delay);
		_blinkOn = !_blinkOn;
	}

	private async Task Explosion()
	{
		for (var i = 0; i < BombExplosionSymbols.Length; i++)
		{
			foreach (var coord in _explosionCoords)
				_renderer.Render(coord, $"{BombExplosionSymbols[i]}", ColorType.BombOff);
			_renderer.Render(Coord, $"{BombExplosionCenterSymbols[i]}", ColorType.BombOn);
			await Task.Delay(ANIMATION_DELAY);
		}
		foreach (var coord in _explosionCoords)
			_renderer.Render(coord, $"{EMPTY_SYMBOL}", ColorType.BombOff);
		_renderer.Render(Coord, $"{EMPTY_SYMBOL}", ColorType.BombOff);
	}
}
