namespace SubC.MachEcs.Events
{
    /// <summary>
    /// This class represents event arguments.
    /// </summary>
    /// <typeparam name="T">The type of data included in the event arguments.</typeparam>
    public sealed class MachEventArgs<T>
    {
        /// <summary>
        /// The user defined data included in the event arguments.
        /// </summary>
        public T EventArgs = default;
    }
}
