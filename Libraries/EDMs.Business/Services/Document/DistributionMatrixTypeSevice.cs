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
    public class DistributionMatrixTypeService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly DistributionMatrixTypeDAO repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="DistributionMatrixTypeService"/> class.
        /// </summary>
        public DistributionMatrixTypeService()
        {
            this.repo = new DistributionMatrixTypeDAO();
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
        public List<DistributionMatrixType> GetAll()
        {
            return this.repo.GetAll().ToList();
        }

        /// <summary>
        /// Get All DistributionMatrixType Active
        /// </summary>
        /// <returns></returns>
        public List<DistributionMatrixType> GetAllActive()
        {
            return this.repo.GetAll().Where(t=> t.Active.GetValueOrDefault()).ToList();
        }
        
        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of category
        /// </param>
        /// <returns>
        /// The category</returns>
        public DistributionMatrixType GetById(int id)
        {
            return this.repo.GetById(id);
        }

        public DistributionMatrixType GetByName(string _Name)
        {
            return this.repo.GetByName(_Name);
        }

        #endregion

        #region Insert, Update, Delete
        /// <summary>
        /// Insert Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public int? Insert(DistributionMatrixType bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(DistributionMatrixType bo)
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
        public bool Delete(DistributionMatrixType bo)
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
