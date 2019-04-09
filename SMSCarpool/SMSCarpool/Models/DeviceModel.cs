using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMSCarpool.Models
{
    public class DeviceModel
    {
        public int id;
        public string initial;
        public string mode;
        public string protocol;
        public int comm_port;
        public int bit_rate;
        public int send_timeout;
        public int send_interval;
        public int retry_times;
        public string sms_validity;
        public string folder;
        public int auto_reject_incoming_call;
        public int send_reject_incoming_call;
        public string message_reject_incomming_call;
        public int request_send_report;
        public int auto_delete_new_sms;
        public int auto_delete_all_report;
        public int is_proses_new_sms;
        public int is_proses_schedule;
        public int is_no_prefix;
        public string pesan_no_prefix;
        public string sms_type;
        public string wap_push_url;
        public string nomor_cek_pulsa;
        public string nomor_hp_ini;
        public string keterangan;
        public int status;
        public int is_on;
        public int waktu_cek_schedule;
        public int waktu_cek_pesan_pending;
        public int waktu_cek_koneksi;
        public int cek_pesan_masuk;
    }
}
