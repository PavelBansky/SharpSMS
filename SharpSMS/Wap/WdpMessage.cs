using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SharpSMS.Wap
{
    /// <summary>
    /// Represents WDP Message
    /// </summary>
    public abstract class WdpMessage
    {
        /// <summary>
        /// Source port of the message
        /// </summary>
        public int SourcePort { get; set; }
        /// <summary>
        /// Destination port of the message
        /// </summary>
        public int DestinationPort { get; set; }

        /// <summary>
        /// Generates the WDP (Wireless Datagram Protocol) or UDH (User Data Header) for the 
        /// SMS message. In the case comprising the Application Port information element
        /// indicating to the handset which application to start on receipt of the message
        /// </summary>
        /// <returns>byte array comprising the header</returns>
        public byte[] GetWdpHeader()
        {
            MemoryStream stream = new MemoryStream();

            stream.WriteByte(Wdp.INFORMATIONELEMENT_IDENTIFIER_APPLICATIONPORT);

            byte[] destPort = ToBigEndian(DestinationPort, 2);
            byte[] sourcePort = ToBigEndian(SourcePort, 2);

            // Length of port information = 2*16 bit numbers = 4 bytes            
            stream.WriteByte((byte)(destPort.Length + sourcePort.Length));
            stream.Write(destPort, 0, destPort.Length);
            stream.Write(sourcePort, 0, sourcePort.Length);

            MemoryStream headerStream = new MemoryStream();

            stream.WriteTo(headerStream);
            return headerStream.ToArray();
        }

        /// <summary>
        /// Converts number into BigEndian array of bytes
        /// </summary>
        /// <param name="number">Number</param>
        /// <param name="byteLen">Number of bytes to return</param>        
        private byte[] ToBigEndian(long number, byte byteLen)
        {
            byte[] outputArray = new byte[byteLen];            

            outputArray[byteLen - 1] = (byte)number;
            for (int i = byteLen - 2; i >= 0; i--)
                outputArray[i] = (byte)(number >> (8 * (i + 1)));

            return outputArray;
        }
    }
}
