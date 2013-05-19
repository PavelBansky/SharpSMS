namespace SharpSMS
{
    /// <summary>
    /// Message class indicates where message will be stored
    /// </summary>
    public enum MessageClass
    {
        /// <summary>
        /// Flash message only to display
        /// </summary>
        ImmediateDisplay,
        /// <summary>
        /// Default store
        /// </summary>
        MESpecific,
        /// <summary>
        /// Message for the SIM
        /// </summary>
        SIMSpecific,
        /// <summary>
        /// TE Specific
        /// </summary>
        TESpecific
    }
}