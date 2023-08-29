// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttachDocToTransmittalService.cs" company="">
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
    public class AttachDocToTransmittalService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly AttachDocToTransmittalDAO repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="AttachDocToTransmittalService"/> class.
        /// </summary>
        public AttachDocToTransmittalService()
        {
            this.repo = new AttachDocToTransmittalDAO();
        }

        #region Get (Advances)

        public bool IsExist(Guid transId, Guid docId)
        {
            return this.repo.GetAll().Any(t => t.TransmittalId == transId && t.DocumentId == docId);
        }

        public AttachDocToTransmittal GetByDoc(Guid transId, Guid docId)
        {
            return this.repo.GetAll().FirstOrDefault(t => t.TransmittalId == transId && t.DocumentId == docId);
        }

        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<AttachDocToTransmittal> GetAll()
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
        public List<AttachDocToTransmittal> GetAllByDocId(Guid docId)
        {
            return this.repo.GetAll().Where(t => t.DocumentId == docId).ToList();
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
        public List<AttachDocToTransmittal> GetAllByTransId(Guid transId)
        {
            return this.repo.GetAll().Where(t => t.TransmittalId == transId).ToList();
        }

        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of category
        /// </param>
        /// <returns>
        /// The category</returns>
        public AttachDocToTransmittal GetById(int id)
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
        public int? Insert(AttachDocToTransmittal bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Delete Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Delete(AttachDocToTransmittal bo)
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
