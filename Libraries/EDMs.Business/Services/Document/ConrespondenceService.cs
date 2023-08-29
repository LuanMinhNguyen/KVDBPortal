namespace EDMs.Business.Services.Document
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.DAO.Document;
    using EDMs.Data.Entities;
    public class ConrespondenceService
    {

        /// <summary>
        /// The repo.
        /// </summary>
        private readonly ConrespondenceDAO repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentService"/> class.
        /// </summary>
        public ConrespondenceService()
        {
            this.repo = new ConrespondenceDAO();
        }

        #region Get (Advances)

        /// <summary>
        /// The get specific document.
        /// </summary>
        /// <param name="listDocId">
        /// The list doc id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<Conrespondence> GetSpecificDocument(List<int> listDocId)
        {
            return this.repo.GetSpecificConrespondence(listDocId);
        }

        /// <summary>
        /// The get specific Conrespondence.
        /// </summary>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        /// <param name="dirName">
        /// The dir name.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public Conrespondence GetSpecificDocument(string fileName, string dirName)
        {
            return this.repo.GetSpecificConrespondence(fileName, dirName);
        }

        /// <summary>
        /// The get all by folder.
        /// </summary>
        /// <param name="folderId">
        /// The folder id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>. Conrespondence
        /// </returns>
        public List<Conrespondence> GetAllByFolder(int folderId)
        {
            return this.repo.GetAllByFolder(folderId);
        }

        public List<Conrespondence> GetAllByFolder(List<int> folderIds)
        {
            return this.repo.GetAllByFolder(folderIds);
        }

        public int GetItemCount()
        {
            return this.repo.GetItemCount();
        }
        /// <summary>
        /// The get all relate Conrespondence.
        /// </summary>
        /// <param name="docId">
        /// The doc id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<Conrespondence> GetAllRelateDocument(int docId)
        {
            return this.repo.GetAllRelateConrespondence(docId);
        }

       

        /// <summary>
        /// The quick search.
        /// </summary>
        /// <param name="keyword">
        /// The keyword.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<Conrespondence> QuickSearch(string keyword, int folId)
        {
            return this.repo.QuickSearch(keyword, folId);
        }


        /// <summary>
        /// The get all doc version.
        /// </summary>
        /// <param name="docId">
        /// The doc id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<Conrespondence> GetAllDocVersion(int docId)
        {
            return this.repo.GetAllDocVersion(docId);

        }

        /// <summary>
        /// The is Conrespondence exist.
        /// </summary>
        /// <param name="folderId">
        /// The folder id.
        /// </param>
        /// <param name="ConrespondenceName">
        /// The Conrespondence name.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool IsConrespondenceExist(int folderId, string documentName)
        {
            return this.repo.IsConrespondenceExist(folderId, documentName);
        }

        /// <summary>
        /// The is document exist update.
        /// </summary>
        /// <param name="folderId">
        /// The folder id.
        /// </param>
        /// <param name="docName">
        /// The doc name.
        /// </param>
        /// <param name="docId">
        /// The doc id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool IsDocumentExistUpdate(int folderId, string docName, int docId)
        {
            return this.repo.IsConrespondenceExistUpdate(folderId, docName, docId);
        }

        /// <summary>
        /// The get specific Conrespondence.
        /// </summary>
        /// <param name="folderId">
        /// The folder id.
        /// </param>
        /// <param name="documentName">
        /// The document name.
        /// </param>
        /// <returns>
        /// The <see cref="Document"/>.
        /// </returns>
        public Conrespondence GetSpecificDocument(int folderId, string documentName)
        {
            return this.repo.GetSpecificConrespondence(folderId, documentName);
        }
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<Conrespondence> GetAll()
        {
            return this.repo.GetAll().ToList();
        }
        public Conrespondence IsExist(int folderId, string documentName)
        {
            return this.repo.GetAll().FirstOrDefault(t=> t.FolderID==folderId && !t.IsDelete.GetValueOrDefault() && t.FileName==documentName);
        }
        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of category
        /// </param>
        /// <returns>
        /// The category</returns>
        public Conrespondence GetById(int id)
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
        public int? Insert(Conrespondence bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(Conrespondence bo)
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
        public bool Delete(Conrespondence bo)
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
