namespace EDMs.Business.Services.Security
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.DAO.Security;
    using EDMs.Data.Entities;

    /// <summary>
    /// The category service.
    /// </summary>
    public class DocPropertiesViewService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly DocPropertiesViewDAO repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocPropertiesViewService"/> class.
        /// </summary>
        public DocPropertiesViewService()
        {
            this.repo = new DocPropertiesViewDAO();
        }

        #region Get (Advances)
        ////public List<DocPropertiesView> GetAllWithoutDeparment()
        ////{
        ////    return this.repo.GetAll().Where(t => t.RoleId == 0 || t.RoleId == null).ToList();
        ////}

        ////public List<DocPropertiesView> GetAllByDeparment(int deparmentId)
        ////{
        ////    return this.repo.GetAll().Where(t => t.RoleId == deparmentId).ToList();
        ////}
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<DocPropertiesView> GetAll()
        {
            return this.repo.GetAll().ToList();
        }

        public List<DocPropertiesView> GetAllSpecial(int categoryId, int roleId)
        {
            return
                this.repo.GetAll().Where(
                    t =>
                    t.CategoryId == categoryId 
                    && t.RoleId == roleId).ToList();
        }

        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of category
        /// </param>
        /// <returns>
        /// The category</returns>
        public DocPropertiesView GetById(int id)
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
        public int? Insert(DocPropertiesView bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(DocPropertiesView bo)
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
        public bool Delete(DocPropertiesView bo)
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
