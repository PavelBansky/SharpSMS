namespace SharpSMS
{
    /// <summary>
    /// Type of message that indication is representing
    /// </summary>
    public enum IndicationType
    {
        /// <summary>
        /// Voicmail Message Waiting
        /// </summary>
        Voicemail,
        /// <summary>
        /// Fax Message Waiting
        /// </summary>
        FaxMessage,
        /// <summary>
        /// Email Message Waiting
        /// </summary>
        EmailMessage,
        /// <summary>
        /// Other Message Waiting
        /// </summary>
        OtherMessage
    }
}
