// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContractorTransmittalService.cs" company="">
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
    public class ContractorTransmittalService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly ContractorTransmittalDAO repo;


        /// <summary>
        /// Initializes a new instance of the <see cref="ContractorTransmittalService"/> class.
        /// </summary>
        public ContractorTransmittalService()
        {
            this.repo = new ContractorTransmittalDAO();
        }

        #region Get (Advances)

        public int GetCurrentSequence(int year, int group)
        {
            var result = 1;
            var currentSequence = this.repo.GetAll().Where(t => t.Year == year && t.TypeId ==2 && t.GroupId==group).Max(t => t.Sequence);
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
        public List<ContractorTransmittal> GetSpecific(Guid tranId)
        {
            return this.repo.GetSpecific(tranId);
        }

        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<ContractorTransmittal> GetAll()
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
        public List<ContractorTransmittal> GetAllByOwner(int userId)
        {
            return this.repo.GetAllByOwner(userId);
        }

        public List<ContractorTransmittal> GetAllByProject(List<int> projectId, int typeId)
        {
            return this.repo.GetAll().Where(t => projectId.Contains(t.ProjectId.GetValueOrDefault())).ToList();
        }

        public List<ContractorTransmittal> GetAllByProject(int projectId, int typeId, string searchAll)
        {
            var listkey = searchAll.ToLower().Split(' ').ToArray();
            return this.repo.GetAll().Where(t =>
            t.ProjectId == projectId
            && t.TypeId == typeId
             && (string.IsNullOrEmpty(searchAll) ||  listkey.All(k =>(t.TransNo.ToLower()+ " "+ t.Description.ToLower()).Contains(k)))).ToList();
            //    || (!string.IsNullOrEmpty(t.TransNo) && t.TransNo.ToLower().Contains(searchAll.ToLower()))
            //    || (!string.IsNullOrEmpty(t.OriginatingOrganizationName) && t.OriginatingOrganizationName.ToLower().Contains(searchAll.ToLower()))
            //    || (!string.IsNullOrEmpty(t.ReceivingOrganizationName) && t.ReceivingOrganizationName.ToLower().Contains(searchAll.ToLower()))
            //    || (!string.IsNullOrEmpty(t.Description) && t.Description.ToLower().Contains(searchAll.ToLower()))
            //    || (!string.IsNullOrEmpty(t.CCOrganizationName) && t.CCOrganizationName.ToLower().Contains(searchAll.ToLower()))
            //    || (!string.IsNullOrEmpty(t.GroupCode) && t.GroupCode.ToLower().Contains(searchAll.ToLower()))
            //    || (!string.IsNullOrEmpty(t.FromValue) && t.FromValue.ToLower().Contains(searchAll.ToLower()))
            //    || (!string.IsNullOrEmpty(t.ToValue) && t.ToValue.ToLower().Contains(searchAll.ToLower()))
            //    || (!string.IsNullOrEmpty(t.CCValue) && t.CCValue.ToLower().Contains(searchAll.ToLower()))
            //    || (!string.IsNullOrEmpty(t.PurposeName) && t.PurposeName.ToLower().Contains(searchAll.ToLower()))
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
        public ContractorTransmittal GetById(Guid id)
        {
            return this.repo.GetById(id);
        }


        public ContractorTransmittal GetByDQRETransmittal(Guid DQREid)
        {
            return this.repo.GetByDQRETransmittal(DQREid);
        }

        #endregion

        #region Insert, Update, Delete
        /// <summary>
        /// Insert Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public Guid? Insert(ContractorTransmittal bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(ContractorTransmittal bo)
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
        public bool Delete(ContractorTransmittal bo)
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
