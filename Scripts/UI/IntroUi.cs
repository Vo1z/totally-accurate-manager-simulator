using Godot;
using System;
using Ingame.Audio;
using Ingame.SceneManagement;
using Ingame.Service;

namespace Ingame.UI;

public partial class IntroUi : Control
{
	[Export] private Control firstFrame;
	[Export] private Control secondFrame;
	
	private readonly Lazy<SceneService> _sceneService = new(ServiceLocator.Get<SceneService>);
	private readonly Lazy<AudioService> _audioService = new(ServiceLocator.Get<AudioService>);

	public override void _Ready()
	{
		_audioService.Value.PlaySound(AudioClip.Cinematic);
		firstFrame.Show();
		secondFrame.Hide();
	}

	private void OnFirstButtonPressed()
	{
		_audioService.Value.PlaySound(AudioClip.Cinematic);
		firstFrame.Hide();
		secondFrame.Show();
	}
	
	private void OnSecondButtonPressed()
	{
		_audioService.Value.PlaySound(AudioClip.UiClick);
		_sceneService.Value.LoadScene(SceneType.Gameplay);
	}
}
