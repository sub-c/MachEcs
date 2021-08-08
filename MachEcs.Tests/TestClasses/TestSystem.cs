using System;
using SubC.MachEcs;

namespace MachEcs.Tests.TestClasses
{
    internal sealed class TestSystem : MachSystem
    {
        protected override Type[] ComponentTypes => new Type[]
        {
            typeof(TestComponent1),
            typeof(TestComponent2)
        };
    }
}
