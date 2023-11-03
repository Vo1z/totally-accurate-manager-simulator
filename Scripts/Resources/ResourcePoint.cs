using Godot;
using Ingame.Service;

namespace Ingame.Resources;

public partial class ResourcePoint : Node2D
{
	[Export] private ResourceType resourceType;
	public ResourceType ResourceType => resourceType;
	
	public override void _Ready()
	{
		ServiceLocator.Get<ResourcesService>().RegisterResourcePoint(this);
	}
	
	private void _on_resource_trigger_body_entered(Node2D body)
	{ 
		GD.Print(body.Name);
	}
}
