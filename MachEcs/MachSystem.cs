using System;
using System.Collections.Generic;

namespace SubC.MachEcs
{
    public abstract class MachSystem
    {
        internal MachAgent InternalAgent;

        internal Type[] InternalComponentTypes => ComponentTypes;

        internal ICollection<MachEntity> InternalEntities = new List<MachEntity>();

        internal readonly MachSignature Signature = new MachSignature();

        protected MachAgent Agent => InternalAgent;

        protected abstract Type[] ComponentTypes { get; }

        protected IEnumerable<MachEntity> Entities => InternalEntities;
    }
}
