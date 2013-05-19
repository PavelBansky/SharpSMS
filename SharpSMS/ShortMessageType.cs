namespace SharpSMS
{
    /// <summary>
    /// Represents message type field for Protocol Identifier
    /// </summary>
    public enum ShortMessageType : byte
    {
        /// <summary>
        /// A short message type 0 indicates that the ME must acknowledge receipt of the short message but may discard its contents. 
        /// </summary>
        ShortMessageType0 = 0x00,
        /// <summary>
        /// Replace Short Message Type 1
        /// </summary>
        ReplaceMessageType1 = 0x01,
        /// <summary>
        /// Replace Short Message Type 2
        /// </summary>
        ReplaceMessageType2 = 0x02,
        /// <summary>
        /// Replace Short Message Type 3
        /// </summary>
        ReplaceMessageType3 = 0x03,
        /// <summary>
        /// Replace Short Message Type 4
        /// </summary>
        ReplaceMessageType4 = 0x04,
        /// <summary>
        /// Replace Short Message Type 5
        /// </summary>
        ReplaceMessageType5 = 0x05,
        /// <summary>
        /// Replace Short Message Type 6
        /// </summary>
        ReplaceMessageType6 = 0x06,
        /// <summary>
        /// Replace Short Message Type 7
        /// </summary>
        ReplaceMessageType7 = 0x07,
        /// <summary>
        /// Inicates to the MS to inform the user that a call can be established to the address specified within the TP-OA
        /// </summary>
        ReturnCall = 0x1F,
        /// <summary>
        /// ME Data download is facility whereby the ME shall process the short message in its entirety including all SMS elements contained in the SMS deliver to the ME
        /// </summary>
        MEDataDownload = 0x3D,
        /// <summary>
        /// The ME De-personalization Short Message is an ME-specific message which instructs the ME to de-personalities the ME
        /// </summary>
        MEDePersonlaization = 0x3E,
        /// <summary>
        /// SIM Data download is a facility whereby the ME must pass the short message in its entirety including all SMS elements
        /// </summary>
        SIMDataDownload = 0x3F,
    }
}