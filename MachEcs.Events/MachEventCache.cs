using System;

namespace SubC.MachEcs.Events
{
    internal sealed class MachEventCache<T> : IMachEventCache
    {
        public Action<T> EventListeners { get; set; }
    }
}
