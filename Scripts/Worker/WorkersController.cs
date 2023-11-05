using System;
using System.Collections.Generic;
using Godot;
using Ingame.GameSession;
using Ingame.Service;
using Ingame.Utils;

namespace Ingame.Npc;

public partial class WorkersController : Node2D
{
	private static readonly int MAX_AMOUNT_OF_WORKERS = Enum.GetValues<WorkerId>().Length;
	
	[Export] private PackedScene workerScene;
	[Export] private Node2D workerSpawnPoint;
	[Export] private float maxSpawnOffset;

	private readonly Lazy<GameSessionService> _gameSessionService = new(ServiceLocator.Get<GameSessionService>);
	private readonly List<Worker> _spawnedWorkers = new();
	
	public override void _Ready()
	{
		_gameSessionService.Value.OnAmountOfWorkerUpdated += OnAmountOfWorkerUpdated;
		_gameSessionService.Value.OnWorkerSpeedBoostUpdated += OnWorkerSpeedBoostUpdated;
		
		OnAmountOfWorkerUpdated(_gameSessionService.Value.CurrentAmountOfWorkers);
		OnWorkerSpeedBoostUpdated(_gameSessionService.Value.CurrentWorkerSpeedBoost);
	}

	public override void _ExitTree()
	{
		_gameSessionService.Value.OnAmountOfWorkerUpdated -= OnAmountOfWorkerUpdated;
		_gameSessionService.Value.OnWorkerSpeedBoostUpdated -= OnWorkerSpeedBoostUpdated;
	}

	private void OnAmountOfWorkerUpdated(int amountOfWorkers)
	{
		if(_spawnedWorkers.Count >= amountOfWorkers)
			return;
		
		int amountToSpawn = amountOfWorkers - _spawnedWorkers.Count;

		for(int i = 0; i < amountToSpawn && _spawnedWorkers.Count < MAX_AMOUNT_OF_WORKERS; i++)
		{
			var workerId = (WorkerId) _spawnedWorkers.Count;
			var workerInstance = workerScene.Instantiate<Worker>();
			var spawnPosition = workerSpawnPoint.GlobalPosition;
			spawnPosition.X += RndUtils.Range(-maxSpawnOffset, maxSpawnOffset);
			workerInstance.workerId = workerId;
			
			_spawnedWorkers.Add(workerInstance);
			
			AddChild(workerInstance);
			workerInstance.GlobalPosition = spawnPosition;
		}
	}
	
	private void OnWorkerSpeedBoostUpdated(int currentSpeedBoost)
	{
		foreach(var worker in _spawnedWorkers) 
			worker.CurrentSpeedBoost = currentSpeedBoost;
	}
}