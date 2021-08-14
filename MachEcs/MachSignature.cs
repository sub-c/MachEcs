using System.Diagnostics;

namespace SubC.MachEcs
{
    /// <summary>
    /// This class represents a grouping of components condensed into a (bit) signature for quick comparing with
    /// other signatures when checking for matching components.
    /// </summary>
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
            Debug.Assert(position >= 0, "Bit position to disable is not positive.");
            Debug.Assert(position < MaxSupportedSignatures, "Bit position to disable exceeds maximum supported signatures.");
            _bits &= ~(ulong)1 << position;
        }

        internal void EnableBit(int position)
        {
            Debug.Assert(position >= 0, "Bit position to enable is not positive.");
            Debug.Assert(position < MaxSupportedSignatures, "Bit position to enable exceeeds maximum supported signatures.");
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
