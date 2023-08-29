// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessPlanedService.cs" company="">
//   
// </copyright>
// <summary>
//   The ProcessPlaned service.
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
    /// The ProcessPlaned service.
    /// </summary>
    public class ProcessPlanedService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly ProcessPlanedDAO repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessPlanedService"/> class.
        /// </summary>
        public ProcessPlanedService()
        {
            this.repo = new ProcessPlanedDAO();
        }

        #region Get (Advances)

        public List<ProcessPlaned> GetAllByProject(int projectId, int workgroupId)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId && (workgroupId == 0 || t.WorkgroupId == workgroupId)).ToList();
        }

        public ProcessPlaned GetByProjectAndWorkgroup(int projectId, int workgroupid)
        {
            return this.repo.GetAll().FirstOrDefault(t => t.ProjectId == projectId && t.WorkgroupId == workgroupid);
        }
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The ProcessPlaned
        /// </returns>
        public List<ProcessPlaned> GetAll()
        {
            return this.repo.GetAll().ToList();
        }

        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of ProcessPlaned
        /// </param>
        /// <returns>
        /// The ProcessPlaned</returns>
        public ProcessPlaned GetById(int id)
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
        public int? Insert(ProcessPlaned bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(ProcessPlaned bo)
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
        public bool Delete(ProcessPlaned bo)
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
