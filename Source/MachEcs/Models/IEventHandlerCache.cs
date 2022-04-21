using System;

namespace SubC.MachEcs.Models
{
  internal interface IEventHandlerCache
  {
    Action? EventHandlers { get; set; }
  }
}
