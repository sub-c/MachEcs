namespace SubC.MachEcs.Events
{
    /// <summary>
    /// This class represents an event identifier/topic. The class reference is used, so no unique data is needed in
    /// the class itself; ensure user code uses the same class reference for the same event ID.
    /// </summary>
    public sealed class MachEventTopic
    {
        /// <summary>
        /// The delegate for an event handler.
        /// </summary>
        /// <typeparam name="TA">The data type included with the event arguments.</typeparam>
        /// <param name="eventArgs">The event arguments for the event.</param>
        public delegate void MachEventHandler<TA>(MachEventArgs<TA> eventArgs);
    }
}
