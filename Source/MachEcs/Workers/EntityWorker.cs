using System.Collections.Generic;
using System.Diagnostics;

namespace SubC.MachEcs.Workers
{
    internal sealed class EntityWorker
    {
        private readonly Queue<Entity> _entities;
        private readonly int _maximumEntities;

        public EntityWorker(int maximumEntities)
        {
            _entities = new Queue<Entity>(maximumEntities);
            for (var currentCount = 0; currentCount < maximumEntities; ++currentCount)
            {
                _entities.Enqueue(new Entity());
            }
            _maximumEntities = maximumEntities;
        }

        public Entity CreateEntity()
        {
            Debug.Assert(_entities.Count > 0, $"Cannot create entity, exceeded maximum amount of entities: {_maximumEntities}");
            return _entities.Dequeue();
        }

        public void DestroyEntity(Entity entity)
        {
            entity.Signature.Reset();
            _entities.Enqueue(entity);
        }
    }
}
