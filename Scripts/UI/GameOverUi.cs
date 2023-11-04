using Godot;
using System;
using Ingame.GameSession;
using Ingame.SceneManagement;
using Ingame.Service;

namespace Ingame.UI;

public partial class GameOverUi : Control
{
	[Export] private string victoryText;
	[Export] private string failureText;
	[Export] private Label resultLabel;
	
	private readonly Lazy<GameSessionService> _gameSessionService = new(ServiceLocator.Get<GameSessionService>);
	private readonly Lazy<SceneService> _sceneService = new(ServiceLocator.Get<SceneService>);

	public override void _Ready()
	{
		string resultText = _gameSessionService.Value.CurrentState == GameSessionState.EndedWithVictory ? victoryText : failureText;
		resultLabel.Text = resultText;
	}
	
	private void OnContinuePressed()
	{
		_sceneService.Value.LoadScene(SceneType.MainMenu);
	}
}
