using Godot;
using Ingame.FSM;
using Ingame.GameSession;
using Ingame.Resources;
using Ingame.Scripts;
using Ingame.Utils;

namespace Ingame.Npc;

public sealed class WorkingState : IState
{
	private readonly GameConfig _gameConfig;
	private readonly Worker _worker;
	private readonly ResourcesService _resourcesService;
	private readonly GameSessionService _gameSessionService;
	private double _timeLeftTillNextEvent;

	public WorkingState(GameConfig gameConfig, Worker worker, ResourcesService resourcesService, GameSessionService gameSessionService)
	{
		_gameConfig = gameConfig;
		_worker = worker;
		_resourcesService = resourcesService;
		_gameSessionService = gameSessionService;
	}
	
	public void OnEnter()
	{
		_timeLeftTillNextEvent = RndUtils.Range(_gameConfig.workerNewEventIntervalRange);
		_gameSessionService.ContributeToProjectProgress(Mathf.FloorToInt(_timeLeftTillNextEvent * _gameConfig.projectContributionPerSecond));
	}

	public void OnTick(double deltaTime)
	{
		_timeLeftTillNextEvent -= deltaTime;

		if(_timeLeftTillNextEvent <= 0)
		{
			var requiredResourceType = RndUtils.RandomEnumValue<ResourceType>();
			var resourcePoint = _resourcesService.GetResourcePoint(requiredResourceType);
			
			_worker.stateMachine.SwitchState(new ReachingResourceState(resourcePoint, _worker));
		}
	}
}