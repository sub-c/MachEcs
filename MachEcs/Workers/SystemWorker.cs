using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SubC.MachEcs.Workers
{
    internal sealed class SystemWorker
    {
        private readonly IDictionary<Type, MachSystem> _systems = new Dictionary<Type, MachSystem>();

        public void EntityDestroyed(MachEntity entity)
        {
            foreach (var system in _systems)
            {
                system.Value.InternalEntities.Remove(entity);
            }
        }

        public void EntitySignatureChanged(MachEntity entity)
        {
            foreach (var system in _systems)
            {
                if (entity.Signature.Matches(system.Value.Signature))
                {
                    system.Value.InternalEntities.Add(entity);
                }
                else
                {
                    system.Value.InternalEntities.Remove(entity);
                }
            }
        }

        public T GetSystem<T>()
            where T : MachSystem
        {
            var type = typeof(T);
            Debug.Assert(_systems.ContainsKey(type), $"Cannot get system: System {type.Name} was not registered before using.");
            return (T)_systems[type];
        }

        public T RegisterSystem<T>(MachAgent agent)
            where T : MachSystem, new()
        {
            var type = typeof(T);
            Debug.Assert(!_systems.ContainsKey(type), $"Cannot register system: system {type.Name} is already registered.");
            var system = new T() { InternalAgent = agent };
            SetupSystemSignature(system, agent);
            _systems.Add(type, system);
            return system;
        }

        private void SetupSystemSignature(MachSystem system, MachAgent agent)
        {
            foreach (var componentType in system.InternalComponentTypes)
            {
                Debug.Assert(
                    componentType.GetInterfaces().Contains(typeof(IMachComponent)),
                    $"Cannot use {componentType.Name} as a component, it does not implement {typeof(IMachComponent).Name}");
                var componentSignature = agent.GetComponentSignature(componentType);
                system.Signature.Add(componentSignature);
            }
        }
    }
}
