using System;
using Godot;
using Ingame.Npc;
using Ingame.SceneManagement;
using Ingame.Scripts;
using Ingame.Service;
using Ingame.Utils;

namespace Ingame.GameSession;

public sealed class GameSessionService : IGameService
{
	private static readonly int MAX_AMOUNT_OF_WORKERS = Enum.GetValues<WorkerId>().Length; 
	private readonly SceneService _sceneService;
	private readonly GameConfig _gameConfig;
	
	private int _currentProjectProgress;
	private int _targetProjectProgress;

	public int CurrentWorkerSpeedBoost { get; private set; }
	public int CurrentAmountOfWorkers { get; private set; }
	public int DaysLeft { get; private set; }
	public GameSessionState CurrentState { get; private set; } = GameSessionState.None;
	
	public event Action<int> OnAmountOfWorkerUpdated; 
	public event Action<int> OnWorkerSpeedBoostUpdated; 
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

		CurrentWorkerSpeedBoost = 0;
		CurrentAmountOfWorkers = _gameConfig.initialAmountOfWorkers;
		DaysLeft = _gameConfig.daysToCompleteProject;
		CurrentState = GameSessionState.None;
		
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
		var boostType = RndUtils.RandomEnumValue<BoostType>();

		GD.Print($"Boost type: {boostType}");
		
		if(boostType == BoostType.HireWorker && CurrentAmountOfWorkers < MAX_AMOUNT_OF_WORKERS)
		{
			CurrentAmountOfWorkers++;
			OnAmountOfWorkerUpdated?.Invoke(CurrentAmountOfWorkers);
			return;
		}
		
		CurrentWorkerSpeedBoost += _gameConfig.workerSpeedBoost;
		OnWorkerSpeedBoostUpdated?.Invoke(CurrentWorkerSpeedBoost);
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
