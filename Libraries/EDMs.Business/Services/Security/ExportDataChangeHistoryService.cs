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
    public class ExportDataChangeHistoryService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly ExportDataChangeHistoryDAO repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExportDataChangeHistoryService"/> class.
        /// </summary>
        public ExportDataChangeHistoryService()
        {
            this.repo = new ExportDataChangeHistoryDAO();
        }

        #region Get (Advances)
        public int GetCount()
        {
            return this.repo.GetAll().Count;
        }
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<ExportDataChangeHistory> GetAll()
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
        public ExportDataChangeHistory GetById(int id)
        {
            return this.repo.GetById(id);
        }
        #endregion

        #region Insert, Update, Delete
        public bool Update(ExportDataChangeHistory bo)
        {
            try
            {
                return this.repo.Update(bo);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Insert Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public int? Insert(ExportDataChangeHistory bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Delete Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Delete(ExportDataChangeHistory bo)
        {
            try
            {
                return this.repo.Delete(bo);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Delete Resource By ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            try
            {
                return this.repo.Delete(id);
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }
}
