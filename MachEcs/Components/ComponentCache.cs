using System.Collections.Generic;
using System.Diagnostics;
using SubC.MachEcs.Entities;

namespace SubC.MachEcs.Components
{
    internal sealed class ComponentCache<T> : IComponentCache
        where T : IMachComponent
    {
        public MachSignature Signature { get; } = new MachSignature();

        private readonly Dictionary<MachEntity, T> _componentCache = new Dictionary<MachEntity, T>();

        public void EntityDestroyed(MachEntity entity)
        {
            if (_componentCache.ContainsKey(entity))
            {
                RemoveEntity(entity);
            }
        }

        public T GetComponent(MachEntity entity)
        {
            Debug.Assert(_componentCache.ContainsKey(entity), $"The given entity does not have the expected component to retrieve.");
            return _componentCache[entity];
        }

        public void InsertComponent(MachEntity entity, T component)
        {
            Debug.Assert(!_componentCache.ContainsKey(entity), $"The given entity already contains a component of the given type.");
            _componentCache.Add(entity, component);
        }

        public void RemoveEntity(MachEntity entity)
        {
            Debug.Assert(_componentCache.ContainsKey(entity), $"The given entity does not have the expected component to remove.");
            _componentCache.Remove(entity);
        }
    }
}
