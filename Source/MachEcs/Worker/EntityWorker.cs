using SubC.MachEcs.Models;
using System.Collections.Generic;
using System.Diagnostics;

namespace SubC.MachEcs.Worker
{
  internal sealed class EntityWorker<S>
    where S : EcsSignature<S>, new()
  {
    private readonly Queue<IEcsEntity> _availableEntities;
    private readonly IDictionary<IEcsEntity, S> _entitySignatures;

    public EntityWorker(int maximumEntities)
    {
      _availableEntities = new Queue<IEcsEntity>(maximumEntities);
      _entitySignatures = new Dictionary<IEcsEntity, S>(maximumEntities);
      for (var index = 0; index < maximumEntities; ++index)
      {
        var newEntity = new EcsEntity();
        _availableEntities.Enqueue(newEntity);
        _entitySignatures.Add(newEntity, new S());
      }
    }

    public IEcsEntity CreateEntity()
    {
      Debug.Assert(_availableEntities.Count > 0, "Cannot create entity: exceeded maximum amount of entities.");
      return _availableEntities.Dequeue();
    }

    public void DestroyEntity(IEcsEntity entity)
    {
      Debug.Assert(_availableEntities.Contains(entity) == false, "Cannot destroy entity: entity is already destroyed.");
      _entitySignatures[entity].Reset();
      _availableEntities.Enqueue(entity);
    }

    public S GetEntitySignature(IEcsEntity entity)
    {
      return _entitySignatures[entity];
    }
  }
}
