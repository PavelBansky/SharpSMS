using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SharpSMS.Wbxml
{
    /// <summary>
    /// Class represents Service Indication Messages
    /// </summary>
    public class ServiceIndication : WBXMLDocument
    {
        #region Constants
        // ServiceIndication 1.0 Public Identifier
        public const byte DOCUMENT_DTD_ServiceIndication = 0x05;

        // Tag Tokens
        public const byte TAGTOKEN_si = 0x05;
        public const byte TAGTOKEN_indication = 0x06;
        public const byte TAGTOKEN_info = 0x07;
        public const byte TAGTOKEN_item = 0x08;

        // Attribute Tokens
        public const byte ATTRIBUTESTARTTOKEN_action_signal_none = 0x05;
        public const byte ATTRIBUTESTARTTOKEN_action_signal_low = 0x06;
        public const byte ATTRIBUTESTARTTOKEN_action_signal_medium = 0x07;
        public const byte ATTRIBUTESTARTTOKEN_action_signal_high = 0x08;
        public const byte ATTRIBUTESTARTTOKEN_action_signal_delete = 0x09;
        public const byte ATTRIBUTESTARTTOKEN_created = 0x0A;
        public const byte ATTRIBUTESTARTTOKEN_href = 0x0B;
        public const byte ATTRIBUTESTARTTOKEN_href_http = 0x0C;         // http://
        public const byte ATTRIBUTESTARTTOKEN_href_http_www = 0x0D;     // http://www.
        public const byte ATTRIBUTESTARTTOKEN_href_https = 0x0E;		// https://
        public const byte ATTRIBUTESTARTTOKEN_href_https_www = 0x0F;	// https://www.
        public const byte ATTRIBUTESTARTTOKEN_si_expires = 0x10;
        public const byte ATTRIBUTESTARTTOKEN_si_id = 0x11;
        public const byte ATTRIBUTESTARTTOKEN_class = 0x12;

        // Attribute Value Tokens
        public const byte ATTRIBUTEVALUETOKEN_com = 0x85;				// .com/
        public const byte ATTRIBUTEVALUETOKEN_edu = 0x86;				// .edu/
        public const byte ATTRIBUTEVALUETOKEN_net = 0x87;				// .net/
        public const byte ATTRIBUTEVALUETOKEN_org = 0x88;				// .org/
        #endregion

        #region Properties
        /// <summary>
        /// Text of the service Indication Message
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Href - link in the Service Indication Message
        /// </summary>
        public string Href { get; set; }

        /// <summary>
        /// Level of notification to the user (optional)
        /// </summary>
        public ServiceIndicationAction Action { get; set; }

        /// <summary>
        /// Creation DateTime of the SI message (optional)
        /// </summary>
        public DateTime Created {get; set; }

        /// <summary>
        /// Expiration DateTime of the SI message (optional)
        /// </summary>
        public DateTime Expires { get; set; }

        /// <summary>
        /// ID of the SI message (optional)
        /// </summary>
        public string Id { get; set; }
        #endregion

        #region Fields
        private Dictionary<string, byte> hrefStartTokens;
        private Dictionary<string, byte> attributeValueTokens;
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public ServiceIndication()
        {
            // Set the content type
            this.ContentType = "application/vnd.wap.sic"; // HEADER_CONTENTTYPE_application_vnd_wap_sic;

            // Create token dictonary
            hrefStartTokens = new Dictionary<string, byte>();
            hrefStartTokens.Add("https://www.", ATTRIBUTESTARTTOKEN_href_https_www);
            hrefStartTokens.Add("http://www.", ATTRIBUTESTARTTOKEN_href_http_www);
            hrefStartTokens.Add("https://", ATTRIBUTESTARTTOKEN_href_https);
            hrefStartTokens.Add("http://", ATTRIBUTESTARTTOKEN_href_http);

            attributeValueTokens = new Dictionary<string, byte>();
            attributeValueTokens.Add(".com/", ATTRIBUTEVALUETOKEN_com);
            attributeValueTokens.Add(".edu/", ATTRIBUTEVALUETOKEN_edu);
            attributeValueTokens.Add(".net/", ATTRIBUTEVALUETOKEN_net);
            attributeValueTokens.Add(".org/", ATTRIBUTEVALUETOKEN_org);
        }
        
        /// <summary>
        /// Creates Service Indication Message
        /// </summary>
        /// <param name="text">Text of the message</param>
        /// <param name="href">Link in the message</param>
        /// <param name="action">Action to the user</param>
        public ServiceIndication(string text, string href, ServiceIndicationAction action) : this()
        {
            this.Text = text;
            this.Href = href;
            this.Action = action;
        }

        /// <summary>
        /// Creates Service Indication Message
        /// </summary>
        /// <param name="text">Text of the message</param>
        /// <param name="href">Link in the message</param>
        public ServiceIndication(string text, string href)
            : this(text, href, ServiceIndicationAction.NotSet)
        {
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Returns tokenized Service Indication message.
        /// </summary>
        /// <returns>byte array</returns>
        public override byte[] GetWBXMLBytes()
        {
            MemoryStream wbxmlBytes = new MemoryStream();

            // header
            wbxmlBytes.WriteByte(WBXMLDocument.VERSION_1_2);
            wbxmlBytes.WriteByte(DOCUMENT_DTD_ServiceIndication);
            wbxmlBytes.WriteByte(WBXMLDocument.CHARSET_UTF_8);
            wbxmlBytes.WriteByte(WBXMLDocument.NULL); // String table length

            // xml
            wbxmlBytes.WriteByte(SetTagTokenIndications(TAGTOKEN_si, false, true));  // <si> with content
            wbxmlBytes.WriteByte(SetTagTokenIndications(TAGTOKEN_indication, true, true)); // <indication> with cont. and attr.

            #region Tokenizing of the HREF

            // find the starting attribute token (http:// https:// etc..)
            int pos = 0;
            byte hrefTagToken = ATTRIBUTESTARTTOKEN_href;
            foreach (string startString in hrefStartTokens.Keys)
            {
                if (this.Href.StartsWith(startString))
                {
                    hrefTagToken = hrefStartTokens[startString];
                    pos = startString.Length;
                    break;
                }
            }

            // find the `A` domain token. (.com .net .org etc..)
            int domainPos = -1;
            string domain = string.Empty;
            foreach (string domainString in attributeValueTokens.Keys)
            {
                domainPos = this.Href.IndexOf(domainString);
                if (domainPos >= 0)
                {
                    domain = domainString;
                    break;
                }
            }
            #endregion

            // write href url
            if (domainPos >= 0) // encode domain
            {
                wbxmlBytes.WriteByte(hrefTagToken);
                WriteInlineString(wbxmlBytes, Href.Substring(pos, domainPos - pos));
                wbxmlBytes.WriteByte(attributeValueTokens[domain]);
                WriteInlineString(wbxmlBytes, Href.Substring(domainPos + domain.Length));
            }
            else // no domain to encode
            {
                wbxmlBytes.WriteByte(hrefTagToken);                
                WriteInlineString(wbxmlBytes, Href.Substring(pos));
            }

            // writes action signal
            if (Action != ServiceIndicationAction.NotSet)
                wbxmlBytes.WriteByte(GetActionToken(Action));

            // writes Created dateTime signal (if set)
            if (Created != DateTime.MinValue)
            {
                wbxmlBytes.WriteByte(ATTRIBUTESTARTTOKEN_created);                
                WriteDate(wbxmlBytes, Created);
            }

            // writes Experi dateTime signal (if set)
            if (Expires != DateTime.MinValue)
            {
                wbxmlBytes.WriteByte(ATTRIBUTESTARTTOKEN_si_expires);                
                WriteDate(wbxmlBytes, Expires);
            }

            // writes Si-id (if set)
            if (Id != null)
            {
                wbxmlBytes.WriteByte(ATTRIBUTESTARTTOKEN_si_id);                
                WriteInlineString(wbxmlBytes, Id);
            }

            // end indication
            wbxmlBytes.WriteByte(WBXMLDocument.TAGTOKEN_END); // </indication>

            // Writes texts            
            WriteInlineString(wbxmlBytes, Text);

            // End tags
            wbxmlBytes.WriteByte(WBXMLDocument.TAGTOKEN_END);
            wbxmlBytes.WriteByte(WBXMLDocument.TAGTOKEN_END);

            byte[] wbxmlBytesArray = wbxmlBytes.ToArray();
            wbxmlBytes.Close();

            return wbxmlBytesArray;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Returns token for give Action
        /// </summary>
        /// <param name="action">Service Indication Action</param>
        /// <returns>Token</returns>
        protected byte GetActionToken(ServiceIndicationAction action)
        {
            byte actionToken;

            switch (action)
            {
                case ServiceIndicationAction.Delete:
                        actionToken = ATTRIBUTESTARTTOKEN_action_signal_delete;
                    break;
                case ServiceIndicationAction.Signal_high:
                        actionToken = ATTRIBUTESTARTTOKEN_action_signal_high;
                    break;
                case ServiceIndicationAction.Signal_low:
                        actionToken = ATTRIBUTESTARTTOKEN_action_signal_low;
                    break;
                case ServiceIndicationAction.Signal_medium:
                        actionToken = ATTRIBUTESTARTTOKEN_action_signal_medium;
                    break;
                default:
                        actionToken = ATTRIBUTESTARTTOKEN_action_signal_none;
                    break;
            }

            return actionToken;
        }
        #endregion
    }
}
