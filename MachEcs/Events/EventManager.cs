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
        private readonly IDictionary<string, IMachEventSubscribers> _eventSubscriptions = new Dictionary<string, IMachEventSubscribers>();

        public void RegisterEvent<T>()
        {
            var typeName = typeof(T).Name;
            Debug.Assert(!_eventSubscriptions.ContainsKey(typeName), $"Event {typeName} is already registered.");
            var eventSubscribers = new MachEventSubscribers<T>();
            _eventSubscriptions.Add(typeName, eventSubscribers);
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
            var typeName = typeof(T).Name;
            Debug.Assert(_eventSubscriptions.ContainsKey(typeName), $"Type {typeName} is not currently registered.");
            _eventSubscriptions.Remove(typeName);
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
            var typeName = typeof(T).Name;
            Debug.Assert(_eventSubscriptions.ContainsKey(typeName), $"Type {typeName} does not exist in the event subscriptions dictionary.");
            return _eventSubscriptions[typeName] as MachEventSubscribers<T>;
        }
    }
}
