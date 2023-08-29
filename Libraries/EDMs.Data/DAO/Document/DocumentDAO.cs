// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Documentcs" company="">
//   
// </copyright>
// <summary>
//   The category 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EDMs.Data.DAO.Document
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.DAO;
    using EDMs.Data.Entities;

    /// <summary>
    /// The category 
    /// </summary>
    public class DocumentDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentDAO"/> class.
        /// </summary>
        public DocumentDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<Document> GetIQueryable()
        {
            return this.EDMsDataContext.Documents;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<Document> GetAll()
        {
            return this.EDMsDataContext.Documents.ToList();
        }

        /// <summary>
        /// The get by id.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Resource"/>.
        /// </returns>
        public Document GetById(int id)
        {
            return this.EDMsDataContext.Documents.FirstOrDefault(ob => ob.ID == id);
        }
       
        #endregion

        #region GET ADVANCE

        /// <summary>
        /// The get all by folder.
        /// </summary>
        /// <param name="folderId">
        /// The folder id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>. document
        /// </returns>
        public List<Document> GetAllByFolder(int folderId)
        {
            return this.EDMsDataContext.Documents.Where(t => t.FolderID == folderId && t.IsLeaf == true && t.IsDelete == false).OrderByDescending(t => t.ID).ToList();
        }

        public List<Document> GetAllByFolder(List<int> folderIds)
        {
            return this.EDMsDataContext.Documents.ToArray().Where(t => folderIds.Contains(t.FolderID.GetValueOrDefault()) && t.IsLeaf == true && t.IsDelete == false).OrderByDescending(t => t.ID).ToList();
            
        }

        public int GetItemCount()
        {
            return this.EDMsDataContext.Documents.Count(t => t.IsLeaf == true && t.IsDelete == false);
        }
        /// <summary>
        /// The get all doc revision has parent.
        /// </summary>
        /// <param name="parentId">
        /// The parent id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<Document> GetAllDocRevision(int parentId)
        {
            return this.EDMsDataContext.Documents.Where(t => (t.ID == parentId || t.ParentID == parentId) && t.RevisionID != 0 && t.IsDelete == false)
                                                .OrderByDescending(t => t.RevisionID).ToList();
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
            return this.EDMsDataContext.Documents.Where(t => (t.ID == docId || t.ParentID == docId) && t.IsDelete == false).ToList();
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
            return this.EDMsDataContext.Documents.Where(t => (t.Name.ToLower().Contains(keyword.ToLower()) || t.Title.ToLower().Contains(keyword.ToLower()) || t.DocumentNumber.ToLower().Contains(keyword.ToLower())) && 
                t.FolderID == folId && t.IsLeaf == true && t.IsDelete == false).OrderByDescending(t => t.ID).ToList();
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
            return this.EDMsDataContext.Documents.Where(t => t.ID == docId || t.ParentID == docId).ToList();
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
        /// The doc Number.
        /// </param>
        /// <param name="keyword">
        /// The keyword.
        /// </param>
        /// <param name="revisionId">
        /// The revision Id.
        /// </param>
        /// <param name="docTypeId">
        /// The doc Type Id.
        /// </param>
        /// <param name="statusId">
        /// The status Id.
        /// </param>
        /// <param name="receivedFromId">
        /// The received From Id.
        /// </param>
        /// <param name="disciplineId">
        /// The discipline Id.
        /// </param>
        /// <param name="languageId">
        /// The language Id.
        /// </param>
        /// <param name="dateFrom">
        /// The date From.
        /// </param>
        /// <param name="dateTo">
        /// The date To.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public IQueryable<Document> SearchDocument(int categoryId, string name, string title, string docNumber, string keyword, int revisionId, int docTypeId, int statusId, int receivedFromId, int disciplineId, int languageId, DateTime? dateFrom, DateTime? dateTo, string transmittalNumber, int pageSize, int startingRecordNumber)
        {
            var listDocs =
                this.EDMsDataContext.Documents.Where(
                    t =>
                    t.CategoryID == categoryId
                    && t.IsLeaf == true 
                    && t.IsDelete == false
                    && (string.IsNullOrEmpty(transmittalNumber) || t.TransmittalNumber.ToLower().Contains(transmittalNumber.ToLower()))
                    && (string.IsNullOrEmpty(name) || t.Name.ToLower().Contains(name.ToLower()))
                    && (string.IsNullOrEmpty(title) || t.Title.ToLower().Contains(title.ToLower()))
                    && (string.IsNullOrEmpty(docNumber) || t.DocumentNumber.Contains(docNumber.ToLower()))
                    && (string.IsNullOrEmpty(keyword) || t.KeyWords.ToLower().Contains(keyword.ToLower()))
                    && (revisionId == 0 || t.RevisionID == revisionId)
                    && (docTypeId == 0 || t.DocumentTypeID == docTypeId)
                    && (statusId == 0 || t.StatusID == statusId)
                    && (receivedFromId == 0 || t.ReceivedFromID == receivedFromId)
                    && (disciplineId == 0 || t.DisciplineID == disciplineId)
                    && (languageId == 0 || t.LanguageID == languageId)
                    && (dateFrom == null || t.ReceivedDate >= dateFrom)
                    && (dateTo == null || t.ReceivedDate <= dateTo)).OrderBy(t => t.Name).Skip(startingRecordNumber).Take(pageSize);

            return listDocs;
        }

        public int GetItemCount(int categoryId, string name, string title, string docNumber, string keyword, int revisionId, int docTypeId, int statusId, int receivedFromId, int disciplineId, int languageId, DateTime? dateFrom, DateTime? dateTo, string transmittalNumber)
        {
            return this.EDMsDataContext.Documents.Count(
                    t =>
                    t.CategoryID == categoryId
                    && t.IsLeaf == true
                    && t.IsDelete == false
                    && (string.IsNullOrEmpty(transmittalNumber) || t.TransmittalNumber.ToLower().Contains(transmittalNumber.ToLower()))
                    && (string.IsNullOrEmpty(name) || t.Name.ToLower().Contains(name.ToLower()))
                    && (string.IsNullOrEmpty(title) || t.Title.ToLower().Contains(title.ToLower()))
                    && (string.IsNullOrEmpty(docNumber) || t.DocumentNumber.Contains(docNumber.ToLower()))
                    && (string.IsNullOrEmpty(keyword) || t.KeyWords.ToLower().Contains(keyword.ToLower()))
                    && (revisionId == 0 || t.RevisionID == revisionId)
                    && (docTypeId == 0 || t.DocumentTypeID == docTypeId)
                    && (statusId == 0 || t.StatusID == statusId)
                    && (receivedFromId == 0 || t.ReceivedFromID == receivedFromId)
                    && (disciplineId == 0 || t.DisciplineID == disciplineId)
                    && (languageId == 0 || t.LanguageID == languageId)
                    && (dateFrom == null || t.ReceivedDate >= dateFrom)
                    && (dateTo == null || t.ReceivedDate <= dateTo));
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
            return
                this.EDMsDataContext.Documents.Any(
                    t =>
                    t.IsDelete == false && 
                    t.ParentID == null &&
                    t.FolderID == folderId && 
                    t.FileNameOriginal == documentName);
        }

        /// <summary>
        /// The is document exist.
        /// </summary>
        /// <param name="folderId">
        /// The folder id.
        /// </param>
        /// <param name="fileName">
        /// The file Name.
        /// </param>
        /// <param name="docId"> </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool IsDocumentExistUpdate(int folderId, string docName, int docId)
        {
            return this.EDMsDataContext.Documents.Any(t => t.FolderID == folderId && t.FileNameOriginal == docName && t.IsLeaf == true && t.ID != docId);
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
        public Document GetSpecificDocument(int folderId, string documentName)
        {
            var docList = this.EDMsDataContext.Documents.FirstOrDefault(t => t.FolderID == folderId && t.Name == documentName && t.IsLeaf == true);
            ////if (docList.Any())
            ////{
            ////    return docList.First();
            ////}

            return docList;
        }

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
            return this.EDMsDataContext.Documents.ToArray().Where(t => listDocId.Contains(t.ID)).ToList();
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
            return this.EDMsDataContext.Documents.Where(t => t.DirName == dirName && t.FileNameOriginal == fileName && t.IsDelete == false).ToList();
        }

        #endregion

        #region Insert, Update, Delete

        /// <summary>
        /// The insert.
        /// </summary>
        /// <param name="ob">
        /// The ob.
        /// </param>
        /// <returns>
        /// The <see cref="int?"/>.
        /// </returns>
        public int? Insert(Document ob)
        {
            try
            {
                this.EDMsDataContext.AddToDocuments(ob);
                this.EDMsDataContext.SaveChanges();
                return ob.ID;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="src">
        /// Entity for update
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// True if update success, false if not
        /// </returns>
        public bool Update(Document src)
        {
            try
            {
                Document des = (from rs in this.EDMsDataContext.Documents
                                where rs.ID == src.ID
                                select rs).First();

                des.Name = src.Name;
                des.DocumentNumber = src.DocumentNumber;
                des.Title = src.Title;
                des.RevisionID = src.RevisionID;
                des.RevisionName = src.RevisionName;
                des.DocumentTypeID = src.DocumentTypeID;
                des.DisciplineID = src.DisciplineID;
                des.StatusID = src.StatusID;
                des.ReceivedFromID = src.ReceivedFromID;
                des.ReceivedDate = src.ReceivedDate;
                des.LanguageID = src.LanguageID;
                des.Well = src.Well;
                des.KeyWords = src.KeyWords;
                des.Remark = src.Remark;
                des.TransmittalNumber = src.TransmittalNumber;

                des.IsLeaf = src.IsLeaf;
                des.IsDelete = src.IsDelete;
                des.DocIndex = src.DocIndex;
                des.ParentID = src.ParentID;
                des.FolderID = src.FolderID;

                des.LastUpdatedDate = src.LastUpdatedDate;
                des.LastUpdatedBy = src.LastUpdatedBy;

                this.EDMsDataContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="src">
        /// The src.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// True if delete success, false if not
        /// </returns>
        public bool Delete(Document src)
        {
            try
            {
                var des = this.GetById(src.ID);
                if (des != null)
                {
                    this.EDMsDataContext.DeleteObject(des);
                    this.EDMsDataContext.SaveChanges();
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Delete By ID
        /// </summary>
        /// <param name="ID"></param>
        /// ID of entity
        /// <returns></returns>
        public bool Delete(int ID)
        {
            try
            {
                var des = this.GetById(ID);
                if (des != null)
                {
                    this.EDMsDataContext.DeleteObject(des);
                    this.EDMsDataContext.SaveChanges();
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
