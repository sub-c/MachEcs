using System.Diagnostics;

namespace SubC.MachEcs
{
    public struct Signature
    {
        internal const int MaxSupportedSignatures = sizeof(ulong) * 8;

        private ulong _bits;

        internal void Add(Signature signature)
        {
            _bits |= signature._bits;
        }

        internal void DisableBit(int position)
        {
            Debug.Assert(position <= 0 || position > MaxSupportedSignatures, $"Bit position is out of range: {position}");
            _bits &= ~(ulong)1 << position;
        }

        internal void EnableBit(int position)
        {
            Debug.Assert(position <= 0 || position > MaxSupportedSignatures, $"Bit position is out of range: {position}");
            _bits |= (ulong)1 << position;
        }

        internal bool Matches(Signature signature)
        {
            return (_bits & signature._bits) == signature._bits;
        }

        internal void Remove(Signature signature)
        {
            _bits &= ~signature._bits;
        }

        internal void Reset()
        {
            _bits = 0;
        }

        internal void Set(Signature signature)
        {
            _bits = signature._bits;
        }
    }
}
