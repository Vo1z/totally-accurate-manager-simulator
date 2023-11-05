using Godot;
using Ingame.Scripts;
using Ingame.Utils;

namespace Ingame.Puzzles;

public partial class PuzzleController : Control
{
	[Export] private GameConfig gameConfig;
	[Export] private Timer puzzleEventTimer;
	[Export] private KicelPuzzle kicelPuzzle;
	[Export] private BabchaPuzzle babchaPuzzle;
	[Export] private TeamsPuzzle teamsPuzzle;

	[Export] private float showHideAnimationDuration;
	[Export] private Vector2 showPos;
	[Export] private Vector2 hidePos;

	private bool _isShown;
	private Tween _currentTween;
	
	public override void _Ready()
	{
		ResetPuzzleTimer();
		HideMonitor();
	}

	private void OnPuzzleEventTimeout()
	{
		if(_isShown)
			return;
		
		ShowMonitor();
		
		switch(RndUtils.Range(0, 3))
		{
			case 0:
				babchaPuzzle.Hide();
				kicelPuzzle.Show();
				teamsPuzzle.Hide();
				
				kicelPuzzle.StartPuzzle();
				break;
			case 1:
				babchaPuzzle.Show();
				kicelPuzzle.Hide();
				teamsPuzzle.Hide();
				
				babchaPuzzle.StartPuzzle();
				break;
			case 2:
				babchaPuzzle.Hide();
				kicelPuzzle.Hide();
				teamsPuzzle.Show();
				
				teamsPuzzle.StartPuzzle();
				break;
		}
	}
	
	public void ResetPuzzleTimer()
	{
		puzzleEventTimer.WaitTime = RndUtils.Range(gameConfig.minMaxPauseBetweenEvents);
		puzzleEventTimer.Start();
	}
	
	public void ShowMonitor()
	{
		_isShown = true;
		
		Position = hidePos;
		_currentTween?.Kill();
		_currentTween = CreateTween();
		_currentTween.TweenProperty(this, "modulate", new Color(1f, 1f, 1f, 1f), showHideAnimationDuration);
		_currentTween.TweenProperty(this, "position", showPos, showHideAnimationDuration);
	}
	
	public void HideMonitor()
	{
		_isShown = false;
		_currentTween?.Kill();
		_currentTween = CreateTween();
		_currentTween.TweenProperty(this, "modulate", new Color(1f, 1f, 1f, 0f), showHideAnimationDuration);
		_currentTween.TweenProperty(this, "position", hidePos, showHideAnimationDuration);
	}
}
