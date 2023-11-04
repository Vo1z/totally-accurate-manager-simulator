using System;
using Godot;
using Ingame.Service;

namespace Ingame.SceneManagement;

public partial class SceneService : Node, IGameService
{
	[Export] private PackedScene gameplayScene;
	[Export] private Node sceneRoot;
	
	private Node _currentScene;
	
	public event Action<SceneType> OnSceneStartedLoading;

	public void LoadScene(SceneType sceneType)
	{
		if(_currentScene != null)
			_currentScene.QueueFree();

		switch(sceneType)
		{
			case SceneType.MainMenu:
				break;
			case SceneType.Gameplay:
				OnSceneStartedLoading?.Invoke(sceneType);
				_currentScene = gameplayScene.Instantiate();
				sceneRoot.AddChild(_currentScene);
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(sceneType), sceneType, null);
		}
	}
}
