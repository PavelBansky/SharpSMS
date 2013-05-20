SharpSMS
========

SharpSMS is a library to create SMS messages in PDU format. It provides full control over the SMS including the UDH header, it supports WAP Push messages, Service Indication and Service Location. Messages can be signed for authentication with IMSI or user PIN.


Creating simple text message
----------------------------

Following code will create plain text message with content *Hello World from SharpSMS!* to be delivered to phone number *+1 (234) 456 7890*

	TextMessage textMessage = new TextMessage();
	textMessage.Text = "Hello World from SharpSMS!";
	
	SMSSubmit sms = new SMSSubmit();
	sms.PhoneNumber = "+1234567890";
	sms.MessageToSend = textMessage;  	

	List<byte[]> messageList = sms.GetPDUList();

Now, the **messageList** contains PDU formatted message(s) that are ready to be send over GSM modem or internet gateway. 


Tweaking SMS header
-------------------

SMS message have several header fields defined according to GSM 03.40 specification. These fields are exposed as properties of **SMSSubmit** class:
       
    Indication 
	MessageReference
	PhoneNumber 
    ProtocolIdentifier
    RequestDeliveryConfirmation
    ValidityPeriod       
    
### Flash Message

Flash message refers to message that is displayed on the phone screen but not stored. This type of message is usually sent by GSM operator to display prepaid balance or confirm message delivery. To create a flash message **MessageClass** must be set. Following code demonstrates how to create flash message:

    TextMessage textMessage = new TextMessage();
    textMessage.DataEncoding = DataEncoding.Default7bit;
    textMessage.Text = "Flash message from SharpSMS!";

    SMSSubmit sms = new SMSSubmit();
	sms.PhoneNumber = "+1234567890";
    sms.MessageToSend = textMessage;	
    sms.Indication.Class = MessageClass.ImmediateDisplay;


### Voicemail Indication

GSM 03.40 specification defines several *Indication types*, for example voicemail, email etc. For each indication can be defined *Operation*, which determines how the indication will be treated when received by the phone. Following code creates SMS message that will make the **voicemail** icon appear on the phone:

    TextMessage textMessage = new TextMessage();
    textMessage.Text = "You have a voicemail.";

    SMSSubmit sms = new SMSSubmit();
	sms.PhoneNumber = "+1234567890";
    sms.MessageToSend = textMessage;

    sms.Indication.Operation = MessageIndicationOperation.Discard;
    sms.Indication.Type = IndicationType.Voicemail;
    sms.Indication.IsActive = true;

Following code will remove the **voicemail** icon from the phone status bar:

    TextMessage textMessage = new TextMessage();

    SMSSubmit sms = new SMSSubmit();
	sms.PhoneNumber = "+1234567890";
    sms.MessageToSend = textMessage;

    sms.Indication.Operation = MessageIndicationOperation.Discard;
    sms.Indication.Type = IndicationType.Voicemail;
    sms.Indication.IsActive = false;


User Data Header and Wap Push
-----------------------------

SMS message can contain user data header, which extends possibilities to address specific functions of the phone; including ring tones, logos or configurations delivered using SMS message. This functionality is called Over The Air Provisioning (OTA). Modern smartphones like Android, Windows Phone or iPhone do not support this type of messages anymore. 
However, some Android devices are equipped with push router, that can process OTA provisioning messages.
Following code will create Wap Push message with configuration for Windows Mobile. Message will be signed with user PIN, so user has to enter the PIN in order for message to be processed:

    string configXML = @"<wap-provisioningdoc><characteristic type=""BrowserFavorite""><characteristic type=""SharpSMS""><parm name=""URL"" value=""https://github.com/pavel-b/SharpSMS""/></characteristic></characteristic></wap-provisioningdoc>";

    WapPushMessage wapPushMessage = new WapPushMessage();
    wapPushMessage.XWapInitiatorURI = "SharpSMS";

    wapPushMessage.ContentType = "text/vnd.wap.connectivity-xml";
    wapPushMessage.Data = Encoding.UTF8.GetBytes(configXML);

    wapPushMessage.Security = Wsp.SecurityMethod.USERPIN;
    wapPushMessage.UserPin = "1234";

    SMSSubmit sms = new SMSSubmit();
	sms.PhoneNumber = "+1234567890";
    sms.MessageToSend = wapPushMessage


Sending the message
-------------------
		
PDU formatted message(s) obtained using **SMSSubmit.GetPDUList()** method can be send via GSM modem using **AT+CMGS** command. This is demonstrated in sample codes. Internet gateways can be used as an alternative to modem.
