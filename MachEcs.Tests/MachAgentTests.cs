using System;
using System.Collections.Generic;
using System.Linq;
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
        }

        [TestMethod]
        public void RegisterSystem_WhenGivenGenericType_ReturnsSystemInstance()
        {
        }

        [TestMethod]
        public void RemoveComponent_WhenGivenGenericType_RemovesFromEntity()
        {
        }
    }
}
