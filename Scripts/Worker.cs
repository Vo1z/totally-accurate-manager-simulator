using Godot;

public partial class Worker : CharacterBody2D
{
	private const float TARGET_POSITION_DELTA = 5f;
	
	[Export] private float walkingSpeed;
	[Export] private NavigationAgent2D navigationAgent;
	
	public Vector2 TargetPosition
	{
		get => navigationAgent.TargetPosition;
		set => navigationAgent.TargetPosition = value;
	}
	
	public override void _PhysicsProcess(double delta)
	{
		TargetPosition = GetGlobalMousePosition();
		
		var position = GlobalPosition;
		var nextPosition = navigationAgent.GetNextPathPosition();
		var deltaVector = nextPosition - position;
		
		if(deltaVector.Length() < TARGET_POSITION_DELTA)
			return;
		
		var direction = deltaVector.Normalized();
		var velocity = direction * walkingSpeed;

		Velocity = velocity;
		MoveAndSlide();
	}
}
