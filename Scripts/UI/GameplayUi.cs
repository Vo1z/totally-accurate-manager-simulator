using System;
using Godot;
using Ingame.GameSession;
using Ingame.Scripts;
using Ingame.Service;

namespace Ingame.UI;

public partial class GameplayUi : Control
{
	private readonly Lazy<GameSessionService> _gameSessionService = new(ServiceLocator.Get<GameSessionService>);

	[Export] private GameConfig gameConfig;
	[Export] private ProgressBar projectProgress;
	[Export] private Label daysLeftLabel;
	[Export] private Label clockLabel;
	[Export] private float progressTweenDuration = 1f;

	private Tween _currentTween;
	private int _currentSecondsLeftTillTheEndOfTheDay = 0;
	
	public override void _Ready()
	{
		_currentSecondsLeftTillTheEndOfTheDay = gameConfig.dayDuration;
		_gameSessionService.Value.OnGameProgressChanged += OnGameProgressChanged;
	}

	public override void _ExitTree()
	{
		_gameSessionService.Value.OnGameProgressChanged -= OnGameProgressChanged;
	}

	private void OnTimerTick()
	{
		_currentSecondsLeftTillTheEndOfTheDay--;
		clockLabel.Text = $"{_currentSecondsLeftTillTheEndOfTheDay / 60:00}:{_currentSecondsLeftTillTheEndOfTheDay % 60:00}";
		
		if(_currentSecondsLeftTillTheEndOfTheDay <= 0)
			_gameSessionService.Value.SkipDay();
	}
	
	private void OnGameProgressChanged(float progress, int daysLeft)
	{
		_currentTween?.Kill();
		_currentTween = CreateTween();
		_currentTween.TweenProperty(projectProgress, "value", progress, progressTweenDuration).SetEase(Tween.EaseType.InOut);
		
		daysLeftLabel.Text = $"{daysLeft} DAYS LEFT";
	}
}
