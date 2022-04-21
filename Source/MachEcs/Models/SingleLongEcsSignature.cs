using System.Diagnostics;

namespace SubC.MachEcs.Models
{
  internal sealed class SingleLongEcsSignature : EcsSignature<SingleLongEcsSignature>
  {
    private long _bits;

    public override int MaximumSupportedBits { get; } = 64;

    public override void DisableBit(int position)
    {
      Debug.Assert(position >= 0 && position < MaximumSupportedBits, $"Bit position is out of range: {position}.");
      _bits &= ~(1 << position);
    }

    public override void DisableBitsFrom(SingleLongEcsSignature signature)
    {
      _bits &= ~signature._bits;
    }

    public override void EnableBit(int position)
    {
      Debug.Assert(position >= 0 && position < MaximumSupportedBits, $"Bit position is out of range: {position}.");
      _bits |= (long)(1 << position);
    }

    public override void EnableBitsFrom(SingleLongEcsSignature signature)
    {
      _bits |= signature._bits;
    }

    public override bool IsMatching(SingleLongEcsSignature signature)
    {
      return (_bits & signature._bits) == signature._bits;
    }

    public override void Reset()
    {
      _bits = 0;
    }
  }
}
