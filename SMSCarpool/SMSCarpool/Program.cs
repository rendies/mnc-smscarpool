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
                        
            Application.Run(new HiddenContext());

        }
    }

    class HiddenContext : ApplicationContext
    {
        public HiddenContext()
        {
            FrmDevice frmDevice = new FrmDevice();

            frmDevice.Presenter = new FrmDevicePresenter(frmDevice);
            frmDevice.frmKonfigurasi = new FrmKonfigurasi(frmDevice);
            frmDevice.frmKonfigurasi.Presenter = new FrmKonfigurasiPresenter(frmDevice.frmKonfigurasi);
            if (frmDevice.frmKonfigurasi.Presenter.chekcDBConnection())
            {
                //frmDevice.mShowAllowed = false;
                frmDevice.Visible = false;
                frmDevice.frmKonfigurasi.ShowSelf();

            } else
            {
                frmDevice.Visible = false;
                frmDevice.frmKonfigurasi.ShowSelf();

            }

            frmDevice.FormClosed += new System.Windows.Forms.FormClosedEventHandler(form1_FormClosed);
            frmDevice.FormClosing += new FormClosingEventHandler(form1_FormClosing);


        }

        void form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to close the application?", "Application Closing", MessageBoxButtons.YesNo) == DialogResult.No)
                e.Cancel = true;
        }

        void form1_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            Environment.Exit(0);
            this.ExitThread();

        }
    }
}
