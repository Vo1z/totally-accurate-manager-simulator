using Godot;
using Ingame.Audio;
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
	[Export] private AudioService audioService;
	[Export] private Cursor cursor;
	
	public override void _Ready()
	{
		ServiceLocator.Reset();
		ServiceLocator.Register(sceneService);
		ServiceLocator.Register(inputService);
		ServiceLocator.Register(audioService);
		ServiceLocator.Register(cursor);
		ServiceLocator.Register(new ResourcesService(sceneService));
		ServiceLocator.Register(new GameSessionService(sceneService, gameConfig));

		sceneService.LoadScene(SceneType.MainMenu);
	}
}
