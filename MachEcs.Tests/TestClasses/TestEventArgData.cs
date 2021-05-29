using SubC.MachEcs.Events;

namespace MachEcs.Tests.TestClasses
{
    public sealed class TestEventArgData : IMachEventArgData
    {
        public string TestString { get; set; } = string.Empty;
    }
}
