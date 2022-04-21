using SubC.MachEcs.Models;
using System;
using System.Collections.Generic;

namespace SubC.MachEcs.Worker
{
  internal sealed class EventWorker
  {
    private readonly IDictionary<Type, IEventHandlerCache> _eventHandlerCaches = new Dictionary<Type, IEventHandlerCache>();

    public void RegisterEventHandler<T>(Action eventHandler)
      where T : IEcsEvent
    {
      GetEventHandlerCache<T>().EventHandlers += eventHandler;
    }

    public void RegisterEventHandler<T>(Action<T> eventHandler)
      where T : IEcsEvent
    {
      GetEventHandlerCache<T>().EventDataHandlers += eventHandler;
    }

    public void SendEvent<T>()
      where T : IEcsEvent
    {
      GetEventHandlerCache<T>().EventHandlers?.Invoke();
    }

    public void SendEvent<T>(T eventData)
      where T : IEcsEvent
    {
      GetEventHandlerCache<T>().EventDataHandlers?.Invoke(eventData);
    }

    public void UnregisterEventHandler<T>(Action eventHandler)
      where T : IEcsEvent
    {
      GetEventHandlerCache<T>().EventHandlers -= eventHandler;
    }

    public void UnregisterEventHandler<T>(Action<T> eventHandler)
      where T : IEcsEvent
    {
      GetEventHandlerCache<T>().EventDataHandlers -= eventHandler;
    }

    private EventHandlerCache<T> GetEventHandlerCache<T>()
      where T : IEcsEvent
    {
      if (_eventHandlerCaches.TryGetValue(typeof(T), out var cache))
      {
        return (EventHandlerCache<T>)cache;
      }
      var newCache = new EventHandlerCache<T>();
      _eventHandlerCaches.Add(typeof(T), newCache);
      return newCache;
    }
  }
}
