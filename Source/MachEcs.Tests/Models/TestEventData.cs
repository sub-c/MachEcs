using SubC.MachEcs;

namespace MachEcs.Tests.Models
{
    internal sealed class TestEventData : IEventData
    {
        public string Data { get; set; } = string.Empty;
    }
}
