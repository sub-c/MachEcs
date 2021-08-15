using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace SubC.MachEcs.Workers
{
    internal sealed class ComponentWorker
    {
        private readonly IDictionary<Type, IComponentCache> _caches = new Dictionary<Type, IComponentCache>();
        private int _nextSignatureBit = 0;

        public void AddComponent<T>(MachEntity entity, T component)
            where T : IMachComponent
        {
            GetCache<T>().AddComponent(entity, component);
        }

        public void EntityDestroyed(MachEntity entity)
        {
            foreach (var cache in _caches)
            {
                cache.Value.EntityDestroyed(entity);
            }
        }

        public T GetComponent<T>(MachEntity entity)
            where T : IMachComponent
        {
            return GetCache<T>().GetComponent(entity);
        }

        public MachSignature GetComponentSignature<T>()
            where T : IMachComponent
        {
            var type = typeof(T);
            Debug.Assert(
                _caches.ContainsKey(type),
                $"Cannot get component signature: component {type.Name} was not registered before use.");
            return _caches[type].Signature;
        }

        public MachSignature GetComponentSignature(Type componentType)
        {
            Debug.Assert(
                _caches.ContainsKey(componentType),
                $"Cannot get component signature: component {componentType.Name} was not registered before use.");
            return _caches[componentType].Signature;
        }

        public void RegisterComponent<T>()
            where T : IMachComponent
        {
            Debug.Assert(
                !_caches.ContainsKey(typeof(T)),
                $"Cannot register component: component is already registered {typeof(T).Name}");
            Debug.Assert(
                _nextSignatureBit < MachSignature.MaxSupportedSignatures,
                $"Cannot register component: exceeded maximum amount of signatures {MachSignature.MaxSupportedSignatures}");
            var cache = new ComponentCache<T>();
            cache.Signature.EnableBit(_nextSignatureBit++);
            _caches.Add(typeof(T), cache);
        }

        public void RegisterComponents(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (typeof(IMachComponent).IsAssignableFrom(type) &&
                    !type.IsAbstract &&
                    !type.IsInterface)
                {
                    var genericType = typeof(ComponentCache<>).MakeGenericType(new Type[] { type });
                    var constructor = genericType.GetConstructor(Type.EmptyTypes);

                    Debug.Assert(
                        constructor != null,
                        $"Cannot register component: constructor using type {type.Name} is null.");
                    var cache = (IComponentCache)constructor.Invoke(Type.EmptyTypes);

                    Debug.Assert(
                        _nextSignatureBit < MachSignature.MaxSupportedSignatures,
                        $"Cannot register component: exceeded maximum amount of signatures {MachSignature.MaxSupportedSignatures}");
                    cache.Signature.EnableBit(_nextSignatureBit++);
                    _caches.Add(type, cache);
                }
            }
        }

        public void RemoveComponent<T>(MachEntity entity)
            where T : IMachComponent
        {
            GetCache<T>().RemoveComponent(entity);
        }

        private ComponentCache<T> GetCache<T>()
            where T : IMachComponent
        {
            var type = typeof(T);
            Debug.Assert(
                _caches.ContainsKey(type),
                $"Cannot get component cache: component {type.Name} was not registered before use.");
            return (ComponentCache<T>)_caches[type];
        }
    }
}
