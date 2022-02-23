using SubC.MachEcs;

namespace MachEcs.Tests.Models
{
    internal sealed class TestComponent : IComponent
    {
        public string Data { get; set; } = string.Empty;
    }
}
