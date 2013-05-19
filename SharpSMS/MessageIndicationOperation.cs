namespace SharpSMS
{
    /// <summary>
    /// Specifies if phone stores or discards message indication
    /// </summary>
    public enum MessageIndicationOperation
    {
        /// <summary>
        /// Message Indication Operation is not used - Default
        /// </summary>
        NotSet,
        /// <summary>
        /// Store Indication Message
        /// </summary>
        Store,
        /// <summary>
        /// Discard Indication Message
        /// </summary>
        Discard
    }
}
