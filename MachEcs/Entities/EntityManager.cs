using System.Collections.Generic;
using System.Diagnostics;

namespace SubC.MachEcs.Entities
{
    internal sealed class EntityManager
    {
        public int EntitiesActive => MaximumEntities - _entityQueue.Count;
        public int EntitiesFree => _entityQueue.Count;
        public int MaximumEntities { get; }

        private readonly Queue<MachEntity> _entityQueue = new Queue<MachEntity>();

        public EntityManager(int maximumEntities)
        {
            MaximumEntities = maximumEntities;
            for (var i = 0; i < maximumEntities; ++i)
            {
                _entityQueue.Enqueue(new MachEntity());
            }
        }

        public MachEntity CreateEntity()
        {
            Debug.Assert(_entityQueue.Count > 0, "All available entities are in use.");
            return _entityQueue.Dequeue();
        }

        public void DestroyEntity(MachEntity entity)
        {
            Debug.Assert(!_entityQueue.Contains(entity), "Entity is destroyed more than once.");
            entity.Signature.ResetSignature();
            _entityQueue.Enqueue(entity);
        }
    }
}
