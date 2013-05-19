using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using SharpSMS.Wbxml;

namespace SharpSMS.Wap
{
    /// <summary>
    /// Class representing Wap Push sms 
    /// </summary>
    public class WapPushMessage : WdpMessage, ISmsMessageContent
    {
        #region Constructors

        /// <summary>
        /// Creates instance of the WapPushMessage. Data will be taken from WBXML document
        /// </summary>
        public WapPushMessage()
        {
            this.DestinationPort = Wdp.WAP_PORT_PUSH_SESSION_DESTINATION;
            this.SourcePort = Wdp.WAP_PORT_PUSH_SESSION_SOURCE;

            this.DataEncoding = DataEncoding.Data8bit;
            this.PushFlag = 0x00;            
            this.Security = Wsp.SecurityMethod.None;
            this.ContentType = "application/vnd.wap.connectivity-wbxml";
        }

        /// <summary>
        /// Creates instance of the WapPushMessage
        /// </summary>
        /// <param name="wbxmlDocument">Tokenized XML message</param>
		public WapPushMessage(WBXMLDocument wbxmlDocument)
            : this()
		{
            this.Data = wbxmlDocument.GetWBXMLBytes();
            this.ContentType = wbxmlDocument.ContentType;
        }

        /// <summary>
        /// Creates instance of the WapPushMessage
        /// </summary>
        /// <param name="data">Binary data representing the message</param>
        public WapPushMessage(byte[] data)
        {
            this.Data = data;
        }

        /// <summary>
        /// Creates instance of the WapPushMessage
        /// </summary>
        /// <param name="message">Message content</param>
        public WapPushMessage(string message)
        {
            this.Data = Encoding.UTF8.GetBytes(message);
            this.DataEncoding = DataEncoding.Data8bit;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns byte array suitable to be send in sms message. All wraped in the Wap header
        /// </summary>
        /// <returns>byte array</returns>
        public byte[] GetSMSBytes()
        {
            byte[] headerBuffer = GetWAPWSPHeaderBytes();

            MemoryStream stream = new MemoryStream();
            stream.Write(headerBuffer, 0, headerBuffer.Length);
            stream.Write(this.Data, 0, this.Data.Length);
            return stream.ToArray();
        }

        /// <summary>
        /// Generates the WDP (Wireless Datagram Protocol) or UDH (User Data Header) for the 
        /// SMS message. In the case comprising the Application Port information element
        /// indicating to the handset which application to start on receipt of the message
        /// </summary>
        /// <returns>byte array comprising the header</returns>
        public byte[] GetUDHBytes()
        {
            return this.GetWdpHeader();
        }

        /// <summary>
        /// Get security MAC for the message
        /// </summary>
        /// <returns></returns>
        public byte[] GetSecurityKey()
        {
            byte[] key = new byte[] { 0x00 };

            switch (Security)
            {
                case Wsp.SecurityMethod.NETWPIN:
                    key = ImsitoKey(this.NetworkPin);
                    break;
                case Wsp.SecurityMethod.USERPIN:
                    key = Encoding.ASCII.GetBytes(this.UserPin);
                    break;
                case Wsp.SecurityMethod.USERNETWPIN:
                    byte[] userPin = Encoding.ASCII.GetBytes(this.UserPin);
                    byte[] netPin = ImsitoKey(this.NetworkPin);
                    key = new byte[userPin.Length + netPin.Length];
                    netPin.CopyTo(key, 0);
                    userPin.CopyTo(key, netPin.Length);
                    break;
                case Wsp.SecurityMethod.USERPINMAC:
                    throw new NotSupportedException("USERPINMAC is not supported");
            }

            return key;
        }        
        #endregion

        #region Private methods

        /// Generates the WAP and WSP (Wireless Session Protocol) headers with the well known
        /// byte values specfic to a ContentType
        private byte[] GetWAPWSPHeaderBytes()
        {            
            MemoryStream stream = new MemoryStream();
            stream.WriteByte(Wsp.TRANSACTIONID_CONNECTIONLESSWSP);
            stream.WriteByte(Wsp.PDUTYPE_PUSH);            

            #region WSP Header
            MemoryStream headersStream = new MemoryStream();
      
            #region Security header
            MemoryStream securityStream = new MemoryStream();

            // Write content type
            Wsp.WriteHeaderContentType(securityStream, this.ContentType);

            // Accept charset header        0x01 | 0x80 = 0x81
            // Accept charset value: UTF8 = 0x61 | 0x80 = 0xEA

            // Security method and MAC
            if (this.Security != Wsp.SecurityMethod.None)
            {
                byte[] macKey = ComputeMac(GetSecurityKey(), this.Data);
                Wsp.WriteHeaderSecurity(securityStream, this.Security, macKey);
            }

            // Security Header Length
            Wsp.WriteValueLength(headersStream, securityStream.Length);

            // Write security header to header stream
            securityStream.WriteTo(headersStream);
            #endregion

            // Push Flag
            if (this.PushFlag != 0x00)
                Wsp.WriteHeaderPushFlag(headersStream, this.PushFlag);

            // X WAP Initiator URI
            if (!string.IsNullOrEmpty(this.XWapInitiatorURI))
                Wsp.WriteHeaderXWAPInitiatorURI(headersStream, this.XWapInitiatorURI);

            // X WAP Content URI
            if (!string.IsNullOrEmpty(this.XWapContentURI))
                Wsp.WriteHeaderXWAPContentURI(headersStream, this.XWapContentURI);

            // X WAP Application ID
            if (!string.IsNullOrEmpty(this.XWapApplicationType))
                Wsp.WriteHeaderXWAPApplicationID(headersStream, this.XWapApplicationType);            

            // Write complete header length
            stream.WriteByte((byte)headersStream.Length);
            headersStream.WriteTo(stream);
            #endregion

            return stream.ToArray();
        }

        private byte[] ComputeMac(byte[] key, byte[] message)
        {
            HMACSHA1 hmac = new HMACSHA1(key, true);
            return hmac.ComputeHash(message);
        }

        private byte[] ImsitoKey(string imsi)
        {
            imsi = imsi.Trim();

            if ((imsi.Length % 2) == 1)
            {
                imsi = "9" + imsi;
            }
            else
            {
                imsi = "1" + imsi;
                imsi = imsi + "F";
            }

            int numDigit = imsi.Length;
            string temp;
            char c1;
            char c2;
            byte[] key = new byte[numDigit / 2]; // always even
            int t = 0;
            for (int i = 0; i < numDigit; i++)
            {
                c1 = imsi[i];
                c2 = imsi[++i];
                temp = "" + c2 + c1;

                if (!byte.TryParse(temp, System.Globalization.NumberStyles.HexNumber, null, out key[t]))
                {
                    throw new Exception("No chars in IMSI");
                }

                t++;
            }

            return key;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Encoding of the data (7-bit, 8-bit, 16-bit)
        /// </summary>        
        public DataEncoding DataEncoding { get; set; }

        /// <summary>
        /// MIME Content type of the message
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Data (body) of the message that will be wrapped into the WAP push headers
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// X-Wap-Initiator-URI
        /// </summary>
        public string XWapInitiatorURI { get; set; }

        /// <summary>
        /// X-Wap-Content-URI
        /// </summary>
        public string XWapContentURI { get; set; }

        /// <summary>
        /// X-Wap-Application-ID Type
        /// </summary>
        public string XWapApplicationType { get; set; }

        /// <summary>
        /// Push Flag
        /// </summary>
        public byte PushFlag { get; set; }

        /// <summary>
        /// Security method
        /// </summary>
        public Wsp.SecurityMethod Security { get; set; }

        /// <summary>
        /// User shared secret
        /// </summary>
        public string UserPin { get; set; }

        /// <summary>
        /// Network pin (IMSI) - Subscriber ID
        /// </summary>
        public string NetworkPin { get; set; }

        #endregion
    }

}
