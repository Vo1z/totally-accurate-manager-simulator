using System;
using Godot;
using Ingame.Service;

namespace Ingame.SceneManagement;

public partial class SceneService : Node, IGameService
{
	[Export] private PackedScene mainMenuScene;
	[Export] private PackedScene gameplayScene;
	[Export] private PackedScene daysPassedScene;
	[Export] private PackedScene gameOverScene;
	[Export] private Node sceneRoot;
	
	private Node _currentScene;
	
	public event Action<SceneType> OnSceneStartedLoading;

	public void LoadScene(SceneType sceneType)
	{
		if(_currentScene != null)
			_currentScene.QueueFree();

		OnSceneStartedLoading?.Invoke(sceneType);

		_currentScene = sceneType switch
		{
			SceneType.MainMenu => mainMenuScene.Instantiate(),
			SceneType.Gameplay => gameplayScene.Instantiate(),
			SceneType.DayPassed => daysPassedScene.Instantiate(),
			SceneType.GameOver => gameOverScene.Instantiate(),
			_ => throw new ArgumentOutOfRangeException(nameof(sceneType), sceneType, null)
		};
		
		sceneRoot.AddChild(_currentScene);
	}
}
