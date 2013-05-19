using System;
using System.Text;

namespace SharpSMS
{
    /// <summary>
    /// Class represent standard plain text message
    /// </summary>
    public class TextMessage : ISmsMessageContent
    {
        #region Constuctor
        /// <summary>
        /// Creates new plain text message
        /// </summary>
        public TextMessage()
        {            
            this.DataEncoding = DataEncoding.Default7bit;
            this.Text = string.Empty;
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Method returns byte array suitable to be send in SMS
        /// </summary>
        /// <returns>byte array</returns>
        public byte[] GetSMSBytes()
        {
            byte[] text = GetDataBytes();
            return text;
        }

        /// <summary>
        /// Method returns user header bytes. In plain text message it's always empty
        /// </summary>
        /// <returns></returns>
        public byte[] GetUDHBytes()
        {
            return new byte[]{};
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Returns message text encoded in DataEncoding
        /// </summary>
        /// <returns>byte array</returns>
        protected byte[] GetDataBytes()
        {            
            byte[] data = new byte[] {};

            switch (DataEncoding)
            {
                case DataEncoding.Default7bit:
                        data = Encoding.ASCII.GetBytes(Text);
                    break;
                case DataEncoding.Data8bit:
                        data = Encoding.GetEncoding("iso-8859-1").GetBytes(Text);
                    break;
                case DataEncoding.UCS2_16bit:
                        data = Encoding.BigEndianUnicode.GetBytes(Text);
                    break;            
            }

            return data;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Text of the message
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Encoding of the data (7-bit, 8-bit, 16-bit)
        /// </summary>
        public DataEncoding DataEncoding { get; set; }
        #endregion    
    }
}
