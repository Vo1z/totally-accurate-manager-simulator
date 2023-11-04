namespace Ingame.FSM;

public interface IState
{
	void OnEnter();
	void OnExit();
	void OnTick(double deltaTime);
}