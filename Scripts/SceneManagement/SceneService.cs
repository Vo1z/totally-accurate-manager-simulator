using System;
using Godot;
using Ingame.Service;

namespace Ingame.SceneManagement;

public partial class SceneService : Node, IGameService
{
	[Export] private PackedScene gameplayScene;
	[Export] private Node sceneRoot;
	
	private Node _currentScene;
	
	public event Action<SceneType> OnSceneLoaded;

	public void LoadScene(SceneType sceneType)
	{
		if(_currentScene != null)
			_currentScene.QueueFree();

		switch(sceneType)
		{
			case SceneType.MainMenu:
				break;
			case SceneType.Gameplay:
				_currentScene = gameplayScene.Instantiate();
				sceneRoot.AddChild(_currentScene);
				OnSceneLoaded?.Invoke(sceneType);
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(sceneType), sceneType, null);
		}
	}
}
