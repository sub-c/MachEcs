using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using MachEcs.Entities;

namespace MachEcs.Components
{
    internal sealed class ComponentManager
    {
        public int ComponentsActive => _nextComponentBit;
        public int ComponentsFree => MachSignature.MaxSignatureBits - _nextComponentBit;

        private readonly Dictionary<string, IComponentCache> _componentCaches = new Dictionary<string, IComponentCache>();
        private int _nextComponentBit = 0;

        public void AddComponent<T>(MachEntity entity, T component)
            where T : IMachComponent
        {
            GetComponentCache<T>().InsertComponent(entity, component);
        }

        public void EntityDestroyed(MachEntity entity)
        {
            foreach (var componentCachePair in _componentCaches)
            {
                componentCachePair.Value.EntityDestroyed(entity);
            }
        }

        public T GetComponent<T>(MachEntity entity)
            where T : IMachComponent
        {
            return GetComponentCache<T>().GetComponent(entity);
        }

        public MachSignature GetComponentSignature<T>()
            where T : IMachComponent
        {
            var typeName = typeof(T).Name;
            Debug.Assert(_componentCaches.ContainsKey(typeName), $"Component was not registered before use: {typeName}.");
            return _componentCaches[typeName].Signature;
        }

        public void RegisterComponent<T>()
            where T : IMachComponent
        {
            var componentCache = new ComponentCache<T>();
            Debug.Assert(_nextComponentBit < MachSignature.MaxSignatureBits, $"Too many components to register.");
            componentCache.Signature.EnableBit(_nextComponentBit);
            ++_nextComponentBit;
            _componentCaches.Add(typeof(T).Name, componentCache);
        }

        public void RegisterComponentsInAssembly(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (typeof(IMachComponent).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface)
                {
                    var componentCacheGenericType = typeof(ComponentCache<>).MakeGenericType(new Type[] { type });
                    var componentCacheConstructor = componentCacheGenericType.GetConstructor(Type.EmptyTypes);
                    Debug.Assert(componentCacheConstructor != null, $"Could not instanciate ComponentCache<> with the found {nameof(IMachComponent)}.");

                    var componentCache = componentCacheConstructor.Invoke(Type.EmptyTypes) as IComponentCache;
                    Debug.Assert(_nextComponentBit < MachSignature.MaxSignatureBits, $"Too many components to register.");
                    componentCache.Signature.EnableBit(_nextComponentBit);
                    ++_nextComponentBit;

                    _componentCaches.Add(type.Name, componentCache);
                }
            }
        }

        public void RemoveComponent<T>(MachEntity entity)
            where T : IMachComponent
        {
            GetComponentCache<T>().RemoveEntity(entity);
        }

        private ComponentCache<T> GetComponentCache<T>()
            where T : IMachComponent
        {
            var typeName = typeof(T).Name;
            Debug.Assert(_componentCaches.ContainsKey(typeName), $"Component not registered before use.");
            return _componentCaches[typeName] as ComponentCache<T>;
        }
    }
}
