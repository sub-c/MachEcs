using System;
using System.Collections.Generic;
using SubC.MachEcs.Systems;

namespace MachEcs.Sandbox
{
    internal sealed class TestMachSystem : MachSystem
    {
        protected override Type[] ComponentSignatureTypes => new Type[]
        {
            typeof(DuckComponent)
        };

        public void UpdateSystem()
        {
            var a = Entities.Count;
            var b = Agent.GetComponent<DuckComponent>(Entities[0]);
        }
    }
}
