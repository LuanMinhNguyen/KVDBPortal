// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessActualService.cs" company="">
//   
// </copyright>
// <summary>
//   The ProcessActual service.
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
    /// The ProcessActual service.
    /// </summary>
    public class ProcessActualService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly ProcessActualDAO repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessActualService"/> class.
        /// </summary>
        public ProcessActualService()
        {
            this.repo = new ProcessActualDAO();
        }

        #region Get (Advances)
        public List<ProcessActual> GetAllByProject(int projectId, int workgroupId)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId && (workgroupId == 0 || t.WorkgroupId == workgroupId)).ToList();
        }

        public ProcessActual GetByProjectAndWorkgroup(int projectId, int workgroupid)
        {
            return this.repo.GetAll().FirstOrDefault(t => t.ProjectId == projectId && t.WorkgroupId == workgroupid);
        }
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The ProcessActual
        /// </returns>
        public List<ProcessActual> GetAll()
        {
            return this.repo.GetAll().ToList();
        }

        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of ProcessActual
        /// </param>
        /// <returns>
        /// The ProcessActual</returns>
        public ProcessActual GetById(int id)
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
        public int? Insert(ProcessActual bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(ProcessActual bo)
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
        public bool Delete(ProcessActual bo)
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
