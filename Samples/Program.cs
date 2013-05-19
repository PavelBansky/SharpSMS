using SharpSMS;
using System;

namespace Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            Modem modem = new Modem("COM3", 115200);

            while (true)
            {
                PrintMenu();
                ConsoleKeyInfo key = Console.ReadKey(true);
                SMSSubmit sms = new SMSSubmit();

                switch (key.KeyChar)
                {
                    case '1':
                    sms = SampleMessage.PlainText();                    
                    break;
                    case '2':
                    sms = SampleMessage.FlashMessage();
                    break;
                    case '3':
                    sms = SampleMessage.ReplacebleMessage("This message will be replaced: FOO");
                    break;
                    case '4':
                    sms = SampleMessage.ReplacebleMessage("This is replacement message: BAR");
                    break;
                    case '5':
                    sms = SampleMessage.ActivateVoicemailIndication();
                    break;
                    case '6':
                    sms = SampleMessage.DeactivateVoicemailIndication();
                    break;
                    case '7':
                    sms = SampleMessage.ServiceIndicationMessage();
                    break;
                    case '8':
                    sms = SampleMessage.ServiceLoadingMessage();
                    break;
                    case '9':
                    sms = SampleMessage.WapPushConfiguration();
                    break;
                    case '0':
                    return;                    
                }

                Console.Write("\nEnter phone number: ");
                sms.PhoneNumber = Console.ReadLine();

                modem.SendSMS(sms);
            } 
        }

        static void PrintMenu()
        {
            Console.WriteLine("[1] Send \"Hello world\" message");
            Console.WriteLine("[2] Send flash message");
            Console.WriteLine("[3] Send replacable message");
            Console.WriteLine("[4] Send replacement message");
            Console.WriteLine("[5] Activate Voicemail indication");
            Console.WriteLine("[6] Deactivate Voicemail indication");
            Console.WriteLine("[7] Send SI wap push message");
            Console.WriteLine("[8] Send SL wap push message");
            Console.WriteLine("[9] Send Configuration wap push message");
            Console.WriteLine("[0] Exit");

        }
    }
}
