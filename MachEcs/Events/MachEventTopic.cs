namespace SubC.MachEcs.Events
{
    /// <summary>
    /// This abstract class represents an event identifier/topic. The class reference is used, so no unique data is
    /// needed in the class itself.
    /// </summary>
    /// <typeparam name="T">The event argument data type.</typeparam>
    public abstract class MachEventTopic<T> : IMachEventTopic
        where T : IMachEventArgData
    {
        /// <summary>
        /// The delegate for an event handler.
        /// </summary>
        /// <param name="eventArgs">The event arguments for the event.</param>
        public delegate void MachEventHandler(T eventArgs);
    }
}
