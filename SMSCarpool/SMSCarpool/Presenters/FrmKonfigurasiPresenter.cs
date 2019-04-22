using SMSCarpool.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMSCarpool.Views;

namespace SMSCarpool.Presenters
{
    public class FrmKonfigurasiPresenter
    {
        IFrmKonfigurasi frmKonfigurasi;
        FrmKonfigurasiPresenter presenter;
        IDBConnectionService DBConnService;
        IDBConnectionService DBConnService2;

        public FrmKonfigurasiPresenter(IFrmKonfigurasi frmKonfigurasiView)
        {
            frmKonfigurasi = frmKonfigurasiView;
            presenter = this;
            Initate();
        }

        public void Initate()
        {

        }

        public bool initiateDBConnection(string serverName, string dbName, string userName, string password)
        {            
            string connectionString = "Server = " + serverName + ";Uid = " + userName + ";Database = " + dbName + "; Pwd =" + password + "";

            DBConnService = new DBConnectionService(connectionString);

            try{
                DBConnService.OpenConnection();

                Properties.Settings.Default.Server = serverName;
                Properties.Settings.Default.DBName = dbName;
                Properties.Settings.Default.UserName = userName;
                Properties.Settings.Default.Password = password;
                Properties.Settings.Default.ConnectionString = connectionString;
                Properties.Settings.Default.Save();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            
        }

        public bool initiateDBConnection2(string serverName, string dbName, string userName, string password)
        {
            string connectionString = "Server = " + serverName + ";Uid = " + userName + ";Database = " + dbName + "; Pwd =" + password + "";

            DBConnService2 = new DBConnectionService(connectionString);

            try
            {
                DBConnService2.OpenConnection();

                Properties.Settings.Default.Server2 = serverName;
                Properties.Settings.Default.DBName2 = dbName;
                Properties.Settings.Default.UserName2 = userName;
                Properties.Settings.Default.Password2 = password;
                Properties.Settings.Default.ConnectionString2 = connectionString;
                Properties.Settings.Default.Save();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }

        }

        public void formClosing(IFrmDevice frmDevice)
        {
            frmDevice.ShowSelf();
            frmKonfigurasi.HideSelf();
        }

        public bool chekcDBConnection()
        {
            
            DBConnService = new DBConnectionService(Properties.Settings.Default.ConnectionString);
            DBConnService2 = new DBConnectionService(Properties.Settings.Default.ConnectionString2);
            try
            {
                DBConnService.OpenConnection();
                DBConnService.CloseConnection();
                DBConnService2.OpenConnection();
                DBConnService2.CloseConnection();
                return true;

            } catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
