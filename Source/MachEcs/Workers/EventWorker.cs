using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SubC.MachEcs.Workers
{
    internal sealed class EventWorker
    {
        private readonly IDictionary<Type, IEventHandlerCache> _eventHandlerCaches = new Dictionary<Type, IEventHandlerCache>();

        public void AddEventHandler<T>(Action eventHandler)
            where T : IEventData
        {
            GetEventHandlerCache(typeof(T)).AddEventHandler(eventHandler);
        }

        public void AddEventHandler<T>(Action<T> eventDataHandler)
            where T : IEventData
        {
            GetEventHandlerCache<T>().AddEventHandler(eventDataHandler);
        }

        public void RegisterEvent<T>()
            where T : IEventData
        {
            var type = typeof(T);
            if (_eventHandlerCaches.ContainsKey(type))
            {
                return;
            }
            _eventHandlerCaches.Add(type, new EventHandlerCache<T>());
        }

        public void RemoveEvent<T>()
            where T : IEventData
        {
            Debug.Assert(
                _eventHandlerCaches.ContainsKey(typeof(T)),
                $"Cannot remove event, event data is not registered: {typeof(T).Name}");
            _eventHandlerCaches.Remove(typeof(T));
        }

        public void RemoveEventHandler<T>(Action eventHandler)
            where T : IEventData
        {
            GetEventHandlerCache(typeof(T)).RemoveEventHandler(eventHandler);
        }

        public void RemoveEventHandler<T>(Action<T> eventDataHandler)
            where T : IEventData
        {
            GetEventHandlerCache<T>().RemoveEventHandler(eventDataHandler);
        }

        public void RemoveEventHandlers<T>()
        {
            GetEventHandlerCache(typeof(T)).RemoveEventHandlers();
        }

        public void SendEvent<T>()
            where T : IEventData
        {
            GetEventHandlerCache(typeof(T)).SendEvent();
        }

        public void SendEvent<T>(T eventData)
            where T : IEventData
        {
            GetEventHandlerCache<T>().SendEvent(eventData);
        }

        private EventHandlerCache<T> GetEventHandlerCache<T>()
            where T : IEventData
        {
            Debug.Assert(_eventHandlerCaches.ContainsKey(
                typeof(T)),
                $"Cannot get event handler cache, event data not registered before use: {typeof(T).Name}.");
            return (EventHandlerCache<T>)_eventHandlerCaches[typeof(T)];
        }

        private IEventHandlerCache GetEventHandlerCache(Type type)
        {
            Debug.Assert(_eventHandlerCaches.ContainsKey(
                type),
                $"Cannot get event handler cache, event data not registered before use: {type.Name}.");
            return _eventHandlerCaches[type];
        }
    }
}
