using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SubC.MachEcs.Workers
{
    internal sealed class SystemWorker
    {
        private readonly IDictionary<Type, EcsSystem> _systems = new Dictionary<Type, EcsSystem>();

        public void EntityDestroyed(Entity entity)
        {
            foreach (var system in _systems)
            {
                system.Value.InternalEntities.Remove(entity);
            }
        }

        public void EntitySignatureChanged(Entity entity)
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
            where T : EcsSystem
        {
            Debug.Assert(_systems.ContainsKey(typeof(T)), $"Cannot get system, system not registered before use: {typeof(T).Name}");
            return (T)_systems[typeof(T)];
        }

        public T RegisterSystem<T>(Agent agent)
            where T : EcsSystem, new()
        {
            Debug.Assert(!_systems.ContainsKey(typeof(T)), $"Cannot register system, system already registered: {typeof(T).Name}");
            var system = new T() { InternalAgent = agent };
            SetupSystemSignature(system, agent);
            _systems.Add(typeof(T), system);
            return system;
        }

        private static void SetupSystemSignature(EcsSystem system, Agent agent)
        {
            foreach (var component in system.InternalComponents)
            {
                Debug.Assert(
                    component.IsAssignableTo(typeof(IComponent)),
                    $"Cannot use {component.Name} as a component, it does not implement {typeof(IComponent).Name}.");
                var componentSignature = agent.GetComponentSignature(component);
                system.Signature.Add(componentSignature);
            }
        }
    }
}
