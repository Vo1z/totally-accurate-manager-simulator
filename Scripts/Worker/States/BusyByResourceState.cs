using Ingame.FSM;
using Ingame.Resources;

namespace Ingame.Npc;

public sealed class BusyByResourceState : IState
{
	public readonly ResourcePoint resourcePoint;
	private readonly Worker _worker;

	public BusyByResourceState(ResourcePoint resourcePoint, Worker worker)
	{
		this.resourcePoint = resourcePoint;
		_worker = worker;
	}
	
	public void OnEnter()
	{
		resourcePoint.SetBusy(_worker);
	}

	public void OnExit()
	{
		
	}

	public void OnTick(double deltaTime)
	{
		
	}
}