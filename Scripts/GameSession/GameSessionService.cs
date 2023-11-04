using System;
using Ingame.SceneManagement;
using Ingame.Scripts;
using Ingame.Service;
using Ingame.Utils;

namespace Ingame.GameSession;

public sealed class GameSessionService : IGameService
{
	private readonly SceneService _sceneService;
	private readonly GameConfig _gameConfig;

	private bool _isInteractionLocked;
	private bool _isSpeedBoostActive;
	private int _currentAmountOfWorkers;
	
	private int _currentProjectProgress;
	private int _targetProjectProgress;
	
	public int DaysLeft { get; private set; }

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
		_currentProjectProgress = 0;
		_targetProjectProgress = _gameConfig.amountOfWorkToBeDone;
	}
	
	public void SkipDay()
	{
		DaysLeft--;
		
		OnGameProgressChanged?.Invoke(_currentProjectProgress / (float) _targetProjectProgress, DaysLeft);

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
		_currentProjectProgress += amount;
		
		OnGameProgressChanged?.Invoke(_currentProjectProgress / (float) _targetProjectProgress, DaysLeft);

		if(_currentProjectProgress < _targetProjectProgress)
			return;
		
		CurrentState = GameSessionState.EndedWithVictory;
		_sceneService.LoadScene(SceneType.GameOver);
	}

	public void MakeRandomBoost()
	{
		
	}

	private enum BoostType
	{
		HireWorker,
		IncreaseSpeed
	}
}

public enum GameSessionState
{
	None,
	EndedWithVictory,
	EndedWithDefeat
}
