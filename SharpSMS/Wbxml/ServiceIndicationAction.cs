namespace SharpSMS.Wbxml
{
    /// <summary>
    /// Values for Action attribute in Service Indication Message 
    /// </summary>
    public enum ServiceIndicationAction : byte
    {
        /// <summary>
        /// Action not set
        /// </summary>
        NotSet,
        /// <summary>
        /// No signaling
        /// </summary>
        Signal_none,
        /// <summary>
        /// Low signaling
        /// </summary>
        Signal_low,
        /// <summary>
        /// Medium signaling
        /// </summary>
        Signal_medium,
        /// <summary>
        /// High signaling
        /// </summary>       
        Signal_high,
        /// <summary>
        /// Delete after display
        /// </summary>
        Delete
    }
}