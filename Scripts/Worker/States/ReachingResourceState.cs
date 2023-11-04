using Ingame.FSM;
using Ingame.Resources;

namespace Ingame.Npc;

public sealed class ReachingResourceState : IState
{
	public readonly ResourcePoint resourcePoint;
	private readonly Worker _worker;

	public ReachingResourceState(ResourcePoint resourcePoint, Worker worker)
	{
		this.resourcePoint = resourcePoint;
		_worker = worker;
	}

	public void OnEnter()
	{
		_worker.TargetPosition = resourcePoint.GlobalPosition;
	}

	public void OnExit()
	{
		
	}

	public void OnTick(double deltaTime)
	{
		if(!resourcePoint.IsBusy)
			return;
		
		_worker.stateMachine.SwitchState(new WaitingForAvailableResourcePointState(resourcePoint, _worker));
	}
}
