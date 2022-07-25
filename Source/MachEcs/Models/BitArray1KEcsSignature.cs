using System.Collections;
using System.Diagnostics;

namespace SubC.MachEcs.Models
{
  internal sealed class BitArray1KEcsSignature : EcsSignature<BitArray1KEcsSignature>
  {
    public override int MaximumSupportedBits => 1000;

    private readonly BitArray _bits = new BitArray(1000);

    public override void DisableBit(int position)
    {
      Debug.Assert(position >= 0 && position < MaximumSupportedBits, $"Bit position is out of range: {position}.");
      _bits.Set(position, false);
    }

    public override void DisableBitsFrom(BitArray1KEcsSignature signature)
    {
      for (var index = 0; index < signature._bits.Length; ++index)
      {
        if (signature._bits[index])
        {
          _bits.Set(index, false);
        }
      }
    }

    public override void EnableBit(int position)
    {
      Debug.Assert(position >= 0 && position < MaximumSupportedBits, $"Bit position is out of range: {position}.");
      _bits.Set(position, true);
    }

    public override void EnableBitsFrom(BitArray1KEcsSignature signature)
    {
      _bits.Or(signature._bits);
    }

    public override bool IsMatching(BitArray1KEcsSignature signature)
    {
      for (var index = 0; index < signature._bits.Length; ++index)
      {
        if (_bits[index] != signature._bits[index])
        {
          return false;
        }
      }
      return true;
    }

    public override void Reset()
    {
      _bits.SetAll(false);
    }
  }
}
