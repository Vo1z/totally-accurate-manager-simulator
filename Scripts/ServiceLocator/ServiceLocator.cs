using System;
using System.Collections.Generic;

namespace Ingame.Service;

public sealed class ServiceLocator
{
	private static ServiceLocator _instance;
	private static ServiceLocator Instance => _instance ??= new ServiceLocator();

	private readonly Dictionary<Type, IGameService> _services = new();
	
	public static void Register<T>(T service) where T : IGameService
	{
		Instance._services.Add(typeof(T), service);
	}
	
	public static T Get<T>() where T : IGameService
	{
		return (T)Instance._services[typeof(T)];
	}
	
	public static void Reset()
	{
		Instance._services.Clear();
	}
}
