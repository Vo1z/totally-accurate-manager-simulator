namespace Ingame.FSM;

public interface IState
{
	void OnEnter();
	void OnTick(double deltaTime);
}