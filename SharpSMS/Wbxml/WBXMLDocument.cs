using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SharpSMS.Wbxml
{
    /// <summary>
    /// Abstract class for creating messages tokenized into WBXML document
    /// </summary>
    public abstract class WBXMLDocument
    {
        #region Constants
        /// <summary>
        /// Binary representation of NULL
        /// </summary>
        public const byte NULL = 0x00;

        /// <summary>
        /// Binary representation of v1.1
        /// </summary>
        public const byte VERSION_1_1 = 0x01;
        /// <summary>
        /// Binary representation of v1.2
        /// </summary>
        public const byte VERSION_1_2 = 0x02;
        /// <summary>
        /// Binary representation of UTF-8 encoding
        /// </summary>
        public const byte CHARSET_UTF_8 = 0x6A;
        /// <summary>
        /// Binary representation of token end
        /// </summary>
        public const byte TAGTOKEN_END = 0x01;
        /// <summary>
        /// Binary representation of inline string identifier
        /// </summary>
        public const byte TOKEN_INLINE_STRING_FOLLOWS = 0x03;
        /// <summary>
        /// Binary representation of quoted data
        /// </summary>
        public const byte TOKEN_OPAQUEDATA_FOLLOWS = 0xC3;
        #endregion

        #region Properties
        /// <summary>
        /// MIME Content type of the message
        /// </summary>
        public string ContentType { get; set; }
        #endregion

        #region Abstract Method

        /// <summary>
        /// Method return tokenized XML
        /// </summary>
        /// <returns>byte of array</returns>
        public abstract byte[] GetWBXMLBytes();

        #endregion

        #region Public Methods

        /// <summary>
        /// Methods returns tokenized tag according to conditions
        /// </summary>
        /// <param name="token">original value of the tokenized-tag</param>
        /// <param name="hasAttributes"></param>
        /// <param name="hasContent"></param>
        /// <returns>Modified value of the tokenized tag</returns>
        public byte SetTagTokenIndications(byte token, bool hasAttributes, bool hasContent)
        {
            if (hasAttributes)
                token |= 0x80;
            if (hasContent)
                token |= 0x40;

            return token;
        }        

        /// <summary>
        /// Methods writes text into stream, tokenized to be used in the WBXML document
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="text">Text tobe tokenized</param>
        protected void WriteInlineString(MemoryStream stream, string text)
        {
            stream.WriteByte(WBXMLDocument.TOKEN_INLINE_STRING_FOLLOWS);

            byte[] bytes = Encoding.UTF8.GetBytes(text);
            stream.Write(bytes, 0, bytes.Length);

            stream.WriteByte(WBXMLDocument.NULL); // end of the string
        }

        /// <summary>
        /// Method writes date into stream, tokenized to be used in the WBXML document
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="date"></param>
        protected void WriteDate(MemoryStream stream, DateTime date)
        {
            byte[] buffer = new byte[7];

            buffer[0] = Convert.ToByte(Convert.ToString(date.Year / 100), 16);
            buffer[1] = Convert.ToByte(Convert.ToString(date.Year % 100), 16);
            buffer[2] = Convert.ToByte(Convert.ToString(date.Month), 16);
            buffer[3] = Convert.ToByte(Convert.ToString(date.Day), 16);

            int dateLength = 4;

            if (date.Hour > 0)
            {
                buffer[4] = Convert.ToByte(Convert.ToString(date.Hour), 16);
                dateLength = 5;
            }

            if (date.Minute > 0)
            {
                buffer[5] = Convert.ToByte(Convert.ToString(date.Minute), 16);
                dateLength = 6;
            }

            if (date.Second > 0)
            {
                buffer[6] = Convert.ToByte(Convert.ToString(date.Second), 16);
                dateLength = 7;
            }

            // write to stream
            stream.WriteByte(WBXMLDocument.TOKEN_OPAQUEDATA_FOLLOWS);
            stream.WriteByte((byte)dateLength);
            stream.Write(buffer, 0, dateLength);
        }

        #endregion
    }
}
