namespace SharpSMS
{
    /// <summary>
    /// Encodings for SMS messages
    /// </summary>
    public enum DataEncoding
    {
        /// <summary>
        /// Default 7bit alphabet used in GSM
        /// </summary>
        Default7bit,
        /// <summary>
        /// 8 bit encoding
        /// </summary>
        Data8bit,
        /// <summary>
        /// UCS2 16 bit encoding
        /// </summary>
        UCS2_16bit
    }
}