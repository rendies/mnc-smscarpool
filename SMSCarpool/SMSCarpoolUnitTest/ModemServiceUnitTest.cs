using Microsoft.VisualStudio.TestTools.UnitTesting;
using SMSCarpool.Services;
using System;
using GsmComm.GsmCommunication;
using System.IO;


namespace SMSCarpoolUnitTest
{
    [TestClass]
    public class ModemServiceUnitTest
    {
        [TestMethod]
        public void ModemConnectTest()
        {
            ModemService modemService = ModemService.Instance;
            //var result = modemService.ModemConnect(3,115200,100);
            var result = new GsmCommMain("COM3");
            Assert.AreEqual(true, result);

        }

        [TestMethod]
        public void SendSMSTest()
        {
            ModemService modemService = new ModemService(3, 115200, 100);
            bool result = false;
            if (modemService.IsModemConnected())
            {
                result = modemService.SendSMS("08978235095", "test sms from modem");

            }


            Assert.AreEqual(true, result);

        }
    }
}
