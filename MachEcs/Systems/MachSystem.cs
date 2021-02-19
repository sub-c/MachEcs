using System;
using System.Collections.Generic;
using MachEcs.Entities;

namespace MachEcs.Systems
{
    /// <summary>
    /// This class contains the necessary properties to facilitate a system in an entity-component-system program.
    /// Additionally, classes that inherit from this class will be automatically registered as a system when <see cref="MachAgent"/>
    /// registers systems via scanning an assembly.
    /// </summary>
    public abstract class MachSystem
    {
        /// <summary>
        /// The <see cref="MachAgent"/> that registered this system into its entity-component-system world.
        /// </summary>
        public MachAgent Agent { get; internal set; } = null;

        /// <summary>
        /// The entities whos signature match (at a minimum) the components in the <see cref="ComponentSignatureTypes"/> array.
        /// </summary>
        public List<MachEntity> Entities { get; } = new List<MachEntity>();

        /// <summary>
        /// The component types which this system is interested in working with; entities with components that match (at a minimum)
        /// the components in this array will be populated into <see cref="Entities"/> automatically.
        /// </summary>
        public abstract Type[] ComponentSignatureTypes { get; }

        internal MachSignature Signature { get; } = new MachSignature();
    }
}
