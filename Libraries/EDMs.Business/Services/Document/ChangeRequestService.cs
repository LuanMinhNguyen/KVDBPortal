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
    public class ChangeRequestService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly ChangeRequestDAO repo;

        private readonly AttachDocToTransmittalService attachDocToTransmittalService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransmittalService"/> class.
        /// </summary>
        public ChangeRequestService()
        {
            this.repo = new ChangeRequestDAO();
            this.attachDocToTransmittalService = new AttachDocToTransmittalService();
        }

        #region Get (Advances)

        public List<ChangeRequest> GetAllRevChangeRequest(Guid parentId)
        {
            return this.repo.GetAll().Where(t => !t.IsDelete.GetValueOrDefault() && (t.ID == parentId || t.ParentId == parentId)).OrderByDescending(t => t.CreatedDate).ToList();
        }

        public List<ChangeRequest> GetAllByIncomingTrans(Guid incomingTransId)
        {
            return this.repo.GetAll().Where(t => t.IncomingTransId == incomingTransId).ToList();
        }

        public List<ChangeRequest> GetAllByOutTransTrans(Guid incomingTransId)
        {
            return this.repo.GetAll().Where(t => t.OutgoingTransId == incomingTransId).ToList();
        }

        public ChangeRequest GetAllByChangeRequestNo(string changeRequestNo)
        {
            return this.repo.GetAll().FirstOrDefault(t => !t.IsDelete.GetValueOrDefault() && t.Number == changeRequestNo);
        }

        public List<ChangeRequest> GetByChangeRequestNo(string changeRequestNo)
        {
            return this.repo.GetAll().Where(t => !t.IsDelete.GetValueOrDefault() && t.Number == changeRequestNo).ToList();
        }

        public bool IsExist(string number)
        {
            return this.repo.GetAll().Any(t =>
                !t.IsDelete.GetValueOrDefault()
                && t.Number == number);
        }

        public int GetCurrentSequence(int year)
        {
            var result = 1;
            var currentSequence = this.repo.GetAll().Where(t => t.Year == year).Max(t => t.Sequence);
            if (currentSequence != null)
            {
                result = currentSequence.Value + 1;
            }

            return result;
        }

        public List<ChangeRequest> SearchChageRequest(int projectId, int typeId, string docNo, string docTitle, string searchFullFields)
        {
            return this.repo.GetAll().Where(
                t => !t.IsDelete.GetValueOrDefault()
                && (projectId == t.ProjectId.GetValueOrDefault() || projectId == 0)
                && (typeId == 0 || t.TypeId.GetValueOrDefault() == typeId)
                && (string.IsNullOrEmpty(docNo) || t.Number.ToLower().Contains(docNo.ToLower()))
                && (string.IsNullOrEmpty(docTitle) || t.Description.ToLower().Contains(docTitle.ToLower()))
                ).OrderBy(t => t.Number).ToList();
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
        //public List<ChangeRequest> GetSpecific(int tranId)
        //{
        //    return this.repo.GetSpecific(tranId);
        //}

        //public List<ChangeRequest> GetAllByDocumentId(int documentId)
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
        public List<ChangeRequest> GetAll()
        {
            return this.repo.GetAll().ToList();
        }

        public List<ChangeRequest> GetAllByProject(int projectId)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId).ToList();
        }

        public List<ChangeRequest> GetAllByProject(int projectId, int confidential)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId && t.ConfidentialityId <= confidential).ToList();
        }

        public List<ChangeRequest> GetAllByProject(int projectId, string searchAll)
        {
            var listkey = searchAll.ToLower().Split(' ').ToArray();
            return this.repo.GetAll().Where(t => t.ProjectId == projectId 
                && t.IsLeaf.GetValueOrDefault()
               && (string.IsNullOrEmpty(searchAll) || listkey.All(k =>(t.Number.ToLower()+" "+ t.Description.ToLower()).Contains(k)))).ToList();
            //|| (!string.IsNullOrEmpty(t.Number) && t.Number.ToLower().Contains(searchAll.ToLower()))
            //|| (!string.IsNullOrEmpty(t.Description) && t.Description.ToLower().Contains(searchAll.ToLower()))
            //|| (!string.IsNullOrEmpty(t.ConfidentialityName) && t.ConfidentialityName.ToLower().Contains(searchAll.ToLower()))
            //|| (!string.IsNullOrEmpty(t.Description) && t.Description.ToLower().Contains(searchAll.ToLower()))
            //|| (!string.IsNullOrEmpty(t.AreaCode) && t.AreaCode.ToLower().Contains(searchAll.ToLower()))
            //|| (!string.IsNullOrEmpty(t.UnitCode) && t.UnitCode.ToLower().Contains(searchAll.ToLower()))
            //|| (!string.IsNullOrEmpty(t.TypeName) && t.TypeName.ToLower().Contains(searchAll.ToLower()))
            //|| (!string.IsNullOrEmpty(t.GroupName) && t.GroupName.ToLower().Contains(searchAll.ToLower()))
            //|| (!string.IsNullOrEmpty(t.ReasonForChange) && t.ReasonForChange.ToLower().Contains(searchAll.ToLower()))
            //|| (!string.IsNullOrEmpty(t.ExistingCondition) && t.ExistingCondition.ToLower().Contains(searchAll.ToLower()))
            //|| (!string.IsNullOrEmpty(t.ChangeGradeCodeName) && t.ChangeGradeCodeName.ToLower().Contains(searchAll.ToLower()))
            //|| (!string.IsNullOrEmpty(t.RefDocNo) && t.RefDocNo.ToLower().Contains(searchAll.ToLower()))
            //|| (!string.IsNullOrEmpty(t.ReviewResultName) && t.ReviewResultName.ToLower().Contains(searchAll.ToLower()))
            //|| (!string.IsNullOrEmpty(t.PECC2ReviewResultName) && t.PECC2ReviewResultName.ToLower().Contains(searchAll.ToLower()))
            //|| (!string.IsNullOrEmpty(t.OwnerReviewResultName) && t.OwnerReviewResultName.ToLower().Contains(searchAll.ToLower()))
            //|| (!string.IsNullOrEmpty(t.DocToBeRevisedNo) && t.DocToBeRevisedNo.ToLower().Contains(searchAll.ToLower()))
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
        public ChangeRequest GetById(Guid id)
        {
            return this.repo.GetById(id);
        }

        public List<ChangeRequest> GetByListId(List<Guid> Listid)
        {
            return this.repo.GetAll().Where(t => Listid.Contains(t.ID)).ToList();
        }
        #endregion

        #region Insert, Update, Delete
        /// <summary>
        /// Insert Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public Guid? Insert(ChangeRequest bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(ChangeRequest bo)
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
        public bool Delete(ChangeRequest bo)
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
