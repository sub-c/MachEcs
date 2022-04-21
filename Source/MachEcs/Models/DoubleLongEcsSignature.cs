using System.Diagnostics;

namespace SubC.MachEcs.Models
{
  internal sealed class DoubleLongEcsSignature : EcsSignature<DoubleLongEcsSignature>
  {
    public override int MaximumSupportedBits => 128;

    private long _bits1;
    private long _bits2;

    public override void DisableBit(int position)
    {
      Debug.Assert(position >= 0 && position < MaximumSupportedBits, $"Bit position is out of range: {position}.");
      if (position >= 64)
      {
        _bits2 &= ~(1 << (position % 64));
      }
      else
      {
        _bits1 &= ~(1 << position);
      }
    }

    public override void DisableBitsFrom(DoubleLongEcsSignature signature)
    {
      _bits1 &= ~signature._bits1;
      _bits2 &= ~signature._bits2;
    }

    public override void EnableBit(int position)
    {
      Debug.Assert(position >= 0 && position < MaximumSupportedBits, $"Bit position is out of range: {position}.");
      if (position >= 64)
      {
        _bits2 |= (long)(1 << (position % 64));
      }
      else
      {
        _bits1 |= (long)(1 << position);
      }
    }

    public override void EnableBitsFrom(DoubleLongEcsSignature signature)
    {
      _bits1 |= signature._bits1;
      _bits2 |= signature._bits2;
    }

    public override bool IsMatching(DoubleLongEcsSignature signature)
    {
      return (_bits1 & signature._bits1) == signature._bits1
        && (_bits2 & signature._bits2) == signature._bits2;
    }

    public override void Reset()
    {
      _bits1 = 0;
      _bits2 = 0;
    }
  }
}
