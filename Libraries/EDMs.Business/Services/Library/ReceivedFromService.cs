namespace EDMs.Business.Services.Library
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.DAO.Library;
    using EDMs.Data.Entities;

    /// <summary>
    /// The category service.
    /// </summary>
    public class ReceivedFromService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly ReceivedFromDAO repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReceivedFromService"/> class.
        /// </summary>
        public ReceivedFromService()
        {
            this.repo = new ReceivedFromDAO();
        }

        #region Get (Advances)

        /// <summary>
        /// The get all by category.
        /// </summary>
        /// <param name="categoryId">
        /// The category id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<ReceivedFrom> GetAllByCategory(int categoryId)
        {
            return this.repo.GetAllByCategory(categoryId);
        }

        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<ReceivedFrom> GetAll()
        {
            return this.repo.GetAll().ToList();
        }

        /// <summary>
        /// The get all by category.
        /// </summary>
        /// <param name="categoryIds">
        /// The category ids.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<ReceivedFrom> GetAllByCategory(List<int> categoryIds)
        {
            return this.repo.GetAllByCategory(categoryIds);
        }

        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of category
        /// </param>
        /// <returns>
        /// The category</returns>
        public ReceivedFrom GetById(int id)
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
        public int? Insert(ReceivedFrom bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(ReceivedFrom bo)
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
        public bool Delete(ReceivedFrom bo)
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

        public ReceivedFrom GetByName(string name)
        {
            return this.repo.GetByName(name);
        }
    }
}
