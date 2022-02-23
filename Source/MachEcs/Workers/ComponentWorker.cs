using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace SubC.MachEcs.Workers
{
    internal sealed class ComponentWorker
    {
        private readonly IDictionary<Type, IComponentCache> _caches = new Dictionary<Type, IComponentCache>();
        private int _nextSignatureBit;

        public void AddComponent<T>(Entity entity, T component)
            where T : IComponent
        {
            GetCache<T>().AddComponent(entity, component);
        }

        public bool DoesEntityHaveComponent<T>(Entity entity)
            where T : IComponent
        {
            return GetCache(typeof(T)).DoesEntityHaveComponent(entity);
        }

        public void EntityDestroyed(Entity entity)
        {
            foreach (var cache in _caches)
            {
                cache.Value.EntityDestroyed(entity);
            }
        }

        public T GetComponent<T>(Entity entity)
            where T : IComponent
        {
            return GetCache<T>().GetComponent(entity);
        }

        public Signature GetComponentSignature<T>()
            where T : IComponent
        {
            return GetCache(typeof(T)).Signature;
        }

        public Signature GetComponentSignature(Type componentType)
        {
            return GetCache(componentType).Signature;
        }

        public void RegisterComponent<T>()
            where T : IComponent
        {
            var type = typeof(T);
            var cache = new ComponentCache<T>();
            AddCache(type, cache);
        }

        public void RegisterComponents(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (!type.IsAssignableTo(typeof(IComponent))
                    || type.IsAbstract
                    || type.IsInterface)
                {
                    continue;
                }

                var genericType = typeof(ComponentCache<>).MakeGenericType(new Type[] { type });
                var constructor = genericType.GetConstructor(Type.EmptyTypes);
                Debug.Assert(constructor != null, $"Cannot register component, constructor with type {type.Name} is null.");
                var cache = (IComponentCache)constructor.Invoke(Type.EmptyTypes);
                AddCache(type, cache);
            }
        }

        public void RemoveComponent<T>(Entity entity)
        {
            GetCache(typeof(T)).RemoveComponent(entity);
        }

        private void AddCache(Type type, IComponentCache cache)
        {
            Debug.Assert(
                !_caches.ContainsKey(type),
                $"Cannot add component cache, component is already registered: {type.Name}");
            Debug.Assert(
                _nextSignatureBit < Signature.MaxSupportedSignatures,
                $"Cannot add component cache, exceeded maximum amount of signatures: {Signature.MaxSupportedSignatures}");
            cache.Signature.EnableBit(_nextSignatureBit++);
            _caches.Add(type, cache);
        }

        private ComponentCache<T> GetCache<T>()
            where T : IComponent
        {
            var type = typeof(T);
            Debug.Assert(
                _caches.ContainsKey(type),
                $"Cannot get component cache: component {type.Name} was not registered before use.");
            return (ComponentCache<T>)_caches[type];
        }

        private IComponentCache GetCache(Type componentType)
        {
            Debug.Assert(
                _caches.ContainsKey(componentType),
                $"Cannot get component cache, component {componentType.Name} was not registered before use.");
            return _caches[componentType];
        }
    }
}
