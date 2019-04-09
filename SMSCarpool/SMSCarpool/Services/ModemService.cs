using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GsmComm.PduConverter;
using GsmComm.PduConverter.SmartMessaging;
using GsmComm.GsmCommunication;
using GsmComm.Interfaces;
using GsmComm.Server;

namespace SMSCarpool.Services
{
    public sealed class ModemService
    {
        
        private int Comm_Port = 0;
        private int Comm_BaudRate = 0;
        private int Comm_TimeOut = 0;
        public GsmCommMain comm;

        private static ModemService instance = null;

        private ModemService()
        {

        }

        public static ModemService Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new ModemService();
                }
                return instance;
            }
        }

        public void Initiate(int commPort, int commBaudRate, int commTimeOut)
        {
            Comm_Port = commPort;
            Comm_BaudRate = commBaudRate;
            Comm_TimeOut = commTimeOut;

            comm = new GsmCommMain(Comm_Port, Comm_BaudRate, Comm_TimeOut);

            try
            {
                comm.Open();
            } catch (Exception ex)
            {
                throw new CommException("Device Not Found");
            }
        }

        public ModemService(int commPort, int commBaudRate, int commTimeOut)
        {
            Comm_Port = commPort;
            Comm_BaudRate = commBaudRate;
            Comm_TimeOut = commTimeOut;

            comm = new GsmCommMain(Comm_Port, Comm_BaudRate, Comm_TimeOut);
        }

        public Boolean IsModemConnected()
        {
            try
            {
                
                if (comm.IsConnected()) return true;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return false;
        }

        public Boolean ModemDisconnect(int commPort, int commBaudRate, int commTimeOut)
        {
            Comm_Port = commPort;
            Comm_BaudRate = commBaudRate;
            Comm_TimeOut = commTimeOut;

            comm = new GsmCommMain(Comm_Port, Comm_BaudRate, Comm_TimeOut);

            try
            {
                comm.Close();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return false;
        }

        public Boolean ModemConnect(int commPort, int commBaudRate, int commTimeOut)
        {
            Comm_Port = commPort;
            Comm_BaudRate = commBaudRate;
            Comm_TimeOut = commTimeOut;

            comm = new GsmCommMain(Comm_Port, Comm_BaudRate, Comm_TimeOut);
            
            try
            {
                comm.Open();
                if (comm.IsConnected())
                {
                    

                    return true;

                }

            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return false;
        }

        //
        // Summary:
        //     Send message directly from connected modem
        //
        // Parameters:
        //   noHP:
        //     Destination Phone Number
        //
        //   pesan:
        //     Message content.
        public Boolean SendSMS(string noHP, string pesan)
        {
            bool result = false;

            try
            {
                
            SmsSubmitPdu pdu = new SmsSubmitPdu(pesan, noHP);

            comm.SendMessage(pdu);

                return true;
            } catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                
            }

            return result;
        }

        //
        // Summary:
        //     Send message via wap servers
        //
        // Parameters:
        //   noHP:
        //     Destination Phone Number
        //
        //   pesan:
        //     Message content.
        //
        //   url:
        //      Server URL address
        public Boolean SendSMS(string noHP, string pesan, string url)
        {
            bool result = false;

            try
            {

                SmsSubmitPdu pdu = new SmsSubmitPdu(pesan, noHP);

                comm.SendMessage(pdu);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

            }

            return result;
        }

    }
}
