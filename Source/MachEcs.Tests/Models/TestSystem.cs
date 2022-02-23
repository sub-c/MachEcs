using SubC.MachEcs;
using System;

namespace MachEcs.Tests.Models
{
    internal sealed class TestSystem : EcsSystem
    {
        protected override Type[] Components => new Type[] { typeof(TestComponent) };
    }
}
