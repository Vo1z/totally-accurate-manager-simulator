using System;

namespace Ingame.FSM;

public sealed class StateMachine
{
	public IState CurrentState { get; private set; }

	public event Action<IState> OnStateChanged; 
	
	public void SwitchState(IState newState)
	{
		CurrentState = newState;
		CurrentState.OnEnter();
		
		OnStateChanged?.Invoke(CurrentState);
	}
}
