using System;
using Godot;

namespace Ingame.Utils;

public static class RndUtils
{
	private static readonly RandomNumberGenerator _randomNumberGenerator = new();
	public static float Value => _randomNumberGenerator.Randf();

	static RndUtils()
	{
		_randomNumberGenerator.Seed = (ulong)DateTime.Now.Second;
	}
	
	public static float Range(float from, float to)
	{
		return _randomNumberGenerator.RandfRange(from, to);
	}
	
	public static int Range(int from, int to)
	{
		return _randomNumberGenerator.RandiRange(from, to);
	}

	public static float Range(Vector2 range)
	{
		return Range(range.X, range.Y);
	}

	public static T RandomEnumValue<T>() where T : Enum
	{
		var enumValues = Enum.GetValues(typeof(T)); 
		return (T)enumValues.GetValue(Range(0, enumValues.Length - 1));
	}
}