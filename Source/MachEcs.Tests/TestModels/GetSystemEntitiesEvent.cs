using SubC.MachEcs;
using System.Collections.Generic;

namespace MachEcs.Tests.TestModels
{
  internal sealed class GetSystemEntitiesEvent : IEcsEvent
  {
    public IEnumerable<IEcsEntity>? Entities { get; set; }
  }
}
