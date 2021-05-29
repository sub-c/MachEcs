using System.Collections.Generic;
using System.Linq;
using MachEcs.Tests.TestClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SubC.MachEcs;
using SubC.MachEcs.Events;

namespace MachEcs.Tests
{
    [TestClass]
    public sealed class MachAgentTestsEvents
    {
        private static readonly MachEventTopic<TestEventArgData> ExpectedEventTopic = new TestEventTopic();

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
        public void RemoveAllSubscribersFromEvent_WhenPassedTopic_RemovesAllSubscribers()
        {
            // Arrange
            var testEventHandler = new TestEventHandler();
            _agent.RegisterEventTopic(ExpectedEventTopic);
            _agent.SubscribeToEventTopic(ExpectedEventTopic, testEventHandler.TestHandler);
            var subscribersBefore =
                (EventManagerTopicSubscriptions[ExpectedEventTopic] as MachEventSubscribers<TestEventArgData>).Subscribers.GetInvocationList().Length;

            // Act
            _agent.RemoveAllSubscribersFromEvent(ExpectedEventTopic);

            // Assert
            Assert.AreEqual(1, subscribersBefore, "Subscribing did not add method to handlers.");
            Assert.AreEqual(
                null,
                (EventManagerTopicSubscriptions[ExpectedEventTopic] as MachEventSubscribers<TestEventArgData>).Subscribers,
                "Removing all subscribers did not remove method from handlers.");
        }

        [TestMethod]
        public void SendEvent_WhenInvoked_EventHandlerInvoked()
        {
            // Arrange
            _agent.RegisterEventTopic(ExpectedEventTopic);
            var actualEventString = string.Empty;
            _agent.SubscribeToEventTopic(
                ExpectedEventTopic,
                (eventArgs) =>
                {
                    actualEventString = eventArgs.EventData.TestString;
                });
            var eventArgs = new MachEventArgs<TestEventArgData>(new TestEventArgData { TestString = "ttteeesssttt" });

            // Act
            _agent.SendEvent(ExpectedEventTopic, eventArgs);

            // Assert
            Assert.AreEqual(eventArgs.EventData.TestString, actualEventString, "Sending the event did not invoke the event topic handlers.");
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
            var eventSubscribers = EventManagerTopicSubscriptions[ExpectedEventTopic] as MachEventSubscribers<TestEventArgData>;
            MachEventTopic<TestEventArgData>.MachEventHandler methodAsDelegate = testEventHandler.TestHandler;
            Assert.AreEqual(1, eventSubscribers.Subscribers.GetInvocationList().Length, "Method was not added to event topic subscriptions.");
            Assert.AreEqual(
                (MachEventTopic<TestEventArgData>.MachEventHandler)testEventHandler.TestHandler,
                eventSubscribers.Subscribers.GetInvocationList()[0],
                "Method subscribed to event topic was not expected method.");
        }

        [TestMethod]
        public void UnregisterEventTopic_WhenInvoked_EventTopicIsUnregistered()
        {
            // Arrange
            _agent.RegisterEventTopic(ExpectedEventTopic);
            bool topicRegisteredBefore = EventManagerTopicSubscriptions.ContainsKey(ExpectedEventTopic);

            // Act
            _agent.UnregisterEventTopic(ExpectedEventTopic);

            // Assert
            Assert.IsTrue(topicRegisteredBefore, "Event topic was not registered.");
            Assert.IsFalse(EventManagerTopicSubscriptions.ContainsKey(ExpectedEventTopic), "Event topic was not unregistered.");
        }

        [TestMethod]
        public void UnsubscribeFromEventTopic_WhenInvoked_RemovesSubscribedMethodFromTopic()
        {
            // Arrange
            var mockEventHandler = new Mock<MachEventTopic<TestEventArgData>.MachEventHandler>();
            _agent.RegisterEventTopic(ExpectedEventTopic);
            _agent.SubscribeToEventTopic(ExpectedEventTopic, mockEventHandler.Object);
#pragma warning disable CS0252
            var subscribedMethodBefore = (EventManagerTopicSubscriptions[ExpectedEventTopic] as MachEventSubscribers<TestEventArgData>)
                .Subscribers
                .GetInvocationList()
                .ToList()
                .FirstOrDefault(x => x == mockEventHandler.Object);
#pragma warning restore CS0252

            // Act
            _agent.UnsubscribeFromEventTopic(ExpectedEventTopic, mockEventHandler.Object);

            // Assert
            Assert.IsNotNull(subscribedMethodBefore, "Method was not subscribed before unsubscribing.");
            var expectedEventSubscribers = EventManagerTopicSubscriptions[ExpectedEventTopic] as MachEventSubscribers<TestEventArgData>;
            Assert.IsNull(expectedEventSubscribers.Subscribers, "Method was not unsubscribed.");
        }
    }
}
