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
    public class PECC2TransmittalService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly PECC2TransmittalDAO repo;

        private readonly AttachDocToTransmittalService attachDocToTransmittalService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransmittalService"/> class.
        /// </summary>
        public PECC2TransmittalService()
        {
            this.repo = new PECC2TransmittalDAO();
            this.attachDocToTransmittalService = new AttachDocToTransmittalService();
        }

        #region Get (Advances)

        public int GetCurrentSequence(int year, int group)
        {
            var result = 1;
            var currentSequence = this.repo.GetAll().Where(t => t.Year == year && t.TypeId == 2 && t.GroupId==group).Max(t => t.Sequence);
            if (currentSequence != null)
            {
                result = currentSequence.GetValueOrDefault() + 1;
            }

            return result;
        }

        /// <summary>
        /// The get specific.
        /// </summary>
        /// <param name="tranId">
        /// The tran id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        //public List<PECC2Transmittal> GetSpecific(int tranId)
        //{
        //    return this.repo.GetSpecific(tranId);
        //}

        //public List<PECC2Transmittal> GetAllByDocumentId(int documentId)
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
        public List<PECC2Transmittal> GetAll()
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
        //public List<PECC2Transmittal> GetAllByOwner(int userId)
        //{
        //    return this.repo.GetAllByOwner(userId);
        //}
        public List<PECC2Transmittal> GetAllByProject(int projectId, int typeId, string searchAll)
        {
            var listkey = searchAll.ToLower().Split(' ').ToArray();
            return this.repo.GetAll().Where(t => 
            t.ProjectCodeId == projectId 
            && t.TypeId == typeId
             && (string.IsNullOrEmpty(searchAll) ||  listkey.All(k =>(t.TransmittalNo.ToLower()+" "+ t.Description.ToLower()).Contains(k)))).ToList();
            //|| (!string.IsNullOrEmpty(t.TransmittalNo) && t.TransmittalNo.ToLower().Contains(searchAll.ToLower()))
            //|| (!string.IsNullOrEmpty(t.OriginatingOrganizationName) && t.OriginatingOrganizationName.ToLower().Contains(searchAll.ToLower()))
            //|| (!string.IsNullOrEmpty(t.ReceivingOrganizationName) && t.ReceivingOrganizationName.ToLower().Contains(searchAll.ToLower()))
            //|| (!string.IsNullOrEmpty(t.ConfidentialityName) && t.ConfidentialityName.ToLower().Contains(searchAll.ToLower()))
            //|| (!string.IsNullOrEmpty(t.Description) && t.Description.ToLower().Contains(searchAll.ToLower()))
            //|| (!string.IsNullOrEmpty(t.CCOrganizationName) && t.CCOrganizationName.ToLower().Contains(searchAll.ToLower()))
            //|| (!string.IsNullOrEmpty(t.GroupCode) && t.GroupCode.ToLower().Contains(searchAll.ToLower()))
            //|| (!string.IsNullOrEmpty(t.FromValue) && t.FromValue.ToLower().Contains(searchAll.ToLower()))
            //|| (!string.IsNullOrEmpty(t.ToValue) && t.ToValue.ToLower().Contains(searchAll.ToLower()))
            //|| (!string.IsNullOrEmpty(t.CCValue) && t.CCValue.ToLower().Contains(searchAll.ToLower()))
            //)).ToList();
        }

        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of category
        /// </param>
        /// <returns>
        /// The category</returns>
        public PECC2Transmittal GetById(Guid id)
        {
            return this.repo.GetById(id);
        }

        public PECC2Transmittal GetByRefId(Guid id)
        {
            return this.repo.GetByRefId(id);
        }
        #endregion

        #region Insert, Update, Delete
        /// <summary>
        /// Insert Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public Guid? Insert(PECC2Transmittal bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(PECC2Transmittal bo)
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
        public bool Delete(PECC2Transmittal bo)
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
