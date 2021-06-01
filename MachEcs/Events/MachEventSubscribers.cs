namespace SubC.MachEcs.Events
{
    internal sealed class MachEventSubscribers<T> : IMachEventSubscribers
    {
        public MachEventTopic<T>.MachEventHandler Subscribers;

        public void RemoveAllSubscribers()
        {
            Subscribers = null;
        }
    }
}
