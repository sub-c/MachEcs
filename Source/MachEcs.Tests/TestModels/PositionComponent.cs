using SubC.MachEcs;

namespace MachEcs.Tests.TestModels
{
  public class PositionComponent : IEcsComponent
  {
    public float X { get; set; }
    public float Y { get; set; }
  }
}
