// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorLogDAO.cs" company="">
//   
// </copyright>
// <summary>
//   The category dao.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using EDMs.Data.Entities;

namespace EDMs.Data.DAO.Security
{
    /// <summary>
    /// The category dao.
    /// </summary>
    public class ErrorLogDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorLogDAO"/> class.
        /// </summary>
        public ErrorLogDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<ErrorLog> GetIQueryable()
        {
            return this.EDMsDataContext.ErrorLogs;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<ErrorLog> GetAll()
        {
            return this.EDMsDataContext.ErrorLogs.OrderByDescending(t => t.ID).ToList();
        }

        /// <summary>
        /// The get by id.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Resource"/>.
        /// </returns>
        public ErrorLog GetById(Guid id)
        {
            return this.EDMsDataContext.ErrorLogs.FirstOrDefault(ob => ob.ID == id);
        }

        #endregion

        #region GET ADVANCE

        #endregion

        #region Insert, Update, Delete

        public Guid? Insert(ErrorLog ob)
        {
            try
            {
                this.EDMsDataContext.AddToErrorLogs(ob);
                this.EDMsDataContext.SaveChanges();
                return ob.ID;
            }
            catch
            {
                return null;
            }
        }


        #endregion
    }
}
