namespace SubC.MachEcs.Models
{
  internal abstract class EcsSignature
  {
    public abstract int MaximumSupportedBits { get; }
    public abstract void DisableBit(int position);
    public abstract void EnableBit(int position);
    public abstract void Reset();
  }

  internal abstract class EcsSignature<T> : EcsSignature
    where T : EcsSignature
  {
    public abstract void DisableBitsFrom(T signature);
    public abstract void EnableBitsFrom(T signature);
    public abstract bool IsMatching(T signature);
  }
}
