using Godot;
using Ingame.Npc;
using Ingame.Service;
using Ingame.Utils;
using sperasoftgamejam.Scripts;

namespace Ingame.Resources;

public partial class ResourcePoint : Node2D
{
	[Export] private GameConfig gameConfig;
	[Export] private ResourceType resourceType;
	
	[Export] private ProgressBar progressBar;
	[Export] private Button speedUpButton;
	
	public ResourceType ResourceType => resourceType;

	private double _currentProgress = 0f;
	private double _currentFinalValue = 0f;
	private Worker _worker;
	
	public bool IsBusy { get; private set; }

	public override void _EnterTree()
	{
		ServiceLocator.Get<ResourcesService>().RegisterResourcePoint(this);
	}

	public override void _Process(double delta)
	{
		_currentProgress += delta;
	}

	private void OnBodyEntered(Node2D body)
	{
		if(body is not Worker worker)
			return;
		
		worker.OnResourcePointEntered(this);
	}

	public void SetBusy(Worker worker)
	{
		if(IsBusy)
			throw new System.Exception("Resource point is already busy");
		
		IsBusy = true;
		_worker = worker;
		_currentProgress = 0f;
		_currentFinalValue = RndUtils.Range(gameConfig.ResourceCollectionDurationRange);
	}

	public void SetFree()
	{
		IsBusy = false;
		_worker = null;
	}
}
