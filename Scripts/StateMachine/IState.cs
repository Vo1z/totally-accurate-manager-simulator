namespace Ingame.StateMachine;

public interface IState
{
	void OnEnter();
	void OnExit();
	void OnTick();
}