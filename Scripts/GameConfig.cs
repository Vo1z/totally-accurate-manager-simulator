using Godot;

namespace sperasoftgamejam.Scripts;

[Tool]
public partial class GameConfig : Resource
{
	[Export] public Vector2 WorkerNewEventIntervalRange;
	[Export] public Vector2 ResourceCollectionDurationRange;
}
