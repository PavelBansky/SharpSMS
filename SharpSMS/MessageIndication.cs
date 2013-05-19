namespace SharpSMS
{
    /// <summary>
    /// Class represents Message Indication/Data Coding scheme of the message
    /// </summary>
    public class MessageIndication
    {
        #region Constants
        /// <summary>
        /// Coding Scheme Bit 4 set; message class is specified by least significant bits 1,0
        /// </summary>
        const byte CODING_SCHEME_MESSAGE_CLASS_SPECIFIED = 0x10;
        /// <summary>
        /// Coding Scheme Bits 3,2; default encoding
        /// </summary>
        const byte CODING_SCHEME_DEFAULT_ENCODING = 0x00;
        /// <summary>
        /// Coding Scheme Bits 3,2; 8-bit encoding
        /// </summary>
        const byte CODING_SCHEME_8BIT_ENCODING = 0x04;
        /// <summary>
        /// Coding Scheme Bits 3,2; UCS2 encoding
        /// </summary>
        const byte CODING_SCHEME_UCS2_ENCODING = 0x08;
        /// <summary>
        /// Coding Scheme Bits 1,0; Message Class - Immediate Display
        /// </summary>
        const byte CODING_SCHEME_MESSAGE_CLASS_IMMEDIATE_DISPLAY = 0x00;
        /// <summary>
        /// Coding Scheme Bits 1,0; Message Class - ME Specific
        /// </summary>
        const byte CODING_SCHEME_MESSAGE_CLASS_MESPECIFIC = 0x01;
        /// <summary>
        /// Coding Scheme Bits 1,0; Message Class - SIM Specific
        /// </summary>
        const byte CODING_SCHEME_MESSAGE_CLASS_SIMSPECIFIC = 0x02;
        /// <summary>
        /// Coding Scheme Bits 1,0; Message Class - TE Specific
        /// </summary>
        const byte CODING_SCHEME_MESSAGE_CLASS_TESPECIFIC = 0x03;
        /// <summary>
        /// Coding Scheme Bits 7..4; Message Indication Discard
        /// </summary>
        const byte CODING_SCHEME_MESSAGE_INDICATION_DISCARD = 0xC0;
        /// <summary>
        /// Coding Scheme Bits 7..4; Message Indication Store - default encoding
        /// </summary>
        const byte CODING_SCHEME_MESSAGE_INDICATION_STORE_DEFAULT_ENCODING = 0xD0;
        /// <summary>
        /// Coding Scheme Bits 7..4; Message Indication Store - UCS2 encoding
        /// </summary>
        const byte CODING_SCHEME_MESSAGE_INDICATION_STORE_UCS2_ENCODING = 0xE0;
        /// <summary>
        /// Coding Scheme Bit 3; Message Indication set to active
        /// </summary>
        const byte CODING_SCHEME_INDICATION_ACTIVE = 0x08;
        /// <summary>
        /// Coding Scheme Bit 1,0; Message Indication - Voicemail
        /// </summary>
        const byte CODING_SCHEME_INDICATION_VOICEMAIL = 0x00;
        /// <summary>
        /// Coding Scheme Bit 1,0; Message Indication - Fax
        /// </summary>
        const byte CODING_SCHEME_INDICATION_FAX = 0x01;
        /// <summary>
        /// Coding Scheme Bit 1,0; Message Indication - Email
        /// </summary>
        const byte CODING_SCHEME_INDICATION_EMAIL = 0x02;
        /// <summary>
        /// Coding Scheme Bit 1,0; Message Indication - Other
        /// </summary>
        const byte CODING_SCHEME_INDICATION_OTHER = 0x03;
        #endregion

        /// <summary>
        /// Creates new message indication with default options
        /// </summary>
        public MessageIndication()
            : this(MessageClass.MESpecific)
        {
        }

        /// <summary>
        /// Creates new message indication
        /// </summary>
        /// <param name="messageClass">Define if and where message will be stored</param>
        public MessageIndication(MessageClass messageClass)
        {
            this.Class = messageClass; 
            this.Type = IndicationType.Voicemail;
            this.Operation = MessageIndicationOperation.NotSet;
            this.IsActive = false;            
        }

        /// <summary>
        /// Returns TP-DCS Data coding scheme value that represents the message indication information
        /// </summary>
        /// <param name="dataEncoding">Data encoding used in the message</param>
        /// <returns></returns>
        public byte ToByte(DataEncoding dataEncoding)
        {
            byte dataCodingScheme = CODING_SCHEME_MESSAGE_CLASS_SPECIFIED;

            // Set encoding
            if (dataEncoding == DataEncoding.Default7bit)
                dataCodingScheme |= CODING_SCHEME_DEFAULT_ENCODING;
            else if (dataEncoding == DataEncoding.Data8bit)
                dataCodingScheme |= CODING_SCHEME_8BIT_ENCODING;
            else if (dataEncoding == DataEncoding.UCS2_16bit)
                dataCodingScheme |= CODING_SCHEME_UCS2_ENCODING;

            // Set indication
            if (this.Class == SharpSMS.MessageClass.ImmediateDisplay)
                dataCodingScheme |= CODING_SCHEME_MESSAGE_CLASS_IMMEDIATE_DISPLAY;
            else if (this.Class == SharpSMS.MessageClass.MESpecific)
                dataCodingScheme |= CODING_SCHEME_MESSAGE_CLASS_MESPECIFIC;
            else if (this.Class == SharpSMS.MessageClass.SIMSpecific)
                dataCodingScheme |= CODING_SCHEME_MESSAGE_CLASS_SIMSPECIFIC;
            else if (this.Class == SharpSMS.MessageClass.TESpecific)
                dataCodingScheme |= CODING_SCHEME_MESSAGE_CLASS_TESPECIFIC;

            if (this.Operation != MessageIndicationOperation.NotSet)
                dataCodingScheme = GetWaitingIndication(dataEncoding);

            return dataCodingScheme;
        }

        /// <summary>
        /// Returns coding scheme part for MessageWaitingIndication
        /// </summary>
        /// <returns></returns>
        private byte GetWaitingIndication(DataEncoding dataEncoding)
        {
            byte result = 0x00;

            // Is it Discard or Store? Store depends on alphabet encoding
            if (this.Operation == MessageIndicationOperation.Discard)
                result |= CODING_SCHEME_MESSAGE_INDICATION_DISCARD;
            else if (this.Operation == MessageIndicationOperation.Store && dataEncoding == DataEncoding.Default7bit)
                result |= CODING_SCHEME_MESSAGE_INDICATION_STORE_DEFAULT_ENCODING;
            else if (this.Operation == MessageIndicationOperation.Store && dataEncoding == DataEncoding.UCS2_16bit)
                result |= CODING_SCHEME_MESSAGE_INDICATION_STORE_UCS2_ENCODING;

            // Activeting or Deactivating Indication
            if (this.IsActive)
                result |= CODING_SCHEME_INDICATION_ACTIVE;

            switch (this.Type)
            {
                case IndicationType.Voicemail:
                    result |= CODING_SCHEME_INDICATION_VOICEMAIL;
                    break;
                case IndicationType.FaxMessage:
                    result |= CODING_SCHEME_INDICATION_FAX;
                    break;
                case IndicationType.EmailMessage:
                    result |= CODING_SCHEME_INDICATION_EMAIL;
                    break;
                case IndicationType.OtherMessage:
                    result |= CODING_SCHEME_INDICATION_OTHER;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Message classs pecifies how message will be treated on recipients device
        /// </summary>
        public MessageClass Class { get; set; }

        /// <summary>
        /// Type of the Indication (Voicemail, Fax, Email ...)
        /// </summary>
        public IndicationType Type { get; set; }

        /// <summary>
        /// Defines if indication message will be discarded or stored on the device
        /// </summary>
        public MessageIndicationOperation Operation { get; set; }

        /// <summary>
        /// True sets indication to active state. False removes the indication
        /// </summary>
        public bool IsActive { get; set; }
    }
}
