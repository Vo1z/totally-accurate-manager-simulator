using Ingame.FSM;
using Ingame.Resources;
using Ingame.Utils;
using sperasoftgamejam.Scripts;

namespace Ingame.Npc;

public sealed class WorkingState : IState
{
	private readonly GameConfig _gameConfig;
	private readonly Worker _worker;
	private readonly ResourcesService _resourcesService;
	private double _timeLeftTillNextEvent;

	public WorkingState(GameConfig gameConfig, Worker worker, ResourcesService resourcesService)
	{
		_gameConfig = gameConfig;
		_worker = worker;
		_resourcesService = resourcesService;
	}
	
	public void OnEnter()
	{
		_timeLeftTillNextEvent = RndUtils.Range(_gameConfig.WorkerNewEventIntervalRange);
	}

	public void OnExit()
	{
		
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