using System;
using Godot;
using Ingame.Npc;

namespace Ingame.Utils;

public static class ColorUtils
{
	public static Color GetWorkerColor(WorkerId workerId)
	{
		return workerId switch
		{
			WorkerId.Red => Colors.Red,
			WorkerId.Green => Colors.Green,
			WorkerId.Blue => Colors.Blue,
			WorkerId.Purple => Colors.Purple,
			WorkerId.Yellow => Colors.Yellow,
			WorkerId.Black => Colors.Black,
			_ => throw new ArgumentOutOfRangeException(nameof(workerId), workerId, null)
		};
	}
}