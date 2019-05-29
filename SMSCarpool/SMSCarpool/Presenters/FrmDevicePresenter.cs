using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMSCarpool.Views;
using SMSCarpool.Services;
using SMSCarpool.Models;
using GsmComm.GsmCommunication;
using GsmComm.Interfaces;
using GsmComm.Server;

namespace SMSCarpool.Presenters
{
    public class FrmDevicePresenter
    {
        IFrmDevice frmDevice;
        IDBConnectionService DBConnService;
        DeviceModel deviceModel;
        ModemService modemService = ModemService.Instance;
        MessageService messageService;
        public bool isSendMessageThreadRunning = false;

        public FrmDevicePresenter(IFrmDevice FrmDeviceView)
        {
            frmDevice = FrmDeviceView;

            deviceModel = new DeviceModel();
            deviceModel.id = 8;
            deviceModel.initial = "-";
            messageService = new MessageService();
            DBConnService = new DBConnectionService(Properties.Settings.Default.ConnectionString);

        }

        public void chekcDBConnection()
        {
            if (frmDevice.frmKonfigurasi.Presenter.chekcDBConnection())
            {
                //frmDevice.ShowSelf();
                //frmDevice.frmKonfigurasi.HideSelf();
            } else
            {
                //frmDevice.HideSelf();
                //frmDevice.frmKonfigurasi.ShowSelf();
            }
        }

        public void formClosing(IFrmKonfigurasi frmKonfigurasi)
        {
            frmKonfigurasi.ShowDialogSelf();
            //frmDevice.HideSelf();
            
        }

        public void SendMessage()
        {
            
            List<MessageModel> datas = messageService.CheckMessage();
            List<MessageModel> datas2 = messageService.CheckMessage2();
            foreach (MessageModel data in datas)
            {
                if (modemService.SendSMS(data.noTelp, data.pesan))
                {
                    messageService.MessageFail(data);
                }
                else
                {
                    messageService.MessageSent(data);
                }

                messageService.MessageProcessed(data.id);
                
            }

            foreach (MessageModel data in datas2)
            {
                if (modemService.SendSMS(data.noTelp, data.pesan))
                {
                    messageService.MessageFail2(data.id);
                }
                else
                {
                    messageService.MessageSent2(data.id);
                }

                messageService.MessageProcessed2(data.id);
                
            }
        }

        public void SendMessageWAP()
        {

            List<MessageModel> datas = messageService.CheckMessage();
            List<MessageModel> datas2 = messageService.CheckMessage2();
            String url = messageService.GetWAPServerURL();
            foreach (MessageModel data in datas)
            {
                if (modemService.SendSMS(data.noTelp, data.pesan, url))
                {
                    messageService.MessageFail(data);
                }
                else
                {
                    messageService.MessageSent(data);
                }

                messageService.MessageProcessed(data.id);

            }

            foreach (MessageModel data in datas2)
            {
                if (modemService.SendSMS(data.noTelp, data.pesan))
                {
                    messageService.MessageFail2(data.id);
                }
                else
                {
                    messageService.MessageSent2(data.id);
                }

                messageService.MessageProcessed2(data.id);

            }
        }

        public void ModemConnect(int id, string initial)
        {
            DeviceModel model = GetModemConfig(id, initial);

            if (modemService.ModemConnect(model.comm_port, model.bit_rate, model.send_timeout))
            {
                UpdateModemActive(model.id, 1, 1);
            } 
        }

        public bool UpdateModemActive(int modem_id, int status, int is_on)
        {

            try
            {
                return DBConnService.Update("UPDATE sms_modem SET status=" + status + ", is_on = " + is_on + " WHERE id=" + modem_id);
               
            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return false;
        }
        
        public DeviceModel GetModemConfig(int id, string initial)
        {        
            try
            {
                var data = DBConnService.Select("SELECT id, initial, mode, protocol, comm_port, bit_rate, send_timeout, send_interval, retry_times, " +
                                "sms_validity, folder, auto_reject_incoming_call, send_reject_incoming_call, message_reject_incoming_call, " +
                                "request_send_report, auto_delete_new_sms, auto_delete_all_report, is_proses_schedule, is_proses_new_sms, " +
                                "is_no_prefix, pesan_no_prefix, sms_type, wap_push_url, nomor_cek_pulsa, keterangan, status, waktu_cek_schedule, waktu_cek_pesan_pending, waktu_cek_koneksi, cek_pesan_masuk  From sms_modem " +
                                "WHERE id = " + id + " AND initial = '" + initial +  "'");

                foreach(var row in data)
                {
                    if (row != null)
                    {
                        deviceModel.id = (int)row["id"];
                        deviceModel.initial = row["initial"].ToString();
                        deviceModel.mode = row["mode"].ToString();
                        deviceModel.protocol = row["protocol"].ToString();
                        deviceModel.comm_port = (int)row["comm_port"];
                        deviceModel.bit_rate = (int)row["bit_rate"];
                        deviceModel.send_timeout = (int)row["send_timeout"];
                        deviceModel.send_interval = (int)row["send_interval"];
                        deviceModel.retry_times = (int)row["retry_times"];
                        deviceModel.sms_validity = row["sms_validity"].ToString();
                        deviceModel.folder = row["folder"].ToString();
                        deviceModel.auto_reject_incoming_call = bool.Parse(row["auto_reject_incoming_call"].ToString()) ? 1 : 0;
                        deviceModel.send_reject_incoming_call = bool.Parse(row["send_reject_incoming_call"].ToString()) ? 1 : 0;
                        deviceModel.message_reject_incomming_call = row["message_reject_incoming_call"].ToString();
                        deviceModel.request_send_report = bool.Parse(row["request_send_report"].ToString()) ? 1 : 0;
                        deviceModel.auto_delete_new_sms = bool.Parse(row["auto_delete_new_sms"].ToString()) ? 1 : 0;
                        deviceModel.auto_delete_all_report = bool.Parse(row["auto_delete_all_report"].ToString()) ? 1 : 0;
                        deviceModel.is_proses_schedule = bool.Parse(row["is_proses_schedule"].ToString()) ? 1 : 0;
                        deviceModel.is_proses_new_sms = bool.Parse(row["is_proses_new_sms"].ToString()) ? 1 : 0;
                        deviceModel.is_no_prefix = bool.Parse(row["is_no_prefix"].ToString()) ? 1 : 0;
                        deviceModel.pesan_no_prefix = row["pesan_no_prefix"].ToString();
                        deviceModel.sms_type = row["sms_type"].ToString();
                        deviceModel.wap_push_url = row["wap_push_url"].ToString();
                        deviceModel.nomor_cek_pulsa = row["nomor_cek_pulsa"].ToString();
                        deviceModel.keterangan = row["keterangan"].ToString();
                        deviceModel.status = bool.Parse(row["status"].ToString()) ? 1 : 0;
                        deviceModel.waktu_cek_schedule = (int)row["waktu_cek_schedule"];
                        deviceModel.waktu_cek_pesan_pending = (int)row["waktu_cek_pesan_pending"];
                        deviceModel.waktu_cek_koneksi = (int)row["waktu_cek_koneksi"];
                        deviceModel.cek_pesan_masuk = bool.Parse(row["cek_pesan_masuk"].ToString()) ? 1 : 0;
                    }
                    
                    
                }

                DBConnService.CloseConnection();
                

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return deviceModel;
        }

        public bool UpdateModemConfig(DeviceModel model)
        {
            var result = false;
            try
            {
                DBConnService.Update("UPDATE `sms_modem` " +
                                        " SET " +
                                            " `initial` = '" + model.initial + "', " +
                                            " `mode` = '" + model.mode + "', " +
                                            " `protocol` = '" + model.protocol + "', " +
                                            " `comm_port` = '" + model.comm_port + "', " +
                                            " `bit_rate` = '" + model.bit_rate + "', " +
                                            "  `send_timeout` = '" + model.send_timeout + "', " +
                                            "  `send_interval` = '" + model.send_interval + "', " +
                                            "  `retry_times` = '" + model.retry_times + "', " +
                                            "  `sms_validity` = '" + model.sms_validity + "', " +
                                            "  `folder` = '" + model.folder + "', " +
                                            "  `auto_reject_incoming_call` = '" + model.auto_reject_incoming_call + "', " +
                                            "  `send_reject_incoming_call` = '" + model.send_reject_incoming_call + "', " +
                                            "  `message_reject_incoming_call` = '" + model.message_reject_incomming_call + "', " +
                                            "  `request_send_report` = '" + model.request_send_report + "', " +
                                            "  `auto_delete_new_sms` = '" + model.auto_delete_new_sms + "', " +
                                            "  `auto_delete_all_report` = '" + model.auto_delete_all_report + "', " +
                                            "  `is_proses_schedule` = '" + model.is_proses_schedule + "', " +
                                            "  `is_proses_new_sms` = '" + model.is_proses_new_sms + "', " +
                                            "  `is_no_prefix` = '" + model.is_no_prefix + "', " +
                                            "  `pesan_no_prefix` = '" + model.pesan_no_prefix + "', " +
                                            "  `sms_type` = '" + model.sms_type + "', " +
                                            "  `wap_push_url` = '" + model.wap_push_url + "', " +
                                            "  `nomor_cek_pulsa` = '" + model.nomor_cek_pulsa + "', " +
                                            "  `nomor_hp_ini` = '" + model.nomor_hp_ini + "', " +
                                            "  `keterangan` = '" + model.keterangan + "', " +
                                            "  `status` = '" + model.status + "', " +
                                            "  `is_on` = '" + model.is_on + "', " +
                                            "  `waktu_cek_schedule` = '" + model.waktu_cek_schedule + "', " +
                                            "  `waktu_cek_pesan_pending` = '" + model.waktu_cek_pesan_pending + "', " +
                                            "  `waktu_cek_koneksi` = '" + model.waktu_cek_koneksi + "', " +
                                            "  `cek_pesan_masuk` = '" + model.cek_pesan_masuk + "' " +
                                            " WHERE (`id` = '" + model.id + "'); ");

                result = true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return result;
        }

    }
}
