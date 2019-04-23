using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SMSCarpool.Services;
using SMSCarpool.Presenters;

namespace SMSCarpool.Views
{
    public partial class FrmKonfigurasi : Form, IFrmKonfigurasi
    {

        DBConnectionService DBConnService;
        public FrmKonfigurasiPresenter Presenter { get; set; }
        private IFrmDevice frmDevice;

        public FrmKonfigurasi(IFrmDevice frmDeviceView)
        {
            frmDevice = frmDeviceView;
            InitializeComponent();
            this.Visible = false;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            string serverName = txtServerName.Text;
            string dbName = txtDatabase.Text;
            string userName = txtUserName.Text;
            string password = txtPassword.Text;

            string serverName2 = txtServerName2.Text;
            string dbName2 = txtDatabase2.Text;
            string userName2 = txtUserName2.Text;
            string password2 = txtPassword2.Text;

            var result1 = Presenter.initiateDBConnection(serverName, dbName, userName, password);
            var result2 = Presenter.initiateDBConnection2(serverName2, dbName2, userName2, password2);

            lblConnectionMessage.Visible = true;
            lblConnectionMessage2.Visible = true;

            if (result1)
            {
                lblConnectionMessage.Text = "Connection Successful!";
                lblConnectionMessage.ForeColor = Color.Green;
                
            } else
            {
                lblConnectionMessage.Text = "Connection Failed!";
                lblConnectionMessage.ForeColor = Color.Red;
            }

            if (result2)
            {
                lblConnectionMessage2.Text = "Connection Successful!";
                lblConnectionMessage2.ForeColor = Color.Green;
            }
            else
            {
                lblConnectionMessage2.Text = "Connection Failed!";
                lblConnectionMessage2.ForeColor = Color.Red;
            }
        }

        public void HideSelf()
        {
            this.Hide();
        }

        public void ShowSelf()
        {
            this.Show();
        }

        public DialogResult ShowDialogSelf()
        {
            return this.ShowDialog();
        }
        
        private void FrmKonfigurasi_Load(object sender, EventArgs e)
        {
            txtServerName.Text = Properties.Settings.Default.Server;
            txtDatabase.Text = Properties.Settings.Default.DBName;
            txtUserName.Text = Properties.Settings.Default.UserName;
            txtPassword.Text = Properties.Settings.Default.Password;

            txtServerName2.Text = Properties.Settings.Default.Server2;
            txtDatabase2.Text = Properties.Settings.Default.DBName2;
            txtUserName2.Text = Properties.Settings.Default.UserName2;
            txtPassword2.Text = Properties.Settings.Default.Password2;

            if (IsConnected())
            {
                Presenter.formClosing(frmDevice);
            }
        }

        public Boolean IsConnected()
        {
            string serverName = txtServerName.Text;
            string dbName = txtDatabase.Text;
            string userName = txtUserName.Text;
            string password = txtPassword.Text;

            string serverName2 = txtServerName2.Text;
            string dbName2 = txtDatabase2.Text;
            string userName2 = txtUserName2.Text;
            string password2 = txtPassword2.Text;

            if (Presenter.initiateDBConnection(serverName, dbName, userName, password) && Presenter.initiateDBConnection(serverName2, dbName2, userName2, password2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void FrmKonfigurasi_FormClosing(object sender, FormClosingEventArgs e)
        {
            Presenter.formClosing(frmDevice);

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Presenter.formClosing(frmDevice);
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void FrmKonfigurasi_FormClosed(object sender, FormClosedEventArgs e)
        {
            Presenter.formClosing(frmDevice);
        }
    }
}
