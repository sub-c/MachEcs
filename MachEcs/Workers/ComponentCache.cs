using System.Collections.Generic;
using System.Diagnostics;

namespace SubC.MachEcs.Workers
{
    internal sealed class ComponentCache<T> : IComponentCache
        where T : IMachComponent
    {
        public MachSignature Signature { get; } = new MachSignature();

        private readonly IDictionary<MachEntity, T> _cache = new Dictionary<MachEntity, T>();

        public void AddComponent(MachEntity entity, T component)
        {
            Debug.Assert(!_cache.ContainsKey(entity), $"Cannot add component: entity already has a {typeof(T).Name} component added to it.");
            _cache.Add(entity, component);
        }

        public void EntityDestroyed(MachEntity entity)
        {
            if (_cache.ContainsKey(entity))
            {
                _cache.Remove(entity);
            }
        }

        public T GetComponent(MachEntity entity)
        {
            Debug.Assert(_cache.ContainsKey(entity), $"Cannot retrieve component: entity does not have a {typeof(T).Name} component added to it.");
            return _cache[entity];
        }

        public void RemoveComponent(MachEntity entity)
        {
            Debug.Assert(_cache.ContainsKey(entity), $"Cannot remove component: entity does not have a {typeof(T).Name} component added to it.");
            _cache.Remove(entity);
        }
    }
}
