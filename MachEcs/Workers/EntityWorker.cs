using System.Collections.Generic;
using System.Diagnostics;

namespace SubC.MachEcs.Workers
{
    internal sealed class EntityWorker
    {
        private readonly Queue<MachEntity> _entities;

        public EntityWorker(int maximumEntities)
        {
            _entities = new Queue<MachEntity>(maximumEntities);
            for (int currentCount = 0; currentCount < maximumEntities; ++currentCount)
            {
                _entities.Enqueue(new MachEntity());
            }
        }

        public MachEntity CreateEntity()
        {
            Debug.Assert(_entities.Count > 0, "Cannot create entity: exceeded maximum amount of entities.");
            return _entities.Dequeue();
        }

        public void DestroyEntity(MachEntity entity)
        {
            entity.Signature.Reset();
            _entities.Enqueue(entity);
        }
    }
}
