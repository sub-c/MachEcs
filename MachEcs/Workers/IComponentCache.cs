namespace SubC.MachEcs.Workers
{
    internal interface IComponentCache
    {
        MachSignature Signature { get; }

        void EntityDestroyed(MachEntity entity);
    }
}
