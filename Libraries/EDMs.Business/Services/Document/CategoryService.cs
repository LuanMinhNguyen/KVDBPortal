// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryService.cs" company="">
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
    public class CategoryService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly CategoryDAO repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryService"/> class.
        /// </summary>
        public CategoryService()
        {
            this.repo = new CategoryDAO();
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
        public List<Category> GetAll()
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
        public Category GetById(int id)
        {
            return this.repo.GetById(id);
        }

        /// <summary>
        /// The get by name.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="Category"/>.
        /// </returns>
        public Category GetByName(string name)
        {
            return this.repo.GetAll().FirstOrDefault(t => t.Name == name);
        }
        #endregion

        #region Insert, Update, Delete
        /// <summary>
        /// Insert Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public int? Insert(Category bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(Category bo)
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
        public bool Delete(Category bo)
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
