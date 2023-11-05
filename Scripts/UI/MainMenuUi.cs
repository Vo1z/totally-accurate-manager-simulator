using Godot;
using System;
using Ingame.Audio;
using Ingame.SceneManagement;
using Ingame.Service;

namespace Ingame.UI;

public partial class MainMenuUi : Control
{
	private readonly Lazy<SceneService> _gameSessionService = new(ServiceLocator.Get<SceneService>);
	private readonly Lazy<AudioService> _audioService = new(ServiceLocator.Get<AudioService>);

	private void OnPlayButtonPressed()
	{
		_audioService.Value.PlaySound(AudioClip.UiClick);
		_gameSessionService.Value.LoadScene(SceneType.IntroScene);
	}

	private void OnExitButtonPressed()
	{
		GetTree().Quit();
	}
}
