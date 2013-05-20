using SharpSMS;
using SharpSMS.Wap;
using SharpSMS.Wbxml;
using System;
using System.Text;

namespace Samples
{
    class SampleMessage
    {
        public static SMSSubmit PlainText()
        {
            TextMessage textMessage = new TextMessage();
            textMessage.DataEncoding = DataEncoding.Default7bit;   
            textMessage.Text = "Hello World from SharpSMS!";            

            SMSSubmit sms = new SMSSubmit();
            sms.MessageToSend = textMessage;  
          
            // SMS center will retry the delivery for 5 days
            sms.ValidityPeriod = new TimeSpan(5, 0, 0, 0);

            return sms;
        }

        public static SMSSubmit FlashMessage()
        {
            TextMessage textMessage = new TextMessage();
            textMessage.DataEncoding = DataEncoding.Default7bit;
            textMessage.Text = "Flash message from SharpSMS!";

            SMSSubmit sms = new SMSSubmit();
            sms.MessageToSend = textMessage;
            sms.Indication.Class = MessageClass.ImmediateDisplay;
            
            return sms;
        }

        public static SMSSubmit ReplacebleMessage(string text)
        {
            TextMessage textMessage = new TextMessage();
            textMessage.Text = text;            

            SMSSubmit sms = new SMSSubmit();
            sms.MessageToSend = textMessage;

            sms.ProtocolIdentifier = new ProtocoldentifierBuilder(ShortMessageType.ReplaceMessageType1).ToByte();
            
            return sms;
        }

        public static SMSSubmit ActivateVoicemailIndication()
        {
            TextMessage textMessage = new TextMessage();
            textMessage.Text = "You have a voicemail.";

            SMSSubmit sms = new SMSSubmit();
            sms.MessageToSend = textMessage;

            sms.Indication.Operation = MessageIndicationOperation.Discard;
            sms.Indication.Type = IndicationType.Voicemail;
            sms.Indication.IsActive = true;

            return sms;
        }

        public static SMSSubmit DeactivateVoicemailIndication()
        {       
            SMSSubmit sms = new SMSSubmit(new TextMessage());

            sms.Indication.Operation = MessageIndicationOperation.Discard;
            sms.Indication.Type = IndicationType.Voicemail;            
            sms.Indication.IsActive = false;

            return sms;
        }

        public static SMSSubmit ServiceIndicationMessage()
        {
            ServiceIndication si = new ServiceIndication();
            si.Action = ServiceIndicationAction.Signal_medium;
            si.Text = "Service indication from SharpSMS";
            si.Href = "https://github.com/pavel-b/SharpSMS";
            si.Expires = DateTime.Now.AddDays(3);

            WapPushMessage wapPushMessage = new WapPushMessage(si);
            wapPushMessage.XWapInitiatorURI = "SharpSMS";

            return new SMSSubmit(wapPushMessage); 
        }

        public static SMSSubmit ServiceLoadingMessage()
        {
            ServiceLoading sl = new ServiceLoading();
            sl.Action = ServiceLoadingAction.Execute_high;
            // This is a cab file with Total Commander for Windows Mobile 5/6/6.5
            sl.Href = "http://ghisler.fileburst.com/ce/tcmdphone.cab";

            WapPushMessage wapPushMessage = new WapPushMessage(sl);
            wapPushMessage.XWapInitiatorURI = "SharpSMS";
            return new SMSSubmit(wapPushMessage);
        }

        public static SMSSubmit WapPushConfiguration()
        {
            // This is a configuration XML for Windows Mobile Internet Explorer Favorites
            string configXML = @"<wap-provisioningdoc><characteristic type=""BrowserFavorite""><characteristic type=""SharpSMS""><parm name=""URL"" value=""https://github.com/pavel-b/SharpSMS""/></characteristic></characteristic></wap-provisioningdoc>";

            WapPushMessage wapPushMessage = new WapPushMessage();
            wapPushMessage.XWapInitiatorURI = "SharpSMS";

            wapPushMessage.ContentType = "text/vnd.wap.connectivity-xml";            
            wapPushMessage.Data = Encoding.UTF8.GetBytes(configXML);

            wapPushMessage.Security = Wsp.SecurityMethod.USERPIN;
            wapPushMessage.UserPin = "1234";

            return new SMSSubmit(wapPushMessage);
        }
    }
}
