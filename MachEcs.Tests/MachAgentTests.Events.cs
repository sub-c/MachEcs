using System.Collections.Generic;
using MachEcs.Tests.TestClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SubC.MachEcs;
using SubC.MachEcs.Events;

namespace MachEcs.Tests
{
    [TestClass]
    public sealed class MachAgentTests
    {
        private static readonly MachEventTopic<TestEventArgs> ExpectedEventTopic = new TestEventTopic();

        private EventManager AgentEventManager =>
            TestUtilities.GetPrivateInstanceField<EventManager>(_agent, "_eventManager");

        private IDictionary<IMachEventTopic, IMachEventSubscribers> EventManagerTopicSubscriptions =>
            TestUtilities.GetPrivateInstanceField<IDictionary<IMachEventTopic, IMachEventSubscribers>>(AgentEventManager, "_topicSubscriptions");
        
        private MachAgent _agent = null;

        [TestInitialize]
        public void Initialize()
        {
            _agent = new MachAgent(1000);
        }

        [TestMethod]
        public void RegisterEventTopic_WhenInvoked_RegistersEventTopic()
        {
            // Arrange
            var registeredTopicsBefore = EventManagerTopicSubscriptions.Count;

            // Act
            _agent.RegisterEventTopic(ExpectedEventTopic);

            // Assert
            Assert.AreEqual(0, registeredTopicsBefore, "Event topics were registered before registering topic.");
            Assert.IsTrue(EventManagerTopicSubscriptions.ContainsKey(ExpectedEventTopic), "Event topic was not registered.");
        }

        [TestMethod]
        public void SubscribeToEventTopic_WhenPassedMethod_SubscribesMethod()
        {
            // Arrange
            var testEventHandler = new TestEventHandler();
            _agent.RegisterEventTopic(ExpectedEventTopic);

            // Act
            _agent.SubscribeToEventTopic(ExpectedEventTopic, testEventHandler.TestHandler);

            // Assert
        }
    }
}
