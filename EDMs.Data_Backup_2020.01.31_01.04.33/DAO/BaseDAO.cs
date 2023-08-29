using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using EDMs.Data.Entities;

namespace EDMs.Data.DAO
{
    public class BaseDAO
    {
        private static string dbConnection = string.Empty;
        public EDMsEntities EDMsDataContext;
        public BaseDAO()
        {
            if (dbConnection == string.Empty)
                CreateConnection();
            EDMsDataContext = new EDMsEntities(dbConnection);
        }

        private static void CreateConnection()
        {
            string provider = ConfigurationManager.AppSettings.Get("ProviderDB");

            switch (provider)
            {
                case "SqlClient":
                    {
                        dbConnection = ConfigurationManager.AppSettings.Get("SqlClientConnectionString");
                        break;
                    }
                case "OleDb":
                    {

                        dbConnection = ConfigurationManager.AppSettings.Get("OleDbConnectionString");
                        break;
                    }
                case "Oracle":
                    {

                        dbConnection = ConfigurationManager.AppSettings.Get("OracleConnectionString");
                        break;
                    }
                default:
                    {
                        dbConnection = ConfigurationManager.AppSettings.Get("SqlClientConnectionString");
                        break;
                    }
            }
        }
    }
}
