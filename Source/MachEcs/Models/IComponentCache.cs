namespace SubC.MachEcs.Models
{
  internal interface IComponentCache
  {
    void EntityDestroyed(IEcsEntity entity);
    bool IsComponentAttachedToEntity(IEcsEntity entity);
  }
}
