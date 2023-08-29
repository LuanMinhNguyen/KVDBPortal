// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DQREDocumentAttachFileService.cs" company="">
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
    public class DQREDocumentAttachFileService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly DQREDocumentAttachFileDAO repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="DQREDocumentAttachFileService"/> class.
        /// </summary>
        public DQREDocumentAttachFileService()
        {
            this.repo = new DQREDocumentAttachFileDAO();
        }


        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<DQREDocumentAttachFile> GetAll()
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
        public List<DQREDocumentAttachFile> GetAllDocumentFileByDocId(Guid docId)
        {
            return this.repo.GetAll().Where(t => t.ProjectDocumentId == docId && t.TypeId == 1).ToList();
        }

        public List<DQREDocumentAttachFile> GetAllCmtResByDocId(Guid docId)
        {
            return this.repo.GetAll().Where(t => t.ProjectDocumentId == docId && (t.TypeId == 3 || t.TypeId == 4)).ToList();
        }

        public List<DQREDocumentAttachFile> GetAllDocId(Guid docId)
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
        public DQREDocumentAttachFile GetById(Guid id)
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
        /// The <see cref="DQREDocumentAttachFile"/>.
        /// </returns>
        public DQREDocumentAttachFile GetByNameServer(string nameServer)
        {
            return this.repo.GetAll().FirstOrDefault(t => t.FilePath.ToLower().Contains(nameServer.ToLower()));
        }
        #endregion

        #region Insert, Update, Delete
        /// <summary>
        /// Insert Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public Guid? Insert(DQREDocumentAttachFile bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(DQREDocumentAttachFile bo)
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
        public bool Delete(DQREDocumentAttachFile bo)
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
