using System;
using System.Collections.Generic;
using System.Text;

namespace SharpSMS.Wap
{
    /// <summary>
    /// Class represents Wap Push binary sms 
    /// </summary>
    public class WdpBinaryMessage : WdpMessage, ISmsMessageContent
    {

        #region Constuctors

        /// <summary>
        /// Creates instance of the WapPushBinary
        /// </summary>
        public WdpBinaryMessage()
            : this(null)
        {
        }

        /// <summary>
        /// Creates instance of the WapPushBinary
        /// </summary>
        /// <param name="data">Binary data of the message</param>
        public WdpBinaryMessage(byte[] data)
        {
            this.DestinationPort = Wdp.WAP_PORT_PUSH_SESSION_DESTINATION;
            this.SourcePort = Wdp.WAP_PORT_PUSH_SESSION_SOURCE;

            this.DataEncoding = DataEncoding.Data8bit;
            this.Data = data;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Returns array of bytes representing the SMS
        /// </summary>
        /// <returns></returns>
        public byte[] GetSMSBytes()
        {
            return Data;
        }
        /// <summary>
        /// Returns array of bytes repesenting the WDP header
        /// </summary>
        /// <returns></returns>
        public byte[] GetUDHBytes()
        {
            return this.GetWdpHeader();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Data encoding of the message
        /// </summary>        
        public DataEncoding DataEncoding { get; set; }
        /// <summary>
        /// Content of the message
        /// </summary>
        public byte[] Data { get; set; }
        #endregion
    }
}
