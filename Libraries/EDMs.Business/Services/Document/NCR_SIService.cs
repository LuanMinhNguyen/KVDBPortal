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
    public class NCR_SIService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly NCR_SIDAO repo;

        private readonly AttachDocToTransmittalService attachDocToTransmittalService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransmittalService"/> class.
        /// </summary>
        public NCR_SIService()
        {
            this.repo = new NCR_SIDAO();
            this.attachDocToTransmittalService = new AttachDocToTransmittalService();
        }

        #region Get (Advances)
        public int GetNCRSISequence(int typeId, int group, int year)
        {
            var result = 1;
            var currentSequence = this.repo.GetAll().Where(t => !t.IsCancel.GetValueOrDefault() && t.Type == typeId && t.GroupId==group && t.Year==year).Max(t => t.Sequence);
            if (currentSequence != null)
            {
                result = currentSequence.Value + 1;
            }

            return result;
        }

        public NCR_SI GetBySequene(int sequence)
        {
           return this.repo.GetAll().Find(t => t.Sequence == sequence);
        }
        public int GetCSSequence(int year)
        {
            var result = 1;
            var currentSequence = this.repo.GetAll().Where(t =>!t.IsCancel.GetValueOrDefault() && t.Year == year && t.Type == 3).Max(t => t.Sequence);
            if (currentSequence != null)
            {
                result = currentSequence.Value + 1;
            }

            return result;
        }

        public List<NCR_SI> GetAllNCRSIByProject(int projectId, string searchAll)
        {
            //return this.repo.GetAll().Where(t => t.ProjectId == projectId
            //    && (t.Type == 1 || t.Type == 2)
            //    && (string.IsNullOrEmpty(searchAll)
            //        || (!string.IsNullOrEmpty(t.Number) && t.Number.ToLower().Contains(searchAll.ToLower()))
            //        || (!string.IsNullOrEmpty(t.Description) && t.Description.ToLower().Contains(searchAll.ToLower()))
            //        || (!string.IsNullOrEmpty(t.ConfidentialityName) && t.ConfidentialityName.ToLower().Contains(searchAll.ToLower()))
            //        || (!string.IsNullOrEmpty(t.Description) && t.Description.ToLower().Contains(searchAll.ToLower()))
            //        || (!string.IsNullOrEmpty(t.Subject) && t.Subject.ToLower().Contains(searchAll.ToLower()))
            //        || (!string.IsNullOrEmpty(t.ActionTake) && t.ActionTake.ToLower().Contains(searchAll.ToLower()))
            //        || (!string.IsNullOrEmpty(t.SignedByPMC) && t.SignedByPMC.ToLower().Contains(searchAll.ToLower()))
            //        || (!string.IsNullOrEmpty(t.GroupName) && t.GroupName.ToLower().Contains(searchAll.ToLower()))
            //        || (!string.IsNullOrEmpty(t.SignedByPMB) && t.SignedByPMB.ToLower().Contains(searchAll.ToLower()))
            //        || (!string.IsNullOrEmpty(t.Status) && t.Status.ToLower().Contains(searchAll.ToLower()))
            //        || (!string.IsNullOrEmpty(t.Note) && t.Note.ToLower().Contains(searchAll.ToLower()))
            //        || (!string.IsNullOrEmpty(t.RefDocNo) && t.RefDocNo.ToLower().Contains(searchAll.ToLower()))
            //        )).ToList();
            var listkey = searchAll.ToLower().Split(' ').ToArray();
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                && (t.Type == 1 || t.Type == 2)
                && (string.IsNullOrEmpty(searchAll) || listkey.All(k => (t.Number.ToLower()+" "+t.Subject.ToLower()).Contains(k)))).ToList();
        }

        public List<NCR_SI> GetAllCSByProject(int projectId, string searchAll)
        {
            //return this.repo.GetAll().Where(t => t.ProjectId == projectId
            //                                     && t.Type == 3 
            //                                     && (string.IsNullOrEmpty(searchAll)
            //                                         || (!string.IsNullOrEmpty(t.Number) && t.Number.ToLower().Contains(searchAll.ToLower()))
            //                                         || (!string.IsNullOrEmpty(t.Description) && t.Description.ToLower().Contains(searchAll.ToLower()))
            //                                         || (!string.IsNullOrEmpty(t.ConfidentialityName) && t.ConfidentialityName.ToLower().Contains(searchAll.ToLower()))
            //                                         || (!string.IsNullOrEmpty(t.Description) && t.Description.ToLower().Contains(searchAll.ToLower()))
            //                                         || (!string.IsNullOrEmpty(t.Subject) && t.Subject.ToLower().Contains(searchAll.ToLower()))
            //                                         || (!string.IsNullOrEmpty(t.ActionTake) && t.ActionTake.ToLower().Contains(searchAll.ToLower()))
            //                                         || (!string.IsNullOrEmpty(t.SignedByPMC) && t.SignedByPMC.ToLower().Contains(searchAll.ToLower()))
            //                                         || (!string.IsNullOrEmpty(t.GroupName) && t.GroupName.ToLower().Contains(searchAll.ToLower()))
            //                                         || (!string.IsNullOrEmpty(t.SignedByPMB) && t.SignedByPMB.ToLower().Contains(searchAll.ToLower()))
            //                                         || (!string.IsNullOrEmpty(t.Status) && t.Status.ToLower().Contains(searchAll.ToLower()))
            //                                         || (!string.IsNullOrEmpty(t.Note) && t.Note.ToLower().Contains(searchAll.ToLower()))
            //                                         || (!string.IsNullOrEmpty(t.OriginatingOrganizationName) && t.OriginatingOrganizationName.ToLower().Contains(searchAll.ToLower()))
            //                                         || (!string.IsNullOrEmpty(t.ReceivingOrganizationName) && t.ReceivingOrganizationName.ToLower().Contains(searchAll.ToLower()))
            //                                         || (!string.IsNullOrEmpty(t.CCName) && t.CCName.ToLower().Contains(searchAll.ToLower()))
            //                                         || (!string.IsNullOrEmpty(t.RelatedCSNo) && t.RelatedCSNo.ToLower().Contains(searchAll.ToLower()))
            //                                         || (!string.IsNullOrEmpty(t.Treatment) && t.Treatment.ToLower().Contains(searchAll.ToLower()))
            //                                     )).ToList();

            var listkey = searchAll.ToLower().Split(' ').ToArray() ;
            return this.repo.GetAll().Where(t => t.ProjectId == projectId
                && (t.Type == 3)
                && (string.IsNullOrEmpty(searchAll) || listkey.All(k => (t.Number.ToLower() + " " + t.Subject.ToLower()).Contains(k)))).ToList();

        }
        public bool IsExist(string number)
        {
            return this.repo.GetAll().Any(t =>
                !t.IsDelete.GetValueOrDefault()
                && t.Number == number);
        }

        public int GetCurrentSequence(int typeId)
        {
            var result = 1;
            var currentSequence = this.repo.GetAll().Where(t => t.Type == typeId).Max(t => t.Sequence);
            if (currentSequence != null)
            {
                result = currentSequence.Value + 1;
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
        //public List<NCR_SI> GetSpecific(int tranId)
        //{
        //    return this.repo.GetSpecific(tranId);
        //}

        //public List<NCR_SI> GetAllByDocumentId(int documentId)
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
        public List<NCR_SI> GetAll(int typeId)
        {
            return this.repo.GetAll().Where(t => t.Type == typeId).ToList();
        }

        public List<NCR_SI> GetAllByProject(int projectId)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId).ToList();
        }

        public List<NCR_SI> GetAllByProject(int projectId, int typeId)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId && t.Type == typeId).ToList();
        }

        public List<NCR_SI> GetAllByProject(int projectId, int typeId, int confidential)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId && t.Type == typeId && t.ConfidentialityId <= confidential).ToList();
        }

        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of category
        /// </param>
        /// <returns>
        /// The category</returns>
        public NCR_SI GetById(Guid id)
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
        public Guid? Insert(NCR_SI bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(NCR_SI bo)
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
        public bool Delete(NCR_SI bo)
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
