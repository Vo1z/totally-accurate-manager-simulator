using Godot;
using Ingame.FSM;
using Ingame.Input;

namespace Ingame.Npc;

public sealed class BeingDraggedState : IState
{
	private readonly Worker _worker;
	private readonly InputService _inputService;
	private readonly IState _previousState;

	public BeingDraggedState(Worker worker, InputService inputService, IState previousState)
	{
		_worker = worker;
		_inputService = inputService;
		_previousState = previousState;
	}
	
	public void OnEnter()
	{
		
	}

	public void OnTick(double deltaTime)
	{
		_worker.PlayAnimation(Worker.AnimationState.Idle);
		
		if(_inputService.IsLeftMouseButtonPressed) 
			_worker.GlobalPosition = _worker.GetGlobalMousePosition();
		else
			_worker.stateMachine.SwitchState(_previousState);
	}
}