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
        IDBConnectionService DBConnService2;

        public MessageService()
        {
            DBConnService = new DBConnectionService(Properties.Settings.Default.ConnectionString);
            DBConnService2 = new DBConnectionService(Properties.Settings.Default.ConnectionString2);
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

        public List<MessageModel> CheckMessage2()
        {
            List<MessageModel> models = new List<MessageModel>();

            try
            {
                var data = DBConnService2.Select("SELECT id, message, phone_number, process_at, type, `status` FROM messages " +
                                                "WHERE status='0' ORDER BY id DESC LIMIT 1");

                foreach (var row in data)
                {
                    if (row != null)
                    {
                        MessageModel model = new MessageModel();

                        model.id = (int)row["id"];
                        model.initial = "";
                        model.jenis = row["type"].ToString();
                        model.prefix = "";
                        model.nama = "";
                        model.pesan = row["message"].ToString();
                        model.noTelp = row["phone_number"].ToString();
                        DateTime.TryParse(row["process_at"].ToString(), out model.waktu);
                        models.Add(model);
                    }


                }

                DBConnService2.CloseConnection();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return models;

        }

        public string GetWAPServerURL()
        {
            string result = "";

            try
            {
                var data = DBConnService2.Select("SELECT wap_push_url FROM `sms_modem`");
                foreach (var row in data)
                {
                    if (row != null)
                    {
                        result = row["wap_push_url"].ToString();
                        
                    }


                }

                DBConnService2.CloseConnection();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return result;

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

        public void MessageProcessed2(int id)
        {
            try
            {
                DBConnService2.Update("UPDATE messages SET status = '1', updated_at = '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "', updated_by='Carpool System' WHERE id = " + id);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to Update Message Status");
            }
        }

        public void MessageSent2(int id)
        {
            try
            {
                DBConnService2.Update("UPDATE messages SET status = '1', updated_at = '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "', updated_by='Carpool System' WHERE id = " + id);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to Update Message Status");
            }
        }

        public void MessageFail2(int id)
        {
            try
            {
                DBConnService2.Update("UPDATE messages SET status = '2', updated_at = '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "', updated_by='Carpool System' WHERE id = " + id);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to Update Message Status");
            }
        }
    }
}
