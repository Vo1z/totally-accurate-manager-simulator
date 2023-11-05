using System;
using Godot;
using Ingame.Input;
using Ingame.Npc;
using Ingame.Service;

namespace Ingame.CameraWork;

public partial class MainCamera : Camera2D
{
	private readonly Lazy<InputService> _inputService = new(ServiceLocator.Get<InputService>);
	private readonly Lazy<Cursor> _cursor = new(ServiceLocator.Get<Cursor>);

	public override void _Ready()
	{
		_inputService.Value.OnLeftMouseButtonPressed += OnLeftMouseButtonPressed;
	}

	public override void _ExitTree()
	{
		_inputService.Value.OnLeftMouseButtonPressed -= OnLeftMouseButtonPressed;
	}

	public override void _Process(double delta)
	{
		var spaceState = GetWorld2D().DirectSpaceState;
		var parameters = new PhysicsPointQueryParameters2D
		{
			Position = GetGlobalMousePosition()
		};
		
		var result = spaceState.IntersectPoint(parameters);
		
		_cursor.Value.SetGrab(false);
		
		foreach(var dictionary in result)
		{
			var worker = dictionary["collider"].As<Worker>();

			if(worker != null)
			{
				if(worker.stateMachine.CurrentState is not ReachingPcState && worker.stateMachine.CurrentState is not ReachingResourceState)
					return;
				
				_cursor.Value.SetGrab(true);
				return;
			}
		}
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
			return;
		}
	}
}
