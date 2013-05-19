using System;
using System.Collections.Generic;
using System.Text;

namespace SharpSMS
{
    /// <summary>
    /// Abstract class representing body content of the SMS
    /// </summary>
    public interface ISmsMessageContent
    {
        /// <summary>
        /// Encoding of the data (7-bit, 8-bit, 16-bit)
        /// </summary>
        DataEncoding DataEncoding { get; set; }

        /// <summary>
        /// Returns byte array suitable to be send in sms message. Including all user headers (if any)
        /// </summary>
        /// <returns>byte array</returns>
        byte[] GetSMSBytes();

        /// <summary>
        /// Returns byte array of the user header
        /// </summary>
        /// <returns></returns>
        byte[] GetUDHBytes();
    }
}
