using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMSCarpool.Models;

namespace SMSCarpool.Services
{
    public class MessageService
    {
        IDBConnectionService DBConnService;

        public MessageService()
        {
            DBConnService = new DBConnectionService(Properties.Settings.Default.ConnectionString);
        }

        public List<MessageModel> CheckMessage()
        {
            List<MessageModel> models = new List<MessageModel>();

            try
            {
                var data = DBConnService.Select("SELECT id, initial, jenis, prefix, nomor_hp, nama, isi_pesan, waktu FROM sms_pesan " +
                                                "WHERE status='0' ORDER BY id DESC LIMIT 1");

                foreach (var row in data)
                {
                    if (row != null)
                    {
                        MessageModel model = new MessageModel();

                        model.id = (int) row["id"];
                        model.initial = row["initial"].ToString();
                        model.jenis = row["jenis"].ToString();
                        model.prefix = row["prefix"].ToString();
                        model.nama = row["nama"].ToString();
                        model.pesan = row["isi_pesan"].ToString();
                        model.noTelp = row["nomor_hp"].ToString();
                        DateTime.TryParse(row["waktu"].ToString(), out model.waktu);
                        models.Add(model);
                    }
                    

                }

                DBConnService.CloseConnection();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return models;
            
        }

        public void MessageProcessed(int id)
        {
            try
            {
                DBConnService.Update("UPDATE sms_pesan SET status = '1' WHERE id = " + id);

            } catch (Exception ex)
            {
                Console.WriteLine("Failed to Update Message Status");
            }
        }

        public void MessageSent(MessageModel model)
        {
            try
            {
                DBConnService.Insert("INSERT INTO sms_pesan (initial, jenis, prefix, nomor_hp, nama, isi_pesan, waktu, status) VALUES"+
                                        " ('" + model.initial + "','" + model.jenis + "', '', '" + model.noTelp + "', '', '" + model.pesan + "', '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:s") + "', 'TERKIRIM')");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to Insert Message Sent Status");
            }
        }

        public void MessageFail(MessageModel model)
        {
            try
            {
                DBConnService.Insert("INSERT INTO sms_pesan (initial, jenis, prefix, nomor_hp, nama, isi_pesan, waktu, status) VALUES" +
                                        " ('" + model.initial + "','" + model.jenis + "', '', '" + model.noTelp + "', '', '" + model.pesan + "', '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:s") + "', 'GAGAL')");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to Insert Message Sent Status");
            }
        }
    }
}
