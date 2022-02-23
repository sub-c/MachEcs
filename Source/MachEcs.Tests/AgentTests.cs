using MachEcs.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SubC.MachEcs;
using SubC.MachEcs.Workers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MachEcs.Tests
{
    [TestClass]
    public class AgentTests
    {
        private const int MaximumEntities = 5000;

        private IDictionary<Type, IComponentCache> ComponentCaches =>
            _agent.GetPrivateField<ComponentWorker>("_componentWorker").GetPrivateField<IDictionary<Type, IComponentCache>>("_caches");

        private IDictionary<Type, EcsSystem> EcsSystems =>
            _agent.GetPrivateField<SystemWorker>("_systemWorker").GetPrivateField<IDictionary<Type, EcsSystem>>("_systems");

        private IDictionary<Type, IEventHandlerCache> EventCaches =>
            _agent.GetPrivateField<EventWorker>("_eventWorker").GetPrivateField<IDictionary<Type, IEventHandlerCache>>("_eventHandlerCaches");

        private Agent _agent = new(0);

        [TestInitialize]
        public void Initialize()
        {
            _agent = new Agent(MaximumEntities);
        }

        [TestMethod]
        public void AddComponent_GivenNoEntityWithComponent_AddsComponentToSingletonEntity()
        {
            // Arrange
            _agent.RegisterComponent<TestComponent>();
            var component = new TestComponent { Data = "test" };

            // Act
            _agent.AddComponent(component);
            var receivedComponent = _agent.GetComponent<TestComponent>();

            // Assert
            Assert.AreEqual("test", receivedComponent.Data, "Component added did not match the component cached.");
        }

        [TestMethod]
        public void RegisterComponents_WithAssembly_RegistersComponents()
        {
            // Arrange
            var beforeCount = ComponentCaches.Count;

            // Act
            _agent.RegisterComponents(Assembly.GetExecutingAssembly());

            // Assert
            Assert.AreEqual(0, beforeCount, "Components were registered before test.");
            Assert.AreEqual(1, ComponentCaches.Count, "Exactly one component was not registered.");
        }

        [TestMethod]
        public void RegisterComponents_WithComponent_RegistersComponent()
        {
            // Arrange
            var beforeCount = ComponentCaches.Count;

            // Act
            _agent.RegisterComponent<TestComponent>();

            // Assert
            Assert.AreEqual(0, beforeCount, "Components were registered before test.");
            Assert.AreEqual(1, ComponentCaches.Count, "Exactly one component was not registered.");
        }

        [TestMethod]
        public void RegisterSystem_WhenEntityContainsComponent_SystemPopulatedWithEntity()
        {
            // Arrange
            var entity = _agent.CreateEntity();
            var component = new TestComponent();
            _agent.RegisterComponent<TestComponent>();

            // Act
            var system = _agent.RegisterSystem<TestSystem>();
            var before = system.InternalEntities.Count();
            _agent.AddComponent(entity, component);

            // Assert
            Assert.AreEqual(0, before, "System contained entities before adding.");
            Assert.AreEqual(1, system.InternalEntities.Count(), "System did not contain entity after adding component.");
            Assert.AreEqual(entity, system.InternalEntities.First(), "Entity in system is different than the one added.");
            Assert.AreEqual(component, _agent.GetComponent<TestComponent>(entity), "Entity component is different than the one added.");
        }

        [TestMethod]
        public void SendEvent_WhenGivenEventData_InvokesEventHandler()
        {
            // Arrange
            var eventData = new TestEventData { Data = "Test" };
            var dataSent = string.Empty;
            _agent.RegisterEvent<TestEventData>();
            _agent.AddEventHandler<TestEventData>((x) => { dataSent = x.Data; });

            // Act
            _agent.SendEvent(eventData);

            // Assert
            Assert.AreEqual(1, EventCaches.Count(), "Event cache did not contain exactly one event handler cache.");
            Assert.AreEqual("Test", dataSent, "Event data did not propogate to event handler.");
        }

        [TestMethod]
        public void SendEvent_WhenInvoked_InvokesEventHandler()
        {
            // Arrange
            var eventData = new TestEventData();
            var dataSent = false;
            _agent.RegisterEvent<TestEventData>();
            _agent.AddEventHandler<TestEventData>(() => { dataSent = true; });

            // Act
            _agent.SendEvent<TestEventData>();

            // Assert
            Assert.AreEqual(1, EventCaches.Count(), "Event cache did not contain exactly one event handler cache.");
            Assert.AreEqual(true, dataSent, "Event did not propogate to event handler.");
        }
    }
}
