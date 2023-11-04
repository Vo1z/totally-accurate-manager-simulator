using Ingame.FSM;
using Ingame.Resources;

namespace Ingame.Npc;

public sealed class WaitingForAvailableResourcePointState : IState
{
	private readonly ResourcePoint _resourcePoint;
	private readonly Worker _worker;

	public WaitingForAvailableResourcePointState(ResourcePoint resourcePoint, Worker worker)
	{
		_resourcePoint = resourcePoint;
		_worker = worker;
	}
	
	public void OnEnter()
	{
		_worker.TargetPosition = _worker.GlobalPosition;
	}

	public void OnExit()
	{
		
	}

	public void OnTick(double deltaTime)
	{
		if(_resourcePoint.IsBusy)
			return;
		
		_worker.stateMachine.SwitchState(new ReachingResourceState(_resourcePoint, _worker));
	}
}