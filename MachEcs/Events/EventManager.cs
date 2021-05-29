using System.Collections.Generic;
using System.Diagnostics;

namespace SubC.MachEcs.Events
{
    internal sealed class EventManager
    {
        private readonly IDictionary<IMachEventTopic, IMachEventSubscribers> _topicSubscriptions = new Dictionary<IMachEventTopic, IMachEventSubscribers>();

        public void RegisterEventTopic<T>(MachEventTopic<T> eventTopic)
        {
            Debug.Assert(!_topicSubscriptions.ContainsKey(eventTopic), "Event ID cannot be registered twice.");
            _topicSubscriptions.Add(eventTopic, new MachEventSubscribers<T>());
        }

        public void RemoveAllSubscribersFromEvent<T>(MachEventTopic<T> eventID)
        {
            Debug.Assert(_topicSubscriptions.ContainsKey(eventID), "Event ID not registered before use.");
            _topicSubscriptions[eventID].RemoveAllSubscribers();
        }

        public void SendEvent<T>(MachEventTopic<T> eventTopic, MachEventArgs<T> eventArgs)
        {
            var subscribers = GetEventSubscribers<T>(eventTopic);
            subscribers.Subscribers?.Invoke(eventArgs);
        }

        public void SubscribeToEventTopic<T>(MachEventTopic<T> eventTopic, MachEventTopic<T>.MachEventHandler eventHandler)
        {
            var subscribers = GetEventSubscribers<T>(eventTopic);
            subscribers.Subscribers += eventHandler;
        }

        public void UnregisterEventTopic<T>(MachEventTopic<T> eventTopic)
        {
            Debug.Assert(_topicSubscriptions.ContainsKey(eventTopic), "Event ID was not registered before use.");
            _topicSubscriptions[eventTopic].RemoveAllSubscribers();
            _topicSubscriptions.Remove(eventTopic);
        }

        public void UnsubscribeFromEventTopic<T>(MachEventTopic<T> eventTopic, MachEventTopic<T>.MachEventHandler eventHandler)
        {
            var subscribers = GetEventSubscribers<T>(eventTopic);
            subscribers.Subscribers -= eventHandler;
        }

        private MachEventSubscribers<T> GetEventSubscribers<T>(MachEventTopic<T> eventTopic)
        {
            Debug.Assert(_topicSubscriptions.ContainsKey(eventTopic), "Event ID was not registered before use.");
            return _topicSubscriptions[eventTopic] as MachEventSubscribers<T>;
        }
    }
}
