// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransmittalService.cs" company="">
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
    public class TransmittalService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly TransmittalDAO repo;

        private readonly AttachDocToTransmittalService attachDocToTransmittalService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransmittalService"/> class.
        /// </summary>
        public TransmittalService()
        {
            this.repo = new TransmittalDAO();
            this.attachDocToTransmittalService = new AttachDocToTransmittalService();
        }

        #region Get (Advances)

        /// <summary>
        /// The get specific.
        /// </summary>
        /// <param name="tranId">
        /// The tran id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<Transmittal> GetSpecific(int tranId)
        {
            return this.repo.GetSpecific(tranId);
        }

        //public List<Transmittal> GetAllByDocumentId(int documentId)
        //{
        //    var listTransId =
        //        this.attachDocToTransmittalService.GetAllByDocId(documentId).Select(t => t.TransmittalId).Distinct();

        //    return listTransId.Select(tranId => this.repo.GetById(tranId.GetValueOrDefault()))
        //                    .Where(trans => trans != null).ToList();
        //}
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<Transmittal> GetAll()
        {
            return this.repo.GetAll().ToList();
        }

        /// <summary>
        /// The get all by owner.
        /// </summary>
        /// <param name="createdBy">
        /// The created by.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<Transmittal> GetAllByOwner(int userId)
        {
            return this.repo.GetAllByOwner(userId);
        }

        public List<Transmittal> GetAllByProject(List<int> projectId)
        {
            return this.repo.GetAll().Where(t => projectId.Contains(t.ProjectId.GetValueOrDefault())).ToList();
        }

        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of category
        /// </param>
        /// <returns>
        /// The category</returns>
        public Transmittal GetById(int id)
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
        public int? Insert(Transmittal bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(Transmittal bo)
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
        public bool Delete(Transmittal bo)
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
