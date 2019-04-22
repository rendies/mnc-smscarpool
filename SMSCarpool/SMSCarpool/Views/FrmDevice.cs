using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SMSCarpool.Presenters;
using GsmComm.GsmCommunication;
using SMSCarpool.Models;
using SMSCarpool.Services;
using GsmComm.PduConverter;
using System.Threading;

namespace SMSCarpool.Views
{
    public partial class FrmDevice : Form, IFrmDevice
    {

        public IFrmKonfigurasi frmKonfigurasi { get; set; }
        public FrmDevicePresenter Presenter { get; set; }
        
        ModemService modemService = ModemService.Instance;
        public bool mShowAllowed;
        Thread _thread;
        DeviceModel deviceModel = new DeviceModel();



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

        private void SetModemConfig(DeviceModel model)
        {
            cboMode.Text = model.mode;
            cboProtocol.Text = model.protocol;
            cboCommPort.Text = (model.comm_port.ToString());
            cboBitRate.Text = (model.bit_rate.ToString());
            txtSendTimeout.Text = model.send_timeout.ToString();
            txtSendInterval.Text = model.send_interval.ToString();
            txtRetryTimes.Text = model.retry_times.ToString();
            txtSMSValidity.Text = model.sms_validity.ToString();
            cboFolder.Items.Add(model.folder);
            if (model.auto_reject_incoming_call == 1)
            {
                chkAutoReject.Checked = true;
            }
            else
            {
                chkAutoReject.Checked = false;
            }

            if (model.send_reject_incoming_call == 1)
            {
                chkSendReject.Checked = true;
            }
            else
            {
                chkSendReject.Checked = false;
            }

            txtIsiPesan.Text = model.message_reject_incomming_call;
        }

        private void FrmDevice_Load(object sender, EventArgs e)
        {
            btnDisconnect.BackColor = Color.Gray;
            btnDisconnect.Enabled = false;

            toolStripStatusLabel4.Text = DateTime.Now.ToString("dd/MM/yyyy");
            toolStripStatusLabel5.Text = DateTime.Now.ToShortTimeString();
            

            deviceModel = Presenter.GetModemConfig(8, "-");
            SetModemConfig(deviceModel);
            EnableButton(false);

        }

        private void EnableButton(bool state)
        {
            button5.Enabled = state;
            button6.Enabled = state;
            button7.Enabled = state;
            button8.Enabled = state;
            button9.Enabled = state;
            button10.Enabled = state;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(cboCommPort.Text != "" && cboBitRate.Text != "" && txtSendTimeout.Text != "")
            {

                try
                {
                    modemService.Initiate(int.Parse(cboCommPort.Text), int.Parse(cboBitRate.Text), int.Parse(txtSendTimeout.Text));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                timer2.Interval = string.IsNullOrWhiteSpace(txtSendInterval.Text)? 60000: int.Parse(txtSendInterval.Text) * 60000;
                timer2.Enabled = true;

                btnDisconnect.BackColor = Color.IndianRed;
                btnDisconnect.Enabled = true;
                btnConnect.Enabled = false;
                btnConnect.BackColor = Color.Gray;
                EnableButton(true);
            } else
            {
                MessageBox.Show("Parameter Invalid!");
            }
           
        }

        // Refresh Modem Info
        private void button5_Click(object sender, EventArgs e)
        {
            
            bool result = false;
            try
            {
                if (modemService.IsModemConnected())
                {
                    var device = modemService.comm.IdentifyDevice();

                    TxtManufacture.Text = device.Manufacturer;
                    TxtModel.Text = device.Model;
                    TxtIMEI.Text = device.SerialNumber;
                    TxtBattery.Text = modemService.comm.GetBatteryCharge().BatteryChargingStatus.ToString();
                    TxtSignal.Text = modemService.comm.GetSignalQuality().SignalStrength.ToString();
                    TxtNetwork.Text = modemService.comm.GetCurrentOperator().TheOperator;
                    TxtFirmware.Text = device.Revision;
                    TxtHardware.Text = modemService.comm.IsConnected().ToString();
                    //TxtNomorHP.Text = modemService.comm.;
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
           
            
        }

        private void button10_Click(object sender, EventArgs e)
        {
            
            bool result = false;
            if (modemService.IsModemConnected())
            {
                result = modemService.SendSMS(txtNomorHPManual.Text, txtPesanManual.Text);

            }

            if(result)
            {
                MessageBox.Show("SMS Sent!");
            } else
            {
                MessageBox.Show("Failed to send sms!");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel4.Text = DateTime.Now.ToString("dd/MM/yyyy");
            toolStripStatusLabel5.Text = DateTime.Now.ToShortTimeString();
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            
            bool result = false;
            if (modemService.IsModemConnected())
            {
                result = modemService.SendSMS(txtNomorHPManual.Text, txtPesanManual.Text);

            }

            if (result)
            {
                MessageBox.Show("SMS Sent!");
            }
            else
            {
                MessageBox.Show("Failed to send sms!");
            }
        }

        private void tabMemoryPesan_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            
            bool result = false;
            

            string storage = string.Empty;
            
            var comm = modemService.comm;

            try
            {
                if (modemService.IsModemConnected())
                {
                    storage = PhoneStorageType.Sim;
                    //storage = PhoneStorageType.Phone;
                    // Read all SMS messages from the storage
                    DecodedShortMessage[] messages = comm.ReadMessages(PhoneMessageStatus.All, storage);
                    var i = 1;
                    foreach (DecodedShortMessage message in messages)
                    {
                        //Output(string.Format("Message status = {0}, Location = {1}/{2}",
                        //    StatusToString(message.Status), message.Storage, message.Index));
                        //ShowMessage(message.Data);

                        var pdu = message.Data;


                        SmsDeliverPdu data = (SmsDeliverPdu)pdu;
                        DGMessage.Rows.Add(new String[] { i.ToString(),
                                        StatusToString(message.Status),
                                        message.Storage + "/" + message.Index,
                                        data.SCTimestamp.ToString(),
                                        data.OriginatingAddress,
                                        data.MessageType.ToString(),
                                        message.Data.UserDataText });
                        i++;
                        //Output("");
                    }
                    //Output(string.Format("{0,9} messages read.", messages.Length.ToString()));
                    //Output("");
                }

            }
            catch (Exception ex)
            {
                //ShowException(ex);
            }
        }

        private string StatusToString(PhoneMessageStatus status)
        {
            // Map a message status to a string
            string ret;
            switch (status)
            {
                case PhoneMessageStatus.All:
                    ret = "All";
                    break;
                case PhoneMessageStatus.ReceivedRead:
                    ret = "Read";
                    break;
                case PhoneMessageStatus.ReceivedUnread:
                    ret = "Unread";
                    break;
                case PhoneMessageStatus.StoredSent:
                    ret = "Sent";
                    break;
                case PhoneMessageStatus.StoredUnsent:
                    ret = "Unsent";
                    break;
                default:
                    ret = "Unknown (" + status.ToString() + ")";
                    break;
            }
            return ret;
        }

        private void timer3_Tick(object sender, EventArgs e)
        {

        }
        


        private void timer2_Tick(object sender, EventArgs e)
        {
            runThread();
            
        }

        public void runThread()
        {
            var thread = _thread;
            //Prevent optimization from not using the local variable
            Thread.MemoryBarrier();
            if
            (
                thread == null ||
                thread.ThreadState == System.Threading.ThreadState.Stopped
            )
            {
                var newThread = new Thread(Presenter.SendMessage);
                newThread.IsBackground = true;
                newThread.Name = "SendMessageThread";
                newThread.Start();
                //Prevent optimization from setting the field before calling Start
                Thread.MemoryBarrier();
                _thread = newThread;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("SendMessageThread already Running.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (
                _thread != null
            )
            {
                _thread.Abort();
                btnConnect.Enabled = true;
                btnDisconnect.Enabled = false;
                btnDisconnect.BackColor = Color.Gray;
                btnConnect.BackColor = Color.DeepSkyBlue;
                EnableButton(false);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmDevice_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //frmKonfigurasi = new FrmKonfigurasi(this);

            Presenter.formClosing(frmKonfigurasi);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure want to delete all messages?","Delete all messages",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
            {
                modemService.DeleteSMS(DeleteScope.All, PhoneStorageType.Sim);
                DGMessage.Rows.Clear();
                DGMessage.Refresh();
            }
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (Presenter.UpdateModemConfig(deviceModel))
            {
                MessageBox.Show("Modem config updated successfully!");
            } else
            {
                MessageBox.Show("Failed to update Modem config!");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void DGMessage_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
