// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DocumentService.cs" company="">
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
    public class DocumentService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly DocumentDAO repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentService"/> class.
        /// </summary>
        public DocumentService()
        {
            this.repo = new DocumentDAO();
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
        public List<Document> GetSpecificDocument(List<int> listDocId)
        {
            return this.repo.GetSpecificDocument(listDocId);
        }

        /// <summary>
        /// The get specific document.
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
        public List<Document> GetSpecificDocument(string fileName, string dirName)
        {
            return this.repo.GetSpecificDocument(fileName, dirName);
        }

        /// <summary>
        /// The get all by folder.
        /// </summary>
        /// <param name="folderId">
        /// The folder id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>. Document
        /// </returns>
        public List<Document> GetAllByFolder(int folderId)
        {
            return this.repo.GetAllByFolder(folderId);
        }

        public List<Document> GetAllByFolder(List<int> folderIds)
        {
            return this.repo.GetAllByFolder(folderIds);
        }

        public int GetItemCount()
        {
            return this.repo.GetItemCount();
        }
        /// <summary>
        /// The get all relate document.
        /// </summary>
        /// <param name="docId">
        /// The doc id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<Document> GetAllRelateDocument(int docId)
        {
            return this.repo.GetAllRelateDocument(docId);
        }

        /// <summary>
        /// The get all doc revision.
        /// </summary>
        /// <param name="parentId">
        /// The parent id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<Document> GetAllDocRevision(int parentId)
        {
            return this.repo.GetAllDocRevision(parentId);
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
        public List<Document> QuickSearch(string keyword, int folId)
        {
            return this.repo.QuickSearch(keyword, folId);
        }

        /// <summary>
        /// The search document.
        /// </summary>
        /// <param name="categoryId">
        /// The category id.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="title">
        /// The title.
        /// </param>
        /// <param name="docNumber">
        /// The doc number.
        /// </param>
        /// <param name="keyword">
        /// The keyword.
        /// </param>
        /// <param name="revisionId">
        /// The revision id.
        /// </param>
        /// <param name="docTypeId">
        /// The doc type id.
        /// </param>
        /// <param name="statusId">
        /// The status id.
        /// </param>
        /// <param name="receivedFromId">
        /// The received from id.
        /// </param>
        /// <param name="disciplineId">
        /// The discipline id.
        /// </param>
        /// <param name="languageId">
        /// The language id.
        /// </param>
        /// <param name="dateFrom">
        /// The date from.
        /// </param>
        /// <param name="dateTo">
        /// The date to.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public IQueryable<Document> SearchDocument(int categoryId, string name, string title, string docNumber, string keyword, int revisionId, int docTypeId, int statusId, int receivedFromId, int disciplineId, int languageId, DateTime? dateFrom, DateTime? dateTo, string transmittalNumber, int pageSize, int startingRecordNumber)
        {
            return this.repo.SearchDocument(
                categoryId,
                name,
                title,
                docNumber,
                keyword,
                revisionId,
                docTypeId,
                statusId,
                receivedFromId,
                disciplineId,
                languageId,
                dateFrom,
                dateTo,
                transmittalNumber,
                pageSize,
                startingRecordNumber);
        }

        public int GetItemCount(int categoryId, string name, string title, string docNumber, string keyword, int revisionId, int docTypeId, int statusId, int receivedFromId, int disciplineId, int languageId, DateTime? dateFrom, DateTime? dateTo, string transmittalNumber)
        {
            return this.repo.GetItemCount(
                categoryId,
                name,
                title,
                docNumber,
                keyword,
                revisionId,
                docTypeId,
                statusId,
                receivedFromId,
                disciplineId,
                languageId,
                dateFrom,
                dateTo,
                transmittalNumber);
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
        public List<Document> GetAllDocVersion(int docId)
        {
            return this.repo.GetAllDocVersion(docId);

        }

        /// <summary>
        /// The is document exist.
        /// </summary>
        /// <param name="folderId">
        /// The folder id.
        /// </param>
        /// <param name="documentName">
        /// The document name.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool IsDocumentExist(int folderId, string documentName)
        {
            return this.repo.IsDocumentExist(folderId, documentName);
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
            return this.repo.IsDocumentExistUpdate(folderId, docName, docId);
        }

        /// <summary>
        /// The get specific document.
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
        public  Document GetSpecificDocument(int folderId, string documentName)
        {
            return this.repo.GetSpecificDocument(folderId, documentName);
        }
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<Document> GetAll()
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
        public Document GetById(int id)
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
        public int? Insert(Document bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(Document bo)
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
        public bool Delete(Document bo)
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
