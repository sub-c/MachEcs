namespace SubC.MachEcs
{
    public sealed class MachSignature
    {
        internal const int MaxSupportedSignatures = sizeof(ulong) * 8;

        private ulong _bits;

        internal void Add(MachSignature signature)
        {
            _bits |= signature._bits;
        }

        internal void DisableBit(int position)
        {
            _bits &= ~(ulong)1 << position;
        }

        internal void EnableBit(int position)
        {
            _bits |= (ulong)1 << position;
        }

        internal bool Matches(MachSignature signature)
        {
            return (_bits & signature._bits) == signature._bits;
        }

        internal void Remove(MachSignature signature)
        {
            _bits &= ~signature._bits;
        }

        internal void Reset()
        {
            _bits = 0;
        }

        internal void Set(MachSignature signature)
        {
            _bits = signature._bits;
        }
    }
}
