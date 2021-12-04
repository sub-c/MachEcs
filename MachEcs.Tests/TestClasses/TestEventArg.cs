using SubC.MachEcs.Events;

namespace MachEcs.Tests.TestClasses
{
    internal sealed class TestEventArg : IMachEvent
    {
        public static TestEventArg Empty = new TestEventArg();
    }
}
