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
    public class ExportScheduleService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly ExportScheduleDAO repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExportScheduleService"/> class.
        /// </summary>
        public ExportScheduleService()
        {
            this.repo = new ExportScheduleDAO();
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
        public List<ExportSchedule> GetAll()
        {
            return this.repo.GetAll().OrderBy(t => t.ExportTime).ToList();
        }

        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of category
        /// </param>
        /// <returns>
        /// The category</returns>
        public ExportSchedule GetById(int id)
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
        public int? Insert(ExportSchedule bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Delete Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Delete(ExportSchedule bo)
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

        public bool Update(ExportSchedule bo)
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
        #endregion
    }
}
