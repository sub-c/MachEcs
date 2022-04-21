using SubC.MachEcs.Models;
using SubC.MachEcs.Worker;
using System;

namespace SubC.MachEcs
{
  public abstract class Agent
  {
    protected Agent()
    {
    }

    /// <summary>
    /// Creates and returns an implementation of an ECS world.
    /// </summary>
    /// <param name="maximumEntities">
    /// The maximum amount of entities supported at once by the ECS world.
    /// </param>
    /// <param name="signatureType">
    /// The signature implementation to use, which defines the maximum amount of components supported at once
    /// by the ECS world.
    /// </param>
    /// <returns>A configured instance of an ECS world.</returns>
    /// <exception cref="NotImplementedException">
    /// The provided <see cref="EcsSignatureType"/> is invalid.
    /// </exception>
    public static Agent CreateInstance(int maximumEntities, EcsSignatureType signatureType)
    {
      switch (signatureType)
      {
        case EcsSignatureType.SingleLong:
          return new Agent<SingleLongEcsSignature>(maximumEntities);
      }
      throw new NotImplementedException($"Cannot create agent: {signatureType} not supported.");
    }

    /// <summary>
    /// Adds a component to the agent.
    /// </summary>
    /// <typeparam name="T">The type of the component to add.</typeparam>
    /// <param name="component">The instance of the component to add.</param>
    abstract public void AddComponent<T>(T component)
      where T : IEcsComponent;

    /// <summary>
    /// Adds a component to the given entity.
    /// </summary>
    /// <typeparam name="T">The type of the component to add.</typeparam>
    /// <param name="entity">The entity to add the component to.</param>
    /// <param name="component">The instance of the component to add.</param>
    abstract public void AddComponent<T>(IEcsEntity entity, T component)
      where T : IEcsComponent;

    /// <summary>
    /// Creates and returns an empty entity with no components attached.
    /// </summary>
    /// <returns>An empty entity.</returns>
    abstract public IEcsEntity CreateEntity();

    /// <summary>
    /// Destroys an entity and all components attached to it.
    /// </summary>
    /// <param name="entity">The entity to destroy.</param>
    abstract public void DestroyEntity(IEcsEntity entity);

    /// <summary>
    /// Gets a component from the agent.
    /// </summary>
    /// <typeparam name="T">The type of the component to get.</typeparam>
    /// <returns>The component instance from the agent.</returns>
    abstract public T GetComponent<T>()
      where T : IEcsComponent;

    /// <summary>
    /// Gets a component from the given entity.
    /// </summary>
    /// <typeparam name="T">The type of the component to get.</typeparam>
    /// <param name="entity">The entity to get the component from.</param>
    /// <returns>The component instance attached to the entity.</returns>
    abstract public T GetComponent<T>(IEcsEntity entity)
      where T : IEcsComponent;

    /// <summary>
    /// Registers a component for use in the agent.
    /// </summary>
    /// <typeparam name="T">The component type to register.</typeparam>
    abstract public void RegisterComponent<T>()
      where T : IEcsComponent;

    /// <summary>
    /// Registers a method to be invoked when this agent sends an event of the given type.
    /// </summary>
    /// <typeparam name="T">The type of the event.</typeparam>
    /// <param name="eventHandler">The method to invoke when the event type is sent.</param>
    abstract public void RegisterEventHandler<T>(Action eventHandler)
      where T : IEcsEvent;

    /// <summary>
    /// Registers a method to be invoked when this agent sends an event of the given type.
    /// </summary>
    /// <typeparam name="T">The type of the event data.</typeparam>
    /// <param name="eventHandler">The method to invoke when the event data type is sent.</param>
    abstract public void RegisterEventHandler<T>(Action<T> eventHandler)
      where T : IEcsEvent;

    /// <summary>
    /// Registers a system for use in the agent.
    /// </summary>
    /// <typeparam name="T">The system type to register.</typeparam>
    /// <returns>The system instance registered into the agent.</returns>
    abstract public T RegisterSystem<T>()
      where T : EcsSystem;

    /// <summary>
    /// Removes a component from the agent.
    /// </summary>
    /// <typeparam name="T">The component type to remove from the agent.</typeparam>
    abstract public void RemoveComponent<T>()
      where T : IEcsComponent;

    /// <summary>
    /// Removes a component from the given entity.
    /// </summary>
    /// <typeparam name="T">The component type to remove from the given entity.</typeparam>
    /// <param name="entity">The entity to remove the given component from.</param>
    abstract public void RemoveComponent<T>(IEcsEntity entity)
      where T : IEcsComponent;

    /// <summary>
    /// Invokes all registered methods associated with the given event type.
    /// </summary>
    /// <typeparam name="T">The type of the event.</typeparam>
    abstract public void SendEvent<T>()
      where T : IEcsEvent;

    /// <summary>
    /// Invokes all registered methods associated with the given event data type.
    /// </summary>
    /// <typeparam name="T">The type of the event data.</typeparam>
    /// <param name="eventData">The event data passed to each registered method invoked.</param>
    abstract public void SendEvent<T>(T eventData)
      where T : IEcsEvent;

    /// <summary>
    /// Unregisters a method from being invoked when this agent sends an event of the given type.
    /// </summary>
    /// <typeparam name="T">The type of the event.</typeparam>
    /// <param name="eventHandler">The method to no longer invoke when the event type is sent.</param>
    abstract public void UnregisterEventHandler<T>(Action eventHandler)
      where T : IEcsEvent;

    /// <summary>
    /// Unregisters a method from being invoked when this agent sends event data of the given type.
    /// </summary>
    /// <typeparam name="T">The type of the event data.</typeparam>
    /// <param name="eventHandler">The method to no longer invoke when the event data type is sent.</param>
    abstract public void UnregisterEventHandler<T>(Action<T> eventHandler)
      where T : IEcsEvent;
  }

  internal sealed class Agent<S> : Agent
    where S : EcsSignature<S>, new()
  {
    private readonly ComponentWorker<S> _componentWorker = new();
    private readonly EntityWorker<S> _entityWorker;
    private readonly EventWorker _eventWorker = new();
    private readonly IEcsEntity _singletonEntity;
    private readonly SystemWorker<S> _systemWorker = new();

    public Agent(int maximumEntities)
    {
      _entityWorker = new EntityWorker<S>(maximumEntities);
      _singletonEntity = _entityWorker.CreateEntity();
    }

    public override void AddComponent<T>(T component)
    {
      AddComponent(_singletonEntity, component);
    }

    public override void AddComponent<T>(IEcsEntity entity, T component)
    {
      _componentWorker.AddComponent(entity, component);
      var componentSignature = _componentWorker.GetComponentSignature(typeof(T));
      var entitySignature = _entityWorker.GetEntitySignature(entity);
      entitySignature.EnableBitsFrom(componentSignature);
      _systemWorker.EntitySignatureChanged(entity, entitySignature);
    }

    public override IEcsEntity CreateEntity()
    {
      return _entityWorker.CreateEntity();
    }

    public override void DestroyEntity(IEcsEntity entity)
    {
      _componentWorker.EntityDestroyed(entity);
      _systemWorker.EntityDestroyed(entity);
      _entityWorker.DestroyEntity(entity);
    }

    public override T GetComponent<T>()
    {
      return _componentWorker.GetComponent<T>(_singletonEntity);
    }

    public override T GetComponent<T>(IEcsEntity entity)
    {
      return _componentWorker.GetComponent<T>(entity);
    }

    public override void RegisterComponent<T>()
    {
      _componentWorker.RegisterComponent<T>();
    }

    public override void RegisterEventHandler<T>(Action eventHandler)
    {
      _eventWorker.RegisterEventHandler<T>(eventHandler);
    }

    public override void RegisterEventHandler<T>(Action<T> eventHandler)
    {
      _eventWorker.RegisterEventHandler(eventHandler);
    }

    public override T RegisterSystem<T>()
    {
      return _systemWorker.RegisterSystem<T>(this);
    }

    public override void RemoveComponent<T>()
    {
      RemoveComponent<T>(_singletonEntity);
    }

    public override void RemoveComponent<T>(IEcsEntity entity)
    {
      _componentWorker.RemoveComponent<T>(entity);
      var componentSignature = _componentWorker.GetComponentSignature(typeof(T));
      var entitySignature = _entityWorker.GetEntitySignature(entity);
      entitySignature.DisableBitsFrom(componentSignature);
      _systemWorker.EntitySignatureChanged(entity, entitySignature);
    }

    public override void SendEvent<T>()
    {
      _eventWorker.SendEvent<T>();
    }

    public override void SendEvent<T>(T eventData)
    {
      _eventWorker.SendEvent(eventData);
    }

    public override void UnregisterEventHandler<T>(Action eventHandler)
    {
      _eventWorker.UnregisterEventHandler<T>(eventHandler);
    }

    public override void UnregisterEventHandler<T>(Action<T> eventHandler)
    {
      _eventWorker.UnregisterEventHandler(eventHandler);
    }

    internal S GetComponentSignature(Type componentType)
    {
      return _componentWorker.GetComponentSignature(componentType);
    }
  }
}
