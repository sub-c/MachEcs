using System;
using System.Reflection;
using SubC.MachEcs.Workers;

namespace SubC.MachEcs
{
    public sealed class MachAgent
    {
        private readonly ComponentWorker _componentWorker;
        private readonly EntityWorker _entityWorker;
        private readonly MachEntity _singletonEntity = new MachEntity();
        private readonly SystemWorker _systemWorker;

        public MachAgent(int maximumEntities)
        {
            _componentWorker = new ComponentWorker();
            _entityWorker = new EntityWorker(maximumEntities);
            _systemWorker = new SystemWorker();
        }

        public void AddComponent<T>(T component)
            where T : IMachComponent
        {
            AddComponent(_singletonEntity, component);
        }

        public void AddComponent<T>(MachEntity entity, T component)
            where T : IMachComponent
        {
            _componentWorker.AddComponent(entity, component);
            var componentSignature = _componentWorker.GetComponentSignature<T>();
            entity.Signature.Add(componentSignature);
            _systemWorker.EntitySignatureChanged(entity);
        }

        public void AddComponents(params IMachComponent[] components)
        {
            AddComponents(_singletonEntity, components);
        }

        public void AddComponents(MachEntity entity, params IMachComponent[] components)
        {
            foreach (var component in components)
            {
                AddComponent(entity, component);
            }
        }

        public MachEntity CreateEntity()
        {
            return _entityWorker.CreateEntity();
        }

        public void DestroyEntity(MachEntity entity)
        {
            _componentWorker.EntityDestroyed(entity);
            _systemWorker.EntityDestroyed(entity);
            _entityWorker.DestroyEntity(entity);
        }

        public MachSignature GetComponentSignature<T>()
            where T : IMachComponent
        {
            return _componentWorker.GetComponentSignature<T>();
        }

        public MachSignature GetComponentSignature(Type componentType)
        {
            return _componentWorker.GetComponentSignature(componentType);
        }

        public void RegisterComponent<T>()
            where T : IMachComponent
        {
            _componentWorker.RegisterComponent<T>();
        }

        public void RegisterComponents(Assembly assembly)
        {
            _componentWorker.RegisterComponents(assembly);
        }

        public MachSystem RegisterSystem<T>()
            where T : MachSystem, new()
        {
            return _systemWorker.RegisterSystem<T>(this);
        }

        public void RemoveComponent<T>()
            where T : IMachComponent
        {
            RemoveComponent<T>(_singletonEntity);
        }

        public void RemoveComponent<T>(MachEntity entity)
            where T : IMachComponent
        {
            _componentWorker.RemoveComponent<T>(entity);
            var componentSignature = _componentWorker.GetComponentSignature<T>();
            entity.Signature.Remove(componentSignature);
            _systemWorker.EntitySignatureChanged(entity);
        }
    }
}
