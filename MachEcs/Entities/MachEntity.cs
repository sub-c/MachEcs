namespace SubC.MachEcs.Entities
{
    /// <summary>
    /// An entity that can be associated with components, and then be populated into systems that match (at a
    /// minimum) those components.
    /// </summary>
    public sealed class MachEntity
    {
        internal MachSignature Signature { get; } = new MachSignature();
    }
}
