using System;
using System.Collections.Generic;
using MachEcs.Systems;

namespace MachEcs.Sandbox
{
    internal sealed class TestMachSystem : MachSystem
    {
        public override Type[] ComponentSignatureTypes => new Type[]
        {
            typeof(TestMachComponent),
            typeof(DuckComponent)
        };

        public void UpdateSystem()
        {
            var a = Entities.Count;
            var b = Agent.GetComponent<DuckComponent>(Entities[0]);
        }
    }
}
