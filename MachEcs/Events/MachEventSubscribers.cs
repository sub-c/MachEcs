namespace SubC.MachEcs.Events
{
    internal sealed class MachEventSubscribers<T> : IMachEventSubscribers
    {
        public HandleMachEvent<T> MachEventHandlers { get; set; }
    }
}
