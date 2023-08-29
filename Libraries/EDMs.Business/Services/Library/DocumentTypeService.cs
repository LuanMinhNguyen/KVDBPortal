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
    public class DocumentTypeService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly DocumentTypeDAO repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentTypeService"/> class.
        /// </summary>
        public DocumentTypeService()
        {
            this.repo = new DocumentTypeDAO();
        }

        #region Get (Advances)
        public DocumentType GetByDocNo(string docNo)
        {
            return this.repo.GetAll().FirstOrDefault(t => docNo.Contains("-" + t.Code + "-"));
        }

        public DocumentType GetByCode(string code)
        {
            return this.repo.GetAll().FirstOrDefault(t => t.Code == code);
        }

        public List<DocumentType> GetAllByParent(int parentId)
        {
            return this.repo.GetAll().Where(t => t.ParentId == parentId).ToList();
        }

        public List<DocumentType> GetAllByCategory(int categoryId)
        {
            return this.repo.GetAll().Where(t => !string.IsNullOrEmpty(t.CategoryIds) && t.CategoryIds.Split(';').Contains(categoryId.ToString())).ToList();
        }
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<DocumentType> GetAll()
        {
            return this.repo.GetAll().ToList();
        }

        public List<DocumentType> GetAllBySpecial(int plantId, int categoryId)
        {
            return this.repo.GetAllBySpecial(plantId, categoryId);
        }

        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of category
        /// </param>
        /// <returns>
        /// The category</returns>
        public DocumentType GetById(int id)
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
        public int? Insert(DocumentType bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(DocumentType bo)
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
        public bool Delete(DocumentType bo)
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
        /// The <see cref="DocumentType"/>.
        /// </returns>
        public DocumentType GetByName(string name)
        {
            return this.repo.GetAll().FirstOrDefault(t => t.Name == name);
        }
        
    }
}
