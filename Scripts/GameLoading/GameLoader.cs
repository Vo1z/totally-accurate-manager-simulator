using Godot;
using Ingame.Resources;
using Ingame.SceneManagement;
using Ingame.Service;

namespace Ingame.GameLoading;

public partial class GameLoader : Node
{
	[Export] private SceneService sceneService;
	
	public override void _Ready()
	{
		ServiceLocator.Register(sceneService);
		ServiceLocator.Register(new ResourcesService(sceneService));
		
		sceneService.LoadScene(SceneType.Gameplay);
	}
}
