using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SubC.MachEcs.Events
{
    /// <summary>
    /// This class contains extension methods for the <see cref="MachAgent"/> class, adding support for events in an ECS world.
    /// </summary>
    public static class MachAgentEventExtensions
    {
        private static readonly IDictionary<MachAgent, AgentCache> _agentCaches = new Dictionary<MachAgent, AgentCache>();

        /// <summary>
        /// Adds a method to handle when the given event type is sent.
        /// </summary>
        /// <typeparam name="T">The event type.</typeparam>
        /// <param name="agent">The agent this event exists in.</param>
        /// <param name="listenerMethod">The event handler method.</param>
        public static void AddEventHandler<T>(this MachAgent agent, Action<T> listenerMethod)
            where T : IMachEvent
        {
            Debug.Assert(_agentCaches.ContainsKey(agent), "Cannot add event listener, agent is not registered.");
            Debug.Assert(_agentCaches[agent].EventCaches.ContainsKey(typeof(T)), "Cannot add event listener, event type is not registered.");
            ((MachEventCache<T>)_agentCaches[agent].EventCaches[typeof(T)]).EventListeners += listenerMethod;
        }

        /// <summary>
        /// Enables event support to a given agent instance.
        /// </summary>
        /// <param name="agent">The agent to enable event support for.</param>
        public static void RegisterAgentEvents(this MachAgent agent)
        {
            Debug.Assert(!_agentCaches.ContainsKey(agent), "Cannot register agent for events, agent is already registered.");
            _agentCaches.Add(agent, new AgentCache());
        }

        /// <summary>
        /// Registers an event type to later be listened by handlers and sent by senders.
        /// </summary>
        /// <typeparam name="T">The event type.</typeparam>
        /// <param name="agent">The agent to register the event in.</param>
        public static void RegisterEvent<T>(this MachAgent agent)
            where T : IMachEvent
        {
            Debug.Assert(_agentCaches.ContainsKey(agent), "Cannot register event, agent is not registered.");
            Debug.Assert(!_agentCaches[agent].EventCaches.ContainsKey(typeof(T)), "Cannot register event, event type is already registered.");
            _agentCaches[agent].EventCaches.Add(typeof(T), new MachEventCache<T>());
        }

        /// <summary>
        /// Removes event support from a given agent instance.
        /// </summary>
        /// <param name="agent">The agent to remove event support from.</param>
        public static void RemoveAgentEvents(this MachAgent agent)
        {
            Debug.Assert(_agentCaches.ContainsKey(agent), "Cannot remove agent from events, agent is not registered.");
            _agentCaches.Remove(agent);
        }

        /// <summary>
        /// Removes an event handling method from the given event type.
        /// </summary>
        /// <typeparam name="T">The event type.</typeparam>
        /// <param name="agent">The agent to remove the event listener from.</param>
        /// <param name="listenerMethod">The event listener method to remove.</param>
        public static void RemoveEventHandler<T>(this MachAgent agent, Action<T> listenerMethod)
            where T : IMachEvent
        {
            Debug.Assert(_agentCaches.ContainsKey(agent), "Cannot remove event listener, agent is not registered.");
            Debug.Assert(_agentCaches[agent].EventCaches.ContainsKey(typeof(T)), "Cannot remove event listener, event type is not registered.");
            ((MachEventCache<T>)_agentCaches[agent].EventCaches[typeof(T)]).EventListeners -= listenerMethod;
        }

        /// <summary>
        /// Sends an event instance to all listeners of that event type.
        /// </summary>
        /// <typeparam name="T">The event type.</typeparam>
        /// <param name="agent">The agent to send the event instance to.</param>
        /// <param name="eventInstance">The event instance, holding the event data (if any).</param>
        public static void SendEvent<T>(this MachAgent agent, T eventInstance)
            where T : IMachEvent
        {
            Debug.Assert(_agentCaches.ContainsKey(agent), "Cannot send event, agent is not registered.");
            Debug.Assert(_agentCaches[agent].EventCaches.ContainsKey(typeof(T)), "Cannot send event, event type is not registered.");
            ((MachEventCache<T>)_agentCaches[agent].EventCaches[typeof(T)]).EventListeners?.Invoke(eventInstance);
        }
    }
}
