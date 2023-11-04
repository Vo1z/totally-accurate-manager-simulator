using Godot;
using Ingame.GameSession;
using Ingame.Input;
using Ingame.Resources;
using Ingame.SceneManagement;
using Ingame.Scripts;
using Ingame.Service;

namespace Ingame.GameLoading;

public partial class GameLoader : Node
{
	[Export] private GameConfig gameConfig;
	[Export] private SceneService sceneService;
	[Export] private InputService inputService;
	
	public override void _Ready()
	{
		ServiceLocator.Register(sceneService);
		ServiceLocator.Register(inputService);
		ServiceLocator.Register(new ResourcesService(sceneService));
		ServiceLocator.Register(new GameSessionService(sceneService, gameConfig));

		sceneService.LoadScene(SceneType.Gameplay);
	}
}
