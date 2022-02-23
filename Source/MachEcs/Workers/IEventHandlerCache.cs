using System;

namespace SubC.MachEcs.Workers
{
    internal interface IEventHandlerCache
    {
        void AddEventHandler(Action eventHandler);

        void RemoveEventHandler(Action eventHandler);

        void RemoveEventHandlers();

        public void SendEvent();
    }
}
