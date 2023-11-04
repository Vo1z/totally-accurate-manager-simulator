using Ingame.FSM;
using Ingame.Resources;

namespace Ingame.Npc;

public sealed class WaitingForAvailableResourcePointState : IState
{
	public readonly ResourcePoint resourcePoint;
	private readonly Worker _worker;

	public WaitingForAvailableResourcePointState(ResourcePoint resourcePoint, Worker worker)
	{
		this.resourcePoint = resourcePoint;
		_worker = worker;
	}
	
	public void OnEnter()
	{
		_worker.TargetPosition = _worker.GlobalPosition;
	}

	public void OnTick(double deltaTime)
	{
		if(resourcePoint.IsBusy)
			return;
		
		_worker.stateMachine.SwitchState(new ReachingResourceState(resourcePoint, _worker));
	}
}