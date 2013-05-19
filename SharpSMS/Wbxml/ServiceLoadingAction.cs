namespace SharpSMS.Wbxml
{
    /// <summary>
    /// Values for Action attribute in Service Loading Message 
    /// </summary>
    public enum ServiceLoadingAction : byte
    {
        /// <summary>
        /// Action not set
        /// </summary>
        NotSet,
        /// <summary>
        /// Execute low
        /// </summary>
        Execute_low,
        /// <summary>
        /// Execute high
        /// </summary>
        Execute_high,
        /// <summary>
        /// Cache
        /// </summary>
        Cache
    }
}