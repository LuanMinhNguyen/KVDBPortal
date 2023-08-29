// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DistributionMatrixService.cs" company="">
//   
// </copyright>
// <summary>
//   The category service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EDMs.Business.Services.Document
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.DAO.Document;
    using EDMs.Data.Entities;

    /// <summary>
    /// The category service.
    /// </summary>
    public class DistributionMatrixService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly DistributionMatrixDAO repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="DistributionMatrixService"/> class.
        /// </summary>
        public DistributionMatrixService()
        {
            this.repo = new DistributionMatrixDAO();
        }

        #region Get (Advances)
        public List<DistributionMatrix> GetAllByType(int typeId)
        {
            return this.repo.GetAll().Where(t => t.TypeId == typeId).ToList();
        }
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<DistributionMatrix> GetAll()
        {
            return this.repo.GetAll().ToList();
        }

        public List<DistributionMatrix> GetAllByProject(List<int> projectId)
        {
            return this.repo.GetAll().Where(t => projectId.Contains(t.ProjectId.GetValueOrDefault())).ToList();
        }

        public List<DistributionMatrix> GetAllByList(List<int> listId)
        {
            return this.repo.GetAllList(listId).ToList();
        }
        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of category
        /// </param>
        /// <returns>
        /// The category</returns>
        public DistributionMatrix GetById(int id)
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
        public int? Insert(DistributionMatrix bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(DistributionMatrix bo)
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
        public bool Delete(DistributionMatrix bo)
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
