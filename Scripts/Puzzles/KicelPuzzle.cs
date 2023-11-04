using Godot;
using System;
using Ingame.GameSession;
using Ingame.Service;
using Ingame.Utils;

namespace Ingame.Puzzles;

public partial class KicelPuzzle : TextureRect
{
	[Export] private PuzzleController puzzleController;
	[Export] private Label formulaLabel;
	[Export] private TextEdit answer;

	private readonly Lazy<GameSessionService> _gameSessionService = new(ServiceLocator.Get<GameSessionService>);
	
	private int _currentCorrectResult;
	
	public void StartPuzzle()
	{
		int firstNumber = RndUtils.Range(1, 100);
		int secondNumber = RndUtils.Range(1, 100);
		var operation = RndUtils.RandomEnumValue<Operation>();

		formulaLabel.Text = $"{firstNumber} {GetOperationSign(operation)} {secondNumber} =";
		
		switch(operation)
		{
			case Operation.Addition:
				_currentCorrectResult = firstNumber + secondNumber;
				break;
			case Operation.Subtraction:
				_currentCorrectResult = firstNumber - secondNumber;
				break;
			case Operation.Multiplication:
				_currentCorrectResult = firstNumber * secondNumber;
				break;
		}
	}

	private void OnSubmitButtonPressed()
	{
		puzzleController.HideMonitor();
		
		if(int.TryParse(answer.Text, out int result) && result == _currentCorrectResult)
			_gameSessionService.Value.MakeRandomBoost();
	}

	private string GetOperationSign(Operation operation)
	{
		return operation switch
		{
			Operation.Addition => "+",
			Operation.Subtraction => "-",
			Operation.Multiplication => "*",
			_ => "+"
		};
	}

	private enum Operation
	{
		Addition,
		Subtraction,
		Multiplication
	}
}
