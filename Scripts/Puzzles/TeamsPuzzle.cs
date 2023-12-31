using Godot;
using System;
using Ingame.Audio;
using Ingame.GameSession;
using Ingame.Service;
using Ingame.Utils;

namespace Ingame.Puzzles;

public partial class TeamsPuzzle : TextureRect
{
	[Export] private PuzzleController puzzleController;
	[Export] private TextureRect callerAvatarTextureRect;
	[Export] private Label callerNameLabel;
	
	[Export] private Texture2D clientTexture;
	[Export] private Texture2D hrTexture;
	[Export] private Texture2D bossTexture;
	[Export] private Texture2D juniorTexture;
	[Export] private Texture2D catTexture;
	
	[ExportCategory("Audio")]
	[Export] private AudioStreamPlayer teamsCallSound;

	private readonly Lazy<GameSessionService> _gameSessionService = new(ServiceLocator.Get<GameSessionService>);
	private readonly Lazy<AudioService> _audioService = new(ServiceLocator.Get<AudioService>);
	
	private Caller? _currentCaller;
	
	public void StartPuzzle()
	{
		GD.Print("Teams puzzle started");
		var caller = RndUtils.RandomEnumValue<Caller>();
		var callerTexture = GetCallerTexture(caller);
		string callerName = GetCallerName(caller);

		teamsCallSound.Play();
		
		_currentCaller = caller;
		callerAvatarTextureRect.Texture = callerTexture;
		callerNameLabel.Text = callerName;
	}
	
	private void OnAcceptPressed()
	{
		_audioService.Value.PlaySound(AudioClip.UiClick);
		teamsCallSound.Stop();
		puzzleController.HideMonitor();
		puzzleController.ResetPuzzleTimer();
		
		if(_currentCaller == null)
			return;
		
		if(GetCallOutcome(_currentCaller.Value))
			_gameSessionService.Value.MakeRandomBoost();
	}
	
	private void OnDeclinePressed()
	{
		_audioService.Value.PlaySound(AudioClip.UiClick);
		teamsCallSound.Stop();
		puzzleController.ResetPuzzleTimer();
		puzzleController.HideMonitor();
		
		if(_currentCaller == null)
			return;
		
		if(!GetCallOutcome(_currentCaller.Value))
			_gameSessionService.Value.MakeRandomBoost();
	}
	
	private string GetCallerName(Caller caller)
	{
		return caller switch
		{
			Caller.Client => "CLIENT",
			Caller.Hr => "HR",
			Caller.Boss => "BOSS",
			Caller.Junior => "JUNIOR DEV",
			Caller.Cat => "MY CAT",
			_ => string.Empty
		};
	}

	private Texture2D GetCallerTexture(Caller caller)
	{
		return caller switch
		{
			Caller.Client => clientTexture,
			Caller.Hr => hrTexture,
			Caller.Boss => bossTexture,
			Caller.Junior => juniorTexture,
			Caller.Cat => catTexture,
			_ => null
		};
	}

	private bool GetCallOutcome(Caller caller)
	{
		return caller switch
		{
			Caller.Client => true,
			Caller.Hr => true,
			Caller.Boss => true,
			Caller.Junior => false,
			Caller.Cat => false,
			_ => false
		};
	}

	private enum Caller
	{
		Client,
		Hr,
		Boss,
		Junior,
		Cat
	}
}
