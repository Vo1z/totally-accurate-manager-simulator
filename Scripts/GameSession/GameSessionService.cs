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

	public GameSessionState CurrentState { get; private set; } = GameSessionState.Running;
	
	public event Action<float, int> OnGameProgressChanged; 
	public event Action<GameSessionState> OnGameSessionEnded; 
	
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

	private void OnSceneStartedLoading(SceneType _)
	{
		CurrentState = GameSessionState.Running;
		DaysLeft = _gameConfig.daysToCompleteProject;
		CurrentProjectProgress = 0;
		TargetProjectProgress = _gameConfig.amountOfWorkToBeDone;
	}

	private void EndGameSession(GameSessionState state)
	{
		CurrentState = state;
		OnGameSessionEnded?.Invoke(state);
	}
	
	public void SkipDay()
	{
		DaysLeft--;
		
		OnGameProgressChanged?.Invoke(CurrentProjectProgress / (float) TargetProjectProgress, DaysLeft);
		
		if(DaysLeft <= 0)
			EndGameSession(GameSessionState.EndedWithDefeat);
	}
	
	public void ContributeToProjectProgress(int amount)
	{
		CurrentProjectProgress += amount;
		
		OnGameProgressChanged?.Invoke(CurrentProjectProgress / (float) TargetProjectProgress, DaysLeft);
		
		if(CurrentProjectProgress >= TargetProjectProgress)
			EndGameSession(GameSessionState.EndedWithVictory);
	}
}

public enum GameSessionState
{
	Running,
	EndedWithVictory,
	EndedWithDefeat
}
