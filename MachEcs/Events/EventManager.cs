using System.Collections.Generic;
using System.Diagnostics;

namespace SubC.MachEcs.Events
{
    internal sealed class EventManager
    {
        private readonly IDictionary<MachEventTopic, IMachEventSubscribers> _eventTopicSubscriptions = new Dictionary<MachEventTopic, IMachEventSubscribers>();

        public void RegisterEventTopic<T>(MachEventTopic eventTopic)
        {
            Debug.Assert(!_eventTopicSubscriptions.ContainsKey(eventTopic), "Event ID cannot be registered twice.");
            _eventTopicSubscriptions.Add(eventTopic, new MachEventSubscribers<T>());
        }

        public void RemoveAllSubscribersFromEvent(MachEventTopic eventID)
        {
            Debug.Assert(_eventTopicSubscriptions.ContainsKey(eventID), "Event ID not registered before use.");
            _eventTopicSubscriptions[eventID].RemoveAllSubscribers();
        }

        public void SendEvent<T>(MachEventTopic eventTopic, MachEventArgs<T> eventArgs)
        {
            var subscribers = GetEventSubscribers<T>(eventTopic);
            subscribers.Subscribers?.Invoke(eventArgs);
        }

        public void SubscribeToEventTopic<T>(MachEventTopic eventTopic, MachEventTopic.MachEventHandler<T> eventHandler)
        {
            var subscribers = GetEventSubscribers<T>(eventTopic);
            subscribers.Subscribers += eventHandler;
        }

        public void UnregisterEventTopic(MachEventTopic eventTopic)
        {
            Debug.Assert(_eventTopicSubscriptions.ContainsKey(eventTopic), "Event ID was not registered before use.");
            _eventTopicSubscriptions[eventTopic].RemoveAllSubscribers();
            _eventTopicSubscriptions.Remove(eventTopic);
        }

        public void UnsubscribeFromEventTopic<T>(MachEventTopic eventTopic, MachEventTopic.MachEventHandler<T> eventHandler)
        {
            var subscribers = GetEventSubscribers<T>(eventTopic);
            subscribers.Subscribers -= eventHandler;
        }

        private MachEventSubscribers<T> GetEventSubscribers<T>(MachEventTopic eventTopic)
        {
            Debug.Assert(_eventTopicSubscriptions.ContainsKey(eventTopic), "Event ID was not registered before use.");
            return _eventTopicSubscriptions[eventTopic] as MachEventSubscribers<T>;
        }
    }
}
