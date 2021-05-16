using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using MachEcs.Entities;

namespace MachEcs.Systems
{
    internal sealed class SystemManager
    {
        public int SystemsActive => _systems.Count;

        private readonly Dictionary<string, MachSystem> _systems = new Dictionary<string, MachSystem>();

        public void EntityDestroyed(MachEntity entity)
        {
            foreach (var systemPair in _systems)
            {
                systemPair.Value.InternalEntities.Remove(entity);
            }
        }

        public void EntitySignatureChanged(MachEntity entity)
        {
            foreach (var systemPair in _systems)
            {
                if (entity.Signature.MatchesSignature(systemPair.Value.Signature))
                {
                    systemPair.Value.InternalEntities.Add(entity);
                }
                else
                {
                    systemPair.Value.InternalEntities.Remove(entity);
                }
            }
        }

        public T GetSystem<T>()
            where T : MachSystem
        {
            var systemName = typeof(T).Name;
            Debug.Assert(_systems.ContainsKey(systemName), $"System {systemName} was not registered before retrieving.");
            return _systems[systemName] as T;
        }

        public MachSystem RegisterSystem<T>(MachAgent agent)
            where T : MachSystem
        {
            Debug.Assert(!_systems.ContainsKey(typeof(T).Name), $"System is already registered.");
            var system = CreateSystem(typeof(T), agent);
            _systems.Add(typeof(T).Name, system);
            var systemSignature = CreateSystemSignature(system, typeof(T), agent);
            SetSystemSignature<T>(systemSignature);
            return system;
        }

        public void RegisterSystemsInAssembly(Assembly assembly, MachAgent agent)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (typeof(MachSystem).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface)
                {
                    Debug.Assert(!_systems.ContainsKey(type.Name), $"System is already registered.");
                    var system = CreateSystem(type, agent);
                    _systems.Add(type.Name, system);
                    var systemSignature = CreateSystemSignature(system, type, agent);
                    var setSignatureMethodInfo = typeof(MachAgent).GetMethod(nameof(MachAgent.SetSystemSignature));
                    var setSignatureMethod = setSignatureMethodInfo.MakeGenericMethod(type);
                    setSignatureMethod.Invoke(agent, new object[] { systemSignature });
                }
            }
        }

        public void SetSystemSignature<T>(MachSignature signature)
            where T : MachSystem
        {
            var systemName = typeof(T).Name;
            Debug.Assert(_systems.ContainsKey(systemName), $"System not registered before using it: {systemName}.");
            _systems[systemName].Signature.SetSignature(signature);
        }

        private MachSystem CreateSystem(Type systemType, MachAgent agent)
        {
            var systemConstructor = systemType.GetConstructor(Type.EmptyTypes);
            Debug.Assert(systemConstructor != null, "System does not have a parameter-less constructor.");
            var system = systemConstructor.Invoke(Type.EmptyTypes) as MachSystem;
            system.InternalAgent = agent;
            return system;
        }

        private MachSignature CreateSystemSignature(MachSystem system, Type systemType, MachAgent agent)
        {
            MachSignature systemSignature = new MachSignature();
            foreach (var componentSignatureType in system.InternalComponentSignatureTypes)
            {
                var methodInfo = typeof(MachAgent).GetMethod(nameof(MachAgent.GetComponentSignature));
                var genericMethod = methodInfo.MakeGenericMethod(componentSignatureType);
                var componentSignature = genericMethod.Invoke(agent, null) as MachSignature;
                systemSignature.AddSignature(componentSignature);
            }
            return systemSignature;
        }
    }
}
