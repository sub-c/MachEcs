using System;
using System.Collections.Generic;

namespace SubC.MachEcs
{
    public abstract class EcsSystem
    {
        internal static readonly Agent DummyAgent = new(0);

        internal Agent InternalAgent { get; set; } = DummyAgent;

        internal Type[] InternalComponents => Components;

        internal List<Entity> InternalEntities { get; } = new List<Entity>();

        internal Signature Signature { get; } = new();

        protected Agent Agent => InternalAgent;

        protected abstract Type[] Components { get; }

        protected IReadOnlyList<Entity> Entities => InternalEntities;
    }
}
