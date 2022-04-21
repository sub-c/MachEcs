using SubC.MachEcs.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace SubC.MachEcs.Worker
{
  internal sealed class SystemWorker<S>
    where S : EcsSignature<S>, new()
  {
    private readonly IDictionary<Type, EcsSystem> _systems = new Dictionary<Type, EcsSystem>();
    private readonly IDictionary<Type, S> _systemSignatures = new Dictionary<Type, S>();

    public void EntityDestroyed(IEcsEntity entity)
    {
      foreach (var system in _systems)
      {
        system.Value.InternalEntities.Remove(entity);
      }
    }

    public void EntitySignatureChanged(IEcsEntity entity, S entitySignature)
    {
      foreach (var systemSignature in _systemSignatures)
      {
        if (entitySignature.IsMatching(systemSignature.Value))
        {
          _systems[systemSignature.Key].InternalEntities.Add(entity);
        }
        else
        {
          _systems[systemSignature.Key].InternalEntities.Remove(entity);
        }
      }
    }

    public T GetSystem<T>()
      where T : EcsSystem
    {
      Debug.Assert(_systems.ContainsKey(typeof(T)), $"Cannot get system: System not registered before use: {typeof(T).Name}.");
      return (T)_systems[typeof(T)];
    }

    public T RegisterSystem<T>(Agent<S> agent)
      where T : EcsSystem
    {
      Debug.Assert(!_systemSignatures.Any(x => x.Key.GetType() == typeof(T)), $"Cannot register system: {typeof(T).Name} is already registered.");
      var system = CreateSystemInstance<T>(agent);
      var systemComponentTypes = GetSystemComponentTypes(system);
      RegisterSystemComponentTypes(systemComponentTypes, agent);
      var systemSignature = GetSystemSignature(systemComponentTypes, agent);
      _systems.Add(typeof(T), system);
      _systemSignatures.Add(typeof(T), systemSignature);
      return system;
    }

    private static T CreateSystemInstance<T>(Agent<S> agent)
      where T : EcsSystem
    {
      var systemConstructor = typeof(T).GetConstructor(BindingFlags.Instance | BindingFlags.Public, new Type[] { typeof(Agent) })
        ?? throw new NullReferenceException($"Cannot register system: constructor does not take a single agent parameter.");
      var system = systemConstructor.Invoke(new object[] { agent })
        ?? throw new NullReferenceException($"Cannot register system: constructor did not successfully invoke.");
      return (T)system;
    }

    private static IEnumerable<Type> GetSystemComponentTypes(EcsSystem system)
    {
      return system.GetType().BaseType?.GetGenericArguments()
        ?? throw new NullReferenceException($"Cannot set system signature: base type is null.");
    }

    private static S GetSystemSignature(IEnumerable<Type> componentTypes, Agent<S> agent)
    {
      var signature = new S();
      foreach (var componentType in componentTypes)
      {
        var componentSignature = agent.GetComponentSignature(componentType);
        signature.EnableBitsFrom(componentSignature);
      }
      return signature;
    }

    private void RegisterSystemComponentTypes(IEnumerable<Type> types, Agent agent)
    {
      var registerMethodInfo = typeof(Agent).GetMethod(nameof(Agent.RegisterComponent))
        ?? throw new NullReferenceException("Cannot register system component: Cannot get register method info.");
      foreach (var type in types)
      {
        var registerMethod = registerMethodInfo.MakeGenericMethod(new Type[] { type });
        registerMethod.Invoke(agent, null);
      }
    }
  }
}
