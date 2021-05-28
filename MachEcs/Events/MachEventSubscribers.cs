namespace SubC.MachEcs.Events
{
    internal sealed class MachEventSubscribers<T> : IMachEventSubscribers
    {
        public MachEventTopic.MachEventHandler<T> Subscribers;

        public void RemoveAllSubscribers()
            => Subscribers = null;
    }
}
