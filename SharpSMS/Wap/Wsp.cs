using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SharpSMS.Wap
{
    /// <summary>
    /// Helper static class representing Wireless Session Protocol
    /// </summary>
    public class Wsp
    {
        #region Constans
        public const byte TRANSACTIONID_CONNECTIONLESSWSP = 0x01; // Can be random number
        public const byte PDUTYPE_PUSH = 0x06;

        public const byte WSP_ENCODING_VERSION_1_1 = 0x11;
        public const byte WSP_ENCODING_VERSION_1_2 = 0x12;
        public const byte WSP_ENCODING_VERSION_1_3 = 0x13;
        public const byte WSP_ENCODING_VERSION_1_4 = 0x14;
        
        public const byte HEADER_PUSHFLAG = 0x34;               // 0x34 | 0x80
        public const byte HEADER_X_WAP_CONTENT_URI = 0x30;      // 0x30 | 0x80
        public const byte HEADER_X_WAP_INITIATOR_URI = 0x31;    // 0x31 | 0x80
        public const byte HEADER_X_WAP_APPLICATION_ID = 0x2F;   // 0x2F | 0x80        
        public const byte HEADER_SEC = 0x11;                    // 0x11 | 0x80
        public const byte HEADER_MAC = 0x12;                    // 0x12 | 0x80        

        public const byte SEC_NETWPIN       = 0x00;
        public const byte SEC_USERPIN       = 0x01;
        public const byte SEC_USERNETWPIN   = 0x02;
        public const byte SEC_USERPINMAC    = 0x03;
        
        #endregion
        
        /// <summary>
        /// Static constructor
        /// </summary>
        static Wsp()
        {
            // Fill the dictionary
            WspContentTypes = new Dictionary<string, byte>();
            WspContentTypes.Add("*/*", 0x00);
            WspContentTypes.Add("text/*", 0x01);
            WspContentTypes.Add("text/html", 0x02);
            WspContentTypes.Add("text/plain", 0x03);
            WspContentTypes.Add("text/x-hdml", 0x04);
            WspContentTypes.Add("text/x-ttml", 0x05);
            WspContentTypes.Add("text/x-vCalendar", 0x06);
            WspContentTypes.Add("text/x-vCard", 0x07);
            WspContentTypes.Add("text/vnd.wap.wml", 0x08);
            WspContentTypes.Add("text/vnd.wap.wmlscript", 0x09);
            WspContentTypes.Add("text/vnd.wap.wta-event", 0x0A);
            WspContentTypes.Add("multipart/*", 0x0B);
            WspContentTypes.Add("multipart/mixed", 0x0C);
            WspContentTypes.Add("multipart/form-data", 0x0D);
            WspContentTypes.Add("multipart/byteranges", 0x0E);
            WspContentTypes.Add("multipart/alternative", 0x0F);
            WspContentTypes.Add("application/*", 0x10);
            WspContentTypes.Add("application/java-vm", 0x11);
            WspContentTypes.Add("application/x-www-form-urlencoded", 0x12);
            WspContentTypes.Add("application/x-hdmlc", 0x13);
            WspContentTypes.Add("application/vnd.wap.wmlc", 0x14);
            WspContentTypes.Add("application/vnd.wap.wmlscriptc", 0x15);
            WspContentTypes.Add("application/vnd.wap.wta-eventc", 0x16);
            WspContentTypes.Add("application/vnd.wap.uaprof", 0x17);
            WspContentTypes.Add("application/vnd.wap.wtls-ca-certificate", 0x18);
            WspContentTypes.Add("application/vnd.wap.wtls-user-certificate", 0x19);
            WspContentTypes.Add("application/x-x509-ca-cert", 0x1A);
            WspContentTypes.Add("application/x-x509-user-cert", 0x1B);
            WspContentTypes.Add("image/*", 0x1C);
            WspContentTypes.Add("image/gif", 0x1D);
            WspContentTypes.Add("image/jpeg", 0x1E);
            WspContentTypes.Add("image/tiff", 0x1F);
            WspContentTypes.Add("image/png", 0x20);
            WspContentTypes.Add("image/vnd.wap.wbmp", 0x21);
            WspContentTypes.Add("application/vnd.wap.multipart.*", 0x22);
            WspContentTypes.Add("application/vnd.wap.multipart.mixed", 0x23);
            WspContentTypes.Add("application/vnd.wap.multipart.form-data", 0x24);
            WspContentTypes.Add("application/vnd.wap.multipart.byteranges", 0x25);
            WspContentTypes.Add("application/vnd.wap.multipart.alternative", 0x26);
            WspContentTypes.Add("application/xml", 0x27);
            WspContentTypes.Add("text/xml", 0x28);
            WspContentTypes.Add("application/vnd.wap.wbxml", 0x29);
            WspContentTypes.Add("application/x-x968-cross-cert", 0x2A);
            WspContentTypes.Add("application/x-x968-ca-cert", 0x2B);
            WspContentTypes.Add("application/x-x968-user-cert", 0x2C);
            WspContentTypes.Add("text/vnd.wap.si", 0x2D);

            // WSP 1.2
            WspContentTypes.Add("application/vnd.wap.sic", 0x2E);
            WspContentTypes.Add("text/vnd.wap.sl", 0x2F);
            WspContentTypes.Add("application/vnd.wap.slc", 0x30);
            WspContentTypes.Add("text/vnd.wap.co", 0x31);
            WspContentTypes.Add("application/vnd.wap.coc", 0x32);
            WspContentTypes.Add("application/vnd.wap.multipart.related", 0x33);
            WspContentTypes.Add("application/vnd.wap.sia", 0x34);

            // WSP 1.3
            WspContentTypes.Add("text/vnd.wap.connectivity-xml", 0x35);
            WspContentTypes.Add("application/vnd.wap.connectivity-wbxml", 0x36);

            // WSP 1.4
            WspContentTypes.Add("application/pkcs7-mime", 0x37);
            WspContentTypes.Add("application/vnd.wap.hashed-certificate", 0x38);
            WspContentTypes.Add("application/vnd.wap.signed-certificate", 0x39);
            WspContentTypes.Add("application/vnd.wap.cert-response", 0x3A);
            WspContentTypes.Add("application/xhtml+xml", 0x3B);
            WspContentTypes.Add("application/wml+xml", 0x3C);
            WspContentTypes.Add("text/css", 0x3D);
            WspContentTypes.Add("application/vnd.wap.mms-message", 0x3E);
            WspContentTypes.Add("application/vnd.wap.rollover-certificate", 0x3F);

            // WSP 1.5
            WspContentTypes.Add("application/vnd.wap.locc+wbxml", 0x40);
            WspContentTypes.Add("application/vnd.wap.loc+xml", 0x41);
            WspContentTypes.Add("application/vnd.syncml.dm+wbxml", 0x42);
            WspContentTypes.Add("application/vnd.syncml.dm+xml", 0x43);
            WspContentTypes.Add("application/vnd.syncml.notification", 0x44);
            WspContentTypes.Add("application/vnd.wap.xhtml+xml", 0x45);
            WspContentTypes.Add("application/vnd.wv.csp.cir", 0x46);
            WspContentTypes.Add("application/vnd.oma.dd+xml", 0x47);
            WspContentTypes.Add("application/vnd.oma.drm.message", 0x48);
            WspContentTypes.Add("application/vnd.oma.drm.content", 0x49);
            WspContentTypes.Add("application/vnd.oma.drm.rights+xml", 0x4A);
            WspContentTypes.Add("application/vnd.oma.drm.rights+wbxml", 0x4B);

            // http://www.wapforum.org/wina/push-app-id.htm
            WspPushAppTypes = new Dictionary<string, int>();
            WspPushAppTypes.Add("x-wap-application:*",            0x00);
            WspPushAppTypes.Add("x-wap-application:push.sia",     0x01);
            WspPushAppTypes.Add("x-wap-application:wml.ua",       0x02);
            WspPushAppTypes.Add("x-wap-application:wta.ua",       0x03);
            WspPushAppTypes.Add("x-wap-application:mms.ua",       0x04);
            WspPushAppTypes.Add("x-wap-application:push.syncml",  0x05);
            WspPushAppTypes.Add("x-wap-application:loc.ua",       0x06);
            WspPushAppTypes.Add("x-wap-application:syncml.dm",    0x07);
            WspPushAppTypes.Add("x-wap-application:drm.ua",       0x08);
            WspPushAppTypes.Add("x-wap-application:emn.ua",       0x09);
            WspPushAppTypes.Add("x-wap-application:wv.ua",        0x0A);

            WspPushAppTypes.Add("x-wap-microsoft:localcontent.ua",    0x8000);
            WspPushAppTypes.Add("x-wap-microsoft:imclient.ua ",       0x8001);
            WspPushAppTypes.Add("x-wap-docomo:imode.mail.ua ",        0x8002);
            WspPushAppTypes.Add("x-wap-docomo:imode.mr.ua",           0x8003);
            WspPushAppTypes.Add("x-wap-docomo:imode.mf.ua",           0x8004);
            WspPushAppTypes.Add("x-motorola:location.ua ",            0x8005);
            WspPushAppTypes.Add("x-motorola:now.ua",                  0x8006);
            WspPushAppTypes.Add("x-motorola:otaprov.ua",              0x8007);
            WspPushAppTypes.Add("x-motorola:browser.ua",              0x8008);
            WspPushAppTypes.Add("x-motorola:splash.ua",               0x8009);
            WspPushAppTypes.Add("x-wap-nai:mvsw.command ",            0x800B);
            WspPushAppTypes.Add("x-wap-openwave:iota.ua",             0x8010);
        }

        #region Dictionaries Query Methods

        /// <summary>
        /// Method returns token for given Content Type. If not found returns -1
        /// </summary>
        /// <param name="contentType">Content Type to tokenize</param>
        /// <returns>Token</returns>
        public static int GetContentType(string contentType)
        {
            if (WspContentTypes.ContainsKey(contentType))
                return WspContentTypes[contentType];
            else
                return -1;
        }

        /// <summary>
        /// Method returns token for given Application Type. If not found returns -1
        /// </summary>
        /// <param name="applicationType">Application Type to tokenize</param>
        /// <returns>Token</returns>
        public static int GetpplicationType(string applicationType)
        {
            if (WspPushAppTypes.ContainsKey(applicationType))
                return WspPushAppTypes[applicationType];
            else
                return -1;
        }
        #endregion

        #region Methods to Write WSP Values

        /// <summary>
        /// Writes "extension media" (null terminated string) to stream, suitable for WSP protocol
        /// </summary>
        /// <param name="stream">Output stream</param>
        /// <param name="extension">Text to write</param>
        public static void WriteExtensionMedia(MemoryStream stream, string extension)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(extension);

            // write the text
            stream.Write(bytes, 0, bytes.Length);

            // string must be null terminated
            stream.WriteByte(0x00);
        }

        /// <summary>
        /// Writes text to stream, suitable for WSP protocol
        /// </summary>
        /// <param name="stream">Output stream</param>
        /// <param name="text">Text to wrap</param>
        public static void WriteTextString(MemoryStream stream, string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);

            // If the first character in the TEXT is in the range of 128-255, a Quote character must precede it.    
            // Otherwise the Quote character must be omitted. The Quote is not part of the contents
            if ((text[0] & 0x80) > 0x00)
            {
                stream.WriteByte(0x7f);
            }

            // write the text
            stream.Write(bytes, 0, bytes.Length);

            // string must be null terminated
            stream.WriteByte(0x00);
        }

        /// <summary>
        /// Writes integer value to the stream, according to WSP specifications
        /// </summary>
        /// <param name="stream">Output stream</param>
        /// <param name="theValue">value</param>
        public static void WriteInteger(MemoryStream stream, long theValue)
        {
            if (theValue < 128)
                WriteShortInteger(stream, (byte)theValue);
            else
                WriteLongInteger(stream, theValue);
        }

        /// <summary>
        /// Writes WSP-short-integer (byte) value to the stream, according to WSP specifications
        /// </summary>
        /// <param name="stream">Output stream</param>
        /// <param name="theValue">value</param>
        public static void WriteShortInteger(MemoryStream stream, byte theValue)
        {
            stream.WriteByte((byte)(theValue | (byte)0x80));
        }

        /// <summary>
        /// Writes long-integer value to the stream, according to WSP specifications
        /// </summary>
        /// <param name="stream">Output stream</param>
        /// <param name="number">value</param>
        public static void WriteLongInteger(MemoryStream stream, long number)
        {
             int nOctets = 0;
             while ((number >> (8 * nOctets)) > 0)
             {
                 nOctets++;
             }
             stream.WriteByte((byte) nOctets);
     
             for (int i = nOctets; i > 0; i--)
             {
                 byte octet = (byte) (number >> (8 * (i - 1)));
                 byte byteValue = (byte) ((byte) octet & (byte) (0xff));
                 stream.WriteByte(byteValue);
             }            
        }

        /// <summary>
        /// Writes "UIntVar" value to the stream, according to WSP specifications
        /// </summary>
        /// <param name="stream">Output stream</param>
        /// <param name="number">value</param>
        public static void WriteUintvar(MemoryStream stream, long number)
        {
            int nOctets = 1;
            while ((number >> (7 * nOctets)) > 0)
            {
                nOctets++;
            }

            for (int i = nOctets; i > 0; i--)
            {
                byte octet = (byte) (number >> (7 * (i - 1)));
                byte byteValue = (byte) ((byte) octet & (byte) 0x7f);
                if (i > 1)
                {
                    byteValue = (byte) (byteValue | (byte) 0x80);
                }
                stream.WriteByte(byteValue);
            }
        }

        /// <summary>
        /// Writes number into the stream
        /// </summary>
        /// <param name="stream">Output stream</param>
        /// <param name="number">value</param>
        public static void WriteValueLength(MemoryStream stream, long number)
        {
            // ShortLength | (Length-quote Length)
            if (number >=0 && number <= 30)
            {
                // Short-length
                stream.WriteByte((byte)number);
            }
            else
            {
                // Length-quote == Octet 31
                stream.WriteByte(31);
                WriteUintvar(stream, number);
            }
        }
        #endregion

        #region WSP Header Writing Methods

        /// <summary>
        /// Writes header for given Content Type into stream
        /// </summary>
        /// <param name="stream">Output Stream</param>
        /// <param name="contentType">Content Type</param>
        public static void WriteHeaderContentType(MemoryStream stream, string contentType)
        {
            int wellKnownContentType = GetContentType(contentType.ToLower());
     
            if (wellKnownContentType == -1)
            {
                WriteValueLength(stream, contentType.Length + 1);
                WriteExtensionMedia(stream, contentType);
            }
            else
            {
                WriteShortInteger(stream, (byte)wellKnownContentType);
            }
        }

        /// <summary>
        /// Writes header for given X-WAP-Application-ID
        /// </summary>
        /// <param name="stream">Output stream</param>
        /// <param name="applicationType">Application Type</param>
        public static void WriteHeaderXWAPApplicationID(MemoryStream stream, string applicationType)
        {
            int wellKnownAppId = GetpplicationType(applicationType.ToLower());                

            WriteShortInteger(stream, HEADER_X_WAP_APPLICATION_ID);

            if (wellKnownAppId == -1)
                WriteTextString(stream, applicationType);
            else
                WriteInteger(stream, wellKnownAppId);
        }

        /// <summary>
        /// Writes header for given X-WAP-Initiator-URI
        /// </summary>
        /// <param name="stream">Output stream</param>
        /// <param name="initiatorURI">Initiator URI</param>
        public static void WriteHeaderXWAPInitiatorURI(MemoryStream stream, string initiatorURI)
        {
            WriteShortInteger(stream, HEADER_X_WAP_INITIATOR_URI);
            WriteTextString(stream, initiatorURI);
        }

        /// <summary>
        /// Writes header for given X-WAP-Content-URI
        /// </summary>
        /// <param name="stream">Output stream</param>
        /// <param name="contentURI">Content URI</param>
        public static void WriteHeaderXWAPContentURI(MemoryStream stream, string contentURI)
        {
            WriteShortInteger(stream, HEADER_X_WAP_CONTENT_URI);
            WriteTextString(stream, contentURI);
        }

        /// <summary>
        /// Writes header for given Push Flag
        /// </summary>
        /// <param name="stream">Output stream</param>
        /// <param name="pushFlag">Push Flag</param>
        public static void WriteHeaderPushFlag(MemoryStream stream, byte pushFlag)
        {
            WriteShortInteger(stream, HEADER_PUSHFLAG);
            WriteShortInteger(stream, pushFlag);
        }

        /// <summary>
        /// Writes security header
        /// </summary>
        /// <param name="stream">Ouput stream</param>
        /// <param name="method">Security method</param>
        /// <param name="macKey">Key</param>
        public static void WriteHeaderSecurity(MemoryStream stream, SecurityMethod method, byte[] macKey)
        {
            WriteShortInteger(stream, HEADER_SEC);

            switch (method)
            {
                case SecurityMethod.NETWPIN:
                    WriteShortInteger(stream, SEC_NETWPIN);
                    break;
                case SecurityMethod.USERPIN:
                    WriteShortInteger(stream, SEC_USERPIN);
                    break;
                case SecurityMethod.USERNETWPIN:
                    WriteShortInteger(stream, SEC_USERNETWPIN);
                    break;
                case SecurityMethod.USERPINMAC:
                    WriteShortInteger(stream, SEC_USERPINMAC);
                    break;
            }

            WriteShortInteger(stream, HEADER_MAC);

            string macKeyString = string.Empty;
            foreach (byte b in macKey)
                macKeyString += b.ToString("X2");
            WriteTextString(stream, macKeyString);
        }

        #endregion

        #region Fields

        /// <summary>
        /// Represent Content Type to Token Dictionary
        /// </summary>
        private static Dictionary<string, byte> WspContentTypes;

        /// <summary>
        /// Represent Application Type to Token Dictionary
        /// </summary>
        private static Dictionary<string, int> WspPushAppTypes;
        #endregion

        /// <summary>
        /// Security method for message signing
        /// </summary>
        public enum SecurityMethod
        {
            /// <summary>
            /// No security will be applied
            /// </summary>
            None,
            /// <summary>
            /// Message is signed with network PIN
            /// </summary>
            NETWPIN,
            /// <summary>
            /// Message is signed with user PIN
            /// </summary>
            USERPIN,
            /// <summary>
            /// Message is signed with network PIN and user PIN
            /// </summary>
            USERNETWPIN,
            /// <summary>
            /// Not supported
            /// </summary>
            USERPINMAC,
        }     
    }
}
