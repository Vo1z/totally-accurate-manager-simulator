using System;
using Ingame.SceneManagement;
using Ingame.Scripts;
using Ingame.Service;

namespace Ingame.GameSession;

public sealed class GameSessionService : IGameService
{
	private readonly SceneService _sceneService;
	private readonly GameConfig _gameConfig;

	public int DaysLeft { get; private set; }
	public int CurrentProjectProgress { get; private set; }
	public int TargetProjectProgress { get; private set; }

	public GameSessionState CurrentState { get; private set; } = GameSessionState.None;
	
	public event Action<float, int> OnGameProgressChanged; 
	
	public GameSessionService(SceneService sceneService, GameConfig gameConfig)
	{
		_sceneService = sceneService;
		_gameConfig = gameConfig;
		
		_sceneService.OnSceneStartedLoading += OnSceneStartedLoading;
	}

	~GameSessionService()
	{
		_sceneService.OnSceneStartedLoading -= OnSceneStartedLoading;
	}

	private void OnSceneStartedLoading(SceneType sceneType)
	{
		if(sceneType != SceneType.MainMenu)
			return;
		
		CurrentState = GameSessionState.None;
		DaysLeft = _gameConfig.daysToCompleteProject;
		CurrentProjectProgress = 0;
		TargetProjectProgress = _gameConfig.amountOfWorkToBeDone;
	}
	
	public void SkipDay()
	{
		DaysLeft--;
		
		OnGameProgressChanged?.Invoke(CurrentProjectProgress / (float) TargetProjectProgress, DaysLeft);

		if(DaysLeft > 0)
		{
			_sceneService.LoadScene(SceneType.DayPassed);
			return;
		}

		CurrentState = GameSessionState.EndedWithDefeat;
		_sceneService.LoadScene(SceneType.GameOver);
	}
	
	public void ContributeToProjectProgress(int amount)
	{
		CurrentProjectProgress += amount;
		
		OnGameProgressChanged?.Invoke(CurrentProjectProgress / (float) TargetProjectProgress, DaysLeft);

		if(CurrentProjectProgress < TargetProjectProgress)
			return;
		
		CurrentState = GameSessionState.EndedWithVictory;
		_sceneService.LoadScene(SceneType.GameOver);
	}
}

public enum GameSessionState
{
	None,
	EndedWithVictory,
	EndedWithDefeat
}
