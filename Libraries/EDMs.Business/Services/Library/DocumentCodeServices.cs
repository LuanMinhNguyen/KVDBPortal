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
    public class DocumentCodeServices
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly DocumentCodeDAO repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentCodeServices"/> class.
        /// </summary>
        public DocumentCodeServices()
        {
            this.repo = new DocumentCodeDAO();
        }

        #region Get (Advances)
        public List<DocumentCode> GetAllActionCode()
        {
            return this.repo.GetAll().Where(t => t.TypeId == 1).OrderBy(t => t.ID).ToList();
        }

        public List<DocumentCode> GetAllReviewStatus()
        {
            return this.repo.GetAll().Where(t => t.TypeId == 2).OrderBy(t => t.ID).ToList();
        }
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<DocumentCode> GetAll()
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
        public DocumentCode GetById(int id)
        {
            return this.repo.GetById(id);
        }

        public DocumentCode GetByCode(string code)
        {
            return this.repo.GetByCode(code);
        }

        #endregion

        #region Insert, Update, Delete
        /// <summary>
        /// Insert Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public int? Insert(DocumentCode bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(DocumentCode bo)
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
        public bool Delete(DocumentCode bo)
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
        /// The <see cref="Code"/>.
        /// </returns>
        public DocumentCode GetByName(string name)
        {
            return this.repo.GetByName(name);
        }
    }
}
