// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PECC2DocumentAttachFileService.cs" company="">
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
    public class PECC2DocumentAttachFileService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly PECC2DocumentAttachFileDAO repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="PECC2DocumentAttachFileService"/> class.
        /// </summary>
        public PECC2DocumentAttachFileService()
        {
            this.repo = new PECC2DocumentAttachFileDAO();
        }


        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<PECC2DocumentAttachFile> GetAll()
        {
            return this.repo.GetAll().ToList();
        }

        /// <summary>
        /// The get all by doc id.
        /// </summary>
        /// <param name="docId">
        /// The doc id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<PECC2DocumentAttachFile> GetAllDocumentFileByDocId(Guid docId)
        {
            return this.repo.GetAll().Where(t => t.ProjectDocumentId == docId && t.TypeId == 1).ToList();
        }

        public List<PECC2DocumentAttachFile> GetAllCmtResByDocId(Guid docId)
        {
            return this.repo.GetAll().Where(t => t.ProjectDocumentId == docId && (t.TypeId == 3 || t.TypeId == 4)).ToList();
        }
        public List<PECC2DocumentAttachFile> GetAllConsolidateByDocId(Guid docId)
        {
            return this.repo.GetAll().Where(t => t.ProjectDocumentId == docId && (t.TypeId == 3)).OrderByDescending(t=> t.CreatedDate).ToList();
        }
        public List<PECC2DocumentAttachFile> GetAllDocId(Guid docId)
        {
            return this.repo.GetAll().Where(t => t.ProjectDocumentId == docId).OrderBy(t => t.TypeId).ToList();
        }

        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of category
        /// </param>
        /// <returns>
        /// The category</returns>
        public PECC2DocumentAttachFile GetById(Guid id)
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
        /// The <see cref="PECC2DocumentAttachFile"/>.
        /// </returns>
        public PECC2DocumentAttachFile GetByNameServer(string nameServer)
        {
            return this.repo.GetAll().FirstOrDefault(t => t.FilePath.ToLower().Contains(nameServer.ToLower()));
        }

        /// <summary>
        /// The get file consolidate by UserId
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public PECC2DocumentAttachFile GetByUserUploadConsolidateFile(int UserId, Guid docId)
        {
            return this.repo.GetAll().FirstOrDefault(t =>t.ProjectDocumentId==docId && t.CreatedBy.GetValueOrDefault()==UserId && t.TypeId.GetValueOrDefault()==3);
        }

        public PECC2DocumentAttachFile GetByConsolidateFile( Guid docId)
        {
            return this.repo.GetAll().FirstOrDefault(t => t.ProjectDocumentId == docId && t.TypeId.GetValueOrDefault() == 3);
        }

        /// <summary>
        /// Get all file markup;
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="docId"></param>
        /// <returns></returns>
        public List< PECC2DocumentAttachFile> GetByUserUploadMarkupFile(int UserId, Guid docId)
        {
            return this.repo.GetAll().Where(t => t.ProjectDocumentId == docId && t.CreatedBy.GetValueOrDefault() == UserId && t.TypeId.GetValueOrDefault() == 2).ToList();
        }
        #endregion

        #region Insert, Update, Delete
        /// <summary>
        /// Insert Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public Guid? Insert(PECC2DocumentAttachFile bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(PECC2DocumentAttachFile bo)
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
        public bool Delete(PECC2DocumentAttachFile bo)
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
        public bool Delete(Guid id)
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
