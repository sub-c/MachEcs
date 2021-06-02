using System.Collections.Generic;
using MachEcs.Tests.TestClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SubC.MachEcs;
using SubC.MachEcs.Events;

namespace MachEcs.Tests
{
    [TestClass]
    public sealed class MachAgentTestsEvents
    {
        private EventManager AgentEventManager =>
            TestUtilities.GetPrivateInstanceField<EventManager>(_agent, "_eventManager");

        private IDictionary<string, IMachEventSubscribers> EventManagerTopicSubscriptions =>
            TestUtilities.GetPrivateInstanceField<IDictionary<string, IMachEventSubscribers>>(AgentEventManager, "_eventSubscriptions");
        
        private MachAgent _agent = null;

        [TestInitialize]
        public void Initialize()
        {
            _agent = new MachAgent(1000);
        }

        [TestMethod]
        public void RegisterEvent_WhenGivenNewType_RegistersNewType()
        {
            // Arrange
            var registrationsBefore = EventManagerTopicSubscriptions.Count;

            // Act
            AgentEventManager.RegisterEvent<TestEventArgData>();

            // Assert
            Assert.AreEqual(0, registrationsBefore, "There were events registered before registering event.");
            Assert.AreEqual(1, EventManagerTopicSubscriptions.Count, "Exactly one event was not registered.");
        }

        [TestMethod]
        public void SendEvent_WhenGivenEventArgInstance_InvokesSubscribers()
        {
            // Arrange
            AgentEventManager.RegisterEvent<TestEventArgData>();
            var subscriberInvoked = false;
            AgentEventManager.SubscribeToEvent<TestEventArgData>((x) => { subscriberInvoked = true; });

            // Act
            AgentEventManager.SendEvent(new TestEventArgData());

            // Assert
            Assert.IsTrue(subscriberInvoked, "Sending event did not invoke subscribers.");
        }

        [TestMethod]
        public void SubscribeToEvent_WhenGivenEventHandler_AddsToSubscribers()
        {
            // Arrange
            AgentEventManager.RegisterEvent<TestEventArgData>();
            var eventSubscribers = EventManagerTopicSubscriptions[nameof(TestEventArgData)] as MachEventSubscribers<TestEventArgData>;
            var subscribers = eventSubscribers.MachEventHandlers?.GetInvocationList();

            // Act
            AgentEventManager.SubscribeToEvent<TestEventArgData>((x) => { });

            // Assert
            Assert.IsNull(subscribers, "Event handlers were subscribed before running test.");
            Assert.IsNotNull(eventSubscribers.MachEventHandlers?.GetInvocationList(), "Event handlers were not subscribed after running test.");
        }

        [TestMethod]
        public void UnregisterEvent_WhenInvokedWithPreviouslyRegisteredEvent_RemovesEvent()
        {
            // Arrange
            AgentEventManager.RegisterEvent<TestEventArgData>();
            var countBefore = EventManagerTopicSubscriptions.Count;

            // Act
            AgentEventManager.UnregisterEvent<TestEventArgData>();

            // Assert
            Assert.AreEqual(1, countBefore, "Registering event did not add it to subscriptions.");
            Assert.AreEqual(0, EventManagerTopicSubscriptions.Count, "Unregistering event did not remove it from subscriptions.");
        }

        [TestMethod]
        public void UnsubscribeFromEvent_WhenGivenMethod_RemovesMethodFromSubscriptions()
        {
            // Arrange
            AgentEventManager.RegisterEvent<TestEventArgData>();
            var eventHandler = new HandleMachEvent<TestEventArgData>((x) => { });
            AgentEventManager.SubscribeToEvent(eventHandler);
            var subscribers = EventManagerTopicSubscriptions[nameof(TestEventArgData)] as MachEventSubscribers<TestEventArgData>;
            var subscribersBefore = subscribers.MachEventHandlers?.GetInvocationList();

            // Act
            AgentEventManager.UnsubscribeFromEvent(eventHandler);

            // Assert
            Assert.IsNotNull(subscribersBefore, "Subscribing to event did not add handler.");
            Assert.IsNull(subscribers.MachEventHandlers?.GetInvocationList(), "Unsubscribing from event did not remove event handler.");
        }
    }
}
