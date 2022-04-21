using System.Collections.Generic;
using System.Diagnostics;

namespace SubC.MachEcs.Models
{
  internal sealed class ComponentCache<T> : IComponentCache
    where T : IEcsComponent
  {
    private readonly IDictionary<IEcsEntity, T> _cache = new Dictionary<IEcsEntity, T>();

    public void AddComponent(IEcsEntity entity, T component)
    {
      Debug.Assert(!_cache.ContainsKey(entity), $"Cannot add component: entity already has an instance of {typeof(T).Name}.");
      _cache.Add(entity, component);
    }

    public void EntityDestroyed(IEcsEntity entity)
    {
      _cache.Remove(entity);
    }

    public T GetComponent(IEcsEntity entity)
    {
      Debug.Assert(_cache.ContainsKey(entity), $"Cannot get component: entity does not have an instance of {typeof(T).Name}.");
      return _cache[entity];
    }

    public bool IsComponentAttachedToEntity(IEcsEntity entity)
    {
      return _cache.ContainsKey(entity);
    }
  }
}
