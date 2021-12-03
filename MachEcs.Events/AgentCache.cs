using System;
using System.Collections.Generic;

namespace SubC.MachEcs.Events
{
    internal sealed class AgentCache
    {
        public IDictionary<Type, IMachEventCache> EventCaches { get; } = new Dictionary<Type, IMachEventCache>();
    }
}
