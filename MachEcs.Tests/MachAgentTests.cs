using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MachEcs.Tests.TestClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SubC.MachEcs;
using SubC.MachEcs.Workers;

namespace MachEcs.Tests
{
    [TestClass]
    public class MachAgentTests
    {
        private const int MaximumEntities = 5000;

        private MachAgent _agent;

        private IDictionary<Type, IComponentCache> ComponentWorkerCaches =>
            _agent.GetPrivateField<ComponentWorker>("_componentWorker").GetPrivateField<IDictionary<Type, IComponentCache>>("_caches");

        private Queue<MachEntity> EntityWorkerEntities =>
            _agent.GetPrivateField<EntityWorker>("_entityWorker").GetPrivateField<Queue<MachEntity>>("_entities");

        [TestInitialize]
        public void TestInitialize()
        {
            _agent = new MachAgent(MaximumEntities);
        }

        [TestMethod]
        public void AddComponent_WhenGivenComponent_AddsToEntity()
        {
            // Arrange
            _agent.RegisterComponent<TestComponent1>();
            var entity = _agent.CreateEntity();
            var component = new TestComponent1();

            //Act
            _agent.AddComponent(entity, component);

            // Assert
            var componentCache = (ComponentCache<TestComponent1>)ComponentWorkerCaches[typeof(TestComponent1)];
            var cache = componentCache.GetPrivateField<IDictionary<MachEntity, TestComponent1>>("_cache");
            Assert.IsTrue(cache.ContainsKey(entity), $"Given entity was not added to cache.");
            Assert.AreEqual(component, cache[entity], $"Given component was not added to cache.");
        }

        [TestMethod]
        public void AddComponent_WhenMatchesSystemSignature_AddsToSystem()
        {
            // Arrange
            _agent.RegisterComponent<TestComponent1>();
            _agent.RegisterComponent<TestComponent2>();
            var system = _agent.RegisterSystem<TestSystem>();
            var systemEntities = system.GetPrivateProperty<IEnumerable<MachEntity>>("Entities");
            var entity = _agent.CreateEntity();
            var component1 = new TestComponent1();
            var component2 = new TestComponent2();
            Assert.IsTrue(systemEntities.Count() == 0, "System has entities present before adding component and entity.");

            // Act
            _agent.AddComponent(entity, component1);
            _agent.AddComponent(entity, component2);

            // Assert
            Assert.IsTrue(systemEntities.Count() == 1, "System did not populate entity into system when signatures matched.");
            Assert.AreEqual(entity, systemEntities.First(), "Entity populated in system did not match the entity with components added to it.");
        }

        [TestMethod]
        public void CreateEntity_WhenMaximum_ReturnMaximumEntities()
        {
            // Arrange
            var returnedEntities = new List<MachEntity>();

            // Act
            for (var i = 0; i < MaximumEntities; ++i)
            {
                returnedEntities.Add(_agent.CreateEntity());
            }

            // Assert
            Assert.AreEqual(
                MaximumEntities,
                returnedEntities.Count,
                $"Did not return the maximum amount of entities.");
        }

        [TestMethod]
        public void DestroyEntity_WhenDestroyedMaximum_AddsEntitiesBackForUse()
        {
            // Arrange
            var entitiesInUse = new List<MachEntity>();
            for (var i = 0; i < MaximumEntities; ++i)
            {
                entitiesInUse.Add(_agent.CreateEntity());
            }

            // Act
            for (var i = 0; i < MaximumEntities; ++i)
            {
                _agent.DestroyEntity(entitiesInUse.First());
            }

            // Assert
            Assert.AreEqual(
                MaximumEntities,
                EntityWorkerEntities.Count,
                $"Destroying entities did not add it back to the queue.");
        }

        [TestMethod]
        public void GetComponentSignature_WhenGivenGenericType_ReturnsSignature()
        {
            // Arrange
            _agent.RegisterComponent<TestComponent1>();
            _agent.RegisterComponent<TestComponent2>();

            // Act
            var signature1 = _agent.GetComponentSignature<TestComponent1>();
            var signature2 = _agent.GetComponentSignature<TestComponent2>();

            // Assert
            Assert.AreNotEqual(signature1, signature2, $"Component signatures are not unique.");
        }

        [TestMethod]
        public void GetComponentSignature_WhenGivenType_ReturnsSignature()
        {
            // Arrange
            _agent.RegisterComponent<TestComponent1>();

            // Act
            var componentSignature = _agent.GetComponentSignature(typeof(TestComponent1));

            // Assert
            Assert.IsNotNull(componentSignature, "Signature was null.");
        }

        [TestMethod]
        public void RegisterComponent_WhenGivenGenericType_RegistersComponent()
        {
            // Arrange
            Assert.IsFalse(
                ComponentWorkerCaches.ContainsKey(typeof(TestComponent1)),
                $"Component was already registered before calling method.");

            // Act
            _agent.RegisterComponent<TestComponent1>();

            // Assert
            Assert.IsTrue(
                ComponentWorkerCaches.ContainsKey(typeof(TestComponent1)),
                $"Registering component did not add it to the component caches.");
        }

        [TestMethod]
        public void RegisterComponents_WhenGivenAssembly_RegistersComponents()
        {
            // Arrange

            // Act
            _agent.RegisterComponents(Assembly.GetExecutingAssembly());

            // Assert
            Assert.AreEqual(2, ComponentWorkerCaches.Count, $"Exactly two components in the assembly were not registered.");
            Assert.IsTrue(
                ComponentWorkerCaches.ContainsKey(typeof(TestComponent1)),
                $"{nameof(TestComponent1)} was not registered from assembly.");
            Assert.IsTrue(
                ComponentWorkerCaches.ContainsKey(typeof(TestComponent2)),
                $"{nameof(TestComponent2)} was not registered from assembly.");
        }

        [TestMethod]
        public void RegisterSystem_WhenGivenGenericType_ReturnsSystemInstance()
        {
            // Arrange
            _agent.RegisterComponent<TestComponent1>();
            _agent.RegisterComponent<TestComponent2>();

            // Act
            var system = _agent.RegisterSystem<TestSystem>();

            // Assert
            Assert.IsNotNull(system, "System was null.");
        }

        [TestMethod]
        public void RemoveComponent_WhenGivenGenericType_RemovesFromEntity()
        {
            // Arrange
            _agent.RegisterComponent<TestComponent1>();
            var entity = _agent.CreateEntity();
            var component = new TestComponent1();
            _agent.AddComponent(entity, component);
            var componentCache = (ComponentCache<TestComponent1>)ComponentWorkerCaches[typeof(TestComponent1)];
            var cache = componentCache.GetPrivateField<IDictionary<MachEntity, TestComponent1>>("_cache");
            Assert.IsTrue(cache.ContainsKey(entity), "Entity was not added to component cache.");

            // Act
            _agent.RemoveComponent<TestComponent1>(entity);

            // Assert
            Assert.IsFalse(cache.ContainsKey(entity), "Entity was not removed from component cache.");
        }

        [TestMethod]
        public void RemoveComponent_WhenEntityNoLongerMatchesSystemSignature_EntityRemovedFromSystem()
        {
            // Arrange
            _agent.RegisterComponent<TestComponent1>();
            _agent.RegisterComponent<TestComponent2>();
            var system = _agent.RegisterSystem<TestSystem>();
            var systemEntities = system.GetPrivateProperty<IEnumerable<MachEntity>>("Entities");
            var entity = _agent.CreateEntity();
            _agent.AddComponent(entity, new TestComponent1());
            _agent.AddComponent(entity, new TestComponent2());
            Assert.IsTrue(systemEntities.Count() == 1, "Entity was not added to system.");
            Assert.AreEqual(entity, systemEntities.First(), "Entity added to system was not the same one as components were added to.");

            // Act
            _agent.RemoveComponent<TestComponent2>(entity);

            // Assert
            Assert.IsTrue(systemEntities.Count() == 0, "Entity with non-matching components was still present in system entities.");
        }
    }
}
