using SharpSMS;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;

namespace Samples
{
    class Modem
    {
        public Modem(string serialPortName, int baudRate)
        {
            commPort = new SerialPort(serialPortName, baudRate);
        }

        /// <summary>
        /// Send given message to the serial port and initialize modem
        /// </summary>
        /// <param name="sms"></param>
        public void SendSMS(SMSSubmit sms)
        {
            List<byte[]> messageList = sms.GetPDUList();

            if (!InitModem())
                Console.WriteLine("Error: No response from modem.");

            foreach (byte[] messagePart in messageList)
            {
                SendMessageToModem(messagePart);
                Thread.Sleep(500);
                Console.WriteLine("Message sent to modem.");
            }
        }

        /// <summary>
        /// Initialize modem
        /// </summary>
        /// <returns>True if modem was successfuly initialized</returns>
        bool InitModem()
        {
            try
            {
                if (!commPort.IsOpen)
                {
                    WriteModemLog("Initializing modem....\r\n");                    
                    //commPort.Handshake = Handshake.RequestToSend;
                    commPort.WriteBufferSize = 2048;
                    commPort.ReadTimeout = 5000;
                    commPort.ReadTimeout = 5000;
                    commPort.Open();
                    commPort.NewLine = "\r";

                    string dataRead = string.Empty;
                    int initLoop = 0;
                    while (initLoop < 15)
                    {
                        commPort.WriteLine("AT");
                        commPort.WriteLine("AT");
                        commPort.WriteLine("AT");
                        dataRead = ReadFromPort(commPort, 1);

                        if (dataRead.Contains("OK"))
                            break;
                        else
                            initLoop++;
                    }
                    commPort.WriteLine("ATZ");
                    dataRead = ReadFromPort(commPort, 1);

                    if (dataRead.Contains("OK"))
                        return true;
                    else
                        return false;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Send message bytes to serial port modem
        /// </summary>
        /// <param name="messageBytes"></param>
        /// <returns></returns>
        bool SendMessageToModem(byte[] messageBytes)
        {
            bool retValue;
            if (commPort != null && commPort.IsOpen && messageBytes.Length > 0)
            {
                int messageLength = messageBytes.Length - 1;
                string message = BytesToHexString(messageBytes);
                string dataRead = string.Empty;
                commPort.WriteLine(string.Format("AT+CMGS={0}", messageLength));
                Thread.Sleep(250);

                dataRead = ReadFromPort(commPort, 1);

                if (dataRead.Contains(">"))
                {
                    commPort.Write(message);
                    byte[] escChar = new byte[1] { 26 };
                    commPort.Write(escChar, 0, escChar.Length);
                    Thread.Sleep(250);

                    dataRead = "0000";
                    while (!(dataRead.Contains("OK") || dataRead.Contains("ERROR") || dataRead == string.Empty))
                        dataRead = ReadFromPort(commPort, 5);

                    retValue = true;
                }
                else
                    retValue = false;
            }
            else
                retValue = false;

            return retValue;
        }

        /// <summary>
        /// Reads string from given serial port
        /// </summary>
        /// <param name="port">SerialPort to read</param>
        /// <param name="secondsTimeOut">Time out for reading operations in seconds</param>
        /// <returns></returns>
        string ReadFromPort(SerialPort port, int secondsTimeOut)
        {
            int timeOut = 0;
            string dataRead = string.Empty;

            while (timeOut < secondsTimeOut * 10)
            {
                if (port.BytesToRead > 0)
                {
                    dataRead = port.ReadExisting();
                    break;
                }
                timeOut++;
                Thread.Sleep(100);
            }

            WriteModemLog(dataRead);
            return dataRead;
        }

        /// <summary>
        /// Converts array of byte into string of hexa values
        /// </summary>
        /// <param name="srcArray"></param>
        /// <returns></returns>
        string BytesToHexString(byte[] srcArray)
        {
            string retString = string.Empty;
            foreach (byte b in srcArray)
                retString += b.ToString("X2");

            return retString;
        }

        /// <summary>
        /// Writes informations into modem log
        /// </summary>
        /// <param name="message"></param>
        private void WriteModemLog(string message)
        {
            Console.WriteLine("Modem: "+message);            
        }

        SerialPort commPort;
    }
}
