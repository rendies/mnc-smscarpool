using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SMSCarpool.Presenters;

namespace SMSCarpool.Views
{
    public partial class FrmDevice : Form, IFrmDevice
    {
        public IFrmKonfigurasi frmKonfigurasi { get; set; }
        public FrmDevicePresenter Presenter { get; set; }
        public bool mShowAllowed;

        public FrmDevice()
        {
            InitializeComponent();
            Initiate();
        }

        private void Initiate()
        {

            
        }

        
        /*protected override void SetVisibleCore(bool value)
        {
            if (!mShowAllowed) value = false;
            base.SetVisibleCore(value);
        }*/


        public void HideSelf()
        {
            Hide();
        }

        public void ShowSelf()
        {
            Show();
        }
        
        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox18_TextChanged(object sender, EventArgs e)
        {

        }

        private void FrmDevice_Load(object sender, EventArgs e)
        {

            
        }
    }
}
