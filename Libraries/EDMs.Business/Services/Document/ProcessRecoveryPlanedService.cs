// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessRecoveryPlanedService.cs" company="">
//   
// </copyright>
// <summary>
//   The ProcessRecoveryPlaned service.
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
    /// The ProcessRecoveryPlaned service.
    /// </summary>
    public class ProcessRecoveryPlanedService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly ProcessRecoveryPlanedDAO repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessRecoveryPlanedService"/> class.
        /// </summary>
        public ProcessRecoveryPlanedService()
        {
            this.repo = new ProcessRecoveryPlanedDAO();
        }

        #region Get (Advances)

        public List<ProcessRecoveryPlaned> GetAllByProject(int projectId)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId).ToList();
        }

        public List<ProcessRecoveryPlaned> GetAllByProject(int projectId, int index)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId && t.IndexNo == index).ToList();
        }

        public ProcessRecoveryPlaned GetByProjectAndWorkgroup(int projectId, int workgroupid, int indexNo)
        {
            return this.repo.GetAll().FirstOrDefault(t => t.ProjectId == projectId && t.WorkgroupId == workgroupid && t.IndexNo == indexNo);
        }
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The ProcessRecoveryPlaned
        /// </returns>
        public List<ProcessRecoveryPlaned> GetAll()
        {
            return this.repo.GetAll().ToList();
        }

        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of ProcessRecoveryPlaned
        /// </param>
        /// <returns>
        /// The ProcessRecoveryPlaned</returns>
        public ProcessRecoveryPlaned GetById(int id)
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
        public int? Insert(ProcessRecoveryPlaned bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(ProcessRecoveryPlaned bo)
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
        public bool Delete(ProcessRecoveryPlaned bo)
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
