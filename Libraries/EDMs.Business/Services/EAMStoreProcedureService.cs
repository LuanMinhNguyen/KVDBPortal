using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using EDMs.Data.DAO;

namespace EDMs.Business.Services
{
    /// <summary>
    /// The category service.
    /// </summary>
    public class EAMStoreProcedureService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly EAMStoreProcedureDAO repo;

        public EAMStoreProcedureService()
        {
            this.repo = new EAMStoreProcedureDAO();
        }
        public DataTable GetDataTable(string query)
        {
            return repo.GetDataTable(query);
        }

        public object GetDataValue(string SPName, SqlParameter[] SqlPrms = null)
        {
            return repo.GetDataValue(SPName, SqlPrms);
        }

        public List<object> GetOutputValue(string SPName, SqlParameter[] SqlPrms)
        {
            return repo.GetOutputValue(SPName, SqlPrms);
        }

        public string ExcuteQuery(string SPName, SqlParameter[] SqlPrms)
        {
            return repo.ExcuteQuery(SPName, SqlPrms);

        }

        public DataSet GetDataSet(string SPName, SqlParameter[] SqlPrms = null)
        {
            return repo.GetDataSet(SPName, SqlPrms);
        }

        // function that creates a list of an object from the given data table
        public List<T> CreateListFromTable<T>(DataTable tbl) where T : new()
        {
            // define return list
            List<T> lst = new List<T>();

            // go through each row
            foreach (DataRow r in tbl.Rows)
            {
                // add to the list
                lst.Add(CreateItemFromRow<T>(r));
            }

            // return the list
            return lst;
        }

        // function that creates an object from the given data row
        public T CreateItemFromRow<T>(DataRow row) where T : new()
        {
            // create a new object
            T item = new T();

            // set the item
            SetItemFromRow(item, row);

            // return 
            return item;
        }

        public void SetItemFromRow<T>(T item, DataRow row) where T : new()
        {
            // go through each column
            foreach (DataColumn c in row.Table.Columns)
            {
                // find the property for the column
                PropertyInfo p = item.GetType().GetProperty(c.ColumnName);

                // if exists, set the value
                if (p != null && row[c] != DBNull.Value)
                {
                    p.SetValue(item, row[c], null);
                }
            }
        }

    }
}
