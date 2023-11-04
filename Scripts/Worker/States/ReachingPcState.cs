using Ingame.FSM;
using Ingame.Resources;

namespace Ingame.Npc;

public sealed class ReachingPcState : IState
{
	private readonly Worker _worker;
	public readonly PcPoint pcPoint;

	public ReachingPcState(Worker worker, PcPoint pcPoint)
	{
		_worker = worker;
		this.pcPoint = pcPoint;
	}
	
	public void OnEnter()
	{
		_worker.TargetPosition = pcPoint.GlobalPosition;
	}

	public void OnTick(double deltaTime)
	{
		
	}
}