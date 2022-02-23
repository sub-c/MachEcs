namespace SubC.MachEcs.Workers
{
    internal interface IComponentCache
    {
        Signature Signature { get; }

        bool DoesEntityHaveComponent(Entity entity);

        void EntityDestroyed(Entity entity);

        void RemoveComponent(Entity entity);
    }
}
