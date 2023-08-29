using System;
using System.Collections.Generic;
using System.Linq;
using EDMs.Data.DAO.Security;
using EDMs.Data.Entities;

namespace EDMs.Business.Services.Security
{
    /// <summary>
    /// The category service.
    /// </summary>
    public class ErrorLogService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly ErrorLogDAO repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorLogService"/> class.
        /// </summary>
        public ErrorLogService()
        {
            this.repo = new ErrorLogDAO();
        }

        #region Get (Advances)
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<ErrorLog> GetAll()
        {
            return this.repo.GetAll().OrderBy(t => t.ID).ToList();
        }

        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of category
        /// </param>
        /// <returns>
        /// The category</returns>
        public ErrorLog GetById(Guid id)
        {
            return this.repo.GetById(id);
        }
        #endregion

        #region Insert, Update, Delete
        /// <summary>
        /// Insert Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public Guid? Insert(ErrorLog bo)
        {
            return this.repo.Insert(bo);
        }
        #endregion
    }
}
