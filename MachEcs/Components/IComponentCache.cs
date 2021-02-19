using MachEcs.Entities;

namespace MachEcs.Components
{
    internal interface IComponentCache
    {
        MachSignature Signature { get; }

        void EntityDestroyed(MachEntity entity);
    }
}
