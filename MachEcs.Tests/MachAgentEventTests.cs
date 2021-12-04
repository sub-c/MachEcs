using MachEcs.Tests.TestClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SubC.MachEcs;
using SubC.MachEcs.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MachEcs.Tests
{
    [TestClass]
    public class MachAgentEventTests
    {
        private const int MaximumEntities = 5000;

        private AgentCache EventAgentCache => EventAgentCaches[_agent];

        private IDictionary<MachAgent, AgentCache> EventAgentCaches
            => TestExtensions.GetStaticPrivateField<IDictionary<MachAgent, AgentCache>>(typeof(MachAgentEventExtensions), "_agentCaches");

        private MachEventCache<TestEventArg> TestEventArgEventCache
            => (MachEventCache<TestEventArg>)EventAgentCache.EventCaches[typeof(TestEventArg)];

        private MachAgent _agent;

        [TestInitialize]
        public void TestInitialize()
        {
            if (EventAgentCaches.Count() > 0)
            {
                EventAgentCaches.Clear();
            }
            _agent = new MachAgent(MaximumEntities);
        }

        [TestMethod]
        public void AddEventHandler_WhenInvokedWithEventHandler_AddsEventHandler()
        {
            // Arrange
            var eventHandler = new Action<TestEventArg>((args) => { });
            _agent.RegisterAgentEvents();
            _agent.RegisterEvent<TestEventArg>();
            Assert.IsNull(TestEventArgEventCache.EventListeners?.GetInvocationList().Length, "Event handler was present before adding.");

            // Act
            _agent.AddEventHandler(eventHandler);

            // Assert
            Assert.AreEqual(1, TestEventArgEventCache.EventListeners?.GetInvocationList().Length ?? -1, "Event handler was not added.");
        }

        [TestMethod]
        public void RegisterAgentEvents_WhenInvoked_RegistersAgent()
        {
            // Arrange
            Assert.AreEqual(0, EventAgentCaches.Count, "Agent was registered before registering.");

            // Act
            _agent.RegisterAgentEvents();

            // Assert
            Assert.AreEqual(1, EventAgentCaches.Count, "Agent was not registered into event extension.");
            Assert.AreEqual(_agent, EventAgentCaches.First().Key, "Agent registered was not the same agent given.");
        }

        [TestMethod]
        public void SendEvent_WhenInvokedWithHandlers_HandlersAreInvoked()
        {
            // Arrange
            int timesInvoked = 0;
            var eventHandler = new Action<TestEventArg>((args) => { ++timesInvoked; });
            _agent.RegisterAgentEvents();
            _agent.RegisterEvent<TestEventArg>();
            _agent.AddEventHandler(eventHandler);

            // Act
            _agent.SendEvent(TestEventArg.Empty);

            // Assert
            Assert.AreEqual(1, timesInvoked, "Sending event did not invoke the event handler exactly once.");
        }
    }
}
