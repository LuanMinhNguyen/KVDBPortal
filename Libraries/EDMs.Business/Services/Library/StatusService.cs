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
    public class StatusService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly StatusDAO repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusService"/> class.
        /// </summary>
        public StatusService()
        {
            this.repo = new StatusDAO();
        }

        #region Get (Advances)
        public List<Status> GetAllByProject(int projectId)
        {
            return this.repo.GetAll().Where(t => t.ProjectID == projectId).ToList();
        }
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<Status> GetAll()
        {
            return this.repo.GetAll().ToList();
        }

        public List<Status> GetAllByCategory(List<int> categoryIds)
        {
            return this.repo.GetAllByCategory(categoryIds);
        }
        /// <summary>
        /// The get all by category.
        /// </summary>
        /// <param name="categoryId">
        /// The category id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<Status> GetAllByCategory(int categoryId)
        {
            return this.repo.GetAllByCategory(categoryId);
        }

        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of category
        /// </param>
        /// <returns>
        /// The category</returns>
        public Status GetById(int id)
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
        public int? Insert(Status bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(Status bo)
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
        public bool Delete(Status bo)
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

        /// <summary>
        /// The get by name.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="Status"/>.
        /// </returns>
        public Status GetByName(string name)
        {
            return this.repo.GetByName(name);
        }

        public Status GetByName(string name, int projectId)
        {
            return this.repo.GetAll().FirstOrDefault(t => t.Name == name && t.ProjectID == projectId);
        }
    }
}
