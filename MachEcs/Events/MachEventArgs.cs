namespace SubC.MachEcs.Events
{
    /// <summary>
    /// This class represents event arguments.
    /// </summary>
    /// <typeparam name="T">The event argument data type.</typeparam>
    public sealed class MachEventArgs<T>
        where T : IMachEventArgData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MachEventArgs{T}"/> class.
        /// </summary>
        /// <param name="eventData">The data this event holds.</param>
        public MachEventArgs(T eventData)
        {
            EventData = eventData;
        }

        /// <summary>
        /// The user defined data included in the event arguments.
        /// </summary>
        public T EventData = default;
    }
}
