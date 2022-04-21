using System.Collections.Generic;

namespace SubC.MachEcs
{
  /// <summary>
  /// This abstract class represents the base of a system in the ECS world.
  /// </summary>
  public abstract class EcsSystem
  {
    internal readonly List<IEcsEntity> InternalEntities = new();

    /// <summary>
    /// Gets the agent that this system exists in.
    /// </summary>
    protected Agent Agent { get; }

    /// <summary>
    /// Gets the entities that match the signature of this system.
    /// </summary>
    protected IReadOnlyList<IEcsEntity> Entities => InternalEntities;

    /// <summary>
    /// Initializes a new instance of the <see cref="EcsSystem"/> class.
    /// Perform any initialization needed by your system here.
    /// </summary>
    /// <param name="agent">The agent this system exists in.</param>
    public EcsSystem(Agent agent)
    {
      Agent = agent;
    }
  }

  /// <summary>
  /// This abstract class represents a system in the ECS <see cref="IAgent"/>.
  /// </summary>
  /// <typeparam name="C1">The component type that is included in this system's signature.</typeparam>
  public abstract class EcsSystem<C1> : EcsSystem
    where C1 : IEcsComponent
  {
    /// <inheritdoc/>
    public EcsSystem(Agent agent) : base(agent) { }
  }

  /// <summary>
  /// This abstract class represents a system in the ECS <see cref="IAgent"/>.
  /// </summary>
  /// <typeparam name="C1">The component type that is included in this system's signature.</typeparam>
  /// <typeparam name="C2">The component type that is included in this system's signature.</typeparam>
  public abstract class EcsSystem<C1, C2> : EcsSystem
    where C1 : IEcsComponent
    where C2 : IEcsComponent
  {
    /// <inheritdoc/>
    public EcsSystem(Agent agent) : base(agent) { }
  }

  /// <summary>
  /// This abstract class represents a system in the ECS <see cref="IAgent"/>.
  /// </summary>
  /// <typeparam name="C1">The component type that is included in this system's signature.</typeparam>
  /// <typeparam name="C2">The component type that is included in this system's signature.</typeparam>
  /// <typeparam name="C3">The component type that is included in this system's signature.</typeparam>
  public abstract class EcsSystem<C1, C2, C3> : EcsSystem
    where C1 : IEcsComponent
    where C2 : IEcsComponent
    where C3 : IEcsComponent
  {
    /// <inheritdoc/>
    public EcsSystem(Agent agent) : base(agent) { }
  }

  /// <summary>
  /// This abstract class represents a system in the ECS <see cref="IAgent"/>.
  /// </summary>
  /// <typeparam name="C1">The component type that is included in this system's signature.</typeparam>
  /// <typeparam name="C2">The component type that is included in this system's signature.</typeparam>
  /// <typeparam name="C3">The component type that is included in this system's signature.</typeparam>
  /// <typeparam name="C4">The component type that is included in this system's signature.</typeparam>
  public abstract class EcsSystem<C1, C2, C3, C4> : EcsSystem
    where C1 : IEcsComponent
    where C2 : IEcsComponent
    where C3 : IEcsComponent
    where C4 : IEcsComponent
  {
    /// <inheritdoc/>
    public EcsSystem(Agent agent) : base(agent) { }
  }

  /// <summary>
  /// This abstract class represents a system in the ECS <see cref="IAgent"/>.
  /// </summary>
  /// <typeparam name="C1">The component type that is included in this system's signature.</typeparam>
  /// <typeparam name="C2">The component type that is included in this system's signature.</typeparam>
  /// <typeparam name="C3">The component type that is included in this system's signature.</typeparam>
  /// <typeparam name="C4">The component type that is included in this system's signature.</typeparam>
  /// <typeparam name="C5">The component type that is included in this system's signature.</typeparam>
  public abstract class EcsSystem<C1, C2, C3, C4, C5> : EcsSystem
    where C1 : IEcsComponent
    where C2 : IEcsComponent
    where C3 : IEcsComponent
    where C4 : IEcsComponent
    where C5 : IEcsComponent
  {
    /// <inheritdoc/>
    public EcsSystem(Agent agent) : base(agent) { }
  }

  /// <summary>
  /// This abstract class represents a system in the ECS <see cref="IAgent"/>.
  /// </summary>
  /// <typeparam name="C1">The component type that is included in this system's signature.</typeparam>
  /// <typeparam name="C2">The component type that is included in this system's signature.</typeparam>
  /// <typeparam name="C3">The component type that is included in this system's signature.</typeparam>
  /// <typeparam name="C4">The component type that is included in this system's signature.</typeparam>
  /// <typeparam name="C5">The component type that is included in this system's signature.</typeparam>
  /// <typeparam name="C6">The component type that is included in this system's signature.</typeparam>
  public abstract class EcsSystem<C1, C2, C3, C4, C5, C6> : EcsSystem
    where C1 : IEcsComponent
    where C2 : IEcsComponent
    where C3 : IEcsComponent
    where C4 : IEcsComponent
    where C5 : IEcsComponent
    where C6 : IEcsComponent
  {
    /// <inheritdoc/>
    public EcsSystem(Agent agent) : base(agent) { }
  }

  /// <summary>
  /// This abstract class represents a system in the ECS <see cref="IAgent"/>.
  /// </summary>
  /// <typeparam name="C1">The component type that is included in this system's signature.</typeparam>
  /// <typeparam name="C2">The component type that is included in this system's signature.</typeparam>
  /// <typeparam name="C3">The component type that is included in this system's signature.</typeparam>
  /// <typeparam name="C4">The component type that is included in this system's signature.</typeparam>
  /// <typeparam name="C5">The component type that is included in this system's signature.</typeparam>
  /// <typeparam name="C6">The component type that is included in this system's signature.</typeparam>
  /// <typeparam name="C7">The component type that is included in this system's signature.</typeparam>
  public abstract class EcsSystem<C1, C2, C3, C4, C5, C6, C7> : EcsSystem
    where C1 : IEcsComponent
    where C2 : IEcsComponent
    where C3 : IEcsComponent
    where C4 : IEcsComponent
    where C5 : IEcsComponent
    where C6 : IEcsComponent
    where C7 : IEcsComponent
  {
    /// <inheritdoc/>
    public EcsSystem(Agent agent) : base(agent) { }
  }

  /// <summary>
  /// This abstract class represents a system in the ECS <see cref="IAgent"/>.
  /// </summary>
  /// <typeparam name="C1">The component type that is included in this system's signature.</typeparam>
  /// <typeparam name="C2">The component type that is included in this system's signature.</typeparam>
  /// <typeparam name="C3">The component type that is included in this system's signature.</typeparam>
  /// <typeparam name="C4">The component type that is included in this system's signature.</typeparam>
  /// <typeparam name="C5">The component type that is included in this system's signature.</typeparam>
  /// <typeparam name="C6">The component type that is included in this system's signature.</typeparam>
  /// <typeparam name="C7">The component type that is included in this system's signature.</typeparam>
  /// <typeparam name="C8">The component type that is included in this system's signature.</typeparam>
  public abstract class EcsSystem<C1, C2, C3, C4, C5, C6, C7, C8> : EcsSystem
    where C1 : IEcsComponent
    where C2 : IEcsComponent
    where C3 : IEcsComponent
    where C4 : IEcsComponent
    where C5 : IEcsComponent
    where C6 : IEcsComponent
    where C7 : IEcsComponent
    where C8 : IEcsComponent
  {
    /// <inheritdoc/>
    public EcsSystem(Agent agent) : base(agent) { }
  }

  /// <summary>
  /// This abstract class represents a system in the ECS <see cref="IAgent"/>.
  /// </summary>
  /// <typeparam name="C1">The component type that is included in this system's signature.</typeparam>
  /// <typeparam name="C2">The component type that is included in this system's signature.</typeparam>
  /// <typeparam name="C3">The component type that is included in this system's signature.</typeparam>
  /// <typeparam name="C4">The component type that is included in this system's signature.</typeparam>
  /// <typeparam name="C5">The component type that is included in this system's signature.</typeparam>
  /// <typeparam name="C6">The component type that is included in this system's signature.</typeparam>
  /// <typeparam name="C7">The component type that is included in this system's signature.</typeparam>
  /// <typeparam name="C8">The component type that is included in this system's signature.</typeparam>
  /// <typeparam name="C9">The component type that is included in this system's signature.</typeparam>
  public abstract class EcsSystem<C1, C2, C3, C4, C5, C6, C7, C8, C9> : EcsSystem
    where C1 : IEcsComponent
    where C2 : IEcsComponent
    where C3 : IEcsComponent
    where C4 : IEcsComponent
    where C5 : IEcsComponent
    where C6 : IEcsComponent
    where C7 : IEcsComponent
    where C8 : IEcsComponent
    where C9 : IEcsComponent
  {
    /// <inheritdoc/>
    public EcsSystem(Agent agent) : base(agent) { }
  }

  /// <summary>
  /// This abstract class represents a system in the ECS <see cref="IAgent"/>.
  /// </summary>
  /// <typeparam name="C1">The component type that is included in this system's signature.</typeparam>
  /// <typeparam name="C2">The component type that is included in this system's signature.</typeparam>
  /// <typeparam name="C3">The component type that is included in this system's signature.</typeparam>
  /// <typeparam name="C4">The component type that is included in this system's signature.</typeparam>
  /// <typeparam name="C5">The component type that is included in this system's signature.</typeparam>
  /// <typeparam name="C6">The component type that is included in this system's signature.</typeparam>
  /// <typeparam name="C7">The component type that is included in this system's signature.</typeparam>
  /// <typeparam name="C8">The component type that is included in this system's signature.</typeparam>
  /// <typeparam name="C9">The component type that is included in this system's signature.</typeparam>
  /// <typeparam name="C10">The component type that is included in this system's signature.</typeparam>
  public abstract class EcsSystem<C1, C2, C3, C4, C5, C6, C7, C8, C9, C10> : EcsSystem
    where C1 : IEcsComponent
    where C2 : IEcsComponent
    where C3 : IEcsComponent
    where C4 : IEcsComponent
    where C5 : IEcsComponent
    where C6 : IEcsComponent
    where C7 : IEcsComponent
    where C8 : IEcsComponent
    where C9 : IEcsComponent
    where C10 : IEcsComponent
  {
    /// <inheritdoc/>
    public EcsSystem(Agent agent) : base(agent) { }
  }
}
