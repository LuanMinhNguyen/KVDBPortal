// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EAM_PTW_NewICCCommentDAO.cs" company="">
//   
// </copyright>
// <summary>
//   The category dao.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace EDMs.Data.DAO
{
    /// <summary>
    /// The category dao.
    /// </summary>
    public class EAMStoreProcedureDAO
    {
        private SqlConnection conn;

        /// <summary>
        /// Initializes a new instance of the <see cref="EAMStoreProcedureDAO"/> class.
        /// </summary>
        public EAMStoreProcedureDAO() : base() { }

        private readonly string connStr = @"Data Source=SVR-EAMDB-01;Initial catalog=EAMDB;User ID=sa;Password=Ingr.123;Min Pool Size=5;Max Pool Size=100;";
        #region GET (Basic)

        public DataTable GetDataTable(string query)
        {
            conn = new SqlConnection(ConfigurationManager.AppSettings.Get("SQLConn"));
            conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            conn.Close();
            return dt;
        }

        public object GetDataValue(string SPName,SqlParameter[] SqlPrms)
        {
            object obj;
            var constr = ConfigurationManager.AppSettings.Get("SQLConn");
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(SPName, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(SqlPrms);
                    con.Open();
                    obj = cmd.ExecuteScalar();
                    con.Close();
                    cmd.Parameters.Clear();
                }
            }

            return obj;
        }

        public List<object> GetOutputValue(string SPName, SqlParameter[] SqlPrms)
        {
           var objList = new List<object>();
            var constr = ConfigurationManager.AppSettings.Get("SQLConn");
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(SPName, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddRange(SqlPrms);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    foreach (var param in SqlPrms)
                    {
                        objList.Add(param.Value);
                    }

                    con.Close();
                    cmd.Parameters.Clear();
                }
            }

            return objList;
        }

        public string ExcuteQuery(string SPName, SqlParameter[] SqlPrms)
        {
            var mess = string.Empty;
            var objList = new List<object>();
            var constr = ConfigurationManager.AppSettings.Get("SQLConn");
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(SPName, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        cmd.Parameters.AddRange(SqlPrms);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        cmd.Parameters.Clear();
                    }
                    catch (Exception e)
                    {
                        mess = e.Message;
                    }
                    
                }
            }

            return mess;
        }



        public DataSet GetDataSet(string SPName, SqlParameter[] SqlPrms)
        {
            var ds = new DataSet();
            var cmd = new SqlCommand();
            var da = new SqlDataAdapter();
            var conn = new SqlConnection(ConfigurationManager.AppSettings.Get("SQLConn"));

            try
            {
                conn.Open();
                cmd = new SqlCommand(SPName, conn);
                if (SqlPrms != null)
                {
                    cmd.Parameters.AddRange(SqlPrms);
                }
                
                cmd.CommandTimeout = 240;
                cmd.CommandType = CommandType.StoredProcedure;
                da.SelectCommand = cmd;
                da.Fill(ds);

                cmd.Parameters.Clear();

                conn.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ds;
        }

        #endregion
    }
}
