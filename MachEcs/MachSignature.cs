﻿namespace SubC.MachEcs
{
    /// <summary>
    /// This class holds the signature bits that represent components in the entity-component-system world.
    /// </summary>
    public sealed class MachSignature
    {
        internal const int MaxSignatureBits = sizeof(ulong) * 8;

        private ulong _bitSignature = 0;

        /// <summary>
        /// Adds the bits from the given signature.
        /// </summary>
        /// <param name="signature">The signature to add set bits from.</param>
        public void AddSignature(MachSignature signature)
            => _bitSignature |= signature._bitSignature;

        /// <summary>
        /// Removes the set bits from the given signature.
        /// </summary>
        /// <param name="signature">The signature to remove set bits from.</param>
        public void RemoveSignature(MachSignature signature)
            => _bitSignature &= ~signature._bitSignature;

        /// <summary>
        /// Get a human-readable string representation of the bit-signature.
        /// </summary>
        /// <returns>String representation of the bit-signature.</returns>
        public override string ToString()
        {
            var bitSignature = string.Empty;
            for (int i = 0; i < sizeof(ulong); ++i)
            {
                bitSignature += (_bitSignature & ((ulong)1 << i)) != 0 ? "1" : "0";
            }
            return bitSignature;
        }

        internal void DisableBit(int bitPosition)
            => _bitSignature &= ~(ulong)1 << bitPosition;

        internal void EnableBit(int bitPosition)
            => _bitSignature |= (ulong)1 << bitPosition;

        internal bool MatchesSignature(MachSignature signature)
            => (_bitSignature & signature._bitSignature) == signature._bitSignature;

        internal void ResetSignature()
            => _bitSignature = 0;

        internal void SetSignature(MachSignature signature)
            => _bitSignature = signature._bitSignature;
    }
}
