using SubC.MachEcs;

namespace MachEcs.Tests.TestModels
{
  public class VelocityComponent : IEcsComponent
  {
    public float DX { get; set; }
    public float DY { get; set; }
  }
}
