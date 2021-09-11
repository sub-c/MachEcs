using System;
using System.Collections.Generic;

namespace SubC.MachEcs
{
    /// <summary>
    /// This abstract class should be inherited from classes acting as MachECS systems.
    /// </summary>
    public abstract class MachSystem
    {
        internal MachAgent InternalAgent;

        internal Type[] InternalComponentTypes => ComponentTypes;

        internal ICollection<MachEntity> InternalEntities = new List<MachEntity>();

        internal readonly MachSignature Signature = new MachSignature();

        /// <summary>
        /// Gets the agent instance that this system is registered in.
        /// </summary>
        protected MachAgent Agent => InternalAgent;

        /// <summary>
        /// Gets an array of component types that this system is interested in working with.
        /// <para>
        /// The types defined must inherit from <see cref="IMachComponent"/> and have been registered with
        /// <see cref="MachAgent.RegisterComponent{T}"/> prior to registering the system.
        /// </para>
        /// </summary>
        protected abstract Type[] ComponentTypes { get; }

        /// <summary>
        /// Gets an enumeration of entities that have components matching (at a minimum) the system's component types.
        /// </summary>
        protected IEnumerable<MachEntity> Entities => InternalEntities;
    }
}
