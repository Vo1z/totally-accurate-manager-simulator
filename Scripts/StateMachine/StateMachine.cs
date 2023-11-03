namespace Ingame.StateMachine;

public sealed class StateMachine
{
	private IState _currentState;
	
	public void SwitchState(IState newState)
	{
		_currentState.OnExit();
		_currentState = newState;
		_currentState.OnEnter();
	}
}