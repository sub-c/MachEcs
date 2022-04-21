using System;

namespace SubC.MachEcs.Models
{
  internal sealed class EventHandlerCache<T> : IEventHandlerCache
    where T : IEcsEvent
  {
    public Action? EventHandlers { get; set; }
    public Action<T>? EventDataHandlers { get; set; }
  }
}
