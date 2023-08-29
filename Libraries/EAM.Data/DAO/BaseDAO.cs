using System.Configuration;
using EAM.Data.Entities;

namespace EAM.Data.DAO
{
    public class BaseDAO
    {
        private static string dbConnection = string.Empty;
        public EAMEntities EDMsDataContext;
        public BaseDAO()
        {
            if (string.IsNullOrEmpty(dbConnection))
                CreateConnection();
            EDMsDataContext = new EAMEntities(dbConnection);
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
