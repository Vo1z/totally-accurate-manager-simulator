using System;
using Godot;
using Ingame.Service;

namespace Ingame.Input;

public partial class InputService : Node2D, IGameService
{
	public bool IsLeftMouseButtonPressed { get; private set; }

	public event Action OnLeftMouseButtonPressed;
	
	public override void _Input(InputEvent @event)
	{
		if(@event is InputEventMouseButton { ButtonIndex: MouseButton.Left })
			OnLeftMouseButtonPressed?.Invoke();
	}

	public override void _Process(double delta)
	{
		IsLeftMouseButtonPressed = Godot.Input.IsMouseButtonPressed(MouseButton.Left);
	}
}
