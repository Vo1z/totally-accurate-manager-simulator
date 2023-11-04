namespace Ingame.FSM;

public sealed class StateMachine
{
	public IState CurrentState { get; private set; }

	public void SwitchState(IState newState)
	{
		CurrentState.OnExit();
		CurrentState = newState;
		CurrentState.OnEnter();
	}
}