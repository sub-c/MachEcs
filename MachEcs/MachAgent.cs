using System.Diagnostics;
using System.Reflection;
using MachEcs.Components;
using MachEcs.Entities;
using MachEcs.Systems;

namespace MachEcs
{
    /// <summary>
    /// Holds and gives access to an entity-component-system world.
    /// </summary>
    public sealed class MachAgent
    {
        /// <summary>
        /// The count of active components currently registered.
        /// </summary>
        public int ComponentsActive => _componentManager.ComponentsActive;

        /// <summary>
        /// The count of available components that could be registered in the future.
        /// This is based on the size of the signature (not currently editable, limited to 64).
        /// </summary>
        public int ComponentsFree => _componentManager.ComponentsFree;

        /// <summary>
        /// The count of entities that are currently active.
        /// </summary>
        public int EntitiesActive => _entityManager.EntitiesActive;

        /// <summary>
        /// The count of entities that are available to be used in the future.
        /// </summary>
        public int EntitiesFree => _entityManager.EntitiesFree;

        /// <summary>
        /// The count of systems currently registered.
        /// </summary>
        public int SystemsActive => _systemManager.SystemsActive;

        private readonly ComponentManager _componentManager;
        private readonly EntityManager _entityManager;
        private MachEntity _singletonEntity;
        private readonly SystemManager _systemManager;

        /// <summary>
        /// Create a new instance containing a new entity-component-system world.
        /// </summary>
        /// <param name="maximumEntities">The maximum amount of entities that may exist at once.</param>
        public MachAgent(int maximumEntities)
        {
            Debug.Assert(maximumEntities > 0, $"Maximum entities must be a positive number.");
            _componentManager = new ComponentManager();
            _entityManager = new EntityManager(maximumEntities);
            _singletonEntity = _entityManager.CreateEntity();
            _systemManager = new SystemManager();
        }

        /// <summary>
        /// Add a component instance to the singleton entity.
        /// </summary>
        /// <typeparam name="T">The type of the component.</typeparam>
        /// <param name="component">The instance of the component.</param>
        public void AddComponent<T>(T component)
            where T : IMachComponent
            => AddComponent<T>(_singletonEntity, component);

        /// <summary>
        /// Add a component instance to the given entity.
        /// </summary>
        /// <typeparam name="T">The type of the component.</typeparam>
        /// <param name="entity">The entity to attach the component to.</param>
        /// <param name="component">The instance of the component.</param>
        public void AddComponent<T>(MachEntity entity, T component)
            where T : IMachComponent
        {
            _componentManager.AddComponent(entity, component);
            var componentSignature = _componentManager.GetComponentSignature<T>();
            entity.Signature.AddSignature(componentSignature);
            _systemManager.EntitySignatureChanged(entity);
        }

        /// <summary>
        /// Creates an empty entity with no components or signature.
        /// </summary>
        /// <returns>An empty entity.</returns>
        public MachEntity CreateEntity()
            => _entityManager.CreateEntity();

        /// <summary>
        /// Destroys an entity, destroying its components.
        /// </summary>
        /// <param name="entity">The entity to destroy.</param>
        public void DestroyEntity(MachEntity entity)
        {
            _componentManager.EntityDestroyed(entity);
            _systemManager.EntityDestroyed(entity);
            _entityManager.DestroyEntity(entity);
        }

        /// <summary>
        /// Gets a component from the singleton entity.
        /// </summary>
        /// <typeparam name="T">The type of the component to get.</typeparam>
        /// <returns>The instance of the component.</returns>
        public T GetComponent<T>()
            where T : IMachComponent
            => GetComponent<T>(_singletonEntity);

        /// <summary>
        /// Gets a component from the given entity.
        /// </summary>
        /// <typeparam name="T">The type of the component to get.</typeparam>
        /// <param name="entity">The entity the component was attached to.</param>
        /// <returns>The instance of the component.</returns>
        public T GetComponent<T>(MachEntity entity)
            where T : IMachComponent
            => _componentManager.GetComponent<T>(entity);

        /// <summary>
        /// Gets a components signature.
        /// </summary>
        /// <typeparam name="T">The type of the component to get the signature of.</typeparam>
        /// <returns>The components signature.</returns>
        public MachSignature GetComponentSignature<T>()
            where T : IMachComponent
            => _componentManager.GetComponentSignature<T>();

        /// <summary>
        /// Gets a system that was previously registered.
        /// </summary>
        /// <typeparam name="T">The type of the system.</typeparam>
        /// <returns>The instance of the system.</returns>
        public T GetSystem<T>()
            where T : MachSystem
            => _systemManager.GetSystem<T>();

        /// <summary>
        /// Scans an assembly and registers all classes that implement the <see cref="IMachComponent"/> interface as a component.
        /// </summary>
        /// <param name="assembly">The assembly to scan the types of.</param>
        public void RegisterComponentsInAssembly(Assembly assembly)
            => _componentManager.RegisterComponentsInAssembly(assembly);

        /// <summary>
        /// Scans an assembly and registers all classes that implement the <see cref="MachSystem"/> class as a system.
        /// </summary>
        /// <param name="assembly">The assembly to scan the types of.</param>
        public void RegisterSystemsInAssembly(Assembly assembly)
            => _systemManager.RegisterSystemsInAssembly(assembly, this);
        
        /// <summary>
        /// Removes a component from the singleton entity.
        /// </summary>
        /// <typeparam name="T">The type of the component to remove.</typeparam>
        public void RemoveComponent<T>()
            where T : IMachComponent
            => RemoveComponent<T>(_singletonEntity);

        /// <summary>
        /// Removes a component from the given entity.
        /// </summary>
        /// <typeparam name="T">The type of the component to remove.</typeparam>
        /// <param name="entity">The entity to remove the component from.</param>
        public void RemoveComponent<T>(MachEntity entity)
            where T : IMachComponent
        {
            _componentManager.RemoveComponent<T>(entity);
            var componentSignature = _componentManager.GetComponentSignature<T>();
            entity.Signature.RemoveSignature(componentSignature);
            _systemManager.EntitySignatureChanged(entity);
        }

        /// <summary>
        /// Sets the signature of a given system.
        /// </summary>
        /// <typeparam name="T">The system to set the signature of.</typeparam>
        /// <param name="signature">The signature to set the system to.</param>
        public void SetSystemSignature<T>(MachSignature signature)
            where T : MachSystem
            => _systemManager.SetSystemSignature<T>(signature);
    }
}
