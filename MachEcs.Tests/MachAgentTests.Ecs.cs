using System;
using System.Collections.Generic;
using System.Linq;
using MachEcs.Tests.TestClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SubC.MachEcs;
using SubC.MachEcs.Components;
using SubC.MachEcs.Entities;
using SubC.MachEcs.Systems;

namespace MachEcs.Tests
{
    [TestClass]
    public sealed class MachAgentTestsEcs
    {
        private const int MaximumAvailableEntities = MaximumTotalEntities - 1;
        private const int MaximumTotalEntities = 5000;

        private ComponentManager AgentComponentManager
            => TestUtilities.GetPrivateInstanceField<ComponentManager>(_machAgent, "_componentManager");

        private EntityManager AgentEntityManager
            => TestUtilities.GetPrivateInstanceField<EntityManager>(_machAgent, "_entityManager");

        private MachEntity AgentSingletonEntity
            => TestUtilities.GetPrivateInstanceField<MachEntity>(_machAgent, "_singletonEntity");

        private SystemManager AgentSystemManager
            => TestUtilities.GetPrivateInstanceField<SystemManager>(_machAgent, "_systemManager");

        private Dictionary<string, IComponentCache> ComponentManagerCaches
            => TestUtilities.GetPrivateInstanceField<Dictionary<string, IComponentCache>>(AgentComponentManager, "_componentCaches");

        private MachAgent _machAgent = null;

        [TestInitialize]
        public void TestInitialize()
        {
            _machAgent = new MachAgent(MaximumTotalEntities);
        }

        [TestMethod]
        public void AddComponent_WhenInvoked_AddsComponentToSingletonEntity()
        {
            // Arrange
            var component = new TestComponent();
            _machAgent.RegisterComponent<TestComponent>();

            // Act
            _machAgent.AddComponent(component);

            // Assert
            Assert.IsNotNull(_machAgent.GetComponent<TestComponent>(), "Component was not returned from singleton entity.");
        }

        [TestMethod]
        public void AddComponent_WhenInvokedWithEntity_AddsComponentToEntity()
        {
            // Arrange
            var component = new TestComponent();
            var entity = new MachEntity();
            _machAgent.RegisterComponent<TestComponent>();

            // Act
            _machAgent.AddComponent(entity, component);

            // Assert
            Assert.IsNotNull(_machAgent.GetComponent<TestComponent>(entity), "Component was not returned from given entity.");
        }

        [TestMethod]
        public void AddComponent_WhenComponentMatchesSystemSignature_AddsToSystemEntities()
        {
            // Arrange
            _machAgent.RegisterComponent<TestSystemComponent>();
            var system = _machAgent.RegisterSystem<TestSystem>();
            var entity = _machAgent.CreateEntity();
            var component = new TestSystemComponent { TestString = "test" };
            var systemEntitiesBeforeCount = system.InternalEntities.Count;

            // Act
            _machAgent.AddComponent(entity, component);

            // Assert
            Assert.AreEqual(0, systemEntitiesBeforeCount, "System contained entities before adding component.");
            Assert.AreEqual(1, system.InternalEntities.Count, "System did not update to contain entity with matching signature.");
            Assert.AreEqual("test", _machAgent.GetComponent<TestSystemComponent>(entity).TestString,
                "Component from system entity did not match the component added.");
        }

        [TestMethod]
        public void CreateEntity_WhenInvoked_ReturnsNewEntity()
        {
            // Arrange

            // Act
            var entity = _machAgent.CreateEntity();

            // Assert
            Assert.IsNotNull(entity, "Could not create a new entity.");
        }

        [TestMethod]
        public void CreateEntity_WhenInvokedToMaximum_ReturnsNewEntities()
        {
            // Arrange
            var entities = new List<MachEntity>();

            // Act
            for (int i = 0; i < MaximumAvailableEntities; i++)
            {
                entities.Add(_machAgent.CreateEntity());
            }

            // Assert
            Assert.AreEqual(MaximumAvailableEntities, entities.Count, "Could not create exactly maximum entities.");
            Assert.AreEqual(MaximumAvailableEntities, entities.Distinct().Count(), "Entities created are not all unique.");
        }

        [TestMethod]
        public void CreateEntity_WhenInvokedPastMaximum_ThrowsException()
        {
            // Arrange
            var entities = new List<MachEntity>();

            // Act
            for (int i = 0; i < MaximumAvailableEntities; i++)
            {
                entities.Add(_machAgent.CreateEntity());
            }

            // Assert
            ExceptionAssert.Throws<Exception>(() => _machAgent.CreateEntity());
        }

        [TestMethod]
        public void RegisterSystem_WhenInvoked_AbleToRetrieveSystemWithGetSystem()
        {
            // Arrange
            _machAgent.RegisterComponent<TestSystemComponent>();
            _machAgent.RegisterSystem<TestSystem>();

            // Act
            var returnedSystem = _machAgent.GetSystem<TestSystem>();

            // Assert
            Assert.IsNotNull(returnedSystem, "Could not retrieve registered system.");
        }
    }
}
