# MachEcs
MachEcs is an entity-component-system (ECS) library for C# applications:
* ECS is an [architecture pattern](https://www.guru99.com/entity-component-system.html), where state and logic are separated from each other.
* Components only hold state/data.
* Entities only are groupings of components.
* Systems only have logic, operating on entities that have components they are interested in.

# Installation
Add the [Nuget package MachEcs](https://www.nuget.org/packages/MachEcs/) to your .NET project.

# Basic Usage
* Add the [Nuget package](https://www.nuget.org/packages/MachEcs/) to your C# project.
* Add ```using SubC.MachEcs;``` to your using statements.
* Ensure your component classes inherit from ```IMachComponent```.
* Ensure your system classes inherit/implement ```MachSystem```.
* New up a MachAgent with ```var machAgent = new MachAgent(5000);```.
* Register your components with ```machAgent.RegisterComponent<YourComponentType>();```.
* Register your systems with ```var yourSystem = machAgent.RegisterSystem<YourSystemType>();```.
* Systems must implement a type array of components they are interested in working with: entities with matching components will automatically be populated into those systems ```Entities``` member property.
* Full example:
```C#
using SubC.MachEcs;

var machAgent = new MachAgent(5000);
machAgent.RegisterComponent<MyPlayerStatusComponent>();
var myPlayerStatusSystem = machAgent.RegisterSystem<MyPlayerStatusSystem>();
var myPlayerEntity = machAgent.CreateEntity();
var myplayerStatus = new MyPlayerStatusComponent { Lives = 5 };
machAgent.AddComponent(myPlayerEntity, myPlayerStatus);

myPlayerStatusSystem.DoWork(); // system can enumerate the Entities property to automatically get the myPlayerEntity
```
* Systems can use their Agent property to then get at the components from the entities:
```C#
foreach (var entity in Entities)
{
    var playerStatus = Agent.GetComponent<MyPlayerStatusComponent>(entity);
    // do work with each playerStatus...
}
```


# MachEcs.Events
MachEcs.Events is an extension library for MachEcs; this library adds event support to the ECS world using the agent (```MachAgent```) as the anchor for registering event handlers and sending events.

# Installation
Add the [Nuget package](https://www.nuget.org/packages/MachEcs.Events) to your .NET project.

# Basic Usage
* Add the nuget package as described above.
* Add ```using SubC.MachEcs.Events``` to your using statements.
* Register the agent into the event extension by calling the ```RegisterAgentEvents()``` extension method on your agent.
* Register events with the ```RegisterEvent<T>()``` extension method on your agent. Your event classes/DTOs must implement the ```IMachEvent``` interface.
* Add event handlers (it is intended that your systems will have methods to handle events) with ```AddEventHandler<T>(Action<T>)``` extension method on your agent.
* Send events in your program with the ```SendEvent<T>(T)``` extension method on your agent. Using events, you can implement actions without linking the code that raised the event and the code that handles the event.

* Full example:
```C#
using SubC.MachEcs.Events;

class MyEventDto : IMachAgent { }

// .... other MachEcs code that, among other things, creates a MachAgent instance called 'agent'...
agent.RegisterAgentEvents();
agent.RegisterEvent<MyEventDto>();
agent.AddEventHandler(SomeMachSystem.SomeEventHandler);

// .... inside some code in a system...
Agent.SendEvent(new MyEventDto { ... }); // SomeMachSystem.SomeEventHandler() will be invoked and receive the event class
```
