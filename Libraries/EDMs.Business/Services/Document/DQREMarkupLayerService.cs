using System;
using System.Collections.Generic;
using System.Linq;
using EDMs.Data.DAO.Document;
using EDMs.Data.Entities;

namespace EDMs.Business.Services.Document
{
    /// <summary>
    /// The category service.
    /// </summary>
    public class DQREMarkupLayerService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly DQREMarkupLayerDAO repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="DQREMarkupLayerService"/> class.
        /// </summary>
        public DQREMarkupLayerService()
        {
            this.repo = new DQREMarkupLayerDAO();
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
        public List<DQREMarkupLayer> GetAll()
        {
            return this.repo.GetAll().ToList();
        }

        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of category
        /// </param>
        /// <returns>
        /// The category</returns>
        public DQREMarkupLayer GetById(Guid id)
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
        public Guid? Insert(DQREMarkupLayer bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(DQREMarkupLayer bo)
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
        /// Delete Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Delete(DQREMarkupLayer bo)
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
        public bool Delete(Guid id)
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
