using System;
using Godot;
using Ingame.Service;

namespace Ingame.Audio;

public partial class AudioService : Node, IGameService
{
	[Export] private AudioStreamPlayer uiClick;
	[Export] private AudioStreamPlayer cinematic;
	
	public void PlaySound(AudioClip audioClip)
	{
		switch(audioClip)
		{
			case AudioClip.UiClick:
				uiClick.Play();
				break;
			case AudioClip.Cinematic:
				cinematic.Play();
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(audioClip), audioClip, null);
		}
	}
}

public enum AudioClip
{
	UiClick,
	Cinematic
}
