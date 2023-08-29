﻿// --------------------------------------------------------------------------------------------------------------------
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
    public class DQRETransmittalService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly DQRETransmittalDAO repo;

        private readonly AttachDocToTransmittalService attachDocToTransmittalService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransmittalService"/> class.
        /// </summary>
        public DQRETransmittalService()
        {
            this.repo = new DQRETransmittalDAO();
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
        //public List<DQRETransmittal> GetSpecific(int tranId)
        //{
        //    return this.repo.GetSpecific(tranId);
        //}

        //public List<DQRETransmittal> GetAllByDocumentId(int documentId)
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
        public List<DQRETransmittal> GetAll()
        {
            return this.repo.GetAll().ToList();
        }

        /// <summary>
        /// The get all by owner.
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="typeId"></param>
        /// <param name="searchAll"></param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        //public List<DQRETransmittal> GetAllByOwner(int userId)
        //{
        //    return this.repo.GetAllByOwner(userId);
        //}
        public List<DQRETransmittal> GetAllByProject(int projectId, int typeId, string searchAll)
        {
            return this.repo.GetAll().Where(t => 
            t.ProjectCodeId == projectId 
            && t.TypeId == typeId
            && (string.IsNullOrEmpty(searchAll)
            || (!string.IsNullOrEmpty(t.TransmittalNo) && t.TransmittalNo.ToLower().Contains(searchAll.ToLower()))
            || (!string.IsNullOrEmpty(t.OriginatingOrganizationName) && t.OriginatingOrganizationName.ToLower().Contains(searchAll.ToLower()))
            || (!string.IsNullOrEmpty(t.ReceivingOrganizationName) && t.ReceivingOrganizationName.ToLower().Contains(searchAll.ToLower()))
            || (!string.IsNullOrEmpty(t.ConfidentialityName) && t.ConfidentialityName.ToLower().Contains(searchAll.ToLower()))
            )).ToList();
        }

        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of category
        /// </param>
        /// <returns>
        /// The category</returns>
        public DQRETransmittal GetById(Guid id)
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
        public Guid? Insert(DQRETransmittal bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(DQRETransmittal bo)
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
        public bool Delete(DQRETransmittal bo)
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
