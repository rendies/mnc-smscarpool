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

            if(Presenter.initiateDBConnection(serverName, dbName, userName, password))
            {
                MessageBox.Show("Connection Successful!","Connection Status", MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
                this.Close();
            } else
            {
                MessageBox.Show("Connection Failed!", "Connection Status", MessageBoxButtons.OK, MessageBoxIcon.Error);
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


            if (IsConnected())
            {
                this.Close();
            }
        }

        public Boolean IsConnected()
        {
            string serverName = txtServerName.Text;
            string dbName = txtDatabase.Text;
            string userName = txtUserName.Text;
            string password = txtPassword.Text;

            if (Presenter.initiateDBConnection(serverName, dbName, userName, password))
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
            this.Close();
        }
    }
}
