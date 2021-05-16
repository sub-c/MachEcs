# MachEcs
MachEcs is an entity-component-system (ECS) library for C# applications:
* ECS is an [architecture pattern](https://www.guru99.com/entity-component-system.html), where state and logic are separated from each other.
* Entities represent things/objects, and have components added/removed to them.
* Components hold a state and have no logic.
* Systems have logic and have no state.
* Systems iterate over entities, using their state (components) to perform work/output.

# Installation
Add the Nuget package ```MachEcs``` to your .NET project.

# Basic Usage
* Register your components with either ```MachAgent.RegisterComponent<T>()``` or ```MachAgent.RegisterComponentsInAssembly()```.
* Register your systems with either ```MachAgent.RegisterSystem<T>()``` or ```MachAgent.RegisterSystemsInAssembly()```.

* Set your system(s) signature with the component types that they are interested in entities having with ```MachSignature.Add(MachAgent.GetComponentSignature<T>())```, and then ```MachAgent.SetSystemSignature<T>()```.

* Create entities with ```MachAgent.CreateEntity()```.
* New-up a (previously registered) component instance.
* Add the component to your entity with ```MachAgent.AddComponent<T>()```, or add the component to the singleton entity by not providing an entity parameter to ```AddComponent<T>()```.

* Your system(s) will have the entity listed in ```MachSystem.Entities```, if the entity's attached components match the system signature (at minimum, more components can be attached).
* Your systems can use the ```MachSystem.Agent``` to get the components of ```MachSystem.Entities``` by calling ```MachAgent.GetComponent<T>()```.