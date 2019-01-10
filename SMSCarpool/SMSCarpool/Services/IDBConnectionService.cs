using SMSCarpool.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace SMSCarpool.Services
{
    interface IDBConnectionService
    {
        string ConnectionString { get; set; }

        //open connection to database
        bool OpenConnection();

        //Close connection
        bool CloseConnection();

        //Insert statement
        bool Insert(string query);

        //Update statement
        bool Update(string query);

        //Delete statement
        bool Delete(string query);

        //Select statement
        IEnumerable<IDataRecord> Select(string query);

        //Count statement
        int Count(string query);

    }
}
