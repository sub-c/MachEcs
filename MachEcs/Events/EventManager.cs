using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SubC.MachEcs.Events
{
    /// <summary>
    /// Event handler for events in MachECS.
    /// </summary>
    /// <typeparam name="T">Event argument type.</typeparam>
    /// <param name="eventArgs">Event argument passed to subscribers.</param>
    public delegate void HandleMachEvent<T>(T eventArgs);

    internal sealed class EventManager
    {
        private readonly IDictionary<Type, IMachEventSubscribers> _eventSubscriptions = new Dictionary<Type, IMachEventSubscribers>();

        public void RegisterEvent<T>()
        {
            var eventArgType = typeof(T);
            Debug.Assert(!_eventSubscriptions.ContainsKey(eventArgType), $"Event {eventArgType.Name} is already registered.");
            var eventSubscribers = new MachEventSubscribers<T>();
            _eventSubscriptions.Add(eventArgType, eventSubscribers);
        }

        public void SendEvent<T>(T eventArgs)
        {
            var eventSubscribers = GetEventSubscribers<T>();
            eventSubscribers.MachEventHandlers?.Invoke(eventArgs);
        }

        public async Task SendEventAsync<T>(T eventArgs)
        {
            var eventSubscribers = GetEventSubscribers<T>();
            await Task.Factory.StartNew(() =>  eventSubscribers.MachEventHandlers?.Invoke(eventArgs));
        }

        public void SubscribeToEvent<T>(HandleMachEvent<T> eventHandler)
        {
            var eventSubscribers = GetEventSubscribers<T>();
            eventSubscribers.MachEventHandlers += eventHandler;
        }

        public void UnregisterEvent<T>()
        {
            var eventArgType = typeof(T);
            Debug.Assert(_eventSubscriptions.ContainsKey(eventArgType), $"Type {eventArgType.Name} is not currently registered.");
            _eventSubscriptions.Remove(eventArgType);
        }

        public void UnsubscribeAllFromEvent<T>()
        {
            var eventSubscribers = GetEventSubscribers<T>();
            eventSubscribers.MachEventHandlers = null;
        }

        public void UnsubscribeFromEvent<T>(HandleMachEvent<T> eventHandler)
        {
            var eventSubscribers = GetEventSubscribers<T>();
            eventSubscribers.MachEventHandlers -= eventHandler;
        }

        private MachEventSubscribers<T> GetEventSubscribers<T>()
        {
            var eventArgType = typeof(T);
            Debug.Assert(_eventSubscriptions.ContainsKey(eventArgType), $"Type {eventArgType.Name} does not exist in the event subscriptions dictionary.");
            return _eventSubscriptions[eventArgType] as MachEventSubscribers<T>;
        }
    }
}
