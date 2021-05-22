namespace SubC.MachEcs.Entities
{
    /// <summary>
    /// An entity that can have components attached to, and be populated into systems in an entity-component-system program.
    /// </summary>
    public sealed class MachEntity
    {
        internal MachSignature Signature { get; } = new MachSignature();
    }
}
