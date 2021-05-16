using System;
using MachEcs.Systems;

namespace MachEcs.Tests.TestClasses
{
    public sealed class TestSystem : MachSystem
    {
        protected override Type[] ComponentSignatureTypes => new Type[]
        {
            typeof(TestSystemComponent)
        };
    }
}
