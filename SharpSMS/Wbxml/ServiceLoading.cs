using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SharpSMS.Wbxml
{
    /// <summary>
    /// Class represents Service Loading Messages
    /// </summary>
    public class ServiceLoading : WBXMLDocument
    {
        #region Constants
        // ServiceIndication 1.0 Public Identifier
        public const byte DOCUMENT_DTD_ServiceIndication = 0x06;

        // Tag Tokens
        public const byte TAGTOKEN_sl = 0x05;

        // Attribute Tokens
        public const byte ATTRIBUTESTARTTOKEN_action_execute_low = 0x05;        
        public const byte ATTRIBUTESTARTTOKEN_action_execute_high = 0x06;
        public const byte ATTRIBUTESTARTTOKEN_action_cache = 0x07;        
        public const byte ATTRIBUTESTARTTOKEN_href = 0x08;
        public const byte ATTRIBUTESTARTTOKEN_href_http = 0x09;         // http://
        public const byte ATTRIBUTESTARTTOKEN_href_http_www = 0x0A;     // http://www.
        public const byte ATTRIBUTESTARTTOKEN_href_https = 0x0B;		// https://
        public const byte ATTRIBUTESTARTTOKEN_href_https_www = 0x0C;	// https://www.

        // Attribute Value Tokens
        public const byte ATTRIBUTEVALUETOKEN_com = 0x85;				// .com/
        public const byte ATTRIBUTEVALUETOKEN_edu = 0x86;				// .edu/
        public const byte ATTRIBUTEVALUETOKEN_net = 0x87;				// .net/
        public const byte ATTRIBUTEVALUETOKEN_org = 0x88;				// .org/        
        #endregion

        #region Properties

        /// <summary>
        /// Location of the file to download
        /// </summary>
        public string Href { get; set; }

        /// <summary>
        /// Action to do after download
        /// </summary>
        public ServiceLoadingAction Action { get; set; }
        #endregion

        #region Fields
        private Dictionary<string, byte> hrefStartTokens;
        private Dictionary<string, byte> attributeValueTokens;
        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public ServiceLoading()
        {
            // Set the content type
            this.ContentType = "application/vnd.wap.slc";

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
        /// Creates Service Loading Message
        /// </summary>
        /// <param name="href">Link to the file</param>
        /// <param name="action">Action after download</param>
        public ServiceLoading(string href, ServiceLoadingAction action) : this()
        {            
            this.Href = href;
            this.Action = action;
        }

        /// <summary>
        /// Creates Service Loading Message
        /// </summary>
        /// <param name="href">Link to the file</param>
        public ServiceLoading(string href)
            : this(href, ServiceLoadingAction.NotSet)
        {
        }
        #endregion

        #region Override Methods

        /// <summary>
        /// Returns tokenized Service Loading message.
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
            wbxmlBytes.WriteByte(SetTagTokenIndications(TAGTOKEN_sl, true, false));  // <sl> with attributes

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
            if (Action != ServiceLoadingAction.NotSet)
                wbxmlBytes.WriteByte(GetActionToken(Action));

            // End tag
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
        /// <param name="action">Service Loading Action</param>
        /// <returns>Token</returns>
        protected byte GetActionToken(ServiceLoadingAction action)
        {
            byte actionToken;

            switch (action)
            {
                case ServiceLoadingAction.Execute_high:
                        actionToken = ATTRIBUTESTARTTOKEN_action_execute_high;
                    break;
                case ServiceLoadingAction.Execute_low:
                        actionToken = ATTRIBUTESTARTTOKEN_action_execute_low;
                    break;
                default:
                        actionToken = ATTRIBUTESTARTTOKEN_action_cache;
                    break;                
            }

            return actionToken;
        }
        #endregion
    }
}
