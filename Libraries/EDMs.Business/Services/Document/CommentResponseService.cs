// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommentResponseService.cs" company="">
//   
// </copyright>
// <summary>
//   The CommentResponse service.
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
    /// The CommentResponse service.
    /// </summary>
    public class CommentResponseService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly CommentResponseDAO repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentResponseService"/> class.
        /// </summary>
        public CommentResponseService()
        {
            this.repo = new CommentResponseDAO();
        }

        #region Get (Advances)
        public List<CommentResponse> GetAllByContDoc(int contID, int docID)
        {
            return this.repo.GetAll().Where(t => t.DocumentID == docID && t.FromContractorID == contID).ToList();
        }

        public List<CommentResponse> GetAllByContDoc(int contID, int docID, int responseForContId)
        {
            return this.repo.GetAll().Where(t => t.DocumentID == docID && t.FromContractorID == contID && t.ToContractorID == responseForContId).ToList();
        }

        public List<CommentResponse> GetAllReviewByDoc(int docID)
        {
            return this.repo.GetAll().Where(t => t.DocumentID == docID && t.FromContractorTypeID == 2).ToList();
        }

        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The CommentResponse
        /// </returns>
        public List<CommentResponse> GetAll()
        {
            return this.repo.GetAll().ToList();
        }

        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of CommentResponse
        /// </param>
        /// <returns>
        /// The CommentResponse</returns>
        public CommentResponse GetById(int id)
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
        public int? Insert(CommentResponse bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(CommentResponse bo)
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
        public bool Delete(CommentResponse bo)
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
