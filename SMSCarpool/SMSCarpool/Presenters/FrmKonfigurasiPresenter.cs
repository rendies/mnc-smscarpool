﻿using SMSCarpool.Services;
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

        public FrmKonfigurasiPresenter(IFrmKonfigurasi frmKonfigurasiView)
        {
            frmKonfigurasi = frmKonfigurasiView;
            presenter = this;
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
            string serverName = Properties.Settings.Default.Server;
            string dbName = Properties.Settings.Default.DBName;
            string userName = Properties.Settings.Default.UserName;
            string password = Properties.Settings.Default.Password;

            string connectionString = "Server = " + serverName + ";Uid = " + userName + ";Database = " + dbName + "; Pwd =" + password + "";

            DBConnService = new DBConnectionService(connectionString);
            try
            {
                DBConnService.OpenConnection();
                DBConnService.CloseConnection();
                return true;

            } catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
