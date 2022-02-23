using System;

namespace SubC.MachEcs.Workers
{
    internal sealed class EventHandlerCache<T> : IEventHandlerCache
        where T : IEventData
    {
        private Action<T>? _eventDataHandlers;
        private Action? _eventHandlers;

        public void AddEventHandler(Action eventHandler)
        {
            _eventHandlers += eventHandler;
        }

        public void AddEventHandler(Action<T> eventDataHandler)
        {
            _eventDataHandlers += eventDataHandler;
        }

        public void RemoveEventHandler(Action eventHandler)
        {
            _eventHandlers -= eventHandler;
        }

        public void RemoveEventHandler(Action<T> eventDataHandler)
        {
            _eventDataHandlers -= eventDataHandler;
        }

        public void RemoveEventHandlers()
        {
            _eventDataHandlers = null;
            _eventHandlers = null;
        }

        public void SendEvent()
        {
            _eventHandlers?.Invoke();
        }

        public void SendEvent(T eventData)
        {
            _eventDataHandlers?.Invoke(eventData);
        }
    }
}
