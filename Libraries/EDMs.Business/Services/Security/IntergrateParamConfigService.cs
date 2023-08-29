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
    public class IntergrateParamConfigService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly IntergrateParamConfigDAO repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="IntergrateParamConfigService"/> class.
        /// </summary>
        public IntergrateParamConfigService()
        {
            this.repo = new IntergrateParamConfigDAO();
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
        public List<IntergrateParamConfig> GetAll()
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
        public IntergrateParamConfig GetById(int id)
        {
            return this.repo.GetById(id);
        }
        #endregion

        #region Insert, Update, Delete
        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(IntergrateParamConfig bo)
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
