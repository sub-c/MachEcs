namespace SubC.MachEcs
{
    /// <summary>
    /// This class represents an entity, a logical grouping of components.
    /// <para>
    /// To create and destroy entities, use the <see cref="MachAgent.CreateEntity"/> and
    /// <see cref="MachAgent.DestroyEntity(MachEntity)"/> methods.
    /// </para>
    /// </summary>
    public sealed class MachEntity
    {
        internal MachSignature Signature = new MachSignature();

        internal MachEntity()
        {
        }
    }
}
