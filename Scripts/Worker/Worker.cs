using System;
using Godot;
using Ingame.FSM;
using Ingame.GameSession;
using Ingame.Input;
using Ingame.Resources;
using Ingame.Scripts;
using Ingame.Service;
using Ingame.Utils;

namespace Ingame.Npc;

public partial class Worker : CharacterBody2D
{
	private const string WORKER_COLOR_SHADER_PROPERTY = "workerColor";
	private const float TARGET_POSITION_DELTA = 5f;

	[Export] private GameConfig gameConfig;
	[Export] private WorkerId workerId;
	[Export] private float walkingSpeed;
	[Export] private NavigationAgent2D navigationAgent;
	[Export] private AnimationPlayer animationPlayer;
	[Export] private TextureRect resourceIcon;
	[Export] private Node2D uiParent;
	[Export] private Sprite2D workerSprite;

	private Lazy<ResourcesService> _resourcesService = new(ServiceLocator.Get<ResourcesService>);
	private Lazy<InputService> _inputService = new(ServiceLocator.Get<InputService>);
	
	public readonly StateMachine stateMachine = new();
	
	public Vector2 TargetPosition
	{
		get => navigationAgent.TargetPosition;
		set => navigationAgent.TargetPosition = value;
	}

	public override void _Ready()
	{
		(workerSprite.Material as ShaderMaterial)?.SetShaderParameter(WORKER_COLOR_SHADER_PROPERTY, ColorUtils.GetWorkerColor(workerId));
		stateMachine.OnStateChanged += OnStateChanged;
		stateMachine.SwitchState(new ReachingPcState(this, _resourcesService.Value.GetPcPoint(workerId)));
	}

	public override void _Process(double deltaTime)
	{
		stateMachine.CurrentState?.OnTick(deltaTime);
	}

	public override void _PhysicsProcess(double delta)
	{
		MoveTowardsTargetPosition();
	}
	
	private void MoveTowardsTargetPosition()
	{
		var position = GlobalPosition;
		var nextPosition = stateMachine.CurrentState is not ReachingPcState && stateMachine.CurrentState is not ReachingResourceState? position : navigationAgent.GetNextPathPosition();
		var deltaVector = nextPosition - position;

		if(deltaVector.Length() < TARGET_POSITION_DELTA)
		{
			PlayAnimation(AnimationState.Idle);
			return;
		}

		if(Mathf.Abs(deltaVector.Y) > Mathf.Abs(deltaVector.X))
			PlayAnimation(deltaVector.Y > 0 ? AnimationState.WalkingBottom : AnimationState.WalkingTop);
		else
			PlayAnimation(deltaVector.X > 0 ? AnimationState.WalkingRight : AnimationState.WalkingLeft);

		var direction = deltaVector.Normalized();
		var velocity = direction * walkingSpeed;

		Velocity = velocity;
		MoveAndSlide();
	}

	public void PlayAnimation(AnimationState animationState)
	{
		switch(animationState)
		{
			case AnimationState.Idle:
				animationPlayer.Play("idle");
				break;
			case AnimationState.WalkingTop:
				animationPlayer.Play("walk_top");
				break;
			case AnimationState.WalkingBottom:
				animationPlayer.Play("walk_bottom");
				break;
			case AnimationState.WalkingRight:
				animationPlayer.Play("walk_right");
				break;
			case AnimationState.WalkingLeft:
				animationPlayer.Play("walk_left");
				break;
		}
	}

	private void OnStateChanged(IState state)
	{
		if(state is ReachingPcState reachingPcState)
		{
			uiParent.Show();
			resourceIcon.Texture = gameConfig.pcTexture;
			return;
		}

		if(state is ReachingResourceState reachingResourceState)
		{
			uiParent.Show();
			resourceIcon.Texture = gameConfig.GetReSourceTexture(reachingResourceState.resourcePoint.ResourceType);
			return;
		}
		
		if(state is WaitingForAvailableResourcePointState waitingForAvailableResourcePointState)
		{
			uiParent.Show();
			resourceIcon.Texture = gameConfig.GetReSourceTexture(waitingForAvailableResourcePointState.resourcePoint.ResourceType);
			return;
		}
		
		uiParent.Hide();
	}

	public void OnPcPointEntered(PcPoint pcPoint)
	{
		if(stateMachine.CurrentState is BeingDraggedState { previousState: ReachingPcState prevReachingPcState })
		{
			if(pcPoint.WorkerId == workerId)
			{
				GlobalPosition = pcPoint.GlobalPosition;
				stateMachine.SwitchState(new WorkingState(gameConfig, this, ServiceLocator.Get<ResourcesService>(), ServiceLocator.Get<GameSessionService>()));
				return;
			}
		}
		
		if(pcPoint.WorkerId != workerId)
			return;
		
		if(stateMachine.CurrentState is not ReachingPcState)
			return;
		
		stateMachine.SwitchState(new WorkingState(gameConfig, this, ServiceLocator.Get<ResourcesService>(), ServiceLocator.Get<GameSessionService>()));
	}
	
	public void OnResourcePointEntered(ResourcePoint resourcePoint)
	{
		if(stateMachine.CurrentState is BeingDraggedState { previousState: ReachingResourceState prevReachingResourceState })
		{
			if(prevReachingResourceState.resourcePoint.ResourceType == resourcePoint.ResourceType)
			{
				GlobalPosition = resourcePoint.GlobalPosition;
				stateMachine.SwitchState(new BusyByResourceState(resourcePoint, this));
				return;
			}
		}
		
		if(stateMachine.CurrentState is not ReachingResourceState reachingResourceState)
			return;
		
		if(reachingResourceState.resourcePoint.ResourceType != resourcePoint.ResourceType)
			return;

		if(resourcePoint.IsBusy)
			return;
		
		stateMachine.SwitchState(new BusyByResourceState(resourcePoint, this));
	}
	
	public void OnResourceCollected(ResourcePoint resourcePoint)
	{
		if(stateMachine.CurrentState is not BusyByResourceState busyByResourceState)
			return;
		
		if(busyByResourceState.resourcePoint.ResourceType != resourcePoint.ResourceType)
			return;
		
		stateMachine.SwitchState(new ReachingPcState(this, _resourcesService.Value.GetPcPoint(workerId)));
	}
	
	public void OnBeingDragged()
	{
		if(stateMachine.CurrentState is ReachingPcState reachingPcState)
		{
			stateMachine.SwitchState(new BeingDraggedState(this, _inputService.Value, reachingPcState));
			return;
		}

		if(stateMachine.CurrentState is ReachingResourceState reachingResourceState)
		{
			stateMachine.SwitchState(new BeingDraggedState(this, _inputService.Value, reachingResourceState));
		}
	}
	
	public enum AnimationState 
	{
		Idle,
		WalkingTop,
		WalkingBottom,
		WalkingRight,
		WalkingLeft,
	}
}
