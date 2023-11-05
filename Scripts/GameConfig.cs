using System;
using Godot;
using Ingame.Resources;

namespace Ingame.Scripts;

[Tool]
public partial class GameConfig : Resource
{
	[ExportCategory("Workers")]
	[Export] public Vector2 workerNewEventIntervalRange;
	[Export] public Vector2 resourceCollectionDurationRange;
	[Export] public int projectContributionPerSecond;
	[Export] public float speedUpValue = 1f;
	
	[ExportCategory("Progress")]
	[Export] public int initialAmountOfWorkers = 2;
	[Export] public int amountOfWorkToBeDone = 100;
	[Export] public int daysToCompleteProject = 3;
	[Export] public int dayDuration = 120;
	
	[ExportCategory("Progress")]
	[Export] public int workerSpeedBoost = 10;
	
	[ExportCategory("Puzzles")]
	[Export] public Vector2 minMaxPauseBetweenEvents;
	
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
	[Export] public Texture2D pcTexture;

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
