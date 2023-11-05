using Godot;
using Ingame.Service;

public partial class Cursor : Sprite2D, IGameService
{
	public override void _Ready()
	{
		SetGrab(false);
	}

	public override void _Process(double delta)
	{
		GlobalPosition = GetGlobalMousePosition();
	}
	
	public void SetGrab(bool isEnabled)
	{
		if(isEnabled)
		{
			Input.MouseMode = Input.MouseModeEnum.Hidden;
			Show();
			return;
		}
		
		Input.MouseMode = Input.MouseModeEnum.Visible;
		Hide();
	}
}
