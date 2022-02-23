using SubC.MachEcs.Workers;
using System;
using System.Reflection;

namespace SubC.MachEcs
{
    public sealed class Agent
    {
        private readonly ComponentWorker _componentWorker = new();
        private readonly EntityWorker _entityWorker;
        private readonly EventWorker _eventWorker = new();
        private readonly Entity _singletonEntity = new();
        private readonly SystemWorker _systemWorker = new();

        public Agent(int maximumEntities)
        {
            _entityWorker = new EntityWorker(maximumEntities);
        }

        public void AddComponent<T>(T component)
            where T : IComponent
        {
            AddComponent(_singletonEntity, component);
        }

        public void AddComponent<T>(Entity entity, T component)
            where T : IComponent
        {
            _componentWorker.AddComponent(entity, component);
            var componentSignature = _componentWorker.GetComponentSignature<T>();
            entity.Signature.Add(componentSignature);
            _systemWorker.EntitySignatureChanged(entity);
        }

        public void AddEventHandler<T>(Action eventHandler)
            where T : IEventData
        {
            _eventWorker.AddEventHandler<T>(eventHandler);
        }

        public void AddEventHandler<T>(Action<T> eventDataHandler)
            where T : IEventData
        {
            _eventWorker.AddEventHandler(eventDataHandler);
        }

        public Entity CreateEntity()
        {
            return _entityWorker.CreateEntity();
        }

        public void DestroyEntity(Entity entity)
        {
            _componentWorker.EntityDestroyed(entity);
            _systemWorker.EntityDestroyed(entity);
            _entityWorker.DestroyEntity(entity);
        }

        public T GetComponent<T>()
            where T : IComponent
        {
            return GetComponent<T>(_singletonEntity);
        }

        public T GetComponent<T>(Entity entity)
            where T : IComponent
        {
            return _componentWorker.GetComponent<T>(entity);
        }

        public Signature GetComponentSignature<T>()
            where T : IComponent
        {
            return _componentWorker.GetComponentSignature<T>();
        }

        public Signature GetComponentSignature(Type componentType)
        {
            return _componentWorker.GetComponentSignature(componentType);
        }

        public void RegisterComponent<T>()
            where T : IComponent
        {
            _componentWorker.RegisterComponent<T>();
        }

        public void RegisterComponents(Assembly assembly)
        {
            _componentWorker.RegisterComponents(assembly);
        }

        public void RegisterEvent<T>()
            where T : IEventData
        {
            _eventWorker.RegisterEvent<T>();
        }

        public T RegisterSystem<T>()
            where T : EcsSystem, new()
        {
            return _systemWorker.RegisterSystem<T>(this);
        }

        public void RemoveComponent<T>()
            where T : IComponent
        {
            RemoveComponent<T>(_singletonEntity);
        }

        public void RemoveComponent<T>(Entity entity)
            where T : IComponent
        {
            _componentWorker.RemoveComponent<T>(entity);
            var componentSignature = GetComponentSignature<T>();
            entity.Signature.Remove(componentSignature);
            _systemWorker.EntitySignatureChanged(entity);
        }

        public void RemoveEvent<T>()
            where T : IEventData
        {
            _eventWorker.RemoveEvent<T>();
        }

        public void RemoveEventHandler<T>(Action eventHandler)
            where T : IEventData
        {
            _eventWorker.RemoveEventHandler<T>(eventHandler);
        }

        public void RemoveEventHandler<T>(Action<T> eventDataHandler)
            where T : IEventData
        {
            _eventWorker.RemoveEventHandler(eventDataHandler);
        }

        public void RemoveEventHandlers<T>()
            where T : IEventData
        {
            _eventWorker.RemoveEventHandlers<T>();
        }

        public void SendEvent<T>()
            where T : IEventData
        {
            _eventWorker.SendEvent<T>();
        }

        public void SendEvent<T>(T eventData)
            where T : IEventData
        {
            _eventWorker.SendEvent(eventData);
        }
    }
}