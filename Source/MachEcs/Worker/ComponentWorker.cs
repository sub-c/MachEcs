using SubC.MachEcs.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SubC.MachEcs.Worker
{
  internal class ComponentWorker<S>
    where S : EcsSignature<S>, new()
  {
    private readonly IDictionary<Type, IComponentCache> _caches = new Dictionary<Type, IComponentCache>();
    private readonly IDictionary<Type, S> _cacheSignatures = new Dictionary<Type, S>();
    private int _currentSignatureBit;

    public void AddComponent<T>(IEcsEntity entity, T component)
      where T : IEcsComponent
    {
      GetComponentCache<T>().AddComponent(entity, component);
    }

    public void EntityDestroyed(IEcsEntity entity)
    {
      foreach (var cache in _caches)
      {
        cache.Value.EntityDestroyed(entity);
      }
    }

    public T GetComponent<T>(IEcsEntity entity)
      where T : IEcsComponent
    {
      return GetComponentCache<T>().GetComponent(entity);
    }

    public S GetComponentSignature(Type componentType)
    {
      Debug.Assert(_cacheSignatures.ContainsKey(componentType), $"Cannot get component signature: {componentType.Name} is not registered.");
      return _cacheSignatures[componentType];
    }

    public bool IsComponentAttachedToEntity<T>(IEcsEntity entity)
      where T : IEcsComponent
    {
      return GetComponentCache(typeof(T)).IsComponentAttachedToEntity(entity);
    }

    public void RegisterComponent<T>()
      where T : IEcsComponent
    {
      if (_caches.ContainsKey(typeof(T)))
      {
        return;
      }
      var cache = new ComponentCache<T>();
      _caches.Add(typeof(T), cache);
      var signature = new S();
      if (_currentSignatureBit >= signature.MaximumSupportedBits)
      {
        throw new Exception($"Exceeded the maximum amount of unique signatures ({typeof(S).Name} supports up to {signature.MaximumSupportedBits}). Use a larger signature type when creating the agent.");
      }
      signature.EnableBit(_currentSignatureBit++);
      _cacheSignatures.Add(typeof(T), signature);
    }

    public void RemoveComponent<T>(IEcsEntity entity)
      where T : IEcsComponent
    {
      GetComponentCache(typeof(T)).EntityDestroyed(entity);
    }

    private ComponentCache<T> GetComponentCache<T>()
      where T : IEcsComponent
    {
      Debug.Assert(_caches.ContainsKey(typeof(T)), $"Cannot get component cache: {typeof(T).Name} is not registered.");
      return (ComponentCache<T>)_caches[typeof(T)];
    }

    private IComponentCache GetComponentCache(Type componentType)
    {
      Debug.Assert(_caches.ContainsKey(componentType), $"Cannot get component cache: {componentType.Name} is not registered.");
      return _caches[componentType];
    }
  }
}
