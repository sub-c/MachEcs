# MachECS v3.x
MachECS is an entity-component-system (ECS) library for .NET Standard 2.0 and .NET 6 applications:
* ECS is an [architecture pattern](https://www.guru99.com/entity-component-system.html), where state and logic are
separated from each other.
* Components hold state.
* Entities group components.
* Systems apply logic to state via entities.

# Installation
Add the NuGet package to your .NET Standard 2.0 or .NET 6 application.

# Usage
In short summary:
* Define classes that implement ```IEcsComponent``` that hold data (x,y positions, etc).
* Define classes that implement ```IEcsEvent``` that represent events in your application. These may be empty
depending if the event is forwarding (or receiving back) data or not.
* Define classes that implement ```EcsSystem``` that represent systems in your application. The constructor should be
calling ```Agent.RegisterEventHandler<T>()``` to register event handlers.

* Create an ```Agent``` via ```Agent.CreateInstance()```.
* Register your components with ```Agent.RegisterComponent<T>()```.
* Register your systems with ```Agent.RegisterSystem<T>()```
* You should send an event that starts the system(s) logic to perform whatever your application does (game, etc).
You could also create entities that represent things in your application, such as the player or enemies for a game,
and attach relevant components to them.

For more details of what all this means, keep reading!

### Create an agent
An ECS world is interacted with via an ```Agent```. The agent allows you to create entities, get components, etc.
To create one, use something like the following:
```C#
Agent agent = Agent.CreateInstance(5000, EcsSignatureType.SingleLong);
```
The first parameter to *CreateInstance()* is the maximum amount of entities that can exist at once. The second
parameter is the signature type to use: this determines how many unique component types can be used at once:
* ```EcsSignatureType.SingleLong``` is the fastest and supports up to 64 components.
* ```EcsSignatureType.DoubleLong``` is also very fast and supports up to 128 components.
* ```EcsSignatureType.BitArray1K``` is slower but supports up to 1,000 components.

### Define components
Define classes that implement the ```IEcsComponent``` interface, for example:
```C#
  public class PositionComponent : IEcsComponent
  {
    public float X { get; set; }
    public float Y { get; set; }
  }
```
You need to then register them explicitly with the agent via ```myAgent.RegisterComponent<PositionComponent>()```,
or you can implicitly register them via a system. This will be explained later.

### Define systems
Define classes that implement the ```EcsSystem``` abstract class. You can optionally specify generic parameters
when you inherit from this class, from 1 to 10, of components that this system is interested in working with.
For example, this system is interested in entities with the ```PositionComponent``` and ```VelocityComponent```
components (and these components will implicitly registered if they are not already):
```C#
  public sealed class MovementSystem : EcsSystem<PositionComponent, VelocityComponent>
  {
    public MovementSystem(Agent agent) : base(agent)
    {
    }
  }
```
The point of defining components to the system is so that the agent knows what entities to populate each system's
```EcsSystem.Entities``` list with. A system can query all entities in it's ```EcsSystem.Entities``` list for any
of the components in the system's signature/generic parameter list. This is core to how a system works: the system
is invoked via method or event, and it applys logic to all the entities that hold the specified components.

### Register systems
To create and hook a system into the agent, you need to register it:
```C#
var iWantAReferenceToSystem = agent.RegisterSystem<MovementSystem>();
_ = agent.RegisterSystem<SomeOtherSystem>();
```

You are free to discard the system returned by ```RegisterSystem<T>()```, as the agent will hold a reference
for itself. If you're wondering how a system is interacted with if the returned reference is thrown away, you
use constructor of the system to register event handlers which will be explained later.

### Create entities
Things in your application will be defined by their components; these components are grouped together by an entity.
Creating an entity and grouping components is done by:
```C#
var entity = myAgent.CreateEntity();
var position = new PositionComponent { X = 5, Y = 10 };
var velocity = new VelocityComponent { DX = 2.2f, DY = 3.3f };
myAgent.AddComponent(entity, position);
myAgent.AddComponent(entity, velocity);
```
To add/get a component as a "singleton" (a component that exists as part of the global state in the ECS world, such as
a display context), you can leave out the entity:
```C#
  var displayContext = new DisplayContext();
  myAgent.AddComponent(displayContext);
  var iNeedADisplayContext = myAgent.GetComponent<DisplayContext>();
```
The agent reserves the first entity to represent the agent; this agent entity is used to attach components to when no entity
is specified.

### System events
In the constructor of a system, you must pass a provided ```Agent``` instance to the base class. Additionally, you can
perform any initialization your system needs, including registering event handlers. Events are defined by a class that
inherits ```IEcsEvent```, for example:
```C#
  public sealed class GetStuffEvent : IEcsEvent
  {
    public string Stuff { get; set; } = string.Empty;
  }
```
To register a handler when this event is sent:
```C#
  public sealed class MovementSystem : EcsSystem<PositionComponent, VelocityComponent>
  {
    public MovementSystem(Agent agent) : base(agent)
    {
      agent.RegisterEventHandler<GetStuffEvent>(GetStuffHandler);
    }

    // the below line could also accept no parameters if not needed, ie: private void GetStuffHandler()
    private void GetStuffHandler(GetStuffEvent eventData)
    {
      foreach (var entity in Entities)
      {
        var position = Agent.GetComponent<PositionComponent>(entity);
        var velocity = Agent.GetComponent<VelocityComponent>(entity);
        UpdatePosition(position, velocity);
        eventData.Stuff = "Stuff done.";
      }
    }
  }
```
Since the system class has access to the inherited ```EcsSystem.Agent``` and ```EcsSystem.Entities```, events can
kick off logic in the system that can iterate over entities for their components.

# History
As of version 3.0.26, MachECS is now a .NET Standard 2.0 and .NET 6 library.
