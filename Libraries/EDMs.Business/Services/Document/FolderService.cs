// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FolderService.cs" company="">
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
    public class FolderService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly FolderDAO repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="FolderService"/> class.
        /// </summary>
        public FolderService()
        {
            this.repo = new FolderDAO();
        }

        #region Get (Advances)

        /// <summary>
        /// The get specific folder.
        /// </summary>
        /// <param name="listFolId">
        /// The list fol id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<Folder> GetSpecificFolder(List<int> listFolId)
        {
            return this.repo.GetSpecificFolder(listFolId);
        }

        public List<Folder> GetSpecificFolderStatic(List<int> listFolId)
        {
            return this.repo.GetSpecificFolderStatic(listFolId);
        }

        public List<Folder> GetAllRelatedPermittedFolderItems(List<int> listFolId)
        {
            return this.repo.GetAllRelatedPermittedFolderItems(listFolId);
        }

        public List<Folder> GetAllByParentId(int parentId, List<int> listFolId )
        {
            return this.repo.GetAllByParentId(parentId, listFolId);
        }

        public List<Folder> GetAllSpecificFolder(List<int> listFolId)
        {
            return this.repo.GetAllSpecificFolder(listFolId);
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
        public List<Folder> GetAllByCategory(int categoryId)
        {
            return this.repo.GetAllByCategory(categoryId);
        } 

        public Folder GetByDirName(string dirName)
        {
            return this.repo.GetByDirName(dirName);
        }

        public IEnumerable<Folder> GetAllParentsFolderItemsByChildren(IEnumerable<Folder> children)
        {
            return this.repo.GetAllParentsFolderItemsByChildren(children);
        }
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<Folder> GetAll()
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
        public Folder GetById(int id)
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
        public int? Insert(Folder bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(Folder bo)
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
        public bool Delete(Folder bo)
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
