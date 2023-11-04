using Godot;
using Ingame.Input;
using Ingame.Resources;
using Ingame.SceneManagement;
using Ingame.Service;

namespace Ingame.GameLoading;

public partial class GameLoader : Node
{
	[Export] private SceneService sceneService;
	[Export] private InputService inputService;
	
	public override void _Ready()
	{
		ServiceLocator.Register(sceneService);
		ServiceLocator.Register(inputService);
		ServiceLocator.Register(new ResourcesService(sceneService));

		sceneService.LoadScene(SceneType.Gameplay);
	}
}
