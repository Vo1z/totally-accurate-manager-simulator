using Godot;
using System;
using Ingame.GameSession;
using Ingame.Service;
using Ingame.Utils;

namespace Ingame.Puzzles;

public partial class BabchaPuzzle : TextureRect
{
	private const int LAST_BUTTON_INDEX = 9;
	
	[Export] private PuzzleController puzzleController;
	[Export] private Button[] buttons;

	private readonly Lazy<GameSessionService> _gameSessionService = new(ServiceLocator.Get<GameSessionService>);
	
	private int _currentExpectedInput;

	public void StartPuzzle()
	{
		foreach(var button in buttons)
		{
			button.Disabled = false;
			MoveChild(button, RndUtils.Range(0, LAST_BUTTON_INDEX));
		}

		_currentExpectedInput = 1;
	}

	private void Evaluate(int currentInput)
	{
		if(currentInput != _currentExpectedInput)
		{
			puzzleController.HideMonitor();
			return;
		}

		if(currentInput == LAST_BUTTON_INDEX)
		{
			puzzleController.HideMonitor();
			_gameSessionService.Value.MakeRandomBoost();
			return;
		}

		_currentExpectedInput++;
	}

	private void OnButtonPressed1()
	{
		buttons[1].Disabled = true;
		Evaluate(1);
	}

	private void OnButtonPressed2()
	{
		buttons[2].Disabled = true;
		Evaluate(2);
	}

	private void OnButtonPressed3()
	{
		buttons[3].Disabled = true;
		Evaluate(3);
	}

	private void OnButtonPressed4()
	{
		buttons[4].Disabled = true;
		Evaluate(4);
	}
	
	private void OnButtonPressed5()
	{
		buttons[5].Disabled = true;
		Evaluate(5);
	}
	
	private void OnButtonPressed6()
	{
		buttons[6].Disabled = true;
		Evaluate(6);
	}
	
	private void OnButtonPressed7()
	{
		buttons[7].Disabled = true;
		Evaluate(7);
	}
	
	private void OnButtonPressed8()
	{
		buttons[8].Disabled = true;
		Evaluate(8);
	}
	
	private void OnButtonPressed9()
	{
		buttons[9].Disabled = true;
		Evaluate(9);
	}
}
