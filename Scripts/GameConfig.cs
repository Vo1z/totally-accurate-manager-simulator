using System;
using Godot;
using Ingame.Resources;

namespace sperasoftgamejam.Scripts;

[Tool]
public partial class GameConfig : Resource
{
	[Export] public Vector2 WorkerNewEventIntervalRange;
	[Export] public Vector2 ResourceCollectionDurationRange;
	[Export] public float SpeedUpValue = 1f;
	
	[ExportCategory("Resources")]
	[Export] private Texture2D coffee;
	[Export] private Texture2D food;
	[Export] private Texture2D water;
	[Export] private Texture2D videoGames;
	[Export] private Texture2D book;
	[Export] private Texture2D couch;
	[Export] private Texture2D redTrashBin;
	[Export] private Texture2D greenTrashBin;
	[Export] private Texture2D blueTrashBin;
	[Export] public Texture2D PcTexture;

	public Texture2D GetReSourceTexture(ResourceType resourceType)
	{
		return resourceType switch
		{
			ResourceType.Coffee => coffee,
			ResourceType.Food => food,
			ResourceType.Water => water,
			ResourceType.VideoGames => videoGames,
			ResourceType.Book => book,
			ResourceType.Couch => couch,
			ResourceType.RedTrashBin => redTrashBin,
			ResourceType.GreenTrashBin => greenTrashBin,
			ResourceType.BlueTrashBin => blueTrashBin,
			_ => throw new ArgumentOutOfRangeException(nameof(resourceType), resourceType, null)
		};
	}
}
