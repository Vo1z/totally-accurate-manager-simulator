using System.Collections.Generic;
using Ingame.SceneManagement;
using Ingame.Service;
namespace Ingame.Resources;

public sealed class ResourcesService : IGameService
{
	private readonly SceneService _sceneService;
	private readonly Dictionary<ResourceType, ResourcePoint> _resourcePoints = new();

	public ResourcesService(SceneService sceneService)
	{
		_sceneService = sceneService;
		sceneService.OnSceneLoaded += OnSceneLoaded;
	}
	
	~ResourcesService()
	{
		_sceneService.OnSceneLoaded -= OnSceneLoaded;
	}

	private void OnSceneLoaded(SceneType _)
	{
		_resourcePoints.Clear();
	}

	public void RegisterResourcePoint(ResourcePoint resourcePoint)
	{
		_resourcePoints.Add(resourcePoint.ResourceType, resourcePoint);
	}
}
