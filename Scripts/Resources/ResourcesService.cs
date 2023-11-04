using System.Collections.Generic;
using Ingame.Npc;
using Ingame.SceneManagement;
using Ingame.Service;
namespace Ingame.Resources;

public sealed class ResourcesService : IGameService
{
	private readonly SceneService _sceneService;
	private readonly Dictionary<ResourceType, ResourcePoint> _resourcePoints = new();
	private readonly Dictionary<WorkerId, PcPoint> _pcPoints = new();

	public ResourcesService(SceneService sceneService)
	{
		_sceneService = sceneService;
		sceneService.OnSceneStartedLoading += OnSceneStartedLoading;
	}
	
	~ResourcesService()
	{
		_sceneService.OnSceneStartedLoading -= OnSceneStartedLoading;
	}

	private void OnSceneStartedLoading(SceneType _)
	{
		_resourcePoints.Clear();
		_pcPoints.Clear();
	}

	public void RegisterResourcePoint(ResourcePoint resourcePoint)
	{
		_resourcePoints.Add(resourcePoint.ResourceType, resourcePoint);
	}
	
	public void RegisterPcPoint(PcPoint pcPoint)
	{
		_pcPoints.Add(pcPoint.WorkerId, pcPoint);
	}
	
	public ResourcePoint GetResourcePoint(ResourceType resourceType)
	{
		return _resourcePoints[resourceType];
	}
	
	public PcPoint GetPcPoint(WorkerId workerId)
	{
		return _pcPoints[workerId];
	}
}
