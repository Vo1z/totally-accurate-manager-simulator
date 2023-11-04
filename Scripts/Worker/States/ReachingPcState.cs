using Ingame.FSM;
using Ingame.Resources;

namespace Ingame.Npc;

public sealed class ReachingPcState : IState
{
	private readonly Worker _worker;
	private readonly PcPoint _pcPoint;

	public ReachingPcState(Worker worker, PcPoint pcPoint)
	{
		_worker = worker;
		_pcPoint = pcPoint;
	}
	
	public void OnEnter()
	{
		_worker.TargetPosition = _pcPoint.GlobalPosition;
	}

	public void OnExit()
	{
		
	}

	public void OnTick(double deltaTime)
	{
		
	}
}