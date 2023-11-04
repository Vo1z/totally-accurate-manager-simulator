using Godot;
using System;
using Ingame.GameSession;
using Ingame.SceneManagement;
using Ingame.Service;

namespace Ingame.UI;

public partial class DayPassedUi : Control
{
	[Export] private Label daysLeftLabel;
	
	private readonly Lazy<GameSessionService> _gameSessionService = new(ServiceLocator.Get<GameSessionService>);
	private readonly Lazy<SceneService> _sceneService = new(ServiceLocator.Get<SceneService>);
	
	public override void _Ready()
	{
		daysLeftLabel.Text = $"DAYS TILL DEADLINE: {_gameSessionService.Value.DaysLeft}";
	}
	
	private void OnScreenTimeTimeout()
	{
		_sceneService.Value.LoadScene(SceneType.Gameplay);
	}
}
