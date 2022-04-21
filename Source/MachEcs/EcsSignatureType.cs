namespace SubC.MachEcs
{
  /// <summary>
  /// This enumeration references ECS signature sizes; larger signatures support more components
  /// at once, but will be slower than smaller signatures.
  /// </summary>
  public enum EcsSignatureType
  {
    /// <summary>
    /// Supports up to 1000 components.
    /// </summary>
    BitArray1K,

    /// <summary>
    /// Supports up to 128 components.
    /// </summary>
    DoubleLong,

    /// <summary>
    /// Supports up to 64 components.
    /// </summary>
    SingleLong
  }
}
