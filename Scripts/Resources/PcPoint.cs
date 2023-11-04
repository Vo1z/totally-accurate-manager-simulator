using Godot;
using Ingame.Npc;
using Ingame.Service;

namespace Ingame.Resources;

public partial class PcPoint : Node2D
{
	[Export] private WorkerId workerId;

	public WorkerId WorkerId => workerId;
	
	public override void _EnterTree()
	{
		ServiceLocator.Get<ResourcesService>().RegisterPcPoint(this);
	}

	private void OnBodyEntered(Node2D body)
	{
		if(body is not Worker worker)
			return;
		
		worker.OnPcPointEntered(this);
	}

	public override void _Draw()
	{
		DrawCircle(GlobalPosition, 5, Colors.Red);
	}
}
