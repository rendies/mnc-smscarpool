using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using SMSCarpool.Models;
using System.Data.Common;

namespace SMSCarpool.Services
{
    public class DBConnectionService : IDBConnectionService
    {
        private MySqlConnection connection;
        private DBConnectionModel dbConnectionModel;

        public string ConnectionString { get; set; }

        //Constructor
        public DBConnectionService(string conString)
        {
            ConnectionString = conString;
            Initialize();
        }

        public IDbConnection getConnection()
        {
            return connection;
        }
                
        //Initialize values
        private void Initialize()
        {
            connection = new MySqlConnection(ConnectionString);
        }
        
        //open connection to database
        public bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                throw new System.Exception(ex.Number + ": " + ex.Message);
            }
        }

        //Close connection
        public bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                throw new System.Exception(ex.Number + ": " + ex.Message);
            }
        }

        //Insert statement
        public bool Insert(string query)
        {
            try
            {
                OpenConnection();

                //create mysql command
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                cmd.ExecuteNonQuery();
                
                //close connection
                CloseConnection();
                return true;
            }
            catch (Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }

        //Update statement
        public bool Update(string query)
        {
            try
            {
                OpenConnection();
                
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = connection;

                //Execute query
                cmd.ExecuteNonQuery();
                
                //close connection
                CloseConnection();
                return true;
            }
            catch (Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }

        //Delete statement
        public bool Delete(string query)
        {
            try
            {
                OpenConnection();

                //create mysql command
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                CloseConnection();
                return true;
            }
            catch (Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }

        //Select statement
        public IEnumerable<IDataRecord> Select(string query)
        {
            try
            {
                OpenConnection();
            }
            catch (Exception ex)
            {
                throw new System.Exception(ex.Message);
            }

            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader dataReader;
            //Create a data reader and Execute the command
            using (dataReader = cmd.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    yield return dataReader;
                }
                yield return null;
            }

            try
            {
                CloseConnection();
            }
            catch (Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
            
        }

        //Count statement
        public int Count(string query)
        {
            int Count = -1;

            //Open Connection
            try
            {
                this.OpenConnection();

                //Create Mysql Command
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //ExecuteScalar will return one value
                Count = int.Parse(cmd.ExecuteScalar() + "");

                //close Connection
                this.CloseConnection();
                                
                return Count;
                
            } catch(Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
            
        }


    }
}
