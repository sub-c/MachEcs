using System;
using System.Collections.Generic;
using SubC.MachEcs.Entities;

namespace SubC.MachEcs.Systems
{
    /// <summary>
    /// This abstract class represents a system in the entity-component-system architecture; system classes should
    /// inherit from this abstract class.
    /// </summary>
    public abstract class MachSystem
    {
        internal MachAgent InternalAgent { get; set; } = null;
        internal Type[] InternalComponentSignatureTypes => ComponentSignatureTypes;
        internal List<MachEntity> InternalEntities => Entities;
        internal MachSignature Signature { get; } = new MachSignature();

        /// <summary>
        /// The <see cref="MachAgent"/> that registered this system into its entity-component-system world.
        /// </summary>
        protected MachAgent Agent => InternalAgent;

        /// <summary>
        /// The entities whos signature match (at a minimum) the components in the <see cref="ComponentSignatureTypes"/> array.
        /// </summary>
        protected List<MachEntity> Entities { get; } = new List<MachEntity>();

        /// <summary>
        /// The component types which this system is interested in working with; entities with components that match (at a minimum)
        /// the component(s) in this array will be populated into <see cref="Entities"/> automatically.
        /// </summary>
        protected abstract Type[] ComponentSignatureTypes { get; }
    }
}
