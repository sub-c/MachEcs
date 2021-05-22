using System;
using System.Reflection;
using SubC.MachEcs;

namespace MachEcs.Sandbox
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            var agent = new MachAgent(5000);
            agent.RegisterComponentsInAssembly(Assembly.GetExecutingAssembly());
            agent.RegisterSystemsInAssembly(Assembly.GetExecutingAssembly());

            var testSystem = agent.GetSystem<TestMachSystem>();

            var testEntity = agent.CreateEntity();
            var testComponent = new TestMachComponent { X = 5 };
            agent.AddComponent(testEntity, testComponent);

            var duckEntity = agent.CreateEntity();
            var duckComponent = new DuckComponent { DuckSound = "q...qua... feh." };
            agent.AddComponent(duckEntity, duckComponent);
            var duckTestComponent = new TestMachComponent { X = 69 };
            agent.AddComponent(duckEntity, duckTestComponent);

            testSystem.UpdateSystem();

            Console.WriteLine("Done.");
        }
    }
}
