using System;
using Godot;
using Ingame.Service;

namespace Ingame.Audio;

public partial class AudioService : Node, IGameService
{
	[Export] private AudioStreamPlayer uiClick;

	public void PlaySound(AudioClip audioClip)
	{
		switch(audioClip)
		{
			case AudioClip.UiClick:
				uiClick.Play();
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(audioClip), audioClip, null);
		}
	}
}

public enum AudioClip
{
	UiClick
}
