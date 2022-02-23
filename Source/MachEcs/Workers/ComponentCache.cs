using System.Collections.Generic;
using System.Diagnostics;

namespace SubC.MachEcs.Workers
{
    internal sealed class ComponentCache<T> : IComponentCache
        where T : IComponent
    {
        public Signature Signature { get; } = new Signature();

        private readonly IDictionary<Entity, T> _cache = new Dictionary<Entity, T>();

        public void AddComponent(Entity entity, T component)
        {
            Debug.Assert(!_cache.ContainsKey(entity), $"Cannot add component, entity already contains instance of {typeof(T).Name}");
            _cache.Add(entity, component);
        }

        public bool DoesEntityHaveComponent(Entity entity)
        {
            return _cache.ContainsKey(entity);
        }

        public void EntityDestroyed(Entity entity)
        {
            if (_cache.ContainsKey(entity))
            {
                _cache.Remove(entity);
            }
        }

        public T GetComponent(Entity entity)
        {
            Debug.Assert(_cache.ContainsKey(entity), $"Cannot get component, entity does not have an instance of {typeof(T).Name}");
            return _cache[entity];
        }

        public void RemoveComponent(Entity entity)
        {
            Debug.Assert(_cache.ContainsKey(entity), $"Cannot remove component, entity does not have an instance of {typeof(T).Name}");
            _cache.Remove(entity);
        }
    }
}
