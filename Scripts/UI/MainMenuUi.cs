using Godot;
using System;
using Ingame.SceneManagement;
using Ingame.Service;

namespace Ingame.UI;

public partial class MainMenuUi : Control
{
	private readonly Lazy<SceneService> _gameSessionService = new(ServiceLocator.Get<SceneService>);
	
	private void OnPlayButtonPressed()
	{
		_gameSessionService.Value.LoadScene(SceneType.Gameplay);
	}
}
