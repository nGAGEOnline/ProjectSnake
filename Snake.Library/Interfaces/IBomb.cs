using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snake.Library.Interfaces
{
	public interface IBomb
	{
		Coord Coord { get; }

		int DetonationTime { get; }
		bool IsBlinkOn { get; }
		IEnumerable<Coord> ExplosionCoords { get; }

		event Action<IBomb> OnToggleBlink;
		event Action<IBomb> OnExplosion;

		Task Activate();
	}
}