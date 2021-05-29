namespace SubC.MachEcs.Events
{
    internal sealed class MachEventSubscribers<T> : IMachEventSubscribers
        where T : IMachEventArgData
    {
        public MachEventTopic<T>.MachEventHandler Subscribers;

        public void RemoveAllSubscribers()
        {
            Subscribers = null;
        }
    }
}
