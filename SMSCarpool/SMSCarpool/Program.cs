using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SMSCarpool.Presenters;
using SMSCarpool.Views;

namespace SMSCarpool
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            FrmDevice frmDevice = new FrmDevice();
            FrmDevicePresenter frmDevicePresenter = new FrmDevicePresenter(frmDevice);

            Application.Run(frmDevice);
        }
    }
}
