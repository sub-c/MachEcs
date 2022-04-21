using SubC.MachEcs;
using System.Collections.Generic;

namespace MachEcs.Tests.TestModels
{
  public sealed class MovementSystem : EcsSystem<PositionComponent, VelocityComponent>
  {
    public MovementSystem(Agent agent) : base(agent)
    {
      agent.RegisterEventHandler<GetSystemEntitiesEvent>(HandleGetSystemEntitiesEvent);
    }

    public IEnumerable<IEcsEntity> GetAllSystemEntities()
    {
      return Entities;
    }

    private void HandleGetSystemEntitiesEvent(GetSystemEntitiesEvent getSystemEntitiesEvent)
    {
      getSystemEntitiesEvent.Entities = Entities;
    }
  }
}
