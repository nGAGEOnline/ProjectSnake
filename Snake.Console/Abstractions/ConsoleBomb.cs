using Snake.Library;
using Snake.Library.Enums;
using Snake.Library.Interfaces;

namespace Snake.Console.Abstractions;

public class ConsoleBomb //: IBomb
{
	#region CONSTS
	
	private static readonly char[] BombExplosionCenterSymbols = new char[]{ '█', '▓', '▒', ' '};
	private static readonly char[] BombExplosionSymbols = new char[]{ ' ', '█', '▓', '▒'};
	private const int BLINK_TIME = 250;
	private const int ANIMATION_DELAY = 300;
	
	#endregion

	public event Action? OnExplosion;
	public event Action<bool>? OnToggleBlink;

	public Coord Coord { get; }

	public int DetonationTime { get; }
	public int BlinkTime { get; } = BLINK_TIME;
	public bool IsBlinkOn { get; }

	private int _timeRemaining = 0;

	private readonly Coord[] _explosionCoords;

	public ConsoleBomb(Coord coord, int detonationTime = 10000) 
	{
		Coord = coord;
		DetonationTime = detonationTime;
		_timeRemaining = DetonationTime;
		_explosionCoords = new[] { Coord + Coord.Up, Coord + Coord.Down, Coord + Coord.Left, Coord + Coord.Right };
	}

	/*
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
		_renderer.Render(this); //Coord, $"{BOMB_SYMBOL}", _blinkOn ? ColorType.DarkYellow : ColorType.Yellow);
		await Task.Delay(delay);
		IsBlinkOn = !IsBlinkOn;
	}

	private async Task Explosion()
	{
		for (var i = 0; i < BombExplosionSymbols.Length; i++)
		{
			foreach (var coord in _explosionCoords)
				_renderer.Render(this); //coord, $"{BombExplosionSymbols[i]}", ColorType.DarkYellow);
			_renderer.Render(this); //Coord, $"{BombExplosionCenterSymbols[i]}", ColorType.Yellow);
			await Task.Delay(ANIMATION_DELAY);
		}
		foreach (var coord in _explosionCoords)
			_renderer.Render(this); //coord, $"{EMPTY_SYMBOL}", ColorType.DarkYellow);
		_renderer.Render(this); //Coord, $"{EMPTY_SYMBOL}", ColorType.DarkYellow);
	}
	*/
}
