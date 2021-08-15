using System;
using System.Reflection;
using SubC.MachEcs.Workers;

namespace SubC.MachEcs
{
    /// <summary>
    /// This class is a domain facade into an entity-component-framework world.
    /// </summary>
    public sealed class MachAgent
    {
        private readonly ComponentWorker _componentWorker;
        private readonly EntityWorker _entityWorker;
        private readonly MachEntity _singletonEntity = new MachEntity();
        private readonly SystemWorker _systemWorker;

        /// <summary>
        /// Initializes a new instance of the <see cref="MachAgent"/> class.
        /// </summary>
        /// <param name="maximumEntities">The maximum amount of entities allowed at once.</param>
        public MachAgent(int maximumEntities)
        {
            _componentWorker = new ComponentWorker();
            _entityWorker = new EntityWorker(maximumEntities);
            _systemWorker = new SystemWorker();
        }

        /// <summary>
        /// Adds a singleton component.
        /// </summary>
        /// <typeparam name="T">The type of the component.</typeparam>
        /// <param name="component">The instance of the component.</param>
        public void AddComponent<T>(T component)
            where T : IMachComponent
        {
            AddComponent(_singletonEntity, component);
        }

        /// <summary>
        /// Adds a component to the given entity.
        /// </summary>
        /// <typeparam name="T">The type of the component.</typeparam>
        /// <param name="entity">The entity to add the component to.</param>
        /// <param name="component">The instance of the component.</param>
        public void AddComponent<T>(MachEntity entity, T component)
            where T : IMachComponent
        {
            _componentWorker.AddComponent(entity, component);
            var componentSignature = _componentWorker.GetComponentSignature<T>();
            entity.Signature.Add(componentSignature);
            _systemWorker.EntitySignatureChanged(entity);
        }

        /// <summary>
        /// Creates an entity.
        /// </summary>
        /// <returns>The entity instance.</returns>
        public MachEntity CreateEntity()
        {
            return _entityWorker.CreateEntity();
        }

        /// <summary>
        /// Destroys an entity and any components associated with it.
        /// </summary>
        /// <param name="entity">The entity to destroy.</param>
        public void DestroyEntity(MachEntity entity)
        {
            _componentWorker.EntityDestroyed(entity);
            _systemWorker.EntityDestroyed(entity);
            _entityWorker.DestroyEntity(entity);
        }

        /// <summary>
        /// Gets the signature of a component.
        /// </summary>
        /// <typeparam name="T">The component type.</typeparam>
        /// <returns>The component signature.</returns>
        public MachSignature GetComponentSignature<T>()
            where T : IMachComponent
        {
            return _componentWorker.GetComponentSignature<T>();
        }

        /// <summary>
        /// Gets the signature of a component.
        /// </summary>
        /// <param name="componentType">The component type.</param>
        /// <returns>The component signature.</returns>
        public MachSignature GetComponentSignature(Type componentType)
        {
            return _componentWorker.GetComponentSignature(componentType);
        }

        /// <summary>
        /// Registers a component to be added/removed from entities.
        /// </summary>
        /// <typeparam name="T">The component type.</typeparam>
        public void RegisterComponent<T>()
            where T : IMachComponent
        {
            _componentWorker.RegisterComponent<T>();
        }

        /// <summary>
        /// Registers all components that implement the <see cref="IMachComponent"/> interface in the given assembly.
        /// </summary>
        /// <param name="assembly">The assembly to register components from.</param>
        public void RegisterComponents(Assembly assembly)
        {
            _componentWorker.RegisterComponents(assembly);
        }

        /// <summary>
        /// Registers a system, using the types defined within to determine what components the system is intereste
        /// in working with.
        /// Call this after components have been registered.
        /// </summary>
        /// <typeparam name="T">The type of system.</typeparam>
        /// <returns>The system instance.</returns>
        public MachSystem RegisterSystem<T>()
            where T : MachSystem, new()
        {
            return _systemWorker.RegisterSystem<T>(this);
        }

        /// <summary>
        /// Removes a singleton component.
        /// </summary>
        /// <typeparam name="T">The type of component.</typeparam>
        public void RemoveComponent<T>()
            where T : IMachComponent
        {
            RemoveComponent<T>(_singletonEntity);
        }

        /// <summary>
        /// Removes a component from an entity.
        /// </summary>
        /// <typeparam name="T">The type of component.</typeparam>
        /// <param name="entity">The entity instance.</param>
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
