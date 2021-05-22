using SubC.MachEcs.Entities;

namespace SubC.MachEcs.Components
{
    internal interface IComponentCache
    {
        MachSignature Signature { get; }

        void EntityDestroyed(MachEntity entity);
    }
}
