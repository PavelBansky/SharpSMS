namespace SharpSMS
{
    /// <summary>
    /// Represents Telematic device
    /// </summary>
    public enum TelematicDevice : byte
    {
        /// <summary>
        /// implicit - device type is specific or can be concluded on the basis of the address
        /// </summary>
        Implicit = 0x00,
        /// <summary>
        /// Telex (or teletex reduced to telex format)
        /// </summary>        
        Telex = 0x01,
        /// <summary>
        /// Group 3 telefax
        /// </summary>
        Group3Telefax = 0x02,
        /// <summary>
        /// Group 4 telefax
        /// </summary>
        Group4Telefax = 0x03,
        /// <summary>
        /// Voice telephone
        /// </summary>
        VoiceTelephone = 0x04,
        /// <summary>
        /// ERMES
        /// </summary>
        ERMES = 0x05,
        /// <summary>
        /// National Paging System
        /// </summary>
        NationalPagingSystem = 0x06,
        /// <summary>
        /// Videotex (T.100/T.101)
        /// </summary>
        Videotex = 0x07,
        /// <summary>
        /// Teletex
        /// </summary>
        Teletex = 0x08,
        /// <summary>
        /// Teletex, in PSPDN
        /// </summary>
        TeletexPSDN = 0x09,
        /// <summary>
        /// Teletex, in CSPDN
        /// </summary>
        TeletexCSPDN =0x0A,
        /// <summary>
        /// Teletex, in analog PSTN
        /// </summary>
        TeletexPSTN = 0x0B,
        /// <summary>
        /// Teletex, in digital ISDN
        /// </summary>
        TeletexISDN = 0X0C,
        /// <summary>
        /// UCI (Universal Computer Interface, ETSI DE/PS 3 01-3)
        /// </summary>
        UCI = 0x0D,
        /// <summary>
        /// S message handling facility
        /// </summary>
        MessageHandlingFacility = 0x10,
        /// <summary>
        ///	Sny public X.400-based message handling system
        /// </summary>
        X400 = 0x11,
        /// <summary>
        /// Internet Electronic Mail
        /// </summary>
        Email = 0x12,
        /// <summary>
        /// A GSM mobile station
        /// </summary>
        GSMMobileStation = 0x1F
    }
}