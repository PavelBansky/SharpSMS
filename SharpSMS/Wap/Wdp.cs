using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SharpSMS.Wap
{
    /// <summary>
    /// Helper static class representing Wireless Datagram Protocol
    /// </summary>
    public class Wdp
    {        
        // Ports for the WDP information element, instructing the handset which 
        // application to load on receving the message 
        public const byte INFORMATIONELEMENT_IDENTIFIER_APPLICATIONPORT = 0x05; // 16 Bit
     
        public const int WAP_PORT_PUSH_SESSION_DESTINATION      = 0x0B84;
        public const int WAP_PORT_PUSH_SESSION_SOURCE           = 0x23F0;
        public const int WAP_PORT_VCARD                         = 0x23F4;
        public const int WAP_PORT_VCALENDAR                     = 0x23F5;

        public const int WAP_PORT_NOKIA_RINGTONE                = 0x1581;
        public const int WAP_PORT_NOKIA_OPERATORLOGO            = 0x1582;
        public const int WAP_PORT_NOKIA_CLILOGO                 = 0x1583;
        public const int WAP_PORT_NOKIA_MULTIPART               = 0x158A;
        public const int WAP_PORT_NOKIA_OTASETTINGS             = 0xC34F;        
    }
}
