namespace SharpSMS
{
    /// <summary>
    /// Class helps to assemble the TP-PID Protocol Identifier value
    /// </summary>
    public class ProtocoldentifierBuilder
    {
        #region Constants
        /// <summary>
        /// Bits 7, 6, 5 to identify SME to SME message
        /// </summary>
        public const byte TP_PID_SME_TO_SME = 0x00;
        /// <summary>
        /// Bits 7, 6, 5 to identify Telematic device message
        /// </summary>
        public const byte TP_PID_TELEMATIC_DEVICE = 0x20;
        /// <summary>
        /// Bits 7, 6 to identify Short Message
        /// </summary>
        public const byte TP_PID_MESSAGE_TYPE = 0x40;
        /// <summary>
        /// Bits 7, 6 to identify SC specific protocol
        /// </summary>
        public const byte TP_PID_SC_SPECIFIC = 0x0C;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates new TP-PID with value 0x00
        /// </summary>
        public ProtocoldentifierBuilder()
        {
            PTBits = 0x00;
        }

        /// <summary>
        /// Creates new TP-PID for given message type
        /// </summary>
        /// <param name="messageType">Message type</param>
        public ProtocoldentifierBuilder(ShortMessageType messageType)
        {
            SetMessageType(messageType);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Set message type for TP-PID
        /// </summary>
        /// <param name="messageType"></param>
        public void SetMessageType(ShortMessageType messageType)
        {
            PTBits = (byte)(TP_PID_MESSAGE_TYPE | (byte)messageType);
        }
        
        /// <summary>
        /// Set telematic device for TP-PID
        /// </summary>
        /// <param name="device"></param>
        public void SetTelematicDevice(TelematicDevice device)
        {
            PTBits = (byte)(TP_PID_TELEMATIC_DEVICE | (byte)device);
        }

        /// <summary>
        /// Set protocol value for SME to SME communication
        /// </summary>
        /// <param name="protocolBits"></param>
        public void SetSMEtoSME(byte protocolBits)
        {
            protocolBits <<= 2;
            PTBits = (byte)(TP_PID_SME_TO_SME | (protocolBits >> 2));
        }

        /// <summary>
        /// Set protocol value for SC communication
        /// </summary>
        /// <param name="protocolBits">SC specific bits</param>
        public void SetSCSpecific(byte protocolBits)
        {
            protocolBits <<= 2;
            PTBits = (byte)(TP_PID_SC_SPECIFIC | (protocolBits >> 2));
        }        
        
        /// <summary>
        /// Returns value that can be used as Protocol Identifier for message
        /// </summary>
        /// <returns></returns>
        public byte ToByte()
        {
            return PTBits;
        }
        #endregion

        #region Fields
        /// <summary>
        /// Property to store bits fo the TP-PID value
        /// </summary>
        private byte PTBits { get; set; }
        #endregion
    }
}
