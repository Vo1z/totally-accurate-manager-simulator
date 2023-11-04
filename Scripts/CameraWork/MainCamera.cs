using System;
using Godot;
using Ingame.Input;
using Ingame.Npc;
using Ingame.Service;

namespace Ingame.CameraWork;

public partial class MainCamera : Camera2D
{
	private readonly Lazy<InputService> _inputService = new Lazy<InputService>(ServiceLocator.Get<InputService>);

	public override void _Ready()
	{
		_inputService.Value.OnLeftMouseButtonPressed += OnLeftMouseButtonPressed;
	}

	public override void _ExitTree()
	{
		_inputService.Value.OnLeftMouseButtonPressed -= OnLeftMouseButtonPressed;
	}

	private void OnLeftMouseButtonPressed()
	{
		var spaceState = GetWorld2D().DirectSpaceState;
		var parameters = new PhysicsPointQueryParameters2D
		{
			Position = GetGlobalMousePosition()
		};
		
		var result = spaceState.IntersectPoint(parameters);

		foreach(var dictionary in result)
		{
			var worker = dictionary["collider"].As<Worker>();
			
			if(worker == null)
				continue;
			
			worker.OnBeingDragged();
		}
	}
}
