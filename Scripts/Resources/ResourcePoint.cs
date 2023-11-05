using Godot;
using Ingame.Npc;
using Ingame.Scripts;
using Ingame.Service;
using Ingame.Utils;

namespace Ingame.Resources;

public partial class ResourcePoint : Node2D
{
	[Export] private GameConfig gameConfig;
	[Export] private ResourceType resourceType;
	
	[Export] private CpuParticles2D particles;
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
		SetFree();
	}
	
	public override void _Process(double delta)
	{
		_currentProgress += delta;
		progressBar.Value = _currentProgress / _currentFinalValue;

		if(_currentProgress > _currentFinalValue && _worker != null)
		{
			_worker.OnResourceCollected(this);
			SetFree();
		}
	}

	private void OnSpeedUpButtonPressed()
	{
		particles.Emitting = true;
		_currentProgress += gameConfig.speedUpValue;
	}
	
	private void OnBodyEntered(Node2D body)
	{
		if(body is not Worker worker)
			return;
		
		worker.OnResourcePointEntered(this);
	}
	
	private void OnParticlesTurnOffTimerTimeout()
	{
		particles.Emitting = false;
	}

	public void SetBusy(Worker worker)
	{
		if(IsBusy)
			throw new System.Exception("Resource point is already busy");
		
		IsBusy = true;
		_worker = worker;
		_currentProgress = 0f;
		_currentFinalValue = RndUtils.Range(gameConfig.resourceCollectionDurationRange);
		
		progressBar.Show();
		speedUpButton.Show();
	}

	public void SetFree()
	{
		IsBusy = false;
		_worker = null;
		
		progressBar.Hide();
		speedUpButton.Hide();
	}
}
